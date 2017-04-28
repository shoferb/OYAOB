using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.communication.Reactor.Interfaces
{
    public interface IListenerSelector
    {
        IList<TcpListener> Select(ICollection<TcpListener> listeners);
    }
}
