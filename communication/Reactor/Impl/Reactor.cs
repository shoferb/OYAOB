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
                //foreach (TcpListener listener in listeners)
                //{
                //    Socket socket = listener.AcceptSocket();

                //    //socket.Close();

                //    //_handlers[listener].HandleEvent(data.ToArray());
                //}


            }
            _listener.Stop();
        }

        //TODO: this
        public void HandleConnections()
        {
            int dataReceived = 0;
            byte[] buffer = new byte[1];
            IList<byte> data = new List<byte>();

            foreach (Socket socket in _socketsQueue)
            {
                //check if socket can be read:
                if (socket.Connected && socket.Poll(PollMicroSecs, SelectMode.SelectRead))
                {
                    do{
                        dataReceived = socket.Receive(buffer);

                        if (dataReceived > 0)
                        {
                            data.Add(buffer[0]);
                        }

                    } while (dataReceived > 0);

                    //add msg string to queue
                    _receivedMsgQueue.Enqueue(buffer.ToArray().ToString());
                }

                if (CanSendMsg(socket))
                {
                    var msgQueue = _userIdToMsgQueue[_socketToUserId[socket]]; //get msg queue
                    while (!msgQueue.IsEmpty && socket.Poll(PollMicroSecs, SelectMode.SelectWrite))
                    {
                        string msg;
                        msgQueue.TryDequeue(out msg);
                        byte[] bytesToSend = Encoding.UTF8.GetBytes(msg);
                        socket.Send(bytesToSend); //TODO: maybe need to do this in while (if not all bytes are sent)
                    }
                }

                if (!socket.Connected)
                {
                    //TODO: disconnect socket and remove things from maps
                    continue;
                }
                _socketsQueue.Enqueue(socket);
            }
        }

        private bool CanSendMsg(Socket socket)
        {
            if (_socketToUserId.ContainsKey(socket))
            {
                int id = _socketToUserId[socket];
                if (_userIdToMsgQueue.ContainsKey(id))
                {
                    return socket.Connected && _socketToUserId.ContainsKey(socket) &&
                           !_userIdToMsgQueue[id].IsEmpty && socket.Poll(PollMicroSecs, SelectMode.SelectWrite);
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