using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class ListenerSelector : IListenerSelector
    {
        public IList<TcpListener> Select(ICollection<TcpListener> listeners)
        {
            var tcpListeners = new List<TcpListener>(from listener in listeners
                                                     where listener.Pending()
                                                     select listener);
            return tcpListeners;
        }
    }
}