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

        public ReplayBridge()
        {
            _gameService = new GameServiceHandler();
        }

        public List<Tuple<int, int>> GetReplayableGames(int userId)
        {
            throw new NotImplementedException();
          /*  List<Tuple<int, int>> replays = _gameService.GetGamesAvailableForReplayByUser(userId);
            return replays;*/
        }

        public List<string> ViewReplay(int roomId, int gameNum, int userId)
        {
            return _gameService.GetGameReplay(roomId, gameNum, userId);
        }

        public bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum)
        {
            throw new NotImplementedException();
           // return _gameService.SaveFavoriteMove(roomId, gameNum, userId, moveNum);
        }
    }
}
