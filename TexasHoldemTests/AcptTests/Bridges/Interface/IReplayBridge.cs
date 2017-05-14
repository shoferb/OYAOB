using System.Collections.Generic;
using System;
using TexasHoldem.Logic.Replay;

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IReplayBridge
    {
        GameReplay GetReplayableGames(int gameRoomID, int gameNumberint, int userId);
        List<string> ViewReplay(int roomId, int gameNum, int userId);
    }
}