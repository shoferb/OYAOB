using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class MessageEventHandler : IEventHandler
    {
        private readonly TcpListener _listener;

        public MessageEventHandler(IPAddress ipAddress, int port)
        {
            _listener = new TcpListener(ipAddress, port);
        }

        public void HandleEvent(byte[] data)
        {
            //string message = Encoding.UTF8.GetString(data);
            //TODO: fill this up
        }

        public TcpListener GetHandler()
        {
            return _listener;
        }
    }
}