using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    //TODO: this class
    public class ServerEventHandler : IEventHandler
    {
        private readonly UserServiceHandler _userService = new UserServiceHandler(); //TODO init // = new UserServiceHandler();
        private readonly  GameServiceHandler _gameService = new GameServiceHandler();
        private readonly ReplayHandler _replayService = new ReplayHandler();
        private readonly CommunicationHandler _commHandler = CommunicationHandler.GetInstance();
        private readonly ICommMsgXmlParser _parser = new ParserImplementation();

        private readonly TcpClient _socket;

        public ServerEventHandler(TcpClient socket)
        {
            _socket = socket;
        }

        public ServerEventHandler()
        {
        }

        //private readonly LogServiceHandler _logService = new LogServiceHandler(); //TODO: change to log control

        public void HandleEvent(ActionCommMessage msg)
        {
            bool success = false;
            switch (msg.MoveType)
            {
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Fold:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.HandCard:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Join:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Leave:
                    success = _gameService.DoAction(msg.UserId, msg.MoveType, msg.Amount, msg.RoomId);
                    break;

            }
            ResponeCommMessage response = new ResponeCommMessage(msg.UserId, success, msg);
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
        }

        public void HandleEvent(EditCommMessage msg)
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
                   // _logService.CreateNewErrorLog(
                    //    "an unidentified EditCommMessage was received by ServerEventHandler.");
                    return;
            }
            ResponeCommMessage response = new ResponeCommMessage(msg.UserId, success, msg);
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            
            bool success = _userService.LoginUser(msg.UserName, msg.Password);
            if (success)
            {
                IUser user = _userService.GetUserById(msg.UserId);
                ResponeCommMessage response = new LoginResponeCommMessage(user.Id(), user.Name(), user.MemberName(),
                    user.Password(), user.Avatar(), user.Money()
                    , user.Email(),user.GetLeague().ToString(), success, msg);
                _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            }
            else
            {
                ResponeCommMessage response = new LoginResponeCommMessage(-1, "", "",
                    "", "", -1 , "","", success, msg);
                _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            }
           
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            bool success = _userService.RegisterToSystem(msg.UserId, msg.Name, msg.MemberName, msg.Password, msg.Money,
                msg.Email);

            if (_socket != null)
            {
                CommunicationHandler.GetInstance().AddUserId(msg.UserId, _socket); 
            }
            else
            {
                //TODO log here
            }


            ResponeCommMessage response = new RegisterResponeCommMessage(msg.UserId,msg.Name,msg.MemberName,msg.Password,
                "/GuiScreen/Photos/Avatar/devil.png",msg.Money,msg.Email,"unKnow",success,msg);

            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            
        }

        public void HandleEvent(SearchCommMessage msg)
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
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            _commHandler.AddMsgToSend(_parser.SerializeMsg(msg), msg.UserId);
        }

    

        public void HandleEvent(ResponeCommMessage msg)
        {
            _commHandler.AddMsgToSend(_parser.SerializeMsg(msg), msg.UserId);
        }

        public void HandleEvent(CreatrNewRoomMessage msg) //TODO
        {
            int roomId = _gameService.CreateNewRoom(msg.UserId,msg._chipPolicy, msg._canSpectate, msg._mode , msg._minPlayer , msg._maxPlayers , msg._buyInPolicy , msg._minBet);
            bool success;
            if (roomId == -1)
            {
                success = false;
            }
            else
            {
                success = true;
            }
            ResponeCommMessage response = new ResponeCommMessage(msg.UserId, success, msg);
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            //add gae data send. maybe here maybe in game service
        }

        private List<ClientGame> ToClientGameList(List<IGame> toChange)
        {
            List<ClientGame> toReturn = new List<ClientGame>();
            foreach (IGame game in toChange)
            {
                toReturn.Add(new ClientGame(game.IsGameActive(),game.IsSpectatable(),game.GetGameMode(),game.Id,
                    game.GetMinPlayer(),game.GetMaxPlayer(),game.GetMinBet(),game.GetStartingChip(),game.GetBuyInPolicy()
                    ,game.GetLeagueName().ToString(),game.GetPotSize()));
            }
            return toReturn;
        }

        public void HandleEvent(ChatCommMessage msg)
        {
            bool success =false;
            int idReciver= _userService.GetIUserByUserName(msg.ReciverUsername).Id(); // to get id reciver from user name
            string usernameSender = _userService.GetUserById(msg.idSender).MemberName(); //to get from id;

            switch (msg.chatType)
            {
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.PlayerBrodcast:
                    success = _gameService.CanSendPlayerBrodcast(msg.idSender,msg.roomId);
                    idReciver = msg.idSender;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.PlayerWhisper:
                    success = _gameService.CanSendPlayerWhisper(msg.idSender,msg.ReciverUsername, msg.roomId);
                    idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.SpectetorBrodcast:
                    success = _gameService.CanSendSpectetorBrodcast(msg.idSender, msg.roomId);
                    idReciver = msg.idSender;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;
                case TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.SpectetorWhisper:
                    success = _gameService.CanSendSpectetorWhisper(msg.idSender, msg.ReciverUsername, msg.roomId);
                    idReciver = _userService.GetIUserByUserName(msg.ReciverUsername).Id(); ;
                    usernameSender = _userService.GetUserById(msg.idSender).MemberName();
                    break;

            }
            ResponeCommMessage response = new ChatResponceCommMessage(msg.roomId, idReciver, usernameSender, msg.chatType, msg.msgToSend, msg.UserId, success, msg);
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
        }

        public void HandleEvent(ReplayCommMessage msg)
        {
            if(msg.isAllGames==true)
            {
                 //ret
            }
            else if(msg.isAllGames==false)
            {
                //_replayService.ShowGameReplay(msg.gameId, , msg.UserId); game id?
            }
        }
    }
}
