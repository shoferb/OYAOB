using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Security;

namespace TexasHoldem.communication.Impl
{

    public class CommunicationHandler : ICommunicationHandler
    {
        private const int LocalPort = 2000;
        protected bool ShouldClose = false;
        protected bool WasShutDown = false; //used only for testing
        private static readonly object Padlock = new object();

        protected readonly IListenerSelector Selector;
        protected readonly TcpListener _listener;
        protected readonly ConcurrentQueue<TcpClient> _socketsQueue;
        protected readonly ConcurrentQueue<Tuple<string, TcpClient>> _receivedMsgQueue;
        protected readonly IDictionary<int, ConcurrentQueue<string>> _userIdToMsgQueue;
        protected readonly IDictionary<TcpClient, int> _socketToUserId; //sockets to user ids
        protected readonly ManualResetEvent _connectionCleanerMre = new ManualResetEvent(false);
        protected List<Task> taskList;
        private static CommunicationHandler _instance;
        private readonly ISecurity _security = new SecurityHandler();
        
        public static CommunicationHandler GetInstance()
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = new CommunicationHandler();
                }
                return _instance;
            }
        }

        protected CommunicationHandler()
        {
            Selector = new ListenerSelector();
            _socketsQueue = new ConcurrentQueue<TcpClient>();
            _receivedMsgQueue = new ConcurrentQueue<Tuple<string, TcpClient>>();
            _userIdToMsgQueue = new ConcurrentDictionary<int, ConcurrentQueue<string>>();
            _socketToUserId = new ConcurrentDictionary<TcpClient, int>();

            _listener = new TcpListener(IPAddress.Any, LocalPort);
        }

        public List<string> GetMsgsToSend()
        {
            List<string> msgs = new List<string>();
            foreach (var queue in _userIdToMsgQueue.Values)
            {
                foreach (var msg in queue)
                {
                    msgs.Add(msg);
                }
            }
            return msgs;
        }

        //start all threads:
        public void AddUserId(int id, TcpClient socket)
        {
            if (!_userIdToMsgQueue.ContainsKey(id))
            {
                _userIdToMsgQueue.Add(id, new ConcurrentQueue<string>());
            }
            if (!_socketToUserId.ContainsKey(socket))
            {
                _socketToUserId.Add(socket, id);
            }
        }

        public void Start()
        {

            taskList = new List<Task>
            {
                new Task(HandleReading),
                new Task(HandleWriting),
                new Task(RemoveUnconnectedClients)
            };
            taskList.ForEach(task => task.Start());

            AcceptClients();
        }

        public int Port
        {
            get { return LocalPort; }
        }

        public List<Tuple<string, TcpClient>> GetReceivedMessages()
        {
            lock (_receivedMsgQueue)
            {
                var lst = new List<Tuple<string, TcpClient>>();
                while (!_receivedMsgQueue.IsEmpty)
                {
                    Tuple<string, TcpClient> temp;
                    _receivedMsgQueue.TryDequeue(out temp);
                    lst.Add(temp);
                }
                return lst;
            }
        }

        public TcpClient GetSocketById(int id)
        {
            foreach (var keyVal in _socketToUserId)
            {
                if (keyVal.Value == id && keyVal.Key.Connected)
                {
                    return keyVal.Key;
                }
            }
            return null;
        }

        public bool AddMsgToSend(string msg, int userId)
        {
            Console.WriteLine("arrived to AddMsgToSend");
            Console.WriteLine("the user id is: " + userId);
            Console.WriteLine("the msg is: " + msg);

            if (!_userIdToMsgQueue.ContainsKey(userId))
            {
                Console.WriteLine("fucking enter to if");

                _userIdToMsgQueue.Add(userId, new ConcurrentQueue<string>());
            }
            Console.WriteLine("fucking didnt enter to if");

            var queue = _userIdToMsgQueue[userId];
            queue.Enqueue(msg);
            Console.WriteLine("put enqueue" + msg);
            return true;
        }

        //main thread:
        public void AcceptClients()
        {
            Console.WriteLine("comm: accepting clients");
            //main thread so no need to signal it started
            _listener.Start();
            while (!ShouldClose)
            {
                try
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    Console.WriteLine("comm: got connection!" + tcpClient.Client);
                    _socketsQueue.Enqueue(tcpClient);

                    _connectionCleanerMre.Set(); //wake the thread removing unconnected clients
                }
                catch (SocketException e)
                {
                    //TODO: change this to log
                    Console.WriteLine("listener socket has thrown: " + e.Message);
                }
            }
            _listener.Stop();
            ShutDown();
        }

        //thread 1
        protected void HandleReading()
        {
            Console.WriteLine("comm: reading");
            byte[] buffer = new byte[1];

            while (!ShouldClose)
            {
                IList<TcpClient> readyToRead = Selector.SelectForReading(_socketsQueue);
                foreach (var tcpClient in readyToRead)
                {
                    IList<byte> data = new List<byte>();
                    NetworkStream stream = tcpClient.GetStream();
                    var dataReceived = 0;
                    do
                    {
                        dataReceived = stream.Read(buffer, 0, 1);
                        if (dataReceived > 0)
                        {
                            Console.WriteLine("comm: got byte!");
                            data.Add(buffer[0]);
                        }

                    } while (dataReceived > 0 && stream.DataAvailable);
                    Console.WriteLine("comm: after while");

                    //add msg string to queue
                    if (data.Count > 0)
                    {
                        Console.WriteLine("comm: done getting bytes. putting to arr");
                        string msgStr = _security.Decrypt(data.ToArray());
                        lock (_receivedMsgQueue)
                        {
                            _receivedMsgQueue.Enqueue(new Tuple<string, TcpClient>(msgStr, tcpClient));
                        }
                        Console.WriteLine("comm: msg is: " + Encoding.UTF8.GetString(data.ToArray()));
                    }
                } 
            }
        }

        //thread 2
        protected void HandleWriting()
        {
            Console.WriteLine("comm: writing");
            while (!ShouldClose)
            {
                IList<TcpClient> readyToWrite = Selector.SelectForWriting(_socketsQueue);
                foreach (var tcpClient in readyToWrite)
                {

                    if (CanSendMsg(tcpClient))
                    {
                        Console.WriteLine("entered to if inside foreach");

                        Console.WriteLine("comm: got shit to write");
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
        }

        //true if socket and user id exist, msgQueue isn't empty and socket connected
        protected bool CanSendMsg(TcpClient client)
        {

            //_socketToUserId.Add(client, -1);


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

        protected void SendAllMsgFromQueue(ConcurrentQueue<String> msgQueue, TcpClient tcpClient)
        {
            while (!msgQueue.IsEmpty)
            {
                string msg;
                msgQueue.TryDequeue(out msg);
                byte[] bytesToSend = Encoding.UTF8.GetBytes(msg);
                bytesToSend = _security.Encrypt(bytesToSend);
                tcpClient.GetStream().Write(bytesToSend, 0, bytesToSend.Length);
            }
        }

        //thread 3
        protected void RemoveUnconnectedClients()
        {
            while (!ShouldClose)
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

        protected void ShutDown()
        {
            Task.WaitAll(taskList.ToArray());
            //TODO: maybe send shoutdown msg to all clients

            //delete all sockets and connections:
            foreach (var tcpClient in _socketsQueue)
            {
                tcpClient.Close();
            }

            //TODO: log shutdown
        }

        //called from outside to stop reactor
        public void Close()
        {
            lock (Padlock)
            {
                ShouldClose = true;
                _listener.Stop();
                _connectionCleanerMre.Set(); //wake the cleaner up;
            }
        }
    }
}