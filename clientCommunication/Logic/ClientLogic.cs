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
        private  int _userId;
        private ClientEventHandler _eventHandler;
        private communicationHandler _handler;
        private List<Tuple<CommunicationMessage,bool,bool>> messagesSentObserver; //first bool = is response received, second bool = is succeeded
        private readonly Object listLock;
        //chanfajf
        public ClientLogic()
        {
            messagesSentObserver = new List<Tuple<CommunicationMessage, bool, bool>>();
            listLock = new Object();
        }
         public bool SetUserId(int newId)
        {
            _userId = newId;
            return true;
        }
        //needed to be call after create new ClientEventHandler and a new client logic
        public void Init(ClientEventHandler eventHandler, communicationHandler handler)
        {
            _eventHandler = eventHandler;
            _handler = handler;

        }
        public void CloseSystem()
        {
            _eventHandler.close();
            _handler.close();
        }
        public bool editDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField field, string value)
        {

            EditCommMessage toSend = new EditCommMessage(_userId, field, value);
            Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
            messagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(1000); });
                t.Wait();
            }
            bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            messagesSentObserver.Remove(messageToList);
            return toRet;
        }
         public bool joinTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Join, -1, roomId);
             Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
             messagesSentObserver.Add(messageToList);
             _eventHandler.SendNewEvent(toSend);
             while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
             {
                 var t = Task.Run(async delegate { await Task.Delay(1000); });
                 t.Wait();
             }
             bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
             messagesSentObserver.Remove(messageToList);
             return toRet;

             
         }

         public bool leaveTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Leave, -1, roomId);
             Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
             messagesSentObserver.Add(messageToList);
             _eventHandler.SendNewEvent(toSend);
             while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
             {
                 var t = Task.Run(async delegate { await Task.Delay(1000); });
                 t.Wait();
             }
             bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
             messagesSentObserver.Remove(messageToList);
             return toRet;

         }
         public bool startTheGame(int roomId)
         {
             ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.StartGame, -1, roomId);
             Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
             messagesSentObserver.Add(messageToList);
             _eventHandler.SendNewEvent(toSend);
             while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
             {
                 var t = Task.Run(async delegate { await Task.Delay(1000); });
                 t.Wait();
             }
             bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
             messagesSentObserver.Remove(messageToList);
             return toRet;
         }

         public bool login(string userName, string password)//check with oded
         {
             LoginCommMessage toSend = new LoginCommMessage(_userId, true, userName, password);
             Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
             messagesSentObserver.Add(messageToList);
             _eventHandler.SendNewEvent(toSend);
             while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
             {
                 var t = Task.Run(async delegate { await Task.Delay(1000); });
                 t.Wait();
             }
             bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
             messagesSentObserver.Remove(messageToList);
             return toRet;

         }
         public bool logout(string userName, string password)
         {
             LoginCommMessage toSend = new LoginCommMessage(_userId, false, userName, password);
             Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(toSend, false, false);
             messagesSentObserver.Add(messageToList);
             _eventHandler.SendNewEvent(toSend);
             while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
             {
                 var t = Task.Run(async delegate { await Task.Delay(1000); });
                 t.Wait();
             }
             bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
             messagesSentObserver.Remove(messageToList);
             return toRet;

         }

         public bool register(string name, string memberName, string password, int money, string email)
         {
             RegisterCommMessage toSend = new RegisterCommMessage(_userId, name, memberName, password, money,email);
             messagesSentObserver.Add(new Tuple<CommunicationMessage, bool, bool>(toSend, false, false));
             _eventHandler.SendNewEvent(toSend);
             return true;

         }




       public void showOptionsMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options, int roomId)
       
        {
            //GUI
            //after Chosen: Call /notifyChosenMove(options, chosenMove);
          
            
        }//after client chose call notifyChosenMove
        public bool notifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType move, int amount, int roomId)
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
                    ActionCommMessage response = new ActionCommMessage(_userId, move, amount, roomId);
                    Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(response, false, false);
                    messagesSentObserver.Add(messageToList);
                    _eventHandler.SendNewEvent(response);
                    while((messagesSentObserver.Find(x => x.Item1.Equals(response))).Item2==false)
                    {
                         var t = Task.Run(async delegate{ await Task.Delay(1000);});
                        t.Wait();
                    }
                    bool toRet =  (messagesSentObserver.Find(x => x.Item1.Equals(response))).Item3;
                    messagesSentObserver.Remove(messageToList);
                    return toRet;
                }
                else if ((move.Equals(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet)) && (amount >= 0))
                {
                    ActionCommMessage response = new ActionCommMessage(_userId, move, amount, roomId);
                    Tuple<CommunicationMessage, bool, bool> messageToList = new Tuple<CommunicationMessage, bool, bool>(response, false, false);
                    messagesSentObserver.Add(messageToList);
                    _eventHandler.SendNewEvent(response);
                    while ((messagesSentObserver.Find(x => x.Item1.Equals(response))).Item2 == false)
                    {
                        var t = Task.Run(async delegate { await Task.Delay(1000); });
                        t.Wait();
                    }
                    bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(response))).Item3;
                    messagesSentObserver.Remove(messageToList);
                    return toRet;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }
       
    }
}
