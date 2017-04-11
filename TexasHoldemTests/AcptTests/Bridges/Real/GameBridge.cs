using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges.Real
{
    class GameBridge : IGameBridge
    {
        private GameServiceHandler _gameService;
        private UserServiceHandler _userService;

        private const int MinMoney = 0;
        private const int MaxMoney = Int32.MaxValue;
        private const int SmallBlind= 1;
        private const int BigBlind= 2;

        private readonly Random _rand;

        public GameBridge()
        {
            //TODO: init services here
            _rand = new Random();
        }

        private int MakeRoomHelper(int userId, int roomId)
        {
            string name = _rand.Next().ToString();
            var game = _gameService.CreateGameRoom(roomId, name,
                SmallBlind, BigBlind, MinMoney, MaxMoney, 1);
            if (game != null)
            {
                User user = _userService.GetUserFromId(userId);
                if (user != null)
                {
                    Player player = new Player(user.Id, user.Name, user.MemberName, user.Password,
                        user.Points, user.Money, user.Email, roomId, false);
                    if (_gameService.AddPlayerToRoom(player, game))
                    {
                        return game.GameId;
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
            var roomPlayers = _gameService.GetGameById(roomId).RoomPlayers;
            var roomSpect = _gameService.GetGameById(roomId).RoomSpectetors;

            return (roomPlayers.Exists(p => p.Id == userId) ||
                roomSpect.Exists(s => s.Id == userId));
        }

        public bool IsRoomActive(int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            return room != null && room.IsActive;
        }

        public bool StartGame(int roomId)
        {
            return _gameService.MakeRoomActive(_gameService.GetGameById(roomId));
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            var players = _gameService.GetGameById(roomId).RoomPlayers;
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
                toReturn.Add(game.GameId);
            });
            return toReturn;
        }

        public List<int> ListAvailableGamesByUserRank(int userRank)
        {
            List<GameRoom> allGames = new List<GameRoom>(_gameService.GetAvaiableGamesByUserRank(userRank));
            return GamesToIds(allGames);
        }

        public List<int> ListSpectateableRooms()
        {
            List<GameRoom> allGames = new List<GameRoom>(_gameService.GetSpectateableGames());
            return GamesToIds(allGames);
        }

        public List<int> GetAllGames()
        {
            List<GameRoom> allGames = new List<GameRoom>(_gameService.GetAllGames());
            return GamesToIds(allGames);
        }

        public int GetDealerId(int roomId)
        {
            return _gameService.GetGameById(roomId).CurrentDealer.Id;
        }

        public int GetBbId(int roomId)
        {
            return _gameService.GetGameById(roomId).CurrentBb.Id;
        }

        public int GetSbId(int roomId)
        {
            return _gameService.GetGameById(roomId).CurrentSb.Id;
        }

        public int GetDeckSize(int gameId)
        {
            return _gameService.GetGameById(gameId).GetDeckSize();
        }

        public int GetCurrPlayer(int gameId)
        {
            return _gameService.GetGameById(gameId).CurrentPlayer.Id;
        }

        public int GetSbSize(int gameId)
        {
            return _gameService.GetGameById(gameId).Pot.SmallBlind;
        }

        public int GetPotSize(int gameId)
        {
            return _gameService.GetGameById(gameId).Pot.Amount;
        }

        public int GetWinner(int gameId)
        {
            return _gameService.FindWinner(gameId).Id;
        }

        private bool CheckCurrPlayerIsPlayer(int playerId, ConcreteGameRoom room)
        {
            Player player = _userService.GetPlayer(playerId, room.GameId);
            if (player != null && room != null)
            {
                var currPlayer = _gameService.GetGameById(room.GameId).CurrentPlayer;
                if (currPlayer.Equals(player))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Fold(int userId, int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (CheckCurrPlayerIsPlayer(userId, game))
            {
                game.Fold();
                return true;
            }
            return false;
        }

        public bool Check(int userId, int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (CheckCurrPlayerIsPlayer(userId, game))
            {
                game.Check();
                return true;
            }
            return false;
        }

        public bool Call(int userId, int roomId, int amount)
        {
            var game = _gameService.GetGameById(roomId);
            if (CheckCurrPlayerIsPlayer(userId, game))
            {
                game.Call();
                return true;
            }
            return false;
        }

        public bool Raise(int userId, int roomId, int amount)
        {
            var game = _gameService.GetGameById(roomId);
            if (CheckCurrPlayerIsPlayer(userId, game))
            {
                game.Raise(amount);
                return true;
            }
            return false;
        }

        //TODO: figure out what to do with these
        public bool DealFirstCards(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool DealFlop(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool DealSingleCardToTable(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
