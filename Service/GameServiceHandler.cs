using System.Collections.Generic;
using System.Windows.Documents;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
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
            GameManager manager = new GameManager((ConcreteGameRoom)room);
            _roomToManagerDictionary.Add(room, manager);
            return manager;
        }

        public ConcreteGameRoom GetGameFromId(int gameId)
        {
            return _gameCenter.GetRoomById(gameId);
        }

        //TODO: do this
        public abstract GameRoom CreateGameRoom(int userId, int chipsInGame, int roomId, 
            string roomName, int sb, int bb, int minMoney, int maxMoney, int gameNum);

        public int GetNextFreeRoomId()
        {
            return _gameCenter.GetNextIdRoom();
        }

        public ConcreteGameRoom GetGameById(int id)
        {
            return _gameCenter.GetRoomById(id);
        }

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

        //TODO: not sure about this one
        public bool MakeRoomActive(GameRoom room)
        {
            var manager = GetManagerForGame(room);
            return manager.Play();
        }

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

        public List<Player> FindWinner(int gameId)
        {
            List<Player> winningPlayers = new List<Player>();
            var room = _gameCenter.GetRoomById(gameId);
            var manager = GetManagerForGame(room);
            if (room != null && manager != null && manager._gameOver)
            {
                List<Player> activePlayers = room._players.FindAll(p => p.isPlayerActive);
                var winners = manager.FindWinner(room._publicCards, activePlayers);
                winners.ForEach(handEval =>
                {
                    winningPlayers.Add(handEval._player);
                });
            }
            return winningPlayers;
        }

       
        public List<ConcreteGameRoom> GetAllActiveGames()
        {
            List<ConcreteGameRoom>  toReturn = GameCenter.Instance.GetAllActiveGame();
            return toReturn;
        }
        //TODO: do these after searching methods are done
        public abstract List<GameRoom> GetAllGames();
        public abstract List<GameRoom> GetAvaiableGamesByUserRank(int rank);
        public abstract List<GameRoom> GetSpectateableGames();
    }
}