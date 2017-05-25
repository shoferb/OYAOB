using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.Service
{
    public class GameServiceHandler
    {
        
        private static GameCenter _gameCenter = GameCenter.Instance;
        private SystemControl _systemControl;
        private ReplayManager _replayManager;
        private static ServerEventHandler _serverHandler = new ServerEventHandler();

        public GameServiceHandler()
        {
            _systemControl = SystemControl.SystemControlInstance;
            _replayManager = ReplayManager.ReplayManagerInstance;
        }

        public bool DoAction(int userId, CommunicationMessage.ActionType action, int amount, int roomId)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            return _gameCenter.DoAction(user, action, amount, roomId);
        }

        public List<Player> GetPlayersInRoom(int roomId)
        {
            return _gameCenter.getPlayersInRoom(roomId);
        }

        public List<Spectetor> GetSpectatorsInRoom(int roomId)
        {
            return _gameCenter.getSpectatorsInRoom(roomId);
        }
        public IGame GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }

       public bool CreateNewRoomWithRoomId(int roomId,int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            return _gameCenter.CreateNewRoomWithRoomId(roomId, user, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        //create room and add to games list game center
        public int CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            int roomID = _gameCenter.GetNextIdRoom();
            if (!_gameCenter.CreateNewRoomWithRoomId(roomID, user, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet))
            {
                return -1;
            }
            return roomID;
        }
        
        public List<string> GetGameReplay(int roomId, int gameNum, int userId)
        {
            GameReplay replay = _replayManager.GetGameReplayForUser(roomId, gameNum, userId);
            List<string> replays = new List<string>();
            if (replay == null)
            {
                return replays;
            }
            TexasHoldem.Logic.Actions.Action action = replay.GetNextAction();
            while (action != null)
            {
                replays.Add(action.ToString());
                action = replay.GetNextAction();
            }
            return replays;
        }

        public bool RemoveSpectatorFromRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.RemoveSpectetorFromRoom(user);
            }
            else
            {
                return false;
            }
        }

        public bool AddSpectatorToRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.AddSpectetorToRoom(user);
            }
            else
            {
                return false;
            }
        }

        public IGame GetGameById(int id)
        {
            return _gameCenter.GetRoomById(id);
        }


        public List<IGame> GetAllActiveGames()
        {
            List<IGame> toReturn = _gameCenter.GetAllActiveGame();
            return toReturn;
        }
   
        public List<IGame> GetAllActiveGamesAUserCanJoin(int userId)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            List<IGame> toReturn = new List<IGame>();
            if (user != null)
            {
                toReturn = _gameCenter.GetAllActiveGamesAUserCanJoin(user);
            }

            return toReturn;
        }

        public List<IGame> GetAllGames()
        {
            List<IGame> toReturn = _gameCenter.GetAllGames();
            return toReturn;
        }

        public List<IGame> GetSpectateableGames()
        {
            List<IGame> toReturn = _gameCenter.GetAllSpectetorGame();
            return toReturn;
        }

        public List<IGame> GetGamesByPotSize(int potSize)
        {
            List<IGame> toReturn = _gameCenter.GetAllGamesByPotSize(potSize);
            return toReturn;
        }

        public List<IGame> GetGamesByGameMode(GameMode gm)
        {
            List<IGame> toReturn = _gameCenter.GetGamesByGameMode(gm);
            return toReturn;
        }

        //return list of games by buy in policy
        public List<IGame> GetGamesByBuyInPolicy(int buyIn)
        {
            List<IGame> toReturn = _gameCenter.GetGamesByBuyInPolicy(buyIn);
            return toReturn;
        }

        //return list of games by min player in room
        public List<IGame> GetGamesByMinPlayer(int min)
        {
            List<IGame> toReturn = _gameCenter.GetGamesByMinPlayer(min);
            return toReturn;
        }

        //return list of games by min bet in room
        public List<IGame> GetGamesByMinBet(int minBet)
        {
            List<IGame> toRetun = _gameCenter.GetGamesByMinBet(minBet);
            return toRetun;
        }

        //return list of games by starting chip policy
        public List<IGame> GetGamesByStartingChip(int startingChip)
        {
            List<IGame> toRetun = _gameCenter.GetGamesByStartingChip(startingChip);
            return toRetun;
        }

        //return list of games by max player in room
        public List<IGame> GetGamesByMaxPlayer(int max)
        {
            List<IGame> toReturn = _gameCenter.GetGamesByMaxPlayer(max);
            return toReturn;
        }

        public void SendMessageToClientGameData(GameDataCommMessage gameDataMes)
        {
            _serverHandler.HandleEvent(gameDataMes);
        }

        //TODO: not used. maybe remove?
        public void SendMessageToClientResponse(ResponeCommMessage resp)
        {
            _serverHandler.HandleEvent(resp);
        }

        //check player is in the game room 
        public bool CanSendPlayerBrodcast(int playerId, int roomId)
        {
            bool isUserExist = _systemControl.IsUserExist(playerId);
            if (!isUserExist)
            {
                return false;
            }
            IUser user = _systemControl.GetUserWithId(playerId);
            return _gameCenter.CanSendPlayerBrodcast(user,roomId);
        }

        //check spectetor is in the game room 
        public bool CanSendSpectetorBrodcast(int idSpectetor, int roomId)
        {
            bool isUserExist = _systemControl.IsUserExist(idSpectetor);
            if (!isUserExist)
            {
                return false;
            }
            IUser user = _systemControl.GetUserWithId(idSpectetor);
            return _gameCenter.CanSendSpectetorBrodcast(user, roomId);
        }

        //check id sender is player, reciver exist, + rules if can send this to reciver
        public bool CanSendPlayerWhisper(int idSender, string reciverUsername, int roomId)
        {
            bool toReturn = false;
            bool senderExist = _systemControl.IsUserExist(idSender);
            if (!senderExist)
            {
                return toReturn;
            }
            IUser reciver = _systemControl.GetUserByUserName(reciverUsername);
            if(reciver == null)
            {
                return toReturn;
            }
            IUser sender = _systemControl.GetUserWithId(idSender);
            toReturn = _gameCenter.CanSendPlayerWhisper(sender, reciver, roomId);
            return toReturn;
        }

        //check id sender is spectetor, reciver exist, + rules if can send this to reciver
        public bool CanSendSpectetorWhisper(int idSender, string reciverUsername, int roomId)
        {
            bool toReturn = false;
            bool senderExist = _systemControl.IsUserExist(idSender);
            if (!senderExist)
            {
                return toReturn;
            }
            IUser reciver = _systemControl.GetUserByUserName(reciverUsername);
            if (reciver == null)
            {
                return toReturn;
            }
            IUser sender = _systemControl.GetUserWithId(idSender);
            toReturn = _gameCenter.CanSendSpectetorWhisper(sender, reciver, roomId);
            return toReturn;
        }
    }
}