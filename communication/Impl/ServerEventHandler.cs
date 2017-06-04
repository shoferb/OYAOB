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
        private ICommunicationHandler _commHandler;
        private readonly ICommMsgXmlParser _parser = new ParserImplementation();
        public bool ShouldUseDelim { get; set; } = false;
        private ISessionIdHandler _sessionIdHandler;
        private readonly ISecurity _security;

        private readonly TcpClient _socket;

        public ServerEventHandler(ISessionIdHandler sidHandler, TcpClient socket, GameCenter game, SystemControl sys, 
            LogControl log, ReplayManager replay, ICommunicationHandler comm)
        {
            _socket = socket;
            _gameService = new GameServiceHandler(game, sys, log, replay);
            _userService = new UserServiceHandler(game, sys);
            _commHandler = comm;
            _sessionIdHandler = sidHandler;
            _security = new SecurityHandler();
        }

        public void SetSessionIdHandler(ISessionIdHandler handler)
        {
            _sessionIdHandler = handler;
        }

        public string HandleEvent(ActionCommMessage msg)
        {
            bool success = false;
            if (_sessionIdHandler != null)
            {
                ResponeCommMessage response = null;
                switch (msg.MoveType)
                {
                    case CommunicationMessage.ActionType.Bet:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                        break;
                    case CommunicationMessage.ActionType.Fold:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                        break;
                    case CommunicationMessage.ActionType.HandCard:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                        break;
                    case CommunicationMessage.ActionType.Join:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        IGame room = _gameService.GetGameById(msg.RoomId);
                        GameDataCommMessage data = new GameDataCommMessage(msg.UserId, msg.RoomId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), null, null, room.GetPublicCards(), msg.Amount,
                            room.GetPotSize(), GetNamesFromList(room.GetPlayersInRoom()), "", "", "", success, "", "", 0, CommunicationMessage.ActionType.Join);
                        response = new JoinResponseCommMessage(_sessionIdHandler.GetSessionIdByUserId(msg.UserId), msg.UserId, success, msg, data);
                        break;
                    case CommunicationMessage.ActionType.Leave:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                        break;
                    case CommunicationMessage.ActionType.StartGame:
                        success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                        response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                        break;
                }
                if (response != null)
                {
                    return _parser.SerializeMsg(response, ShouldUseDelim);
                } 
            }
            return "";
        }

        private List<string> GetNamesFromList(List<Player> players)
        {
            List<string> names = new List<string>();
            foreach (Player p in players)
            {
                names.Add(p.user.MemberName());
            }
            return names;
        }

        public string HandleEvent(EditCommMessage msg)
        {
            bool success;
            if (_sessionIdHandler != null)
            {
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
                        int newMoney;
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
                        int newId;
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
                        return "";
                } 
                ResponeCommMessage response = new ResponeCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg);
                return _parser.SerializeMsg(response, ShouldUseDelim);
            }
            return "";
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

        public string HandleEvent(LoginCommMessage msg)
        {

            if (_sessionIdHandler != null)
            {
                bool success = _userService.LoginUser(msg.UserName, msg.Password);
                ResponeCommMessage response;
                if (_socket != null)
                {
                    _commHandler.AddUserId(msg.UserId, _socket);
                }
                if (success)
                {
                    long sid = GenerateSid(msg.UserId);
                    IUser user = _userService.GetIUserByUserName(msg.UserName);
                    response = new LoginResponeCommMessage(user.Id(), sid, user.Name(), user.MemberName(),
                        user.Password(), user.Avatar(), user.Money(),
                        user.Email(), user.GetLeague().ToString(), success, msg);
                }
                else
                {
                    response = new LoginResponeCommMessage(-1, -1, "", "",
                        "", "", -1, "", "", success, msg);
                }
                return _parser.SerializeMsg(response, ShouldUseDelim); 
            }
            return "";

        }

        public string HandleEvent(RegisterCommMessage msg)
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

                return _parser.SerializeMsg(response, ShouldUseDelim); 
            }
            return "";
        }

        public string HandleEvent(SearchCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                bool success;
                List<IGame> temp = new List<IGame>();
                List<ClientGame> toSend = new List<ClientGame>();
                switch (msg.searchType)
                {
                    case SearchCommMessage.SearchType.ActiveGamesByUserName:
                        temp = _userService.GetActiveGamesByUserName(msg.SearchByString);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.SpectetorGameByUserName:
                        temp = _userService.GetSpectetorGamesByUserName(msg.SearchByString);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByRoomId:
                        IGame game = _gameService.GetGameById(msg.SearchByInt);
                        if (game != null)
                        {
                            temp.Add(game);
                            toSend = ToClientGameList(temp);
                            success = toSend.Count != 0;
                        }
                        else
                        {
                            success = false;
                        }
                        break;
                    case SearchCommMessage.SearchType.AllSepctetorGame:
                        temp = _gameService.GetSpectateableGames();
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.GamesUserCanJoin:
                        temp = _gameService.GetAllActiveGamesAUserCanJoin(msg.UserId);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByPotSize:
                        temp = _gameService.GetGamesByPotSize(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByGameMode:
                        temp = _gameService.GetGamesByGameMode(msg.SearchByGameMode);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByBuyInPolicy:
                        temp = _gameService.GetGamesByBuyInPolicy(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByMinPlayer:
                        temp = _gameService.GetGamesByMinPlayer(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByMaxPlayer:
                        temp = _gameService.GetGamesByMaxPlayer(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByStartingChip:
                        temp = _gameService.GetGamesByStartingChip(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    case SearchCommMessage.SearchType.ByMinBet:
                        temp = _gameService.GetGamesByMinBet(msg.SearchByInt);
                        toSend = ToClientGameList(temp);
                        success = toSend.Count != 0;
                        break;
                    default:
                        success = false;
                        break;
                }
                ResponeCommMessage response = new SearchResponseCommMessage(toSend, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), msg.UserId, success, msg);
                return _parser.SerializeMsg(response, ShouldUseDelim); 
            }
            return "";
        }

        private LeaderboardLineData UserToLineData(IUser user)
        {
            return new LeaderboardLineData(user.Id(), user.MemberName(), user.Points(),
                user.TotalProfit, user.HighestCashGainInGame, user.WinNum + user.LoseNum);
        }

        public string HandleEvent(UserStatisticsCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                UserStatistics stats = _userService.GetUserStatistics(msg.UserId);
                if (stats != null)
                {
                    var response = new UserStatisticsResponseCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), true,
                        msg, stats.AvgCashGain, stats.AvgGrossProfit);
                    return _parser.SerializeMsg(response, ShouldUseDelim);
                }
            }
            UserStatisticsCommMessage sts = new UserStatisticsCommMessage(1, 1);
            var resp = new UserStatisticsResponseCommMessage(1, 1, true, sts, 2.2, 23.1);
            return _parser.SerializeMsg(resp, ShouldUseDelim);
            return "";
        }

        public string HandleEvent(LeaderboardCommMessage msg)
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
                        return "";
                }
                var leaderboradLines = userLst.ConvertAll(UserToLineData);
                var response = new LeaderboardResponseCommMessage(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId),
                    true, msg, leaderboradLines);
                return _parser.SerializeMsg(response, ShouldUseDelim); 
            }
            return "";
        }

        //this is done differently then other types of msgs because it is called from service
        public string HandleEvent(GameDataCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
                _commHandler.AddMsgToSend(parsed, msg.UserId);
                return parsed; 
            }
            return "";
        }

        //_sessionIdHandler.GetSessionIdByUserId(msg.UserId): maybe problematic
        public string HandleEvent(ResponeCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
                _commHandler.AddMsgToSend(parsed, msg.UserId);
                return parsed; 
            }
            return "";
        }

        public string HandleEvent(CreateNewRoomMessage msg)
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
                        msg._chipPolicy, 0, names, null, null, null, success,
                        "", "", 0, CommunicationMessage.ActionType.CreateRoom);
                    respons = new CreateNewGameResponse(msg.UserId, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), success, msg, gameData);
                }
                else
                {
                    respons = new CreateNewGameResponse();
                }
                return _parser.SerializeMsg(respons, ShouldUseDelim);  
            }
            return "";
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

        public string HandleEvent(ChatCommMessage msg)
        {
            if (_sessionIdHandler != null)
            {
                bool success = false;
                int idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); // to get id reciver from user name
                string usernameSender = _userService.GetUserById(msg.IdSender).MemberName(); //to get from id;

                switch (msg.ChatType)
                {
                    case CommunicationMessage.ActionType.PlayerBrodcast:
                        success = _gameService.CanSendPlayerBrodcast(msg.IdSender, msg.RoomId);
                        idReciver = msg.IdSender;
                        usernameSender = _userService.GetUserById(msg.IdSender).MemberName();
                        break;
                    case CommunicationMessage.ActionType.PlayerWhisper:
                        success = _gameService.CanSendPlayerWhisper(msg.IdSender, msg.ReciverUsername, msg.RoomId);
                        idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                        usernameSender = _userService.GetUserById(msg.IdSender).MemberName();
                        break;
                    case CommunicationMessage.ActionType.SpectetorBrodcast:
                        success = _gameService.CanSendSpectetorBrodcast(msg.IdSender, msg.RoomId);
                        idReciver = msg.IdSender;
                        usernameSender = _userService.GetUserById(msg.IdSender).MemberName();
                        break;
                    case CommunicationMessage.ActionType.SpectetorWhisper:
                        success = _gameService.CanSendSpectetorWhisper(msg.IdSender, msg.ReciverUsername, msg.RoomId);
                        idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                        usernameSender = _userService.GetUserById(msg.IdSender).MemberName();
                        break;

                }
                ResponeCommMessage response = new ChatResponceCommMessage(msg.RoomId, idReciver, _sessionIdHandler.GetSessionIdByUserId(msg.UserId), usernameSender, msg.ChatType, msg.MsgToSend, msg.UserId, success, msg);
                return _parser.SerializeMsg(response, ShouldUseDelim); 
            }
            return "";
        }

        //TODO:
        public string HandleEvent(ReplayCommMessage msg)
        {           
           // _replayService.ShowFirstGameReplay(msg.roomID, msg.UserId);   
            return "";
        }
    }
}
