using System.Collections.Generic;
using System.Net.Sockets;

namespace TexasHoldem.communication.Reactor.Interfaces
{
    public interface IListenerSelector
    {
        IList<TcpClient> SelectForReading(IEnumerable<TcpClient> tcpClients);
        IList<TcpClient> SelectForWriting(IEnumerable<TcpClient> tcpClients);
    }
}
