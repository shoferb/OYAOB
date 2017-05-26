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

namespace TexasHoldem.communication.Impl
{
    public class ServerEventHandler : IEventHandler
    {
        private readonly UserServiceHandler _userService = new UserServiceHandler();
        private readonly  GameServiceHandler _gameService;
        private ICommunicationHandler _commHandler;
        private readonly ICommMsgXmlParser _parser = new ParserImplementation();
        public bool ShouldUseDelim { get; set; } = false;

        private readonly TcpClient _socket;

        public ServerEventHandler(TcpClient socket, GameCenter game, SystemControl sys, LogControl log, ReplayManager replay, ICommunicationHandler comm)
        {
            _socket = socket;
            _gameService = new GameServiceHandler(game, sys, log, replay);
            _commHandler = comm;
        }

        public ServerEventHandler()
        {
        }

        public void SetCommHandler(ICommunicationHandler handler)
        {
            _commHandler = handler;
        }

        public string HandleEvent(ActionCommMessage msg)
        {
            bool success = false;
            ResponeCommMessage response = null;
            switch (msg.MoveType)
            {
                case CommunicationMessage.ActionType.Bet:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    response = new ResponeCommMessage(msg.UserId, success, msg);
                    break;
                case CommunicationMessage.ActionType.Fold:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    response = new ResponeCommMessage(msg.UserId, success, msg);
                    break;
                case CommunicationMessage.ActionType.HandCard:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    response = new ResponeCommMessage(msg.UserId, success, msg);
                    break;
                case CommunicationMessage.ActionType.Join:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    IGame room = _gameService.GetGameById(msg.RoomId);
                    GameDataCommMessage data = new GameDataCommMessage(msg.UserId, msg.RoomId, null, null, room.GetPublicCards(), msg.Amount,
                        room.GetPotSize(), GetNamesFromList(room.GetPlayersInRoom()), "", "", "", success, "", "", 0, CommunicationMessage.ActionType.Join);
                    response = new JoinResponseCommMessage(msg.UserId, success, msg,data);
                    break;
                case CommunicationMessage.ActionType.Leave:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    response = new ResponeCommMessage(msg.UserId, success, msg);
                    break;
                case CommunicationMessage.ActionType.StartGame:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    response = new ResponeCommMessage(msg.UserId, success, msg);
                    break;
            }
            if (response != null)
            {
                return _parser.SerializeMsg(response, ShouldUseDelim); 
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
            ResponeCommMessage response = new ResponeCommMessage(msg.UserId, success, msg);
            return _parser.SerializeMsg(response, ShouldUseDelim);
        }

        public string HandleEvent(LoginCommMessage msg)
        {
            
            bool success = _userService.LoginUser(msg.UserName, msg.Password);
            ResponeCommMessage response;
            if (_socket != null)
            {
                CommunicationHandler.GetInstance().AddUserId(msg.UserId, _socket);
            }
            if (success)
            {
                IUser user = _userService.GetIUserByUserName(msg.UserName);
                response = new LoginResponeCommMessage(user.Id(), user.Name(), user.MemberName(),
                    user.Password(), user.Avatar(), user.Money()
                    , user.Email(),user.GetLeague().ToString(), success, msg);
            }
            else
            {
                response = new LoginResponeCommMessage(-1, "", "",
                    "", "", -1 , "","", success, msg);
            }
            return _parser.SerializeMsg(response, ShouldUseDelim);
           
        }

        public string HandleEvent(RegisterCommMessage msg)
        {
            bool success = _userService.RegisterToSystem(msg.UserId, msg.Name, msg.MemberName, msg.Password, msg.Money,
                msg.Email);

            if (_socket != null)
            {
                CommunicationHandler.GetInstance().AddUserId(msg.UserId, _socket); 
            }
            ResponeCommMessage response = new RegisterResponeCommMessage(msg.UserId,msg.Name,msg.MemberName,msg.Password,
                "/GuiScreen/Photos/Avatar/devil.png",msg.Money,msg.Email,"unKnow",success,msg);

            return _parser.SerializeMsg(response, ShouldUseDelim);
        }

        public string HandleEvent(SearchCommMessage msg)
        {
            bool success;
            List<IGame> temp = new List<IGame>();
            List<ClientGame> toSend = new List<ClientGame>();
            switch (msg.searchType)
            {
                case SearchCommMessage.SearchType.ActiveGamesByUserName:
                    temp = _userService.GetActiveGamesByUserName(msg.searchByString);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.SpectetorGameByUserName:
                    temp = _userService.GetSpectetorGamesByUserName(msg.searchByString);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByRoomId:
                    IGame game = _gameService.GetGameById(msg.searchByInt);
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
                    temp = _gameService.GetGamesByPotSize(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByGameMode:
                    temp = _gameService.GetGamesByGameMode(msg.searchByGameMode);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByBuyInPolicy:
                    temp = _gameService.GetGamesByBuyInPolicy(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByMinPlayer:
                    temp = _gameService.GetGamesByMinPlayer(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByMaxPlayer:
                    temp = _gameService.GetGamesByMaxPlayer(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByStartingChip:
                    temp = _gameService.GetGamesByStartingChip(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                case SearchCommMessage.SearchType.ByMinBet:
                    temp = _gameService.GetGamesByMinBet(msg.searchByInt);
                    toSend = ToClientGameList(temp);
                    success = toSend.Count != 0;
                    break;
                default:
                    success = false;
                    break;
            }
            ResponeCommMessage response = new SearchResponseCommMessage(toSend, msg.UserId, success, msg);
            return _parser.SerializeMsg(response, ShouldUseDelim);
        }

        //TODO
        public string HandleEvent(UserStatisticsCommMessage msg)
        {
            throw new NotImplementedException();
        }

        //TODO:
        public string HandleEvent(LeaderboardCommMessage msg)
        {
            throw new NotImplementedException();
        }

        //this is done differently then other types of msgs because it is called from service
        public string HandleEvent(GameDataCommMessage msg)
        {
            var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
            _commHandler.AddMsgToSend(parsed, msg.UserId);
            return parsed;
        }

        //TODO: maybe problematic
        public string HandleEvent(ResponeCommMessage msg)
        {
            var parsed = _parser.SerializeMsg(msg, ShouldUseDelim);
            _commHandler.AddMsgToSend(parsed, msg.UserId);
            return parsed;
        }

        public string HandleEvent(CreateNewRoomMessage msg)
        {
            int roomId = _gameService.CreateNewRoom(msg.UserId,msg._chipPolicy,
                msg._canSpectate, msg._mode , msg._minPlayer , msg._maxPlayers ,
                msg._buyInPolicy , msg._minBet);
            var success = roomId != -1;

            CreateNewGameResponse respons;
            if (success)
            {
                List<string> names = new List<string>();
                IUser user = _userService.GetUserById(msg.UserId);
                names.Add(user.MemberName());
                var gameData = new GameDataCommMessage(msg.UserId, roomId, null, null, new List<Card>(),
                    msg._chipPolicy, 0,names , null, null, null, success,
                    "","",0,CommunicationMessage.ActionType.CreateRoom);
                respons = new CreateNewGameResponse(msg.UserId, success, msg, gameData);
            }
            else
            {
                respons = new CreateNewGameResponse();
            }
            return _parser.SerializeMsg(respons, ShouldUseDelim); 
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
            bool success =false;
            int idReciver= _userService.GetIUserByUserName(msg.ReciverUsername).Id(); // to get id reciver from user name
            string usernameSender = _userService.GetUserById(msg.idSender).MemberName(); //to get from id;

            switch (msg.chatType)
            {
                case CommunicationMessage.ActionType.PlayerBrodcast:
                    success = _gameService.CanSendPlayerBrodcast(msg.idSender,msg.roomId);
                    idReciver = msg.idSender;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case CommunicationMessage.ActionType.PlayerWhisper:
                    success = _gameService.CanSendPlayerWhisper(msg.idSender,msg.ReciverUsername, msg.roomId);
                    idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case CommunicationMessage.ActionType.SpectetorBrodcast:
                    success = _gameService.CanSendSpectetorBrodcast(msg.idSender, msg.roomId);
                    idReciver = msg.idSender;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case CommunicationMessage.ActionType.SpectetorWhisper:
                    success = _gameService.CanSendSpectetorWhisper(msg.idSender, msg.ReciverUsername, msg.roomId);
                    idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;

            }
            ResponeCommMessage response = new ChatResponceCommMessage(msg.roomId, idReciver, usernameSender, msg.chatType, msg.msgToSend, msg.UserId, success, msg);
            return _parser.SerializeMsg(response, ShouldUseDelim);
        }

        //TODO:
        public string HandleEvent(ReplayCommMessage msg)
        {           
           // _replayService.ShowFirstGameReplay(msg.roomID, msg.UserId);   
            return "";
        }
    }
}
