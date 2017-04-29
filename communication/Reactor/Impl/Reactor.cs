using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class Reactor : IReactor
    {
        private const int PollMicroSecs = 100; //time to wait for answer from polling sockets
        private readonly int _localPort;
        private bool _shouldClose = false;

        private readonly IListenerSelector _selector;
        private readonly IDictionary<TcpClient, IEventHandler> _handlers;
        private readonly TcpListener _listener;
        private readonly ConcurrentQueue<TcpClient> _socketsQueue;
        private readonly ConcurrentQueue<string> _receivedMsgQueue;
        private readonly IDictionary<int, ConcurrentQueue<string>> _userIdToMsgQueue;
        private readonly IDictionary<TcpClient, int> _socketToUserId; //sockets to user ids //TODO: maybe no need for this one

        public Reactor(IListenerSelector selector, int port)
        {
            _selector = selector;
            _localPort = port;
            _handlers = new Dictionary<TcpClient, IEventHandler>();
            _socketsQueue = new ConcurrentQueue<TcpClient>();
            _receivedMsgQueue = new ConcurrentQueue<string>();
            _userIdToMsgQueue = new ConcurrentDictionary<int, ConcurrentQueue<string>>();
            _socketToUserId = new ConcurrentDictionary<TcpClient, int>();

            _listener = new TcpListener(IPAddress.Any, _localPort);

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

        //TODO: change this
        public void AcceptClients()
        {
            _listener.Start();
            while (!_shouldClose)
            {
                TcpClient tcpClient = _listener.AcceptTcpClient();
                _handlers.Add(tcpClient, new MessageEventHandler(tcpClient));
                _socketsQueue.Enqueue(tcpClient);
            }
            _listener.Stop();
        }

        private void RemoveUnconnectedClients()
        {
            //TODO
        }

        private void HandleReading()
        {
            int dataReceived = 0;
            byte[] buffer = new byte[1];
            IList<byte> data = new List<byte>();

            IList<TcpClient> readyToRead = _selector.SelectForReading(_socketsQueue);
            foreach (var tcpClient in readyToRead)
            {
                NetworkStream stream = tcpClient.GetStream();
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

        private void HandleWriting()
        {
            IList<TcpClient> readyToWrite = _selector.SelectForWriting(_socketsQueue);
            foreach (var tcpClient in readyToWrite)
            {
                if (CanSendMsg(tcpClient))
                {
                    var msgQueue = _userIdToMsgQueue[_socketToUserId[tcpClient]]; //get msg queue
                    while (!msgQueue.IsEmpty)
                    {
                        string msg;
                        msgQueue.TryDequeue(out msg);
                        byte[] bytesToSend = Encoding.UTF8.GetBytes(msg);
                        tcpClient.GetStream().Write(bytesToSend, 0, bytesToSend.Length);
                    }
                }
            }
        }


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

        //TODO
        public bool Close()
        {
            _shouldClose = true;
            throw new System.NotImplementedException();
        }
    }
}