using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    //todo fix error
    class ReplayBridge : IReplayBridge
    {
        private readonly GameServiceHandler _gameService;
        private readonly ReplayHandler _replayHandler;
        public ReplayBridge()
        {
            _gameService = new GameServiceHandler();
            _replayHandler = new ReplayHandler();
        }
       
        public GameReplay GetReplayableGames(int gameRoomID, int gameNumber , int userId)
        {
           return _replayHandler.GetGameReplayForUser(gameRoomID, gameNumber, userId);
        }

        public List<string> ViewReplay(int roomId, int gameNum, int userId)
        {
            return _gameService.GetGameReplay(roomId, gameNum, userId);
        }

    }
}
