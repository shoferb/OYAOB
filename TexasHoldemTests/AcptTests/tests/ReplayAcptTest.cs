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
        /*
        [TestCase]
        public void GetReplayableGamesTestGood()
        {
            //create a game to be replayd
            Setup2Users1Game();
            System.Threading.Thread.Sleep(5000);
            UserBridge.RemoveUserFromRoom(User2Id, NewRoomId);
            Assert.IsNotEmpty(ReplayBridge.GetReplayableGames(UserId));
        }
        */

        [TestCase]
        public void GetReplayableGamesTestSad()
        {
            RegisterUser1();
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
            RegisterUser1();
            Assert.IsEmpty(ReplayBridge.ViewReplay(RoomId, 0, UserId));
        }
        /*
        [TestCase]
        public void SaveFavoriteMoveGood()
        {
            //create a game to be replayd
            SetupUser1();

            Assert.IsNotNull(ReplayBridge.ViewReplay(RoomId, 0, UserId));
            Assert.True(ReplayBridge.SaveFavoriteMove(RoomId, 0, UserId, 1));
        }*/

        [TestCase]
        public void SaveFavoriteMoveNoReplayExistsBad()
        {

            RegisterUser1();
            Assert.IsEmpty(ReplayBridge.ViewReplay(RoomId, 0, UserId));
        }

        [TestCase]
        public void SaveFavoriteMoveWrongMoveIndexBad()
        {
            RegisterUser1();
            Assert.IsEmpty(ReplayBridge.ViewReplay(RoomId, 5, UserId));
         //   Assert.False(ReplayBridge.SaveFavoriteMove(UserId, RoomId, 5, 100));
        }

        [TestCase]
        public void SaveFavoriteMoveNegMoveIndexBad()
        {
            RegisterUser1();
            Assert.IsEmpty(ReplayBridge.ViewReplay(RoomId, -5, UserId));
          //  Assert.False(ReplayBridge.SaveFavoriteMove(UserId, RoomId, 1, -1));
        }

    }
}
