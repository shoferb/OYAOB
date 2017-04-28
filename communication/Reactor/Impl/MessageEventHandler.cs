using System.Net;
using System.Net.Sockets;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldem.communication.Reactor.Impl
{
    public class MessageEventHandler : IEventHandler
    {
        private readonly Socket _socket;

        public MessageEventHandler(/*IPAddress ipAddress, int port*/ Socket socket)
        {
            //_socket = new Socket(ipAddress, port);
            _socket = socket;
        }

        public void HandleEvent(byte[] data)
        {
            //string message = Encoding.UTF8.GetString(data);
            //TODO: fill this up
        }

        public Socket GetHandler()
        {
            return _socket;
        }
    }
}