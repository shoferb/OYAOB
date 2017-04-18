using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
    {
        private readonly Dictionary<GameRoom, GameManager> _roomToManagerDictionary;
        private readonly GameCenter _gameCenter;
        private readonly SystemControl _sysControl;

        protected GameServiceHandler()
        {
            _roomToManagerDictionary = new Dictionary<GameRoom, GameManager>();
            _gameCenter = new GameCenter();
            _sysControl = new SystemControl();
        }

        private GameManager GetManagerForGame(GameRoom room)
        {
            if (_roomToManagerDictionary.ContainsKey(room))
            {
                return _roomToManagerDictionary[room];
            }
            GameManager manager = new GameManager((ConcreteGameRoom)room);
            _roomToManagerDictionary.Add(room, manager);
            return manager;
        }

        public ConcreteGameRoom GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }
        public abstract GameRoom CreateGameRoom(int userId, int chipsInGame, int roomId, 
            string roomName, int sb, int bb, int minMoney, int maxMoney, int gameNum);

        public abstract int GetNextFreeRoomId();
        public abstract GameRoom GetGameById(int id);

        public bool AddPlayerToRoom(int userId, int roomId, int amountOfChips)
        {
            return _gameCenter.AddPlayerToRoom(roomId, userId, amountOfChips);
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
		        if (room._players.Exists(p => p.Id == userId))
                {
                    return _gameCenter.RemovePlayerFromRoom(roomId, userId);
                }
	            if (room._spectatores.Exists(s => s.Id == userId))
	            {
	                return _gameCenter.RemoveSpectetorFromRoom(roomId, userId);
	            }
	        }
            return false;
        }
        public abstract bool MakeRoomActive(GameRoom room);

        public bool RemoveRoom(int gameId)
        {
            return _gameCenter.RemoveRoom(gameId);
        }

        public bool Fold(Player player, GameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Fold();
                return true;
            }
            return false;
        }

        public bool Check(Player player, GameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Check();
                return true;
            }
            return false;
        }

        public bool Call(Player player, GameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Call();
                return true;
            }
            return false;
        }

        public bool Raise(Player player, GameRoom room, int sum)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Raise(sum);
                return true;
            }
            return false;
        }
        public abstract Player FindWinner(int gameId);
        public abstract List<GameRoom> GetAllGames();
        public abstract List<GameRoom> GetAvaiableGamesByUserRank(int rank);
        public abstract List<GameRoom> GetSpectateableGames();
    }
}