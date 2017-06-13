using System;
using System.Collections.Generic;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
//using TexasHoldemShared.CommMessaages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    public class GameBridge : IGameBridge
    {
        private readonly GameServiceHandler _gameService;
        private readonly UserServiceHandler _userService;

        public GameBridge(GameCenter gc, SystemControl sys, LogControl log, ReplayManager replay)
        {
            var ses = new SessionIdHandler();
            _gameService = new GameServiceHandler(gc, sys, log, replay,ses);
            _userService = new UserServiceHandler(gc, sys);
            
    }

        private int MakeRoomHelper(int userId, int startingCheap)
        {
            IUser user = _userService.GetUserById(userId);
            if (user != null)
            {
                int roomId = _gameService.CreateNewRoom(userId, startingCheap, true, GameMode.NoLimit, 2, 8, 0, 10);
                if (roomId >= 0)
                {
                    return roomId;
                }
            }
            return -1;
        }

        public int CreateGameRoom(int userId, int startingCheap)
        {
            return MakeRoomHelper(userId, startingCheap);
        }

        public int CreateGameRoomWithPref(int userId, int startingCheap, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            IUser user = _userService.GetUserById(userId);
            if (user != null)
            {
                int roomId = _gameService.CreateNewRoom(userId, startingCheap, isSpectetor, gameModeChosen, minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
                if (roomId >= 0)
                {
                    return roomId;
                }
            }
            return -1;
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
                var roomPlayers = game.GetPlayersInRoom();
                var roomSpectators = game.GetSpectetorInRoom();
                return roomPlayers.Exists(p => p.user.Id() == userId) || roomSpectators.Exists(s => s.user.Id() == userId);
            }
            return false;
        }

        public bool IsRoomActive(int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            return room != null && room.IsGameActive();
        }

        public bool StartGame(int userId, int roomId)
        {
            var room = _gameService.GetGameById(roomId);
            if (room != null)
            {
                return ActionSuccedded(_gameService.DoAction(userId, CommunicationMessage.ActionType.StartGame, 0, roomId));
            }
            return false;
        }

        public List<Player> GetPlayersInRoom(int roomId)
        {
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                List<Player> gmPlayers = _gameService.GetPlayersInRoom(roomId);
                return gmPlayers;
            }
            return null;
        }

        public List<int> GetIdPlayersInRoom(int roomId)
        {
            List<int> res = null;
            var game = _gameService.GetGameById(roomId);
            if (game != null)
            {
                List<Player> gmPlayers = _gameService.GetPlayersInRoom(roomId);
                foreach (Player p in gmPlayers)
                {
                    res.Add(p.user.Id());
                }
                return res;
            }
            return res;
        }

        private List<int> GamesToIds(List<IGame> games)
        {
            if (games != null)
            {
                List<int> toReturn = games.ConvertAll(g => g.Id);
                return toReturn;
            }
            return null;
        }

        public List<int> ListAvailableGamesByUserRank(int userId)
        {
            List<IGame> allGames = _gameService.GetAllActiveGamesAUserCanJoin(userId);
            return GamesToIds(allGames);
        }

        public List<IGame> ListSpectateableRooms()
        {
            return _gameService.GetSpectateableGames();
        }

        public List<IGame> GetGamesByGameMode(GameMode mode)
        {
            return _gameService.GetGamesByGameMode(mode);
        }

        public bool DoAction(int userId, CommunicationMessage.ActionType action, int amount, int roomId)
        {
            return ActionSuccedded(_gameService.DoAction(userId, action, amount, roomId));
        }

        public List<int> GetAllGamesId()
        {
            var allGames = _gameService.GetAllGames();
            return GamesToIds(allGames);
        }

        public List<IGame> GetAllGames()
        {
            var allGames = _gameService.GetAllGames();
            return allGames;
        }

        private bool ActionSuccedded(IEnumerator<ActionResultInfo> results)
        {
            results.MoveNext();
            ActionResultInfo result = results.Current;
            return result.GameData.IsSucceed;
        }


    }
}
