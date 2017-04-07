using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    public interface IReplayBridge
    {
        List<int> GetReplayableGames(int userId);
        //bool AddReplayableGame(int userId, int gameId);
        bool ViewReplay(int userId, int gameId);

        //can be done while viewing replay
        bool SaveFavoriteTurn(int userId, int gameId, int turnNum);
        bool StopReplay(int userId, int gameId);
    }
}