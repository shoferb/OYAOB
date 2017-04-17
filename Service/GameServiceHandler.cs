using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class GameServiceHandler : ServiceHandler
    {
        public abstract GamePrefDecorator GetGameFromId(int gameId);

        public abstract ConcreteGameRoom CreateGameRoom(int id, string name, int sb,
            int bb, int minMoney, int maxMoney, int gameNum);
        public abstract int GetNextFreeRoomId();
        public abstract ConcreteGameRoom GetGameById(int id);
        public abstract bool AddPlayerToRoom(Player player, ConcreteGameRoom room);
        public abstract bool AddSpectatorToRoom(Spectetor spectator, /*TODO: maybe change this*/ ConcreteGameRoom room);
        public abstract bool RemoveUserFromRoom(int userId, int roomId);
        public abstract bool MakeRoomActive(ConcreteGameRoom room);
        public abstract bool RemoveRoom(int gameId);
        public abstract bool Fold(Player player, ConcreteGameRoom room);
        public abstract bool Check(Player player, ConcreteGameRoom room);
        public abstract bool Call(Player player, ConcreteGameRoom room);
        public abstract bool Raise(Player player, ConcreteGameRoom room, int sum);
        public abstract Player FindWinner(int gameId);
        public abstract List<ConcreteGameRoom> GetAllGames();
        public abstract List<ConcreteGameRoom> GetAvaiableGamesByUserRank(int rank);
        public abstract List<ConcreteGameRoom> GetSpectateableGames();
    }
}