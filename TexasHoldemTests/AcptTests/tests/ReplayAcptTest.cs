using NUnit.Framework;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    class ReplayAcptTest : AcptTest
    {
        //setup: (called from base)
        protected override void SubClassInit()
        {
            //delete all games and all users, then register user1
            RestartSystem();
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            //nothing to do here
        }

        [TestCase]
        public void GetReplayableGamesTestGood()
        {
            //create a game to be replayd
            Setup2User1Game();

            Assert.IsNotEmpty(ReplayBridge.GetReplayableGames(UserId));
        }

        [TestCase]
        public void GetReplayableGamesTestSad()
        {
            //user1 has not replayable games
            Assert.IsEmpty(ReplayBridge.GetReplayableGames(UserId));
        }

        [TestCase]
        public void ViewReplayTestGood()
        {
            //create a game to be replayd
            SetupUser1();
            var replays = ReplayBridge.ViewReplay(RoomId, 0, UserId);
            Assert.GreaterOrEqual(6, replays.Count); //join, 2 call, leave, lose, win
        }

        [TestCase]
        public void ViewReplayTestBad()
        {
            //no games to replay
            Assert.IsNull(ReplayBridge.ViewReplay(RoomId,0, UserId));
        }

        [TestCase]
        public void SaveFavoriteMoveGood()
        {
            //create a game to be replayd
            SetupUser1();

            Assert.IsNotNull(ReplayBridge.ViewReplay(RoomId, 0, UserId));
            Assert.True(ReplayBridge.SaveFavoriteMove(RoomId, 0, UserId, 1));
        }

        //[TestCase]
        //public void SaveFavoriteMoveNoReplayExistsBad()
        //{

        //    //no games to replay
        //    Assert.IsNull(ReplayBridge.ViewReplay(RoomId, 1));
        //    //TODO: after service is done
        //    //Assert.False(ReplayBridge.SaveFavoriteMove(UserId, RoomId, 1, 1));
        //}

        //[TestCase]
        //public void SaveFavoriteMoveWrongMoveIndexBad()
        //{
        //    //no games to replay
        //    Assert.IsNull(ReplayBridge.ViewReplay(RoomId, 1));
        //    //TODO: after service is done
        //    //Assert.False(ReplayBridge.SaveFavoriteMove(UserId, RoomId, 1, 100));
        //}

        //[TestCase]
        //public void SaveFavoriteMoveNegMoveIndexBad()
        //{
        //    //create a game to be replayd
        //    SetupUser1();

        //    //no games to replay
        //    Assert.IsNotNull(ReplayBridge.ViewReplay(RoomId, 1));
        //    //TODO: after service is done
        //    //Assert.False(ReplayBridge.SaveFavoriteMove(UserId, RoomId, 1, -1));
        //}

        //[TestCase]
        //public void StopReplayTestGood()
        //{
        //    //create a game to be replayd
        //    SetupUser1();

        //    Assert.IsNotNull(ReplayBridge.ViewReplay(RoomId, 1));
        //}

        //[TestCase]
        //public void StopReplayTestBad()
        //{
        //    //no games to replay
        //    Assert.IsNull(ReplayBridge.ViewReplay(RoomId, 1));
        //}
    }
}
