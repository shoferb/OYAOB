using System.Collections.Generic;
using System;
namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IReplayBridge
    {
        List<Tuple<int, int>> GetReplayableGames(int userId);
        List<string> ViewReplay(int roomId, int gameNum, int userId);
        bool SaveFavoriteMove(int roomId, int gameNum, int userId, int moveNum);
    }
}