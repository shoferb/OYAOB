using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.Service
{
    public class GameServiceHandler
    {
        //TODO - Done! -  Search/ filter active games by: player name/ pot size/ game preference.
        //TODO Join existing games. i Get the User 
        //TODO Spectate active game.
        //TODO Leave a game.
        //TODO - Done! - Find all active games which the user can join.
        private readonly GameCenter _gameCenter;
        private readonly SystemControl _systemControl;
        private ReplayManager _replayManager;
        private static ServerEventHandler serverHandler = new ServerEventHandler();

        public GameServiceHandler()
        {
            _gameCenter = GameCenter.Instance;
            _systemControl = SystemControl.SystemControlInstance;
            serverHandler = new ServerEventHandler();
            _replayManager = ReplayManager.ReplayManagerInstance;
        }

        public IGame GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }

        public static void sendMessageToClient(IUser player, int roomId, CommunicationMessage.ActionType action, bool isSucceed, string msg)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewRoomWithRoomId(int roomId,int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            return _gameCenter.CreateNewRoomWithRoomId(roomId, user, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        //TODO: fix this
        //create room and add to games list game center
        public bool CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            IUser user = _systemControl.GetUserWithId(userId);
            int roomID = _gameCenter.GetNextIdRoom();
            return _gameCenter.CreateNewRoomWithRoomId(roomID, user, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }
        
        //public List<Tuple<int, int>> GetGamesAvailableForReplayByUser(int userID)
        //{
        //    return _gameCenter.GetGamesAvailableForReplayByUser(userID);
        //}

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

        /*//todo fix this ? remove
        public List<IGame> GetAvaiableGamesByUserRank(int userPoints)
        {
            return GameCenter.Instance.GetAvaiableGamesByUserRank(userPoints);
        }*/

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

        //public GameRoom GetGameById(int Id)
        public IGame GetGameById(int id)
        {
            return _gameCenter.GetRoomById(id);
        }


        public List<IGame> GetAllActiveGames()
        {
            List<IGame> toReturn = GameCenter.Instance.GetAllActiveGame();
            return toReturn;
        }
   
        public List<IGame> GetAllActiveGamesAUserCanJoin(int userId)
        {
            IUser user = SystemControl.SystemControlInstance.GetUserWithId(userId);
            List<IGame> toReturn = new List<IGame>();
            if (user != null)
            {
                toReturn = GameCenter.Instance.GetAllActiveGamesAUserCanJoin(user);
            }

            return toReturn;
        }

        //public List<GameRoom> GetAllGames()
        public List<IGame> GetAllGames()
        {
            List<IGame> toReturn = GameCenter.Instance.GetAllGames();
            return toReturn;
        }

        //public  List<GameRoom> GetSpectateableGames()
        public List<IGame> GetSpectateableGames()
        {
            List<IGame> toReturn = GameCenter.Instance.GetAllSpectetorGame();
            return toReturn;
        }

        //public List<GameRoom> GetGamesByPotSize(int potSize)
        public List<IGame> GetGamesByPotSize(int potSize)
        {
            List<IGame> toReturn = GameCenter.Instance.GetAllGamesByPotSize(potSize);
            return toReturn;
        }

        //return list of games with game mode:
        //limit / no - limit / pot limit
        public List<IGame> GetGamesByGameMode(GameMode gm)
        {
            List<IGame> toReturn = GameCenter.Instance.GetGamesByGameMode(gm);
            return toReturn;
        }

        //return list of games by buy in policy
        public List<IGame> GetGamesByBuyInPolicy(int buyIn)
        {
            List<IGame> toReturn = GameCenter.Instance.GetGamesByBuyInPolicy(buyIn);
            return toReturn;
        }

        //return list of games by min player in room
        public List<IGame> GetGamesByMinPlayer(int min)
        {
            List<IGame> toReturn = GameCenter.Instance.GetGamesByMinPlayer(min);
            return toReturn;
        }

        //return list of games by min bet in room
        public List<IGame> GetGamesByMinBet(int minBet)
        {
            List<IGame> toRetun = GameCenter.Instance.GetGamesByMinBet(minBet);
            return toRetun;
        }

        //return list of games by starting chip policy
        public List<IGame> GetGamesByStartingChip(int startingChip)
        {
            List<IGame> toRetun = GameCenter.Instance.GetGamesByStartingChip(startingChip);
            return toRetun;
        }

        //return list of games by max player in room
        public List<IGame> GetGamesByMaxPlayer(int max)
        {
            List<IGame> toReturn = GameCenter.Instance.GetGamesByMaxPlayer(max);
            return toReturn;
        }

        public static void SendMessageToClientGameData(GameDataCommMessage gameDataMes)
        {
            serverHandler.HandleEvent(gameDataMes);
        }

        public static void SendMessageToClientResponse(ResponeCommMessage resp)
        {
            serverHandler.HandleEvent(resp);
        }
    }
}