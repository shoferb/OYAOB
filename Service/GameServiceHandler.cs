using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
    {
        private readonly Dictionary<GameRoom, GameManager> _roomToManagerDictionary;

        protected GameServiceHandler()
        {
            _roomToManagerDictionary = new Dictionary<GameRoom, GameManager>();
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

        public abstract GameRoom GetGameFromId(int gameId);
        public abstract GameRoom CreateGameRoom(int id, string name, int sb,
            int bb, int minMoney, int maxMoney, int gameNum);
        public abstract int GetNextFreeRoomId();
        public abstract GameRoom GetGameById(int id);

        public bool AddPlayerToRoom(Player player, GameRoom room)
        {
            if (player != null && room != null && !room._players.Contains(player))
            {
                room._spectatores.Add(player);
                return true;
            }
            return false;
        }

        public bool AddSpectatorToRoom(Spectetor spectator, GameRoom room)
        {
            if (spectator != null && room != null && !room._spectatores.Contains(spectator))
            {
                room._spectatores.Add(spectator);
                return true;
            }
            return false;
        }

        public abstract bool RemoveUserFromRoom(int userId, int roomId);
        public abstract bool MakeRoomActive(GameRoom room);
        public abstract bool RemoveRoom(int gameId);

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