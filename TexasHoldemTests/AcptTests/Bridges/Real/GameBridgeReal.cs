using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;

namespace TexasHoldemTests.AcptTests.Bridges.Real
{
    class GameBridgeReal : IGameBridge
    {
        private GameServiceHandler _gameService;
        private UserServiceHandler _userService;

        private const int MinMoney = 0;
        private const int MaxMoney = Int32.MaxValue;
        private const int SmallBlind= 1;
        private const int BigBlind= 2;

        private readonly Random _rand;

        public GameBridgeReal()
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
                        //return game._id;
                        return 0;
                        //TODO: wait for Yrden to dix GameRoom
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
            //return _gameService.GetGameById(roomId)
            return false;
            //TODO: fix this after Yarden adds player list 
        }

        public bool IsRoomActive(int roomId)
        {
            throw new NotImplementedException();
        }

        public bool StartGame(int roomId)
        {
            return _gameService.MakeRoomActive(_gameService.GetGameById(roomId));
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            var games = _gameService.GetGameById(roomId);
            //TODO: wait for Yerden's update
            return null;
        }

        private List<int> GamesToIds(List<GameRoom> games)
        {
            List<int> toReturn = new List<int>();
            games.ForEach(game =>
            {
                //toReturn.Add(game.Id);
                //TODO: wait for Yarden
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
            throw new NotImplementedException();
        }

        public int GetBbId(int roomId)
        {
            throw new NotImplementedException();
        }

        public int GetSbId(int roomId)
        {
            throw new NotImplementedException();
        }

        public int GetDeckSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetCurrPlayer(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetSbSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetPotSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetWinner(int gameId)
        {
            return _gameService.FindWinner(gameId).Id;
        }

        public bool Fold(int userId, int roomId)
        {
            throw new NotImplementedException();
        }

        public bool Check(int userId, int roomId)
        {
            throw new NotImplementedException();
        }

        public bool Call(int userId, int roomId, int amount)
        {
            throw new NotImplementedException();
        }

        public bool Raise(int userId, int roomId, int amount)
        {
            throw new NotImplementedException();
        }

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
