using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IReplayBridge
    {
        List<string> GetReplayableGames(int userId);
        List<string> ViewReplay(int roomId, int gameNum);

        //can be done while viewing replay
        bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum);
        bool StopReplay(int userId, int gameId);
    }
}