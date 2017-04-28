using System.Net.Sockets;

namespace TexasHoldem.communication.Reactor.Interfaces
{
    interface IEventHandler
    {
        void HandleEvent(byte[] data);
        TcpListener GetHandler();
    }
}
