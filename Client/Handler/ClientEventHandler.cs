using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.Parser;
using Client.Logic;

namespace Client.Handler
{
    public class ClientEventHandler : IEventHandler
    {

        private int _userId;
        private readonly CommunicationHandler _handler;
        private ClientLogic _logic;
        private ParserImplementation XmlParser;
        private bool _shouldClose;

        public ClientEventHandler(CommunicationHandler handler)
        {
            _handler = handler;
            XmlParser = new ParserImplementation();
            _shouldClose = false;

        }
        //needed to be call after create new ClientEventHandler and a new client logic
        public void Init(ClientLogic logic)
        {
            _logic = logic;
        }
        public void SetNewUserId(int newId)
        {
            this._userId = newId;
        }
        public void close()
        {
            _shouldClose = true;

        }
        public void handleMessages()
        {
            while (!_shouldClose)
            {
                string msg = string.Empty;
                msg = _handler.TryGetMsgReceived();
                if (msg != null)
                {
                    var parsedMsg = XmlParser.ParseString(msg);
                    parsedMsg.Handle(this); 
                }
            }
        }

        public void SendNewEvent(CommunicationMessage msg)
        {
            string parsedMsg = XmlParser.SerializeMsg(msg);
            _handler.addMsgToSend(parsedMsg);
        }
        public void HandleEvent(ActionCommMessage msg)
        {
           
        }

        public void HandleEvent(EditCommMessage msg)
        {
            
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
           
        }

        public void HandleEvent(SearchCommMessage msg)
        {
           
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            _logic.GameUpdateReceived(msg);
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            if (msg.GetType().Equals(typeof(ChatResponceCommMessage)))
            {
                _logic.gotMsg((ChatResponceCommMessage)msg);
            }
            else
            {
                _logic.NotifyResponseReceived(msg);
            }

        }
        public void Start()
        {
            Task task = new Task(handleMessages);
            task.Start();
        }

      
        public void HandleEvent(CreatrNewRoomMessage msg)
        {
          //Client to server msg
        }

      
        public void HandleEvent(ChatCommMessage msg)
        {
            //Client to server msg
        }
      

    }
}
