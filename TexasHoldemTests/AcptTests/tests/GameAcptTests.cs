
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

        [TestCase]
        public void ListGamesByRankTestGood()
        {
            //delete all users and games, register user1
            RestartSystem();

            int rank = UserBridge.GetUserRank(UserId);
            int someUser = GetNextUserId();
            UserBridge.SetUserRank(someUser, rank);

            Assert.True(GameBridge.CreateGameRoom(someUser, RoomId));
            Assert.Contains(RoomId, GameBridge.ListAvailableGamesByUserRank(rank));
        }
        
        [TestCase]
        public void ListGamesByRankTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();

            int rank1 = UserBridge.GetUserRank(UserId);
            int someUser = GetNextUserId();
            UserBridge.SetUserRank(someUser, rank1 + 10);

            Assert.True(GameBridge.CreateGameRoom(someUser, RoomId));
            Assert.IsEmpty(GameBridge.ListAvailableGamesByUserRank(rank1));
        }
        
        [TestCase]
        public void ListSpectatableGamesTestGood()
        {
            //delete all users and games, register user1
            RestartSystem();

            int someUser1 = GetNextUserId();
            int someUser2 = GetNextUserId();

            Assert.True(GameBridge.CreateGameRoom(someUser1, RoomId));
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(someUser2, RoomId, 0));
            Assert.True(GameBridge.StartGame(RoomId));

            Assert.Contains(RoomId, GameBridge.ListSpectateableRooms());
        }
        
        [TestCase]
        public void ListSpectatableGamesTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();

            Assert.IsEmpty(GameBridge.ListSpectateableRooms());
        }

    }
}
