using clientCommunication.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace clientCommunication.Logic
{
    class ClientLogic
    {
        private readonly int _userId;
        private ClientEventHandler _eventHandler;
        private communicationHandler _handler;

        public ClientLogic(int userId)
        {
            _userId = userId;
        }

        //needed to be call after create new ClientEventHandler and a new client logic
        public void init(ClientEventHandler eventHandler, communicationHandler handler)
        {
            _eventHandler = eventHandler;
            _handler = handler;

        }
        public void closeSystem()
        {
            _eventHandler.close();
            _handler.close();
        }
        public void editDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField field, string value)
        {
            //checkifAnyConstraints?
            EditCommMessage toSend = new EditCommMessage(_userId, field, value);
            _eventHandler.SendNewEvent(toSend);
        }
         public void joinTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Join, -1, roomId);
             _eventHandler.SendNewEvent(toSend);
             
         }

         public void leaveTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Leave, -1, roomId);
             _eventHandler.SendNewEvent(toSend);
         }
         public void startTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.StartGame, -1, roomId);
             _eventHandler.SendNewEvent(toSend);
         }

         public void login(string userName, string password)
         {
             LoginCommMessage toSend = new LoginCommMessage(_userId, true, userName, password);
             _eventHandler.SendNewEvent(toSend);
         }
         public void logout(string userName, string password)
         {
             LoginCommMessage toSend = new LoginCommMessage(_userId, false, userName, password);
             _eventHandler.SendNewEvent(toSend);
         }

         public void register(string name, string memberName, string password, int money, string email)
         {
             RegisterCommMessage toSend = new RegisterCommMessage(_userId, name, memberName, password, money,email);
             _eventHandler.SendNewEvent(toSend);
         }




       public void showOptionsMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options, int roomId)
       
        {
            //GUI
            //after Chosen: Call /notifyChosenMove(options, chosenMove);
          
            
        }//after client chose call notifyChosenMove
        public Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int> notifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType move, int amount, int roomId)
        {
            bool legalMove = false;
            foreach(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType action in options)
            {
                if (action.Equals(move))
                {
                    legalMove = true;
                }
            }
            if (legalMove)
            {
                if (move.Equals(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Fold))
                {
                    amount = -1;//amount isnt relevant
                   
                    _eventHandler.handleChosenAction(move, amount, roomId);
                }
                else if ((move.Equals(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet)) && (amount >= 0))
                {
                    Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int> toRet = new Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int>(move, amount);
                    return toRet;
                }
            }
           
                //appropriate Log
                //notify the client for illegal input
                amount = -2;//illegal answer for client
                Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int> ret = new Tuple<TexasHoldemShared.CommMessages.CommunicationMessage.ActionType, int>(move, amount);
                return ret;
        }
    }
}
