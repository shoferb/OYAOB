using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.GuiScreen;
using Client.Handler;
using TexasHoldem.GuiScreen;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.Logic
{
    public class ClientLogic
    {

        private ClientEventHandler _eventHandler;

        private ClientCommunicationHandler _handler;

        //first bool = is response received, second bool = is succeeded
        public List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>> MessagesSentObserver =
            new List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>>();
        
        private readonly Object listLock;
        public List<GameScreen> _games { get; }
        private long _sessionId = -1;
        public ClientUser user { get; set; }
        private SearchScreen _searchScreen;
        private ReturnToGames _returnToGamesScreen;
        private LoginScreen _loginScreen;
        private ILogoutScreen _logoutScreen;
        private ISearchScreen _currSearchScreen = null;
        private Window _currWindow = null;
        private Dictionary<Type, Func<ResponeCommMessage, bool>> _notifyDictionary;
        private readonly IResponseNotifier _notifier;

        public ClientLogic()
        {
            _games = new List<GameScreen>();
            listLock = new Object();
            user = null;
            _notifier = new ResponseNotifier(MessagesSentObserver, this);
            SetupNotifyDictionary();
        }

        private void SetupNotifyDictionary()
        {
            _notifyDictionary = new Dictionary<Type, Func<ResponeCommMessage, bool>>
            {
                {typeof(ChatResponceCommMessage), _notifier.NotifyChat},
                {typeof(RegisterCommMessage), _notifier.ObserverNotify},
                {typeof(CreateNewRoomMessage), _notifier.ObserverNotify},
                {typeof(EditCommMessage), _notifier.ObserverNotify},
                {typeof(ReturnToGameAsPlayerCommMsg), _notifier.NotifyReturnAsPlayer},
                {typeof(ReturnToGameAsSpecCommMsg), _notifier.NotifyReturnAsSpec},
                {typeof(SearchCommMessage), _notifier.NotifySearch},
                {typeof(ActionCommMessage), _notifier.NotifyAction},
                {typeof(ChatCommMessage), _notifier.NotifyChat},
                {typeof(ReplayCommMessage), _notifier.ObserverNotify},
                {typeof(LoginCommMessage), _notifier.NotifyLogin}
            };
        }

        public SearchScreen GetSearchScreen()
        {
            return _searchScreen;
        }

        public void SetSearchScreen(SearchScreen screen)
        {
            _searchScreen = screen;
        }

        public void SetLogoutScreen(ILogoutScreen screen)
        {
            _logoutScreen = screen;
        }

        public void SetCurrSearchScreen(ISearchScreen screen)
        {
            _currSearchScreen = screen;
        }

        public void SetLoginScreen(LoginScreen screen)
        {
            _loginScreen = screen;
        }

        public bool LoginRespReceived(LoginResponeCommMessage loginResp)
        {
            ClientUser cuser = new ClientUser(loginResp.UserId, loginResp.Name, loginResp.Username,
                loginResp.Password, loginResp.Avatar, loginResp.Money, loginResp.Email, loginResp.Leauge);
            user = cuser;
            _loginScreen.LoginOk(loginResp.Success);
            return loginResp.Success;
        }

        public bool LogoutRespReceived(LoginResponeCommMessage logoutResp)
        {
            if (_logoutScreen != null)
            {
                return _logoutScreen.LogoutOk(logoutResp.Success);
            }
            return false;
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

        public void AddNewRoom(GameScreen newWin)
        {
            _games.Add(newWin);
        }

        public void GameUpdateReceived(GameDataCommMessage msg)
        {
            foreach (GameScreen game in _games)
            {
                if (game.RoomId == msg.RoomId)
                {
                    game.UpdateGame(msg);
                }
            }
        }

        public void SpectateRoom(int roomId)
        {
            ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.Spectate,
               -1, roomId);
            _eventHandler.SendNewEvent(toSend);
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
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false,
                    new ResponeCommMessage(user.id));
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

        public void JoinTheGame(int roomId, int startingChip)
        {
             ActionCommMessage toSend = new ActionCommMessage(user.id, _sessionId,
                    CommunicationMessage.ActionType.Join, startingChip, roomId);
            _eventHandler.SendNewEvent(toSend);
        }

        public void ReturnGamePlayer(int roomId)
        {
            ReturnToGameAsPlayerCommMsg toSend = new ReturnToGameAsPlayerCommMsg(user.id, _sessionId, roomId);
            _eventHandler.SendNewEvent(toSend);
        }

        public void ReturnGameSpec(int roomId)
        {
            ReturnToGameAsSpecCommMsg toSend = new ReturnToGameAsSpecCommMsg(user.id, _sessionId,roomId);
            _eventHandler.SendNewEvent(toSend);
        }

        public GameDataCommMessage CreateNewRoom(GameMode mode, int minBet, int chipPol, int buyInPol, bool canSpec,
            int minPlayers, int maxPlayers)
        {
            //should ret int as the roomNumber
            CreateNewRoomMessage toSend = new CreateNewRoomMessage(user.id, _sessionId, mode, minBet, chipPol, buyInPol,
                canSpec, minPlayers, maxPlayers);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false,
                    new ResponeCommMessage(user.id));
            MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            while ((MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item2 == false)
            {
                var t = Task.Run(async delegate { await Task.Delay(10); });
                t.Wait();
            }
            bool isSuccessful = (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item3;
            CreateNewGameResponse res =
                (CreateNewGameResponse) (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
            GameDataCommMessage newRoom;
            if (isSuccessful)
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

        public void LeaveTheGame(int roomId)
        {
            ActionCommMessage toSend =
                new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.Leave, -1, roomId);
          
           _eventHandler.SendNewEvent(toSend);
           
        }

        public void SpectetorLeaveTheGame(int roomId)
        {
            ActionCommMessage toSend =
                new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.SpectatorLeave, -1, roomId);

            _eventHandler.SendNewEvent(toSend);

        }

        public bool SendChatMsg(int _roomId, string _ReciverUsername, string _msgToSend,
            CommunicationMessage.ActionType _chatType)
        {
            ChatCommMessage toSend = new ChatCommMessage(user.id, _roomId, _sessionId, _ReciverUsername, _msgToSend,
                _chatType);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false,
                    new ResponeCommMessage(user.id));
            _eventHandler.SendNewEvent(toSend);    
            return true;
        }

        public void StartTheGame(int roomId)
        {
            ActionCommMessage toSend =
                new ActionCommMessage(user.id, _sessionId, CommunicationMessage.ActionType.StartGame, -1, roomId);
            _eventHandler.SendNewEvent(toSend);
        }

        private int GenerateRandomNegNum()
        {
            int rand = new Random().Next();
            return rand * -1;
        }

        public void Login(string userName, string password)
        {
            int randNedId = GenerateRandomNegNum();
            LoginCommMessage toSend = new LoginCommMessage(randNedId, true, userName, password);
            //var messageToList = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend,
            //    false, false, new ResponeCommMessage(-1));
            //MessagesSentObserver.Add(messageToList);
            _eventHandler.SendNewEvent(toSend);
            //while (MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item2 == false)
            //{
            //    var t = Task.Run(async delegate { await Task.Delay(10); });
            //    t.Wait();
            //}
            //bool toRet = MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item3;

            //if (toRet)
            //{
            //    LoginResponeCommMessage rmsg =
            //        (LoginResponeCommMessage) MessagesSentObserver.Find(x => x.Item1.Equals(toSend)).Item4;
            //    ClientUser cuser = new ClientUser(rmsg.UserId, rmsg.Name, rmsg.Username,
            //        rmsg.Password, rmsg.Avatar, rmsg.Money, rmsg.Email, rmsg.Leauge);
            //    _sessionId = rmsg.SessionId;
            //    user = cuser;

            //}
            //MessagesSentObserver.Remove(messageToList);

        }

        public string AskForReplays(int roomId)
        {
            ReplayCommMessage toSend = new ReplayCommMessage(user.id, _sessionId, roomId);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false,
                    new ResponeCommMessage(user.id));
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
                ReplaySearchResponseCommMessage retMsg =
                    (ReplaySearchResponseCommMessage) (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;

                return retMsg.Replay;
            }
            MessagesSentObserver.Remove(messageToList);
            return null;

        }

        public void Logout(string userName, string password)
        {
            LoginCommMessage toSend = new LoginCommMessage(user.id, false, userName, password);
            _eventHandler.SendNewEvent(toSend);
        }

        public void SearchGame(int userId, SearchCommMessage.SearchType _searchType, string _searchByString, 
            int _searchByInt, GameMode _searchByGameMode, bool isReturnToGame)
        {
            SearchCommMessage toSend = new SearchCommMessage(userId, _sessionId, _searchType, _searchByString,
                _searchByInt, _searchByGameMode) {IsReturnToGame = isReturnToGame};
            _eventHandler.SendNewEvent(toSend);
        }

        public bool Register(int id, string name, string memberName, string password, int money, string email)
        {
            RegisterCommMessage toSend = new RegisterCommMessage(id, name, memberName, password, money, email);
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> messageToList =
                new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toSend, false, false,
                    new ResponeCommMessage(-1));
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
                RegisterResponeCommMessage rmsg =
                    (RegisterResponeCommMessage) (MessagesSentObserver.Find(x => x.Item1.Equals(toSend))).Item4;
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
            foreach (GameScreen game in _games)
            {
                if (game.RoomId == msg.roomId)
                {
                    game.AddChatMsg(msg);
                }
            }
        }

        public void NotifyChosenMove(CommunicationMessage.ActionType move, int amount, int roomId)
        {
            if (move.Equals(CommunicationMessage.ActionType.Fold))
            {
                ActionCommMessage response = new ActionCommMessage(user.id, _sessionId, move, amount, roomId);
                _eventHandler.SendNewEvent(response);

            }
            if ((move.Equals(CommunicationMessage.ActionType.Bet)) && (amount >= 0))
            {
                ActionCommMessage response = new ActionCommMessage(user.id, _sessionId, move, amount, roomId);
               _eventHandler.SendNewEvent(response);
          
            }
         }

        public void PlayerReturnsToGame(GameDataCommMessage gameData)
        {
           _returnToGamesScreen.PlayerReturnResponseReceived(gameData);
        }

        public void SpecReturnsToGame(GameDataCommMessage gameData)
        {
            _returnToGamesScreen.SpecReturnResponseReceived(gameData);
        }

        public void NotifyResponseReceived(ResponeCommMessage msg)
        {
            if (_notifyDictionary.ContainsKey(msg.OriginalMsg.GetType()))
            {
                var func = _notifyDictionary[msg.OriginalMsg.GetType()];
                func(msg);
            }
            else
            {
                _notifier.Default(msg);
            }

            //var notifier = new ResponseNotifier(MessagesSentObserver, this);
            //msg.Notify(notifier, msg);

            //if (msg.OriginalMsg.GetType() == typeof(ChatCommMessage))
            //{
            //    if (((ChatCommMessage)msg.OriginalMsg).ChatType == CommunicationMessage.ActionType.PlayerWhisper ||
            //     ((ChatCommMessage)msg.OriginalMsg).ChatType == CommunicationMessage.ActionType.SpectetorWhisper)
            //    {
            //        return;
            //    }
            //}
            //if ((msg.OriginalMsg.GetType() == typeof(LoginCommMessage)) ||
            //   (msg.OriginalMsg.GetType()) == typeof(RegisterCommMessage)||
            //      (msg.OriginalMsg.GetType()) == typeof(CreateNewRoomMessage))
            //{
            //    Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit =
            //        MessagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            //    MessagesSentObserver.Remove(toEdit);
            //    var toAdd = new Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>(toEdit.Item1, true,
            //        msg.Success,
            //        msg);
            //    MessagesSentObserver.Add(toAdd);
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType()) == typeof(ReturnToGameAsPlayerCommMsg) && _returnToGamesScreen != null)
            //{
            //    PlayerReturnsToGame(msg.GameData);
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType()) == typeof(ReturnToGameAsPlayerCommMsg) && _returnToGamesScreen != null)
            //{
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType()) == typeof(SearchCommMessage))
            //{
            //    SearchResultRecived(((SearchResponseCommMessage) msg).Games);
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType() == typeof(ActionCommMessage) &&
            //     (((ActionCommMessage)msg.OriginalMsg).MoveType == CommunicationMessage.ActionType.Join)))
            //{
            //    JoinAsPlayerReceived(msg as JoinResponseCommMessage);
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType() == typeof(ActionCommMessage) &&
            //     (((ActionCommMessage)msg.OriginalMsg).MoveType == CommunicationMessage.ActionType.Spectate)))
            //{
            //    JoinAsSpectatorReceived(msg as JoinResponseCommMessage);
            //    return;
            //}
            //if ((msg.OriginalMsg.GetType() == typeof(ActionCommMessage) &&
            //   (((ActionCommMessage)msg.OriginalMsg).MoveType == CommunicationMessage.ActionType.Leave)))
            //{
            //    GameDataCommMessage gd = (msg).GameData;
            //    foreach (GameScreen game in _games)
            //    {
            //        if (game.RoomId == gd.RoomId)
            //        {
            //            game.LeaveOkay(gd);
            //        }
            //    }
            //    return;
            //}

            //GameUpdateReceived(msg.GameData);

        }
       
        public void JoinAsPlayerReceived(JoinResponseCommMessage msg)
        {
            _currSearchScreen.JoinOkay(msg.GameData);
        }

        public void LeaveAsPlayer(ResponeCommMessage msg)
        {
            foreach (GameScreen gameScreen in _games)
            {
                if (gameScreen.RoomId == msg.GameData.RoomId)
                {
                    gameScreen.LeaveAsPlayerOk(msg.GameData);
                }
            }
        }

        public void LeaveAsSpectetor(ResponeCommMessage msg)
        {
            foreach (GameScreen gameScreen in _games)
            {
                if (gameScreen.RoomId == msg.GameData.RoomId)
                {
                    gameScreen.LeaveAsSpectetorOk(msg.GameData);
                }
            }
            
        }
        public void JoinAsSpectatorReceived(JoinResponseCommMessage msg)
        {
            _currSearchScreen.JoinOkayAsSpectate(msg.GameData);
        }

        public void SearchResultRecived(List<ClientGame> games, bool isReturnToGame)
        {
            if (isReturnToGame)
            {
                if (_returnToGamesScreen != null)
                {
                    if (games.Any())
                    {
                        _returnToGamesScreen.ResultRecived(games);
                    }
                    else
                    {
                        _returnToGamesScreen.EmptySearch();
                    }
                }
            }
            else
            {
                if (_searchScreen != null)
                {
                    if (games.Any())
                    {
                        _searchScreen.ResultRecived(games);
                    }
                    else
                    {
                        _searchScreen.EmptySearch();
                    }
                }
            }
        }

        public void SetReturnToGameScreen(ReturnToGames returnToGames)
        {
            _returnToGamesScreen = returnToGames;
        }
    }
}

