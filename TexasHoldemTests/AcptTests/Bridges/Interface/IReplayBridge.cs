using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IReplayBridge
    {
        List<int> GetReplayableGames(int userId);
        List<string> ViewReplay(int roomId, int gameNum);

        //can be done while viewing replay
        //TODO: after service is done
        //bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum);
    }
}