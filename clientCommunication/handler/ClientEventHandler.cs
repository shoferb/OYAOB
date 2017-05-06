using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace clientCommunication.handler
{
    class ClientEventHandler : IEventHandler
    {
        private readonly ICommMsgXmlParser _parser;//init
        private readonly int _userId;

        public ClientEventHandler(int id) 
        {
            _userId = id;
        }
        public void HandleEvent(ActionCommMessage msg)
        {
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(EditCommMessage msg)
        {
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            throw new NotImplementedException();
        }

        public void HandleEvent(MoveOptionsCommMessage msg)
        {
            throw new NotImplementedException();
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
