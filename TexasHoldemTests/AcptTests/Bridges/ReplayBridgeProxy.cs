
using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class ReplayBridgeProxy : IReplayBridge
    {
        public List<int> GetReplayableGames(int userId)
        {
            throw new System.NotImplementedException();
        }

        //public bool AddReplayableGame(int userId, int gameId)
        //{
        //    throw new System.NotImplementedException();
        //}

        public bool ViewReplay(int userId, int gameId, int moveNum)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveFavoriteMove(int userId, int gameId, int moveNum)
        {
            throw new System.NotImplementedException();
        }

        public bool StopReplay(int userId, int gameId)
        {
            throw new System.NotImplementedException();
        }

        public bool ViewReplay(int userId, int gameId)
        {
            throw new System.NotImplementedException();
        }
    }
}
