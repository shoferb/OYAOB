
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class GameAcptTests : AcptTest
    {
        private int _userId2 = -1;
        private string _user2Name;
        private string _user2Pw;
        private string _user2EmailGood;


        //setup: (called from base)
        protected override void SubClassInit()
        {
            _userId2 = new Random().Next();
            _user2Name = "yarden";
            _user2EmailGood = "yarden@gmail.com";
            _user2Pw = "12345678";
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            if (_userId2 != -1)
            {
                List<int> user2Games = UserBridge.GetUsersGameRooms(_userId2);
                foreach (int roomId in user2Games)
                {
                    //UserBridge.RemoveUserFromRoom(_userId2, roomId);
                    CleanUp(roomId);
                }

                UserBridge.DeleteUser(_userId2);
            }
            _userId2 = -1;

            if (UserId != -1)
            {
                List<int> user1Games = UserBridge.GetUsersGameRooms(UserId);
                foreach (int roomId in user1Games)
                {
                    //UserBridge.RemoveUserFromRoom(_userId2, roomId);
                    CleanUp(roomId);
                }

                UserBridge.DeleteUser(UserId);
            }
            UserId = -1;
        }

        //general tests:
        [TestCase]
        public void CreateGameTestGood()
        {
            ////RestartSystem();
            RoomId = -1;
            //SetupUser1();
            UserId = new Random().Next();
            RegisterUserToDB(UserId);
           
            Assert.False(UserBridge.getUserById(UserId) == null);
            RoomId = GameBridge.CreateGameRoom(UserId, 100);
            Assert.True(RoomId != -1);
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.AreEqual(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.AreEqual(UserId, GameBridge.GetPlayersInRoom(RoomId).First().user.Id());
            CleanUp(RoomId);
        }

        [TestCase]
        public void CreateGameTestBad()
        {
            int roomId = GameBridge.CreateGameRoom(UserId, -1);
            Assert.True(roomId == -1);
            Assert.False(GameBridge.DoesRoomExist(roomId));
            CleanUp(roomId);
        }

        [TestCase]
        public void CreateGameWithPrefTestGood()
        {
            UserId = new Random().Next();
            RegisterUserToDB(UserId);
            RoomId = -1;
            Assert.True(RoomId == -1);
            RoomId = GameBridge.CreateGameRoomWithPref(UserId, 100, true, GameMode.Limit, 2, 8, 0, 10);
            Assert.True(RoomId != -1);
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.AreEqual(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.AreEqual(UserId, GameBridge.GetPlayersInRoom(RoomId).First().user.Id());
            CleanUp(RoomId);
        }

        [TestCase]
        public void CreateGameWithPrefTestBad()
        {
            RoomId = -1;
            SetupUser1();
            Assert.True(RoomId == -1);
            Assert.False(UserBridge.getUserById(UserId) == null);

            RoomId = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.PotLimit, 0, 0, -1, 0);
            Assert.True(RoomId == -1);
            Assert.False(GameBridge.DoesRoomExist(RoomId));
            CleanUp(RoomId);
        }

        [TestCase]
        public void ListActiveGamesTestGood()
        {
            //delete all users and games, register user1
          //  //RestartSystem();
            SetupUser1();
            RoomId = -1;
            Assert.True(RoomId == -1);
            Assert.True(UserBridge.IsThereUser(UserId));
            RoomId = GameBridge.CreateGameRoom(UserId, 10);
            RegisterUser2();
            Assert.True(UserBridge.IsThereUser(UserId));
            Assert.True(UserBridge.IsThereUser(_userId2));
            IUser user = UserBridge.getUserById(_userId2);
            user.AddMoney(1000);
            Assert.GreaterOrEqual(1, GameBridge.ListAvailableGamesByUserRank(_userId2).Count);
            CleanUp(RoomId);
        }

        [TestCase]
        public void ListActiveGamesTestSad()
        {
            //delete all users and games, register user1
            //RestartSystem();
            SetupUser1();
            int roomId = GameBridge.CreateGameRoom(UserId, 10);
            Assert.True(roomId != -1);
            RegisterUser2();
            IUser user = UserBridge.getUserById(_userId2);
            user.EditUserMoney(0);
            Assert.IsEmpty(GameBridge.ListAvailableGamesByUserRank(_userId2));
            CleanUp(roomId);
        }

        [TestCase]
        public void ListSpectatableGamesTestGood()
        {
            //delete all users and games, register user1
            //RestartSystem();
            SetupUser1();
            int roomId = GameBridge.CreateGameRoomWithPref(UserId, 10, true, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(roomId != -1);
            IGame spec = GameBridge.ListSpectateableRooms().First();
            Assert.True(GameBridge.ListSpectateableRooms().Count == 1);
            Assert.True(spec.Id == roomId);
            CleanUp(roomId);
        }

        [TestCase]
        public void ListSpectatableGamesTestSad()
        {
            //delete all users and games, register user1
            //RestartSystem();
            int id = SetupUser1();
            var spectateableGamesBefore = GameBridge.ListSpectateableRooms();
            int numBefore = spectateableGamesBefore == null ? 0 : spectateableGamesBefore.Count;
            int roomId = GameBridge.CreateGameRoomWithPref(id, 10, false, GameMode.Limit, 
                2, 8, 1, 1);
            Assert.True(roomId != -1);
            var spectateableGamesAfter = GameBridge.ListSpectateableRooms();
            int numAfter = spectateableGamesBefore == null ? 0 : spectateableGamesAfter.Count;
            Assert.AreEqual(numBefore, numAfter);
            CleanUp(roomId);
        }

        [TestCase]
        public void ListGamesByPrefTestGood()
        {
            //delete all users and games
            //RestartSystem();
            SetupUser1();
            int roomId1 = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.Limit, 
                2, 8, 1, 1);
            Assert.True(roomId1 != -1);
            RegisterUser2();
            int roomId2 = GameBridge.CreateGameRoomWithPref(_userId2, 10, false, GameMode.NoLimit, 
                2, 8, 1, 1);
            Assert.True(roomId2 != -1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.Limit).Count == 1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.NoLimit).Count == 1);
            CleanUp(roomId1);
            CleanUp(roomId2);
        }

        private void RegisterUser2()
        {
            _userId2 = new Random().Next();
            RegisterUserToDB(_userId2);
        }

        [TestCase]
        public void ListGamesByPrefTestBad()
        {
            //delete all users and games
            //RestartSystem();
            SetupUser1();
            RoomId = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(RoomId != -1);
            RegisterUser2();
            int roomId2 = GameBridge.CreateGameRoomWithPref(_userId2, 10, false, GameMode.NoLimit, 2, 8, 1, 1);
            Assert.True(roomId2 != -1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.PotLimit).Count == 0);
            CleanUp(RoomId);
            CleanUp(roomId2);
        }

    }
}
