using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class GameBridge : IGameBridge
    {
        private readonly GameServiceHandler _gameService;
        private readonly UserServiceHandler _userService;

        public GameBridge()
        {
            _gameService = new GameServiceHandler();
            _userService = new UserServiceHandler();
        }

        private int MakeRoomHelper(int userId, int roomId)
        {
            User user = _userService.GetUserFromId(userId);
            if (user != null)
            {
                if (_gameService.CreateNewRoomWithRoomId(roomId, userId, 100, false, GameMode.NoLimit, 2, 2, 0, 1))
                {
                    return roomId;
                }
            }
            return -1;
        }

        public bool CreateGameRoom(int userId, int roomId)
        {
            if (!DoesRoomExist(roomId))
            {
                return MakeRoomHelper(userId, roomId) != -1;
            }
            return false;
        }

        public int CreateGameRoom(int userId)
        {
            return MakeRoomHelper(userId, _gameService.GetNextFreeRoomId());
        }

        public bool RemoveGameRoom(int id)
        {
            return _gameService.RemoveRoom(id);
        }

        public int GetNextFreeRoomId()
        {
            return _gameService.GetNextFreeRoomId();
        }

        public bool DoesRoomExist(int id)
        {
            return _gameService.GetGameById(id) != null;
        }

        public bool IsUserInRoom(int userId, int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                var roomPlayers = game._players;
                var roomSpect = game._spectatores;
                return roomPlayers.Exists(p => p.Id == userId) ||
                       roomSpect.Exists(s => s.Id == userId); 
            }
            return false;
        }

        public bool IsRoomActive(int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            return room != null && room._isActiveGame;
        }

        public bool StartGame(int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            if (room != null)
            {
                return _gameService.MakeRoomActive(_gameService.GetGameById(roomId)); 
            }
            return false;
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                List<int> toReturn = game._players.ConvertAll(p => p.Id);
                return toReturn; 
            }
            return new List<int>();
        }

        private List<int> GamesToIds(List<GameRoom> games)
        {
            if (games != null)
            {
                List<int> toReturn = games.ConvertAll(g => g._id);
                return toReturn; 
            }
            return null;
        }

        public List<int> ListAvailableGamesByUserRank(int userRank)
        {
            List<GameRoom> allGames = _gameService.GetAvaiableGamesByUserRank(userRank);
            return GamesToIds(allGames);
        }

        public List<int> ListSpectateableRooms()
        {
            List<GameRoom> allGames = _gameService.GetSpectateableGames();
            return GamesToIds(allGames);
        }

        public List<int> GetAllGames()
        {
            var allGames = _gameService.GetAllGames();
            return GamesToIds(allGames);
        }

        public int GetDealerId(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                return game._players[game._dealerPos].Id; 
            }
            return -1;
        }

        public int GetBbId(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                int bbPos = (game._dealerPos + 2) % game._players.Count;
                return game._players[bbPos].Id;
            }
            return -1;
        }

        public int GetSbId(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                int sbPos = (game._dealerPos + 1) % game._players.Count;
                return game._players[sbPos].Id;
            }
            return - 1;
        }

        public int GetDeckSize(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            if (game != null)
            {
                return game._deck.NumOfCards;
            }
            return -1;
        }

        public int GetCurrPlayer(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            if (game != null)
            {
                int pos = (game._actionPos) % game._players.Count;
                return game._players[pos].Id;
            }
            return -1;
        }

        public int GetSbSize(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            if (game != null)
            {
                return game._sb;
            }
            return -1;
        }

        public int GetPotSize(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            if (game != null)
            {
                return game._potCount;
            }
            return -1;
        }

        public List<int> GetWinner(int gameId)
        {
            var winners = _gameService.FindWinner(gameId);
            if (winners != null)
            {
                return winners.ConvertAll(p => p.Id);
            }
            return new List<int>();
        }

        //public bool Fold(int userId, int roomId)
        //{
        //    //var player = FindPlayerInGame(userId, roomId);
        //    //if (player != null)
        //    //{
        //    //    player.Fold();
        //    //    return true;
        //    //}
        //    //return false;
        //    var game = _gameService.GetGameById(roomId);
        //    if (game != null)
        //    {
        //        new GameManager((ConcreteGameRoom) game).Fold();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Check(int userId, int roomId)
        //{
        //    //var player = FindPlayerInGame(userId, roomId);
        //    //if (player != null)
        //    //{
        //    //    player.Check();
        //    //    return true;
        //    //}
        //    //return false;
        //    var game = _gameService.GetGameById(roomId);
        //    if (game != null)
        //    {
        //        new GameManager((ConcreteGameRoom) game).Check();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Call(int userId, int roomId, int amount)
        //{
        //    //var player = FindPlayerInGame(userId, roomId);
        //    //if (player != null)
        //    //{
        //    //    player.Call(amount);
        //    //    return true;
        //    //}
        //    //return false;
        //    var game = _gameService.GetGameById(roomId);
        //    if (game != null)
        //    {
        //        new GameManager((ConcreteGameRoom) game).Call(amount);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Raise(int userId, int roomId, int amount)
        //{
        //    //var game = _gameService.GetGameById(roomId);
        //    //var player = _userService.GetPlayer(userId, roomId);
        //    //if (CheckCurrPlayerIsPlayer(userId, game))
        //    //{
        //    //    player.Raise(amount, game);
        //    //    return true;
        //    //}
        //    //return false;
        //    var game = _gameService.GetGameById(roomId);
        //    if (game != null)
        //    {
        //        new GameManager((ConcreteGameRoom) game).Raise(amount);
        //        return true;
        //    }
        //    return false;
        //}

    }
}
