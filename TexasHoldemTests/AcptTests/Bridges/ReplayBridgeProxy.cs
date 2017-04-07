
using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class ReplayBridgeProxy : IReplayBridge
    {
        public List<int> GetReplayableGames(int userId)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveFavoriteTurn(int userId, int roomId, int gameNum, int turnNum)
        {
            throw new System.NotImplementedException();
        }

        public bool StopReplay(int userId, int gameId)
        {
            throw new System.NotImplementedException();
        }

        public bool ViewReplay(int userId, int roomId, int gameNum)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetMovesOfFinishedGame(int roomId, int gameNum)
        {
            throw new System.NotImplementedException();
        }
    }
}
