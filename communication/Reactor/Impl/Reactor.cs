using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class Reactor : IReactor
    {
        private readonly int _localPort;
        private bool _shouldClose = false;
        private static readonly object padlock = new object();

        private readonly IListenerSelector _selector;
        private readonly IDictionary<TcpClient, IEventHandler> _handlers;
        private readonly TcpListener _listener;
        private readonly ConcurrentQueue<TcpClient> _socketsQueue;
        private readonly ConcurrentQueue<string> _receivedMsgQueue;
        private readonly IDictionary<int, ConcurrentQueue<string>> _userIdToMsgQueue;
        private readonly IDictionary<TcpClient, int> _socketToUserId; //sockets to user ids
        private readonly ManualResetEvent _connectionCleanerMre = new ManualResetEvent(false);
        private readonly IList<ManualResetEvent> _shutdownMreList;

        public Reactor(IListenerSelector selector, int port)
        {
            _selector = selector;
            _localPort = port;
            _handlers = new Dictionary<TcpClient, IEventHandler>();
            _socketsQueue = new ConcurrentQueue<TcpClient>();
            _receivedMsgQueue = new ConcurrentQueue<string>();
            _userIdToMsgQueue = new ConcurrentDictionary<int, ConcurrentQueue<string>>();
            _socketToUserId = new ConcurrentDictionary<TcpClient, int>();

            _shutdownMreList = new List<ManualResetEvent> {_connectionCleanerMre};
            _listener = new TcpListener(IPAddress.Any, _localPort);
        }

        //start all threads:
        public void Start()
        {
            ThreadPool.QueueUserWorkItem(HandleReading);
            ThreadPool.QueueUserWorkItem(HandleWriting);
            ThreadPool.QueueUserWorkItem(RemoveUnconnectedClients); //starts and sleeps

            //main thread does this:
            AcceptClients();
        }

        public int Port
        {
            get { return _localPort; }
        }

        //TODO: change this
        public void RegisterHandler(IEventHandler eventHandler)
        {
            _handlers.Add(eventHandler.GetHandler(), eventHandler);
        }

        public void RemoveHandler(IEventHandler eventHandler)
        {
            _handlers.Remove(eventHandler.GetHandler());
        }

        //main thread:
        public void AcceptClients()
        {
            //main thread so no need to signal it started
            _listener.Start();
            while (!_shouldClose)
            {
                TcpClient tcpClient = _listener.AcceptTcpClient();
                _handlers.Add(tcpClient, new MessageEventHandler(tcpClient));
                _socketsQueue.Enqueue(tcpClient);

                _connectionCleanerMre.Set(); //wake the thread removing unconnected clients
            }
            _listener.Stop();
        }

        private void RemoveUnconnectedClients(Object threadContext)
        {
            while (!_shouldClose)
            {
                //allready got MRE
                _connectionCleanerMre.Reset(); //sleep until main thread wakes it up
                List<TcpClient> tempHolder = new List<TcpClient>();
                while (!_socketsQueue.IsEmpty)
                {
                    TcpClient client;
                    _socketsQueue.TryDequeue(out client);
                    if (client != null && client.Connected)
                    {
                        tempHolder.Add(client);
                    }
                }
                tempHolder.ForEach(client => _socketsQueue.Enqueue(client));  
            }
            _connectionCleanerMre.Set(); //signal thread is done
        }

        private void HandleReading(Object threadContext)
        {
            ManualResetEvent readingMre = new ManualResetEvent(false);
            _shutdownMreList.Add(readingMre);
            byte[] buffer = new byte[1];
            IList<byte> data = new List<byte>();

            while (!_shouldClose)
            {
                IList<TcpClient> readyToRead = _selector.SelectForReading(_socketsQueue);
                foreach (var tcpClient in readyToRead)
                {
                    NetworkStream stream = tcpClient.GetStream();
                    var dataReceived = 0;
                    do
                    {
                        dataReceived = stream.Read(buffer, 0, 1);
                        if (dataReceived > 0)
                        {
                            data.Add(buffer[0]);
                        }

                    } while (dataReceived > 0);

                    //add msg string to queue
                    _receivedMsgQueue.Enqueue(buffer.ToArray().ToString());
                } 
            }
            readingMre.Set(); //signal thread is done
        }

        private void HandleWriting(Object threadContext)
        {
            ManualResetEvent writingMre = new ManualResetEvent(false);
            _shutdownMreList.Add(writingMre);
            while (!_shouldClose)
            {
                IList<TcpClient> readyToWrite = _selector.SelectForWriting(_socketsQueue);
                foreach (var tcpClient in readyToWrite)
                {
                    if (CanSendMsg(tcpClient))
                    {
                        var msgQueue = _userIdToMsgQueue[_socketToUserId[tcpClient]]; //get msg queue
                        SendAllMsgFromQueue(msgQueue, tcpClient);
                    }
                } 
            }

            //send all remaining messages from all msg queues
            foreach (var socketToIdPair in _socketToUserId)
            {
                int userId = socketToIdPair.Value;
                var tcpClient = socketToIdPair.Key;
                var queue = _userIdToMsgQueue[userId];
                SendAllMsgFromQueue(queue, tcpClient);
            }

            writingMre.Set(); //signal thread is done
        }

        private void SendAllMsgFromQueue(ConcurrentQueue<String> msgQueue, TcpClient tcpClient)
        {
            while (!msgQueue.IsEmpty)
            {
                string msg;
                msgQueue.TryDequeue(out msg);
                byte[] bytesToSend = Encoding.UTF8.GetBytes(msg);
                tcpClient.GetStream().Write(bytesToSend, 0, bytesToSend.Length);
            }
        }

        //true if socket and user id exist, msgQueue isn't empty and socket connected
        private bool CanSendMsg(TcpClient client)
        {
            if (_socketToUserId.ContainsKey(client))
            {
                int id = _socketToUserId[client];
                if (_userIdToMsgQueue.ContainsKey(id))
                {
                    return client.Connected && _socketToUserId.ContainsKey(client) &&
                           !_userIdToMsgQueue[id].IsEmpty;
                }
            }
            return false;
        }

        private void ShutDown()
        {
            WaitHandle.WaitAll(_shutdownMreList.ToArray()); //wait for all threadpool threads to be done

            //TODO: maybe send shoutdown msg to all clients
            //delete all sockets and connections:
            foreach (var tcpClient in _socketsQueue)
            {
                tcpClient.Close();
            }
        }

        //called from outside to stop reactor
        public void Close()
        {
            lock (padlock)
            {
                _shouldClose = true;
                _connectionCleanerMre.Set(); //wake the cleaner up;
            }
        }
    }
}