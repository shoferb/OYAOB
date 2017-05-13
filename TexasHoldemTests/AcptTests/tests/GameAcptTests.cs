
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemTests.AcptTests.Bridges;

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
            _user2Pw = "123";
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            if (_userId2 != -1)
            {
                List<int> user2Games = UserBridge.GetUsersGameRooms(_userId2);
                foreach (int roomId in user2Games)
                {
                    UserBridge.RemoveUserFromRoom(_userId2, RoomId);
                }
               
                UserBridge.DeleteUser(_userId2);
            }

            _userId2 = -1;
        }

        //general tests:
        [TestCase]
        public void CreateGameTestGood()
        {
            int roomId = GameBridge.CreateGameRoom(UserId, 100);
            Assert.True(roomId!=-1);
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.AreEqual(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.AreEqual(UserId, GameBridge.GetPlayersInRoom(RoomId).First().user.Id());
        }

        [TestCase]
        public void CreateGameTestBad()
        {
            int roomId = GameBridge.CreateGameRoom(UserId, -1);
            Assert.True(roomId==-1);
            Assert.False(GameBridge.DoesRoomExist(roomId));
        }

        [TestCase]
        public void CreateGameWithPrefTestGood()
        {
            int roomId = GameBridge.CreateGameRoomWithPref(UserId, 100, true, GameMode.Limit, 2, 8, 0, 10);
            Assert.True(roomId != -1);
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.AreEqual(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.AreEqual(UserId, GameBridge.GetPlayersInRoom(RoomId).First().user.Id());
        }

        [TestCase]
        public void CreateGameWithPrefTestBad()
        {
            int roomId = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.PotLimit, 0, 0, -1, 0);
            Assert.True(roomId == -1);
            Assert.False(GameBridge.DoesRoomExist(roomId));
        }

        [TestCase]
        public void ListActiveGamesTestGood()
        {
            //delete all users and games, register user1
            RestartSystem();
            RegisterUser1();
            int roomId = GameBridge.CreateGameRoom(UserId, 10);
            Assert.True(roomId!= -1);
            RegisterUser2();
            IUser user = UserBridge.getUserById(_userId2);
            user.AddMoney(1000);
            Assert.True(GameBridge.ListAvailableGamesByUserRank(_userId2).Count==1);
            Assert.Contains(_userId2, GameBridge.GetIdPlayersInRoom(roomId));
        }

       [TestCase]
        public void ListActiveGamesTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();
            RegisterUser1();
            int roomId = GameBridge.CreateGameRoom(UserId, 10);
            Assert.True(roomId != -1);
            RegisterUser2();
            IUser user = UserBridge.getUserById(_userId2);
            user.EditUserMoney(0);
            Assert.IsEmpty(GameBridge.ListAvailableGamesByUserRank(_userId2));
        }

        [TestCase]
        public void ListSpectatableGamesTestGood()
        {
            //delete all users and games, register user1
            RestartSystem();
            RegisterUser1();
            int roomId = GameBridge.CreateGameRoomWithPref(UserId, 10, true, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(roomId != -1);
            Assert.Contains(roomId, GameBridge.ListSpectateableRooms());
        }

        [TestCase]
        public void ListSpectatableGamesTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();
            RegisterUser1();
            int roomId = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(roomId != -1);
            Assert.IsEmpty(GameBridge.ListSpectateableRooms());
        }

        [TestCase]
        public void ListGamesByPrefTestGood()
        {
            //delete all users and games
            RestartSystem();
            RegisterUser1();
            int roomId1 = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(roomId1 != -1);
            RegisterUser2();
            int roomId2 = GameBridge.CreateGameRoomWithPref(_userId2, 10, false, GameMode.NoLimit, 2, 8, 1, 1);
            Assert.True(roomId2 != -1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.Limit).Count==1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.NoLimit).Count == 1);
        }

        [TestCase]
        public void ListGamesByPrefTestBad()
        {
            //delete all users and games
            RestartSystem();
            RegisterUser1();
            int roomId1 = GameBridge.CreateGameRoomWithPref(UserId, 10, false, GameMode.Limit, 2, 8, 1, 1);
            Assert.True(roomId1 != -1);
            RegisterUser2();
            int roomId2 = GameBridge.CreateGameRoomWithPref(_userId2, 10, false, GameMode.NoLimit, 2, 8, 1, 1);
            Assert.True(roomId2 != -1);
            Assert.True(GameBridge.GetGamesByGameMode(GameMode.PotLimit).Count == 0);
        }



        private void RegisterUser2()
        {
            if (!UserBridge.IsThereUser(_userId2))
            {
                UserBridge.RegisterUser(_userId2, User1Name, User1Pw, UserEmailGood1);

                Users.Add(UserId);
            }
            else if (!UserBridge.IsUserLoggedIn(UserId))
            {
                UserBridge.LoginUser(User1Name, User1Pw);
            }
        }
    }
}
