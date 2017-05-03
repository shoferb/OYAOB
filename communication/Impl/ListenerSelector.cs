using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Impl
{
    public class ListenerSelector : IListenerSelector
    {
        public IList<TcpClient> SelectForReading(IEnumerable<TcpClient> tcpClients)
        {
            var readyLst = new List<TcpClient>(from client in tcpClients 
                                               where client.Connected && client.Available > 0
                                                     select client);
            return readyLst;
        }

        public IList<TcpClient> SelectForWriting(IEnumerable<TcpClient> tcpClients)
        {
            var readyLst = new List<TcpClient>(from client in tcpClients 
                                               where client.Connected && client.GetStream().CanWrite
                                               select client);
            return readyLst;
        }
    }
}