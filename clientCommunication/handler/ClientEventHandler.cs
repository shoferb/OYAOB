using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;
using TexasHoldemShared.CommMessages.CommunicationMessage;

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
          if(msg.UserId==this._userId)
            {
               TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] Options = msg.Options;
               //TODO: show to GUI the options
               TexasHoldemShared.CommMessages.CommunicationMessage.ActionType ChosenOption;//TODO: get shosen from GUI
               int amount=0;////TODO: get chosen from GUI
              int roomId = msg.roomId;
             
               ActionCommMessage response = new ActionCommMessage(_userId, ChosenOption, amount,roomId );
              //add msg to queue to send

            }
            else
            {
                Console.WriteLine("got message that lost it's way");
            }
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
