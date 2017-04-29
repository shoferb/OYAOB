using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class ListenerSelector : IListenerSelector
    {
        public IList<TcpClient> SelectForReading(ICollection<TcpClient> tcpClients)
        {
            var readyLst = new List<TcpClient>(from client in tcpClients
                                                     where client.Connected && client.Available > 0
                                                     select client);
            return readyLst;
        }
    }
}