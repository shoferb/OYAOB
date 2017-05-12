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
        private parserImplementation XmlParser;
        private bool _shouldClose;

        public ClientEventHandler(CommunicationHandler handler)
        {
            _handler = handler;
            XmlParser = new parserImplementation();
            _shouldClose = false;

        }
        //needed to be call after create new ClientEventHandler and a new client logic
        public void init(ClientLogic logic)
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
                msg = _handler.tryGetMsgReceived();
                var parsedMsg = XmlParser.ParseString(msg);
                parsedMsg.Handle(this);
            }
        }

        public void SendNewEvent(CommunicationMessage msg)
        {
            string parsedMsg = XmlParser.SerializeMsg(msg);
            _handler.addMsgToSend(parsedMsg);
        }
        public void HandleEvent(ActionCommMessage msg)
        {
            //todo change to log
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(EditCommMessage msg)
        {
            //todo change to log
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            //todo change to log
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            //todo change to log
            Console.WriteLine("ActionCommMessage is client to server message");
        }

        public void HandleEvent(SearchCommMessage msg)
        {
            //todo change to log
            Console.WriteLine("SearchCommMessage is client to server message");
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            //show in GUI
            throw new NotImplementedException();
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            _logic.NotifyResponseReceived(msg);

        }
        public void Start()
        {
            Task task = new Task(handleMessages);
            task.Start();
        }

        public void HandleEvent(CreatrNewRoomMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
