
using System;
using System.Linq;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class GameAcptTests : AcptTest
    {
        private int _userId2;

        //setup: (called from base)
        protected override void SubClassInit()
        {
            //nothing at the moment
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            if (_userId2 != -1)
            {
                UserBridge.DeleteUser(_userId2);
            }

            _userId2 = -1;
        }

        [TestCase]
        public void CreateGameTestGood()
        {
            RegisterUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.Equals(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.Equals(UserId, GameBridge.GetPlayersInRoom(RoomId).First());
        }
        
        [TestCase]
        public void CreateGameTestBad()
        {
            //user1 is not logged in
            Assert.False(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.False(GameBridge.DoesRoomExist(RoomId));
        }

        [TestCase]
        public void GameBecomesInactiveGood()
        {
            _userId2 = UserBridge.GetNextFreeUserId();

            RegisterUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 0));
            Assert.True(GameBridge.StartGame(RoomId));
            Assert.True(GameBridge.IsRoomActive(RoomId));

            Assert.True(UserBridge.RemoveUserFromRoom(_userId2, RoomId));
            Assert.False(GameBridge.IsRoomActive(RoomId));
            Assert.False(GameBridge.StartGame(RoomId));
        }

        //TODO: maybe find a better test for these:
        [TestCase]
        public void ListGamesByRankTestGood()
        {
            int rank = UserBridge.GetUserRank(UserId);

            RegisterUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.Contains(rank, GameBridge.ListAvailableGamesByUserRank(rank));
        }
        
        
        [TestCase]
        public void ListSpectatableGames()
        {
            RegisterUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.Contains(RoomId, GameBridge.ListSpectateableRooms());
        }

        //TODO: figure out how to test 'list games''s sad case (no games)
    }
}
