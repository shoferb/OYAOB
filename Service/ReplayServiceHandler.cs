using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using Action = TexasHoldem.Logic.Game.Action;

namespace TexasHoldem.Service
{
    public abstract class ReplayServiceHandler
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

        public abstract List<GameReplay> GetUserReplays(int userId);
        public abstract GameReplay GetGameReplay(int roomId, int gameNum);
        public abstract Action CreateAction(ConcreteGameRoom room, Player player, Actions action);
        public abstract bool AddActionToReplay(Action action, int roomId, int gameNum);
        public abstract Action GetNextAction(int roomId, int gameNum);
        public abstract bool StopReplay(int roomId, int gameNum); //TODO: ?
        public abstract bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum);
    }
}
