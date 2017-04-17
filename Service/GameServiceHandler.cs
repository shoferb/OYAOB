using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
    {
        private readonly Dictionary<ConcreteGameRoom, GameManager> _roomToManagerDictionary;

        protected GameServiceHandler()
        {
            _roomToManagerDictionary = new Dictionary<ConcreteGameRoom, GameManager>();
        }

        private GameManager GetManagerForGame(ConcreteGameRoom room)
        {
            if (_roomToManagerDictionary.ContainsKey(room))
            {
                return _roomToManagerDictionary[room];
            }
            GameManager manager = new GameManager(room);
            _roomToManagerDictionary.Add(room, manager);
            return manager;
        }

        public abstract GamePrefDecorator GetGameFromId(int gameId);
        public abstract ConcreteGameRoom CreateGameRoom(int id, string name, int sb,
            int bb, int minMoney, int maxMoney, int gameNum);
        public abstract int GetNextFreeRoomId();
        public abstract ConcreteGameRoom GetGameById(int id);

        public bool AddPlayerToRoom(Player player, ConcreteGameRoom room)
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
        public abstract bool MakeRoomActive(ConcreteGameRoom room);
        public abstract bool RemoveRoom(int gameId);

        public bool Fold(Player player, ConcreteGameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Fold();
                return true;
            }
            return false;
        }

        public bool Check(Player player, ConcreteGameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Check();
                return true;
            }
            return false;
        }

        public bool Call(Player player, ConcreteGameRoom room)
        {
            var manager = GetManagerForGame(room);
            if (player.Equals(manager._currentPlayer))
            {
                manager.Call();
                return true;
            }
            return false;
        }

        public bool Raise(Player player, ConcreteGameRoom room, int sum)
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
        public abstract List<ConcreteGameRoom> GetAllGames();
        public abstract List<ConcreteGameRoom> GetAvaiableGamesByUserRank(int rank);
        public abstract List<ConcreteGameRoom> GetSpectateableGames();
    }
}