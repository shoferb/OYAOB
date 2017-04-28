using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class Reactor : IReactor
    {
        private readonly IListenerSelector _selector;
        private readonly IDictionary<TcpListener, IEventHandler> _handlers;

        public Reactor(IListenerSelector selector)
        {
            _selector = selector;
            _handlers = new Dictionary<TcpListener, IEventHandler>();
        }

        public void RegisterHandler(IEventHandler eventHandler)
        {
            _handlers.Add(eventHandler.GetHandler(), eventHandler);
        }

        public void RemoveHandler(IEventHandler eventHandler)
        {
            _handlers.Remove(eventHandler.GetHandler());
        }

        public void HandleEvents()
        {
            while (true)
            {
                IList<TcpListener> listeners = _selector.Select(_handlers.Keys);

                foreach (TcpListener listener in listeners)
                {
                    int dataReceived = 0;
                    byte[] buffer = new byte[1];
                    IList<byte> data = new List<byte>();

                    Socket socket = listener.AcceptSocket();

                    do
                    {
                        dataReceived = socket.Receive(buffer);

                        if (dataReceived > 0)
                        {
                            data.Add(buffer[0]);
                        }

                    } while (dataReceived > 0);

                    socket.Close();

                    _handlers[listener].HandleEvent(data.ToArray());
                }
            }
        }
    }
}