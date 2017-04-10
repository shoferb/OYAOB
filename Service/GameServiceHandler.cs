using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
    {
        public abstract GamePrefDecorator GetGameFromId(int gameId);

        public abstract GamePrefDecorator CreateGameRoom(int id, string name, int sb,
            int bb, int minMoney, int maxMoney, int gameNum);
        public abstract int GetNextFreeRoomId();
        public abstract ConcreteGameRoom GetGameById(int id);
        public abstract bool AddPlayerToRoom(Player player, /*TODO: maybe change this*/ GameRoom room);
        public abstract bool AddSpectatorToRoom(Spectetor spectator, /*TODO: maybe change this*/ GameRoom room);
        public abstract bool MakeRoomActive(GameRoom room);
        public abstract bool RemoveRoom(int gameId);
        public abstract bool Fold(Player player, GameRoom room);
        public abstract bool Check(Player player, GameRoom room);
        public abstract bool Call(Player player, GameRoom room);
        public abstract bool Raise(Player player, GameRoom room, int sum);
        public abstract Player FindWinner(int gameId);
        public abstract List<GamePrefDecorator> GetAllGames();
        public abstract List<GamePrefDecorator> GetAvaiableGamesByUserRank(int rank);
        public abstract List<GamePrefDecorator> GetSpectateableGames();
    }
}