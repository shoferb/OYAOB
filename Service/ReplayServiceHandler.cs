using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using Action = TexasHoldem.Logic.Game.Action;

namespace TexasHoldem.Service
{
    public class ReplayServiceHandler
    {
        private readonly GameCenter _gameCenter = GameCenter.Instance;

        public List<GameReplay> GetUserReplays(int userId)
        {
            List<Tuple<int, int>> tuples = _gameCenter.GetGamesAvailableForReplayByUser(userId);
            if (tuples != null)
            {
                return tuples.ConvertAll(tuple => _gameCenter.GetGameReplay(tuple.Item1, tuple.Item2, userId));
            }
            return new List<GameReplay>();
        }

        public GameReplay GetGameReplay(int roomId, int gameNum)
        {
            return _gameCenter.GetReplayManager().GetGameReplay(roomId, gameNum);
        }

        //TODO: do this after Orelie make Save fav move
        //public abstract bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum);
    }
}
