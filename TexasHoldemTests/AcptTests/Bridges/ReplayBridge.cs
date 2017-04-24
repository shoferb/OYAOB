using System.Collections.Generic;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class ReplayBridge : IReplayBridge
    {
        private readonly ReplayServiceHandler _replayService;

        public ReplayBridge()
        {
            _replayService = new ReplayServiceHandler();
        }

        public List<int> GetReplayableGames(int userId)
        {
            var replays = _replayService.GetUserReplays(userId);
            var gameIds = new List<int>();
            replays.ForEach(replay =>
            {
                gameIds.Add(replay._gameRoomID);
            });
            return gameIds;
        }

        public List<string> ViewReplay(int roomId, int gameNum)
        {
            var replay = _replayService.GetGameReplay(roomId, gameNum);
            var toReturn = new List<string>();
            replay._actions.ForEach(action =>
            {
                toReturn.Add(action.ToString());
            });
            return toReturn;
        }

        //TODO: after service is complete
        //public bool SaveFavoriteMove(int userId, int roomId, int gameNum, int moveNum)
        //{
        //    var replay = _replayService.GetGameReplay(roomId, gameNum);
        //    return _replayService.SaveFavoriteMove(userId, roomId, gameNum, moveNum);
        //}
    }
}
