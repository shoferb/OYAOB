using System;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    abstract class ReplayServiceHandler
    {
        public enum Actions
        {
            Check,
            Call,
            Raise,
            Fold,

            Join,
            Leave,

            Lose,
            Win,
        }

        public abstract GameReplay GetGameReplay(int roomId, int gameNum);
        public abstract Action CreateAction(GameRoom room, Player player, Actions action);
        public abstract bool AddActionToReplay(Action action, int roomId, int gameNum);
        public abstract Action GetNextAction(int roomId, int gameNum);
        public abstract bool StopReplay(int roomId, int gameNum); //TODO: ?
        public abstract bool SaveFavoriteTurn(int roomId, int gameNum, int turnNum);
    }
}
