using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;
using TexasHoldemShared.Security;

namespace TexasHoldem.communication.Impl
{
    public class ServerEventHandler : IEventHandler
    {
        private readonly UserServiceHandler _userService ;
        private readonly  GameServiceHandler _gameService;
        private readonly ICommunicationHandler _commHandler;
        private readonly ICommMsgXmlParser _parser = new ParserImplementation();
        public bool ShouldUseDelim { get; set; } = false;
        private ISessionIdHandler _sessionIdHandler;
        private readonly ISecurity _security;
        private ReplayHandler _replayService;
        private readonly TcpClient _socket;

        public ServerEventHandler(SessionIdHandler sidHandler, TcpClient socket, GameCenter game, SystemControl sys, 
            LogControl log, ReplayManager replay, ICommunicationHandler comm)
        {
            _socket = socket;
            _gameService = new GameServiceHandler(game, sys, log, replay, sidHandler);
            _userService = new UserServiceHandler(game, sys);
            _commHandler = comm;
            _sessionIdHandler = sidHandler;
            _security = new SecurityHandler();
            _replayService = new ReplayHandler(replay);
        }

        public void SetSessionIdHandler(ISessionIdHandler handler)
        {
            _sessionIdHandler = handler;
        }

        private ResponeCommMessage SendMessages(int userId, IEnumerator<ActionResultInfo> iterator, 
            CommunicationMessage originalMsg)
        {
            ResponeCommMessage response = null;
            GameDataCommMessage gameData = null;
            bool found = false;
            while (iterator.MoveNext())
            {
                var curr = iterator.Current;
                if (curr != null && curr.Id != userId)
                {
                    gameData = curr.GameData;
                    _commHandler.AddMsgToSend(_parser.SerializeMsg(curr.GameData, ShouldUseDelim), curr.Id);
                }
                else if (curr != null)
                {
                    found = true;
                    response = new ResponeCommMessage(userId, _sessionIdHandler.GetSessionIdByUserId(userId),
                        curr.GameData.IsSucceed, originalMsg);
                    response.SetGameData(curr.GameData);
                }
            }
            if (!found && gameData != null)
            {
                response = new ResponeCommMessage(userId, _sessionIdHandler.GetSessionIdByUserId(userId),
                    gameData.IsSucceed, originalMsg);
                response.SetGameData(gameData);
            }
            return response;
        }

        private JoinResponseCommMessage SendMessagesJoin(int userId, IEnumerator<ActionResultInfo> iterator,
            CommunicationMessage originalMsg)
        {
            JoinResponseCommMessage response = null;
            GameDataCommMessage gameData = null;
            bool found = false;
            while (iterator.MoveNext())
            {
                var curr = iterator.Current;
                if (curr != null && curr.Id != userId)
                {
                    gameData = curr.GameData;
                    _commHandler.AddMsgToSend(_parser.SerializeMsg(curr.GameData, ShouldUseDelim), curr.Id);
                }
                else if (curr != null)
                {
                    response = new JoinResponseCommMessage(_sessionIdHandler.GetSessionIdByUserId(userId), userId,
                        curr.GameData.IsSucceed, originalMsg, curr.GameData);
                    response.SetGameData(curr.GameData);
                }
            }

            if (!found && gameData != null)
            {
                response = new JoinResponseCommMessage(_sessionIdHandler.GetSessionIdByUserId(userId), userId,
                        gameData.IsSucceed, originalMsg, gameData);
                response.SetGameData(gameData);
            }
            return response;
        }

        public ResponeCommMessage HandleEvent(ActionCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                ResponeCommMessage response = null;
                IEnumerator<ActionResultInfo> iter;
                switch (msg.MoveType)
                {
                    case CommunicationMessage.ActionType.Bet:
                    case CommunicationMessage.ActionType.Fold:
                    case CommunicationMessage.ActionType.HandCard:
                    case CommunicationMessage.ActionType.Leave:
                    case CommunicationMessage.ActionType.StartGame:
                    case CommunicationMessage.ActionType.SpectatorLeave:
                        iter = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = SendMessages(msg.UserId, iter, msg);
                        break;
                    case CommunicationMessage.ActionType.Join:
                        iter = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);

                        response = SendMessagesJoin(msg.UserId, iter, msg);
                        break;
                    case CommunicationMessage.ActionType.Spectate:
                        iter = _gameService.AddSpectatorToRoom(msg.UserId, msg.RoomId);
                        response = SendMessagesJoin(msg.UserId, iter, msg);
                        break;
                }
                if (response != null)
                {
                    return response;
                } 
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        //TODO: maybe delete
        //private List<string> GetNamesFromList(List<Player> players)
        //{
        //    List<string> names = new List<string>();
        //    foreach (Player p in players)
        //    {
        //        names.Add(p.user.MemberName());
        //    }
        //    return names;
        //}

        public ResponeCommMessage HandleEvent(EditCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                bool success;
                int newMoney;
                int newId;
                switch (msg.FieldToEdit)
                {
                    case EditCommMessage.EditField.UserName:
                        success = _userService.EditUserName(msg.UserId, msg.NewValue);
                        break;
                    case EditCommMessage.EditField.Password:
                        success = _userService.EditUserPassword(msg.UserId, msg.NewValue);
                        break;
                    case EditCommMessage.EditField.Avatar:
                        success = _userService.EditUserAvatar(msg.UserId, msg.NewValue);
                        break;
                    case EditCommMessage.EditField.Email:
                        success = _userService.EditUserEmail(msg.UserId, msg.NewValue);
                        break;
                    case EditCommMessage.EditField.Money:
                        string temp = msg.NewValue;
                        bool isValid = int.TryParse(temp, out newMoney);
                        if (isValid)
                        {
                            success = _userService.EditMoney(msg.UserId, newMoney);
                        }
                        else
                        {
                            success = false;
                        }
                        break;
                    case EditCommMessage.EditField.Name:
                        success = _userService.EditName(msg.UserId, msg.NewValue);
                        break;
                    case EditCommMessage.EditField.Id:
                        string temp2 = msg.NewValue;
                        bool isValid2 = int.TryParse(temp2, out newId);
                        if (isValid2)
                        {
                            success = _userService.EditId(msg.UserId, newId);
                        }
                        else
                        {
                            success = false;
                        }
                        break;
                    default:
                        return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
                } 
                return new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId),
                    success, msg);
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        private long GenerateSid(int userId)
        {
            long sid = -1;
            if (!_sessionIdHandler.ContainsUserId(userId))
            {
                sid = _security.GenerateNewSessionId();
                _sessionIdHandler.AddSid(userId, sid);
            }
            return sid;
        }

        public ResponeCommMessage HandleEvent(LoginCommMessage msg)
        {

            if (_sessionIdHandler != null)
            {
                IUser user;
                if (msg.IsLogin)
                {
                    user = _userService.LoginUser(msg.UserName, msg.Password);
                }
                else
                {
                    user = _userService.LogoutUser(msg.UserId);
                }
                ResponeCommMessage response;
                if (_socket != null && user != null)
                {
                    _commHandler.AddUserId(user.Id(), _socket);

                    long sid = GenerateSid(msg.UserId);
                    response = new LoginResponeCommMessage(user.Id(), sid, user.Name(), user.MemberName(),
                        user.Password(), user.Avatar(), user.Money(),
                        user.Email(), user.GetLeague().ToString(), true, msg);
                }
                else if (_socket != null)
                {
                    _commHandler.AddUserId(-1, _socket);

                    response = new LoginResponeCommMessage(-1, -1, "", "",
                        "", "", -1, "", "", false, msg);
                }
                else
                {
                    Console.WriteLine("error in login!");
                    response = null;
                }
                return response; 
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);

        }

        public ResponeCommMessage HandleEvent(RegisterCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                bool success = _userService.RegisterToSystem(msg.UserId, msg.Name, msg.MemberName, msg.Password, msg.Money,
                        msg.Email);

                if (_socket != null)
                {
                    _commHandler.AddUserId(msg.UserId, _socket);
                }
                long sid = GenerateSid(msg.UserId);

                ResponeCommMessage response = new RegisterResponeCommMessage(sid, msg.UserId, msg.Name, msg.MemberName, msg.Password,
                    "/GuiScreen/Photos/Avatar/devil.png", msg.Money, msg.Email, "unKnow", success, msg);

                return response; 
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        public ResponeCommMessage HandleEvent(SearchCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                List<IGame> temp = new List<IGame>();
                switch (msg.searchType)
                {
                    case SearchCommMessage.SearchType.ActiveGamesByUserName:
                        temp = _gameService.GetActiveGamesByUserName(msg.SearchByString);                        
                        break;
                    case SearchCommMessage.SearchType.SpectetorGameByUserName:
                        temp = _gameService.GetSpectetorGamesByUserName(msg.SearchByString);
                        break;
                    case SearchCommMessage.SearchType.ByRoomId:
                        IGame game = _gameService.GetGameById(msg.SearchByInt);
                        if (game != null)
                        {
                            temp.Add(game);
                        }
                        break;
                    case SearchCommMessage.SearchType.AllSepctetorGame:
                        temp = _gameService.GetSpectateableGames();
                        break;
                    case SearchCommMessage.SearchType.GamesUserCanJoin:
                        temp = _gameService.GetAllActiveGamesAUserCanJoin(msg.UserId);
                        break;
                    case SearchCommMessage.SearchType.ByPotSize:
                        temp = _gameService.GetGamesByPotSize(msg.SearchByInt);
                        break;
                    case SearchCommMessage.SearchType.ByGameMode:
                        temp = _gameService.GetGamesByGameMode(msg.SearchByGameMode);
                        break;
                    case SearchCommMessage.SearchType.ByBuyInPolicy:
                        temp = _gameService.GetGamesByBuyInPolicy(msg.SearchByInt);
                        break;
                    case SearchCommMessage.SearchType.ByMinPlayer:
                        temp = _gameService.GetGamesByMinPlayer(msg.SearchByInt);
                        break;
                    case SearchCommMessage.SearchType.ByMaxPlayer:
                        temp = _gameService.GetGamesByMaxPlayer(msg.SearchByInt);
                        break;
                    case SearchCommMessage.SearchType.ByStartingChip:
                        temp = _gameService.GetGamesByStartingChip(msg.SearchByInt);
                        break;
                    case SearchCommMessage.SearchType.ByMinBet:
                        temp = _gameService.GetGamesByMinBet(msg.SearchByInt);
                        break;
                    default:
                        break;
                }
                var toSend = ToClientGameList(temp);
                var success = toSend.Count != 0;
                return new SearchResponseCommMessage(toSend, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), 
                    msg.UserId, success, msg);
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        private LeaderboardLineData UserToLineData(IUser user)
        {
            return new LeaderboardLineData(user.Id(), user.MemberName(), user.Points(),
                user.TotalProfit, user.HighestCashGainInGame, user.GetNumberOfGamesUserPlay());
        }

        public ResponeCommMessage HandleEvent(UserStatisticsCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                UserStatistics stats = _userService.GetUserStatistics(msg.UserId);
                if (stats != null)
                {
                    return new UserStatisticsResponseCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), true,
                        msg, stats.AvgCashGain, stats.AvgGrossProfit);
                }
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        public ResponeCommMessage HandleEvent(LeaderboardCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                var sortOption = msg.SortedBy;
                List<IUser> userLst;
                switch (sortOption)
                {
                    case LeaderboardCommMessage.SortingOption.TotalGrossProfit:
                        userLst = _userService.GetUsersByTotalProfit();
                        break;
                    case LeaderboardCommMessage.SortingOption.HighestCashGain:
                        userLst = _userService.GetUsersByHighestCash();
                        break;
                    case LeaderboardCommMessage.SortingOption.NumGamesPlayes:
                        userLst = _userService.GetUsersByNumOfGames();
                        break;
                    default:
                        return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
                }
                var leaderboradLines = userLst.ConvertAll(UserToLineData);
                return new LeaderboardResponseCommMessage(msg.UserId, 
                    _sessionIdHandler.GetSessionIdByUserId(msg.UserId),
                    true, msg, leaderboradLines);
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        private ReturnToGameResponseCommMsg SendMessagesReturnToGame(int userId, IEnumerator<ActionResultInfo> iterator,
            ReturnToGameCommMsg originalMsg)
        {
            ReturnToGameResponseCommMsg response = null;
            while (iterator.MoveNext())
            {
                var curr = iterator.Current;
                if (curr != null && curr.Id != userId)
                {
                    _commHandler.AddMsgToSend(_parser.SerializeMsg(curr.GameData, ShouldUseDelim), curr.Id);
                }
                else if (curr != null)
                {
                    response = new ReturnToGameResponseCommMsg(_sessionIdHandler.GetSessionIdByUserId(userId), userId,
                        curr.GameData.IsSucceed, originalMsg, curr.GameData);
                    response.SetGameData(curr.GameData);
                }
            }
            return response;
        }


        private ResponeCommMessage HandleReturnToGame(ReturnToGameCommMsg msg,
            IEnumerator<ActionResultInfo> iter)
        {
            if (_sessionIdHandler == null || iter == null)
            {
                return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
            }
            ReturnToGameResponseCommMsg response = SendMessagesReturnToGame(msg.UserId, iter, msg);
            if (response != null)
            {
                return response;
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        public ResponeCommMessage HandleEvent(ReturnToGameAsPlayerCommMsg msg)
        {
            var iter = _gameService.ReturnToGameAsPlayer(msg.UserId, msg.RoomId);
            return HandleReturnToGame(msg, iter);
        }

        public ResponeCommMessage HandleEvent(ReturnToGameAsSpecCommMsg msg)
        {
            var iter = _gameService.ReturnToGameAsSpec(msg.UserId, msg.RoomId);
            return HandleReturnToGame(msg, iter);
        }

        //this is done differently then other types of msgs because it is called from service
        public ResponeCommMessage HandleEvent(GameDataCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
                _commHandler.AddMsgToSend(parsed, msg.UserId);
                return new ResponeCommMessage(msg.UserId, msg.SessionId, true, msg); //is not used 
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        public ResponeCommMessage HandleEvent(ResponeCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
                _commHandler.AddMsgToSend(parsed, msg.UserId);
                return new ResponeCommMessage(msg.UserId, msg.SessionId, true, msg); //is not used 
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        public ResponeCommMessage HandleEvent(CreateNewRoomMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                int roomId = _gameService.CreateNewRoom(msg.UserId, msg._chipPolicy,
                        msg._canSpectate, msg._mode, msg._minPlayer, msg._maxPlayers,
                        msg._buyInPolicy, msg._minBet);
                var success = roomId != -1;

                CreateNewGameResponse respons;
                if (success)
                {
                    List<string> names = new List<string>();
                    IUser user = _userService.GetUserById(msg.UserId);
                    names.Add(user.MemberName());
                    var gameData = new GameDataCommMessage(msg.UserId, roomId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), null, null, new List<Card>(),
                        msg._chipPolicy, 0, names, new List<string>(),  null, null, null, true,
                        "", "", 0, CommunicationMessage.ActionType.CreateRoom, GameRoom.HandStep.PreFlop.ToString(), "");
                    respons = new CreateNewGameResponse(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg, gameData);
                }
                else
                {
                    respons = new CreateNewGameResponse();
                }
                return respons;  
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        private List<ClientGame> ToClientGameList(List<IGame> toChange)
        {
            List<ClientGame> toReturn = new List<ClientGame>();
            foreach (IGame game in toChange)
            {
                if (game != null)
                {
                    toReturn.Add(new ClientGame(game.IsGameActive(), game.IsSpectatable(), game.GetGameMode(), game.Id,
                        game.GetMinPlayer(), game.GetMaxPlayer(), game.GetMinBet(), game.GetStartingChip(), game.GetBuyInPolicy()
                        , game.GetLeagueName().ToString(), game.GetPotSize()));
                }
              
            }
            return toReturn;
        }

        private void SendBroadcast(IEnumerator<int> iterator, int senderId, ChatCommMessage msg, string usernameSender)
        {
            while (iterator.MoveNext())
            {
                var curr = iterator.Current;
                var res = new ChatResponceCommMessage(msg.RoomId, curr, msg.SessionId, usernameSender, msg.ChatType,
                    msg.MsgToSend, curr, true, msg);
                if (curr != senderId)
                {
                    _commHandler.AddMsgToSend(_parser.SerializeMsg(res, ShouldUseDelim), curr);
                }
            }
        }

        public ResponeCommMessage HandleEvent(ChatCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                bool success = false;
                int idReciver = 0;
                string usernameSender = _userService.GetUserById(msg.IdSender).MemberName(); //to get from id;
                switch (msg.ChatType)
                {
                    case CommunicationMessage.ActionType.PlayerBrodcast:
                        var enumerator = _gameService.CanSendPlayerBrodcast(msg.IdSender, msg.RoomId);           
                        success = enumerator != null;
                        SendBroadcast(enumerator, msg.UserId, msg, usernameSender);
                        idReciver = msg.IdSender;
                        return new ChatResponceCommMessage(msg.RoomId, idReciver, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), usernameSender, msg.ChatType, msg.MsgToSend, msg.UserId, success, msg);

                        break;
                    case CommunicationMessage.ActionType.PlayerWhisper:
                        IUser reciver = _userService.GetIUserByUserName(msg.ReciverUsername);
                        var res = new ChatResponceCommMessage(msg.RoomId, reciver.Id(), msg.SessionId, usernameSender, msg.ChatType,
                        msg.MsgToSend, reciver.Id(), true, msg);
                        if (reciver.Id() != msg.IdSender)
                            _commHandler.AddMsgToSend(_parser.SerializeMsg(res, ShouldUseDelim), reciver.Id());
                        idReciver = msg.IdSender;
                        break;
                    case CommunicationMessage.ActionType.SpectetorBrodcast:
                         var enumeratorSpec = _gameService.CanSendSpectetorBrodcast(msg.IdSender, msg.RoomId);
                         success = enumeratorSpec != null;
                         SendBroadcastSpec(enumeratorSpec, msg.UserId, msg, usernameSender);
                        idReciver = msg.IdSender;
                        return new ChatResponceCommMessage(msg.RoomId, idReciver, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), usernameSender, msg.ChatType, msg.MsgToSend, msg.UserId, success, msg);

                        break;
                    case CommunicationMessage.ActionType.SpectetorWhisper:
                        var enumeratorSpecWhisper = _gameService.CanSendSpectetorBrodcast(msg.IdSender, msg.RoomId);
                        success = enumeratorSpecWhisper != null;
                        SendBroadcastSpec(enumeratorSpecWhisper, msg.UserId, msg, usernameSender);
                        idReciver = msg.IdSender;
                        break;

                }
            }
            return new ResponeCommMessage(msg.UserId, msg.SessionId, false, msg);
        }

        private void SendBroadcastSpec(IEnumerator<int> iterator, int senderId, ChatCommMessage msg, string usernameSender)
        {
            while (iterator.MoveNext())
            {
                var curr = iterator.Current;
                var res = new ChatResponceCommMessage(msg.RoomId, curr, msg.SessionId, usernameSender, msg.ChatType,
                    msg.MsgToSend, curr, true, msg);
                if (curr != senderId)
                {
                    _commHandler.AddMsgToSend(_parser.SerializeMsg(res, ShouldUseDelim), curr);
                }
            }
        }

     
        public ResponeCommMessage HandleEvent(ReplayCommMessage msg)
        {
            Tuple<bool, string> rep = _replayService.ShowFirstGameReplay(msg.RoomId, msg.UserId);   
            return new ReplaySearchResponseCommMessage(rep.Item2,msg.RoomId, msg.UserId,msg.SessionId,  rep.Item1, msg);
        } 
    }
}
