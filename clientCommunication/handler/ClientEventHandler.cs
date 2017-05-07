using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;


using clientCommunication.Logic;
using TexasHoldemShared.CommMessages;

namespace clientCommunication.handler
{
    class ClientEventHandler : IEventHandler
    {
        private readonly ICommMsgXmlParser _parser;//init
        private readonly int _userId;
        private readonly communicationHandler _handler;
        private bool shouldClose;
        private ClienLogic _logic;
        

        public ClientEventHandler(int id,communicationHandler handler) 
        {
            _userId = id;
            _handler = handler;
            shouldClose = false;
           


        }
        //needed to be call after create new ClientEventHandler and a new client logic
        public void init(ClienLogic logic){
            _logic = logic;
        }
        //TODO
        public void handleMessages()
        {
            while (!shouldClose)
            {
                string msg = string.Empty;
                msg= _handler.tryGetMsgReceived();
                //parsedMsg = parse msg
                //parsedMsg.Handle(this);
                

            }


        }

        public void SendNewEvent(CommunicationMessage msg)
        {
            //parse msg
            //_handler.addMsgToSend(parsedMsg);
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
            //show in GUI
            throw new NotImplementedException();
        }

        public void HandleEvent(MoveOptionsCommMessage msg)
        {
            if (msg.UserId == this._userId)
            {
                TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options = msg.Options;
                int roomId = msg.roomId;
                Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int> gotBack = _logic.showOptionsMove(options, roomId);
                TexasHoldemShared.CommMessages.CommunicationMessage.ActionType ChosenOption = gotBack.Item1;
                int amount = gotBack.Item2;
                
                if (amount > -1)
                {
                    ActionCommMessage response = new ActionCommMessage(_userId, ChosenOption, amount, roomId);
                    //parse response
                    //_handler.addMsgToSend(parsedResponse)
                }
            }
            else
            {
                Console.WriteLine("got message that lost it's way");
            }
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            //notify in GUI
            throw new NotImplementedException();
        }
    }
}
