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
        private const int PollMicroSecs = 100;
        private readonly IListenerSelector _selector;
        private readonly IDictionary<TcpListener, IEventHandler> _handlers;
        private readonly ConcurrentQueue<Socket> _socketsQueue;
        private readonly ConcurrentQueue<string> _receivedMsgQueue;
        private readonly ConcurrentDictionary<int, ConcurrentQueue<string>> _userIdToMsgQueue;
        private readonly ConcurrentDictionary<Socket, int> _socketToUserId; //sockets to user ids

        public Reactor(IListenerSelector selector)
        {
            _selector = selector;
            _handlers = new Dictionary<TcpListener, IEventHandler>();
            _socketsQueue = new ConcurrentQueue<Socket>();
            _receivedMsgQueue = new ConcurrentQueue<string>();
            _userIdToMsgQueue = new ConcurrentDictionary<int, ConcurrentQueue<string>>();
            _socketToUserId = new ConcurrentDictionary<Socket, int>();
        }

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
            while (true)
            {
                IList<TcpListener> listeners = _selector.Select(_handlers.Keys);

                foreach (TcpListener listener in listeners)
                {
                    Socket socket = listener.AcceptSocket();

                    //socket.Close();

                    //_handlers[listener].HandleEvent(data.ToArray());
                }
            }
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
                }
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
    }
}