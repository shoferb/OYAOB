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
        private GameServiceHandler _gameService;
        private UserServiceHandler _userService;

        private const int MinMoney = 0;
        private const int MaxMoney = Int32.MaxValue;
        private const int SmallBlind = 1;
        private const int BigBlind = 2;

        private readonly Random _rand;

        public GameBridge()
        {
            //TODO: init services here
            _rand = new Random();
        }

        private int MakeRoomHelper(int userId, int roomId)
        {
            string name = _rand.Next().ToString();
            User user = _userService.GetUserFromId(userId);
            if (user != null)
            {
                var game = _gameService.CreateGameRoom(userId, user.Money, roomId, name,
                    SmallBlind, BigBlind, MinMoney, MaxMoney, 1);
                if (game != null)
                {
                    Player player = new Player(0, 100, user.Id, user.Name, user.MemberName,
                        user.Password, user.Points, user.Money, user.Email, roomId);
                    if (_gameService.AddPlayerToRoom(player, game))
                    {
                        return game._id.GetHashCode();
                    }
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
            return _gameService.GetGameById(id) == null;
        }

        public bool IsUserInRoom(int userId, int roomId)
        {
            var roomPlayers = _gameService.GetGameById(roomId)._players;
            var roomSpect = _gameService.GetGameById(roomId)._spectatores;

            return roomPlayers.Exists(p => p.Id == userId) ||
                   roomSpect.Exists(s => s.Id == userId);
        }

        public bool IsRoomActive(int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            return room != null && room._isGameOver;
        }

        public bool StartGame(int roomId)
        {
            return _gameService.MakeRoomActive(_gameService.GetGameById(roomId));
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            var players = _gameService.GetGameById(roomId)._players;
            List<int> toReturn = new List<int>();
            players.ForEach(p =>
            {
                toReturn.Add(p.Id);
            });
            return toReturn;
        }

        private List<int> GamesToIds(List<GameRoom> games)
        {
            List<int> toReturn = new List<int>();
            games.ForEach(game =>
            {
                toReturn.Add(game._id.GetHashCode());
            });
            return toReturn;
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
            return game._players[game._dealerPos].Id;
        }

        public int GetBbId(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            int bbPos = (game._dealerPos + 2) % game._players.Count;
            return game._players[bbPos].Id;
        }

        public int GetSbId(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            int sbPos = (game._dealerPos + 1) % game._players.Count;
            return game._players[sbPos].Id;
        }

        public int GetDeckSize(int gameId)
        {
            return _gameService.GetGameById(gameId)._deck.NumOfCards;
        }

        public int GetCurrPlayer(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            int pos = (game._actionPos) % game._players.Count;
            return game._players[pos].Id;
        }

        public int GetSbSize(int gameId)
        {
            return _gameService.GetGameById(gameId)._sb;
        }

        public int GetPotSize(int gameId)
        {
            return _gameService.GetGameById(gameId)._potCount;
        }

        public int GetWinner(int gameId)
        {
            return _gameService.FindWinner(gameId).Id;
        }

        private bool CheckCurrPlayerIsPlayer(int playerId, GameRoom room)
        {
            Player player = _userService.GetPlayer(playerId, room._id.GetHashCode());
            if (player != null && room != null)
            {
                var game = _gameService.GetGameById(room._id.GetHashCode());
                var currPlayer = game._players[game._actionPos].Id;
                if (currPlayer == player.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private Player FindPlayerInGame(int userId, int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            var player = _userService.GetPlayer(userId, roomId);
            if (CheckCurrPlayerIsPlayer(userId, game))
            {
                return player;
            }
            return null;
        }

        public bool Fold(int userId, int roomId)
        {
            //var player = FindPlayerInGame(userId, roomId);
            //if (player != null)
            //{
            //    player.Fold();
            //    return true;
            //}
            //return false;
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                new GameManager((ConcreteGameRoom) game).Fold();
                return true;
            }
            return false;
        }

        public bool Check(int userId, int roomId)
        {
            //var player = FindPlayerInGame(userId, roomId);
            //if (player != null)
            //{
            //    player.Check();
            //    return true;
            //}
            //return false;
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                new GameManager((ConcreteGameRoom) game).Check();
                return true;
            }
            return false;
        }

        public bool Call(int userId, int roomId, int amount)
        {
            //var player = FindPlayerInGame(userId, roomId);
            //if (player != null)
            //{
            //    player.Call(amount);
            //    return true;
            //}
            //return false;
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                new GameManager((ConcreteGameRoom) game).Call(amount);
                return true;
            }
            return false;
        }

        public bool Raise(int userId, int roomId, int amount)
        {
            //var game = _gameService.GetGameById(roomId);
            //var player = _userService.GetPlayer(userId, roomId);
            //if (CheckCurrPlayerIsPlayer(userId, game))
            //{
            //    player.Raise(amount, game);
            //    return true;
            //}
            //return false;
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                new GameManager((ConcreteGameRoom) game).Raise(amount);
                return true;
            }
            return false;
        }

        public bool DealFlop(int gameId)
        {
            const int numOfCardsToDeal = 3;
            for (int i = 0; i < numOfCardsToDeal; i++)
            {
                DealSingleCardToTable(gameId);
            }
            return true;
        }

        public bool DealSingleCardToTable(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            game.AddNewPublicCard();
            return true;
        }
    }
}
