using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using Action = TexasHoldem.Logic.Game.Action;

namespace TexasHoldem.Service
{
    public class GameServiceHandler : ServiceHandler
    {
        private readonly Dictionary<GameRoom, GameManager> _roomToManagerDictionary;
        private readonly GameCenter _gameCenter;

        public GameServiceHandler()
        {
            _roomToManagerDictionary = new Dictionary<GameRoom, GameManager>();
            _gameCenter = GameCenter.Instance;
        }

        private GameManager GetManagerForGame(GameRoom room)
        {
            if (_roomToManagerDictionary.ContainsKey(room))
            {
                return _roomToManagerDictionary[room];
            }
            GameManager manager = ((GameRoom) room)._gm;
            _roomToManagerDictionary.Add(room, manager);
            return manager;
        }

        // public GameRoom GetGameFromId(int gameId)
        public GameRoom GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }

        //create room and add to games list game center
        public bool CreateNewRoomWithRoomId(int roomId,int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            return GameCenter.Instance.CreateNewRoomWithRoomId(roomId,userId, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        //create room and add to games list game center
        public bool CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            return GameCenter.Instance.CreateNewRoom(userId, startingChip, isSpectetor, gameModeChosen,
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

        public bool AddPlayerToRoom(int userId, int roomId, int amountOfChips)
        {
            return _gameCenter.AddPlayerToRoom(roomId, userId);
        }

        public bool AddSpectatorToRoom(int userId, int roomId)
        {
            return _gameCenter.AddSpectetorToRoom(roomId, userId);
        }

        public bool RemoveUserFromRoom(int userId, int roomId)
        {
            GameRoom room = _gameCenter.GetRoomById(roomId);
            if (room != null)
	        {
		        if (room.Players.Exists(p => p.user.Id() == userId))
                {
                    return _gameCenter.RemovePlayerFromRoom(roomId, userId);
                }
	            if (room.Spectatores.Exists(s => s.user.Id() == userId))
	            {
	                return _gameCenter.RemoveSpectetorFromRoom(roomId, userId);
	            }
	        }
            return false;
        }

        public List<GameRoom> GetAvaiableGamesByUserRank(int userPoints)
        {
            var allGames = _gameCenter.Games;
            return allGames.FindAll(game => 
                game.MinRank <= userPoints && game.MaxRank >= userPoints);
        }

        public bool MakeRoomActive(GameRoom room)
        {
            GameManager manager = GetManagerForGame(room);
            if (room.MinPlayersInRoom <= room.Players.Count && !room.IsActiveGame)
            {
                manager.Start();
                return true;
            }
            return false;
        }

        public bool RemoveRoom(int gameId)
        {
            return _gameCenter.RemoveRoom(gameId);
        }

        //public bool Fold(Player player, GameRoom room)
        //{
        //    var manager = GetManagerForGame(room);
        //    if (player.Equals(manager._currentPlayer))
        //    {
        //        manager.Fold();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Check(Player player, GameRoom room)
        //{
        //    var manager = GetManagerForGame(room);
        //    if (player.Equals(manager._currentPlayer))
        //    {
        //        manager.Check();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Call(Player player, GameRoom room)
        //{
        //    var manager = GetManagerForGame(room);
        //    if (player.Equals(manager._currentPlayer))
        //    {
        //        manager.Call();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Raise(Player player, GameRoom room, int sum)
        //{
        //    var manager = GetManagerForGame(room);
        //    if (player.Equals(manager._currentPlayer))
        //    {
        //        manager.Raise(sum);
        //        return true;
        //    }
        //    return false;
        //}

        //public List<Player> FindWinner(int gameId)
        //{
        //    List<Player> winningPlayers = new List<Player>();
        //    var room = _gameCenter.GetRoomById(gameId);
        //    var manager = GetManagerForGame(room);
        //    if (room != null && manager != null && manager._gameOver)
        //    {
        //        List<Player> activePlayers = room.players.FindAll(p => p.isPlayerActive);
        //        var winners = manager.FindWinner(room.PublicCards, activePlayers);
        //        winners.ForEach(handEval =>
        //        {
        //            winningPlayers.Add(handEval._player);
        //        });
        //    }
        //    return winningPlayers;
        //}

        //public List<GameRoom> GetAllActiveGames()
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
        //return list of games by min player in room
        public List<GameRoom> GetGamesByMaxPlayer(int max)
        {
            List<GameRoom> toReturn = GameCenter.Instance.GetGamesByMinPlayer(max);
            return toReturn;
        }

        public String Displaymoves(List<Tuple<Logic.Game.Action, bool, int, int>> moves)
        {
            return GameCenter.Instance.Displaymoves(moves);
        }


        

        public int GetBetFromUser(int bet)
        {
            return bet;
        }

       
        //use only this
        public Tuple<Logic.Game.Action, int> SendUserAvailableMovesAndGetChoosen(List<Tuple<Logic.Game.Action, bool, int, int>> moves)
        {
            
            Displaymoves(moves);
            Tuple<Logic.Game.Action, int> moveAndBet = GetRandomMove(moves);
            bool isValidMove = IsValidMove(moves, moveAndBet);
            while (!isValidMove)
            {
                moveAndBet = GetRandomMove(moves);
                IsValidMove(moves, moveAndBet);
            }

            var ToReturn = SendMoveBackToPlayer(moveAndBet);
            return ToReturn;
        }

        private Tuple<Logic.Game.Action, int> SendMoveBackToPlayer(Tuple<Action, int> moveAndBet)
        {
            return GameCenter.Instance.SendMoveBackToPlayer(moveAndBet);
        }

        private bool IsValidMove(List<Tuple<Action, bool, int, int>> moves, Tuple<Action, int> moveAndBet)
        {
            return GameCenter.Instance.IsValidMove(moves, moveAndBet);
        }

        public Tuple<Logic.Game.Action, int> GetRandomMove(List<Tuple<Action, bool, int, int>> moves)
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
    }
}