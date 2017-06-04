using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.GuiScreen;
using Client.Handler;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using System.Windows.Forms;

namespace Client.Logic
{
    public class ClientLogic
    {
        
        private ClientEventHandler _eventHandler;
        private ClientCommunicationHandler _handler;
        //first bool = is response received, second bool = is succeeded
        public List<Tuple<CommunicationMessage, bool, bool,ResponeCommMessage>> MessagesSentObserver =  new List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>>(); 
        private readonly Object listLock;
        public List<GameScreen> _games { get; }
        private long _sessionId = -1;
        public ClientUser user { get; set; }

        public ClientLogic()
        {
            _games = new List<GameScreen>();
            listLock = new Object();
            //todo - find server name
            user = null;
        }

        public long GetSessionId()
        {
            return _sessionId;
        }

        public bool SetSessionId(long sid)
        {
            _sessionId = sid;
            return true;
            //if (sid != -1)
            //{
            //}
            return false;
        }

        //TODO Add specs
        public void AddNewRoom(GameScreen newWin)
        {
            _games.Add(newWin);
        }

        public void GameUpdateReceived(GameDataCommMessage msg)
        {
            bool isNewGame = true;
            foreach (GameScreen game in _games)
            {
                if (game.RoomId == msg.RoomId)
                {
                    isNewGame = false;
                    game.UpdateGame(msg);
                }
            }
        }

        public bool SpectateRoom(int roomId)
        {
            ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.Spectate, -1, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            return toRet;

        }
       
        //needed to be call after create new ClientEventHandler and a new client logic
        public void Init(ClientEventHandler eventHandler, ClientCommunicationHandler handler)
        {
            _eventHandler = eventHandler;
            _handler = handler;
        }

        public void CloseSystem()
        {
            _eventHandler.Close();
            _handler.Close();
        }

        public bool EditDetails(EditCommMessage.EditField field, string value)
        {

            EditCommMessage toSend = new EditCommMessage(user.id, _sessionId, field, value);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            MessagesSentObserver.Remove(messageToList);
            return toRet;
        }

        public JoinResponseCommMessage JoinTheGame(int roomId, int startingChip)
        {
            ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.Join, startingChip, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            if (toRet)
            {
               
                        JoinResponseCommMessage res = (JoinResponseCommMessage)(MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                        return res;
                    
                

                
            }
            return null;
        }

        public GameDataCommMessage CreateNewRoom(GameMode mode, int minBet, int chipPol, int buyInPol, bool canSpec, int minPlayers, int maxPlayers)
        {//should ret int as the roomNumber
            CreateNewRoomMessage toSend = new CreateNewRoomMessage(user.id, _sessionId, mode, minBet, chipPol, buyInPol, canSpec, minPlayers, maxPlayers);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool isSuccessful = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            CreateNewGameResponse res = (CreateNewGameResponse)(MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
            GameDataCommMessage newRoom;
            if(isSuccessful)
            {
                newRoom = res.GameData;
            }
            else
            {
                newRoom = null;
            }
            MessagesSentObserver.Remove(messageToList);
            return newRoom;
        }

        public bool LeaveTheGame(int roomId)
        {
            ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.Leave, -1, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            MessagesSentObserver.Remove(messageToList);
            return toRet;

        }

        public bool SendChatMsg(int _roomId, string _ReciverUsername, string _msgToSend, CommunicationMessage.ActionType _chatType)
        {
            ChatCommMessage toSend = new ChatCommMessage(user.id, _roomId, _sessionId,  _ReciverUsername,  _msgToSend, _chatType);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id)); MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            MessagesSentObserver.Remove(messageToList);
            return toRet;
        }

        public bool StartTheGame(int roomId)
        {
            ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.StartGame, -1, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id)); MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            MessagesSentObserver.Remove(messageToList);
            return toRet;
        }
       
        public bool Login(string userName, string password)
        {
            LoginCommMessage toSend = new LoginCommMessage(-1, true, userName, password);
            var messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, 
                false, false, new ResponeCommMessage(-1));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while (MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item3;
            
            if (toRet)
            {
                LoginResponeCommMessage rmsg = (LoginResponeCommMessage) MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item4;
                ClientUser cuser = new ClientUser(rmsg.UserId, rmsg.Name, rmsg.Username,
                    rmsg.Password, rmsg.Avatar, rmsg.Money, rmsg.Email, rmsg.Leauge);
                _sessionId = rmsg.SessionId;
                user = cuser;
               
            }
            MessagesSentObserver.Remove(messageToList);
            return toRet;

        }

        public string AskForReplays(int roomId)
        {
            ReplayCommMessage toSend = new ReplayCommMessage(user.id, _sessionId, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id)); MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            List<string> replays = new List<string>();
            if (toRet)
            {
                ReplaySearchResponseCommMessage retMsg = (ReplaySearchResponseCommMessage)(MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;

                return retMsg.Replay;
            }
            MessagesSentObserver.Remove(messageToList);
            return null;

        }

        public bool Logout(string userName, string password)
        {
            LoginCommMessage toSend = new LoginCommMessage(user.id, false, userName, password);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id)); MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while (MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item3;
            MessagesSentObserver.Remove(messageToList);
            return toRet;

        }

        public List<ClientGame> SearchGame(int userId, SearchCommMessage.SearchType _searchType, string _searchByString, int _searchByInt, GameMode _searchByGameMode)
        {
            List<ClientGame> toReturn = new List<ClientGame>();
            SearchCommMessage toSend = new SearchCommMessage(userId, _sessionId,  _searchType, _searchByString, _searchByInt,_searchByGameMode);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(user.id)); MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;

            if (toRet)
            {
                
                SearchResponseCommMessage rmsg = (SearchResponseCommMessage)(MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                toReturn = rmsg.Games;
            }
            MessagesSentObserver.Remove(messageToList);
            return toReturn;

        }

        public bool Register(int id,string name, string memberName, string password, int money, string email)
        {
            RegisterCommMessage toSend = new RegisterCommMessage(id, name, memberName, password, money, email);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false, new ResponeCommMessage(-1));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while (MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool toRet = MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item3;
          
            if (toRet)
            {
                RegisterResponeCommMessage rmsg = (RegisterResponeCommMessage)(MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
                ClientUser cuser = new ClientUser(rmsg.UserId, rmsg.Name, rmsg.Username,
                    rmsg.Password, rmsg.Avatar, rmsg.Money, rmsg.Email, rmsg.Leauge);
                user = cuser;
                _sessionId = rmsg.SessionId;
            }
            MessagesSentObserver.Remove(messageToList);
            return toRet;
        }

        public void GotMsg(ChatResponceCommMessage msg)
        {
            foreach(GameScreen game in _games)
            {
                if(game.RoomId==msg.roomId)
                {
                    game.AddChatMsg(msg);
                }
            }
        }
       
        public bool NotifyChosenMove(CommunicationMessage.ActionType move, int amount, int roomId)
        {
            if (move.Equals(CommunicationMessage.ActionType.Fold))
                {
                    amount = -1;//amount isnt relevant
                    ActionCommMessage response = new ActionCommMessage(user.id, _sessionId, move, amount, roomId);
                    Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(response, false, false, new ResponeCommMessage(user.id));
                    MessagesSentObserver.Add(messageToList);
                    _eventHandler.SendNewEvent(response);
                    while ((MessagesSentObserver.Find(x => x.Item1.Equals(response))).Item2 == false)
                    {
                        var t = Task.Run(async delegate { await Task.Delay(10); });
                        t.Wait();
                    }
                    bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(response))).Item3;
                    MessagesSentObserver.Remove(messageToList);
                    return toRet;
                }
            if ((move.Equals(CommunicationMessage.ActionType.Bet)) && (amount >= 0))
            {
                ActionCommMessage response = new ActionCommMessage(user.id, _sessionId, move, amount, roomId);
                Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(response, false, false, new ResponeCommMessage(user.id));
                MessagesSentObserver.Add(messageToList);
                _eventHandler.SendNewEvent(response);
                while ((MessagesSentObserver.Find(x => x.Item1.Equals(response))).Item2 == false)
                {
                    var t = Task.Run(async delegate { await Task.Delay(10); });
                    t.Wait();
                }
                bool toRet = (MessagesSentObserver.Find(x => x.Item1.Equals(response))).Item3;
                MessagesSentObserver.Remove(messageToList);
                return toRet;
            }
            return false;
        }

        public void NotifyResponseReceived(ResponeCommMessage msg)
        {
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit = MessagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            MessagesSentObserver.Remove(toEdit);
            var toAdd = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toEdit.Item1, true, msg.Success, msg);
            MessagesSentObserver.Add(toAdd);

        }
    }
}
