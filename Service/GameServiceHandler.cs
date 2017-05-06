using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.Service
{
    public class GameServiceHandler : ServiceHandler
    {
        //TODO Search/ filter active games by: player name/ pot size/ game preference.
        //TODO Join existing games. i Get the User 
        //TODO Spectate active game.
        //TODO Leave a game.
        //TODO Find all active games which the user can join.
        private readonly GameCenter _gameCenter;
        private readonly SystemControl _systemControl;
        private static ServerEventHandler serverHandler;

        public GameServiceHandler()
        {
            _gameCenter = GameCenter.Instance;
            _systemControl = SystemControl.SystemControlInstance;
            serverHandler = new ServerEventHandler();
        }

        public GameRoom GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }

        public static void sendMessageToClient(IUser player, int roomId, CommunicationMessage.ActionType action, bool isSucceed, string msg)
        {
            throw new NotImplementedException();
        }

        //TODO +Game center to fix 
        public bool CreateNewRoomWithRoomId(int roomId,int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            return _gameCenter.CreateNewRoomWithRoomId(roomId,userId, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        //TODO: fix this
        //create room and add to games list game center
        public bool CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            return _gameCenter.CreateNewRoom(userId, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        public int GetNextFreeRoomId()
        {
            return _gameCenter.GetNextIdRoom();
        }


        //public GameRoom GetGameById(int Id)
        public GameRoom GetGameById(int id)
        {
            return _gameCenter.GetRoomById(id);
        }

        public bool AddPlayerToRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.AddPlayerToRoom(userId);
            }
            else return false;
        }

        public bool AddSpectatorToRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.AddSpectetorToRoom(userId);
            }
            else return false;
        }

        public bool RemovePlayerFromRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.RemovePlayerFromRoom(userId);
            }
            else return false;
        }

        public bool RemoveSpectatorFromRoom(int userId, int roomId)
        {
            IGame gameRoom = _gameCenter.GetRoomById(roomId);
            IUser user = _systemControl.GetUserWithId(userId);
            if (gameRoom != null && user != null)
            {
                return gameRoom.RemoveSpectetorFromRoom(userId);
            }
            else return false;
        }

        public List<GameRoom> GetAvaiableGamesByUserRank(int userPoints)
        {
            var allGames = _gameCenter.Games;
            return allGames.FindAll(game => 
                game.MinRank <= userPoints && game.MaxRank >= userPoints);
        }

        public bool MakeRoomActive(GameRoom room)
        {
            //TODO
            return false;
        }

        public bool RemoveRoom(int gameId)
        {
            return _gameCenter.RemoveRoom(gameId);
        }

        //TODO This is Use case
        public List<GameRoom> GetAllActiveGames()
        {
            List<GameRoom>  toReturn = GameCenter.Instance.GetAllActiveGame();
            return toReturn;
        }

        public List<GameRoom> GetAllActiveGamesAUserCanJoin(int userId)
        {

            return _gameCenter.GetAllActiveGamesAUserCanJoin(userId);
        }

        //public List<GameRoom> GetAllGames()
        public List<GameRoom> GetAllGames()
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetAllGames();
            return toReturn;
        }

        //public  List<GameRoom> GetSpectateableGames()
        public List<GameRoom> GetSpectateableGames()
        {
            List<GameRoom>  toReturn = GameCenter.Instance.GetAllSpectetorGame();
            return toReturn;
        }

        //public List<GameRoom> GetGamesByPotSize(int potSize)
        public List<GameRoom> GetGamesByPotSize(int potSize)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetAllGamesByPotSize(potSize);
            return toReturn;
        }

        public bool IsGameExist(int roomId)
        {
            bool toReturn = GameCenter.Instance.IsRoomExist(roomId);
            return toReturn;
        }

        public bool IsGameCanSpectete(int roomId)
        {
            bool toReturn = GameCenter.Instance.IsGameCanSpectete(roomId);
            return toReturn;
        }


        //return if game is active game
        public bool IsGameActive(int roomId)
        {
            bool toReturn = GameCenter.Instance.IsGameActive(roomId);
            return toReturn;
        }


        //return list of games with game mode:
        //limit / no - limit / pot limit
        public List<GameRoom> GetGamesByGameMode(GameMode gm)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetGamesByGameMode(gm);
            return toReturn;
        }

        //return list of games by buy in policy
        public List<GameRoom> GetGamesByBuyInPolicy(int buyIn)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetGamesByBuyInPolicy(buyIn);
            return toReturn;
        }


        //return list of games by min player in room
        public List<GameRoom> GetGamesByMinPlayer(int min)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetGamesByMinPlayer(min);
            return toReturn;
        }



        //return list of games by min bet in room
        public List<GameRoom> GetGamesByMinBet(int minBet)
        {
            List<GameRoom> toRetun = GameCenter.Instance.GetGamesByMinBet(minBet);
            return toRetun;
        }



        //return list of games by starting chip policy
        public List<GameRoom> GetGamesByStartingChip(int startingChip)
        {
            List<GameRoom> toRetun = GameCenter.Instance.GetGamesByStartingChip(startingChip);
            return toRetun;
        }

        //TODO: rename this. the name is opposite to what it does
        //return list of games by min player in room
        public List<GameRoom> GetGamesByMaxPlayer(int max)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetGamesByMinPlayer(max);
            return toReturn;
        }

        
        //TODO: probably not needed
        public String Displaymoves(List<Tuple<Logic.Game.GameMove, bool, int, int>> moves)
        {
            return GameCenter.Instance.Displaymoves(moves);
        }


        
        //TODO: probably not needed
        public int GetBetFromUser(int bet)
        {
            return bet;
        }

       
        //use only this
        //TODO: replace random with method that waits until a responce from client was accepted (with some flag / value)
        public Tuple<Logic.Game.GameMove, int> SendUserAvailableMovesAndGetChoosen(List<Tuple<Logic.Game.GameMove, bool, int, int>> moves)
        {
            
            Displaymoves(moves);
            Tuple<Logic.Game.GameMove, int> moveAndBet = GetRandomMove(moves);
            bool isValidMove = IsValidMove(moves, moveAndBet);
            while (!isValidMove)
            {
                moveAndBet = GetRandomMove(moves);
                IsValidMove(moves, moveAndBet);
            }

            var ToReturn = SendMoveBackToPlayer(moveAndBet);
            return ToReturn;
        }

        private Tuple<Logic.Game.GameMove, int> SendMoveBackToPlayer(Tuple<GameMove, int> moveAndBet)
        {
            return GameCenter.Instance.SendMoveBackToPlayer(moveAndBet);
        }

        private bool IsValidMove(List<Tuple<GameMove, bool, int, int>> moves, Tuple<GameMove, int> moveAndBet)
        {
            return GameCenter.Instance.IsValidMove(moves, moveAndBet);
        }

        public Tuple<Logic.Game.GameMove, int> GetRandomMove(List<Tuple<GameMove, bool, int, int>> moves)
        {
            return GameCenter.Instance.GetRandomMove(moves);
        }

        public List<Tuple<int, int>> GetGamesAvailableForReplayByUser(int userID)
        {
            return _gameCenter.GetGamesAvailableForReplayByUser(userID);
        }

        public List<string> GetGameReplay(int roomId, int gameNum, int userId)
        {
            GameReplay replay =_gameCenter.GetGameReplay(roomId, gameNum, userId);
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

        public bool SaveFavoriteMove(int roomID, int gameID, int userID, int actionNum)
        {
            return _gameCenter.saveActionFromGameReplay(roomID, gameID, userID, actionNum);
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