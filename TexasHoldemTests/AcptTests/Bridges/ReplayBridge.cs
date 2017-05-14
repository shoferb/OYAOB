using System;
using System.Collections.Generic;
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
        }
       
        public List<Tuple<int, int>> GetReplayableGames(int userId)
        {
            throw new NotImplementedException();
        }

        public List<string> ViewReplay(int roomId, int gameNum, int userId)
        {
            return _gameService.GetGameReplay(roomId, gameNum, userId);
        }

    }
}
