using Client.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.Logic
{
    public class ClientLogic
    {
        private int _userId;
        private ClientEventHandler _eventHandler;
        private CommunicationHandler _handler;
        public List<Tuple<CommunicationMessage, bool, bool,ResponeCommMessage>> messagesSentObserver =  new List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>>(); //first bool = is response received, second bool = is succeeded
        private readonly Object listLock;

        public ClientUser user { get; set; }
        //chanfajf
        public ClientLogic()
        {
            
            listLock = new Object();
            //todo - find server name
            _handler = new CommunicationHandler("TODO");
            _eventHandler = new ClientEventHandler(_handler);
            user = null;
        }
        public bool SetUserId(int newId)
        {
            _userId = newId;
            this._eventHandler.SetNewUserId(newId);
            this._handler.setUserId(newId);
            return true;
        }
        //needed to be call after create new ClientEventHandler and a new client logic
        public void Init(ClientEventHandler eventHandler, CommunicationHandler handler)
        {
            _eventHandler = eventHandler;
            _handler = handler;
        }

        public void CloseSystem()
        {
            _eventHandler.close();
            _handler.close();
        }
        public bool EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField field, string value)
        {

            EditCommMessage toSend = new EditCommMessage(_userId, field, value);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId));
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
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId));
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

        public int CreateNewRoom(GameMode mode, int minBet, int chipPol, int buyInPol, bool canSpec, int minPlayers, int maxPlayers)
        {//should ret int as the roomNumber
            CreatrNewRoomMessage toSend = new CreatrNewRoomMessage(_userId, mode, minBet, chipPol, buyInPol, canSpec, minPlayers, maxPlayers);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId));
            messagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(1000); });
                t.Wait();
            }
            bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            if(toRet)
            {
                //get room number from response
               //get gameData msg
            }
            messagesSentObserver.Remove(messageToList);
            return 1;
        }

        public bool leaveTheGame(int roomId)
        {
            ActionCommMessage toSend = new ActionCommMessage(_userId, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Leave, -1, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId));
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
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId)); messagesSentObserver.Add(messageToList);
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
       

        public bool Login(string userName, string password)//check with oded
        {
            LoginCommMessage toSend = new LoginCommMessage(_userId, true, userName, password);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId)); messagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(1000); });
                t.Wait();
            }
            bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            
            if (toRet)
            {
                LoginResponeCommMessage rmsg = (LoginResponeCommMessage)(messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                ClientUser cuser = new ClientUser(rmsg.UserId, rmsg.name, rmsg.username,
                    rmsg.password, rmsg.avatar, rmsg.money, rmsg.email, rmsg.leauge);
                this.user = cuser;
                this._userId = user.id;
            }
            messagesSentObserver.Remove(messageToList);
            return toRet;

        }
        public bool Logout(string userName, string password)
        {
            LoginCommMessage toSend = new LoginCommMessage(_userId, false, userName, password);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId)); messagesSentObserver.Add(messageToList);
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

        public List<ClientGame> SearchGame(int userId, SearchCommMessage.SearchType _searchType, string _searchByString, int _searchByInt, GameMode _searchByGameMode)
        {
            List<ClientGame> toReturn = new List<ClientGame>();
            SearchCommMessage toSend = new SearchCommMessage(userId,  _searchType, _searchByString, _searchByInt,_searchByGameMode);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId)); messagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(1000); });
                t.Wait();
            }
            bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;

            if (toRet)
            {
                
                SearchResponseCommMessage rmsg = (SearchResponseCommMessage)(messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                toReturn = rmsg.games;
            }
            messagesSentObserver.Remove(messageToList);
            return toReturn;

        }

        public bool Register(string name, string memberName, string password, int money, string email)
        {
            RegisterCommMessage toSend = new RegisterCommMessage(_userId, name, memberName, password, money, email);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(_userId));
            messagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(1000); });
                t.Wait();
            }
            bool toRet = (messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
          
            if (toRet)
            {
                RegisterResponeCommMessage rmsg = (RegisterResponeCommMessage)(messagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                ClientUser cuser = new ClientUser(rmsg.UserId, rmsg.name, rmsg.username,
                    rmsg.password, rmsg.avatar, rmsg.money, rmsg.email, rmsg.leauge);
                this.user = cuser;
                this._userId = user.id;
            }
            messagesSentObserver.Remove(messageToList);
            return toRet;
        }


        public bool notifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType[] options, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType move, int amount, int roomId)
        {
            bool legalMove = false;
            foreach (TexasHoldemShared.CommMessages.CommunicationMessage.ActionType action in options)
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
                    Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(response, false, false, new ResponeCommMessage(_userId));
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
                else if ((move.Equals(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet)) && (amount >= 0))
                {
                    ActionCommMessage response = new ActionCommMessage(_userId, move, amount, roomId);
                    Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(response, false, false, new ResponeCommMessage(_userId));
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

        public void NotifyResponseReceived(ResponeCommMessage msg)
        {
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit = messagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            messagesSentObserver.Remove(toEdit);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toAdd =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toEdit.Item1, true, msg.Success, msg);
            messagesSentObserver.Add(toAdd);

        }
    }
}
