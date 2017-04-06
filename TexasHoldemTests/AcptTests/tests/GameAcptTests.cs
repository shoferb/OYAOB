
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
        protected override void SubMethodInit()
        {
            GameBridge = new GameBridgeProxy();
            UserBridge = new UserBridgeProxy();
        }

        //tear down: (called from case)
        protected override void SubMethodDispose()
        {
            if (_userId2 != -1)
            {
                UserBridge.DeleteUser(_userId2);
            }

            //remove user1 from all games
            if (UserBridge.GetUsersGameRooms(UserId).Count > 0)
            {
                UserBridge.GetUsersGameRooms(UserId).ForEach(gId =>
                {
                    UserBridge.RemoveUserFromRoom(UserId, gId);
                });
            }

            //delete room1
            if (GameBridge.DoesRoomExist(RoomId))
            {
                GameBridge.RemoveGameRoom(RoomId);
            }

            _userId2 = -1;
        }

        [TestCase]
        public void CreateGameTestGood()
        {
            LoginUser1();

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

            LoginUser1();

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

            LoginUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.Contains(rank, GameBridge.ListAvailableGamesByUserRank(rank));
        }
        
        
        [TestCase]
        public void ListSpectatableGames()
        {
            LoginUser1();

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.Contains(RoomId, GameBridge.ListSpecateableRooms());
        }

        //TODO: figure out how to test 'list games''s sad case (no games)
    }
}
