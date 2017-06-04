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
        private readonly ClientCommunicationHandler _handler;
        private ClientLogic _logic;
        private readonly ParserImplementation _xmlParser;
        private bool _shouldClose;

        public ClientEventHandler(ClientCommunicationHandler handler)
        {
            _handler = handler;
            _xmlParser = new ParserImplementation();
            _shouldClose = false;

        }

        //needed to be call after create new ClientEventHandler and a new client logic
        public void Init(ClientLogic logic)
        {
            _logic = logic;
        }
     
        public void Close()
        {
            _shouldClose = true;

        }

        private bool CheckSessionId(long sidToCheck)
        {
            //return sidToCheck != -1 && sidToCheck == _logic.GetSessionId();
            return true;
        }

        private void GotClientToServerMsg(CommunicationMessage msg)
        {
            //handle error here
        }

        public void HandleMessages()
        {
            while (!_shouldClose)
            {
                string msg = string.Empty;
                msg = _handler.TryGetMsgReceived();
                if (msg != null)
                {
                    var parsedMsg = _xmlParser.ParseString(msg, true);
                    parsedMsg.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Handle(this);
                        }
                    });
                }
            }
        }

        public void SendNewEvent(CommunicationMessage msg)
        {
            string parsedMsg = _xmlParser.SerializeMsg(msg, true);
            _handler.addMsgToSend(parsedMsg);
        }

        public string HandleEvent(ActionCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(EditCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(LoginCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(RegisterCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(SearchCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(LeaderboardCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(GameDataCommMessage msg)
        {
            if (CheckSessionId(msg.SessionId))
            {
                _logic.GameUpdateReceived(msg); 
            }
            return "";
        }

        public string HandleEvent(ResponeCommMessage msg)
        {
            if (SetSetionId(msg.SessionId))
            {
                if (msg.GetType() == typeof(ChatResponceCommMessage))
                {
                    _logic.GotMsg((ChatResponceCommMessage)msg);
                }
                else
                {
                    _logic.NotifyResponseReceived(msg);
                } 
            }
            return "";

        }

        private bool SetSetionId(long sid)
        {
            return _logic.SetSessionId(sid);
        }

        public void Start()
        {
            Task task = new Task(HandleMessages);
            task.Start();
        }

        public string HandleEvent(CreateNewRoomMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(ChatCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(ReplayCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }

        public string HandleEvent(UserStatisticsCommMessage msg)
        {
            GotClientToServerMsg(msg);
            return "";
        }
    }
}