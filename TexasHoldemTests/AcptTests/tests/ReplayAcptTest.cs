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
            SetupUser1();

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

            Assert.True(ReplayBridge.ViewReplay(UserId, RoomId));
        }
        
        [TestCase]
        public void ViewReplayTestBad()
        {
            //no games to replay
            Assert.False(ReplayBridge.ViewReplay(UserId, RoomId));
        }

        [TestCase]
        public void SaveFavoriteMoveGood()
        {
            //create a game to be replayd
            SetupUser1();

            Assert.True(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.True(ReplayBridge.SaveFavoriteTurn(UserId, RoomId, 1));
        }

        [TestCase]
        public void SaveFavoriteMoveNoReplayExistsBad()
        {
            //no games to replay
            Assert.False(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.False(ReplayBridge.SaveFavoriteTurn(UserId, RoomId, 1));
        }
        
        [TestCase]
        public void SaveFavoriteMoveWrongMoveIndexBad()
        {
            //no games to replay
            Assert.False(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.False(ReplayBridge.SaveFavoriteTurn(UserId, RoomId, 100));
        }
        
        [TestCase]
        public void SaveFavoriteMoveNegMoveIndexBad()
        {
            //no games to replay
            Assert.False(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.False(ReplayBridge.SaveFavoriteTurn(UserId, RoomId, -1));
        }

        [TestCase]
        public void StopReplayTestGood()
        {
            //create a game to be replayd
            SetupUser1();

            Assert.True(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.True(ReplayBridge.StopReplay(UserId, RoomId));
        }
        
        [TestCase]
        public void StopReplayTestBad()
        {
            //no games to replay
            Assert.False(ReplayBridge.ViewReplay(UserId, RoomId));
            Assert.False(ReplayBridge.StopReplay(UserId, RoomId));
        }
    }
}
