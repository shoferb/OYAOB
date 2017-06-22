using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldem.Logic.Users;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    class ReplayAcptTest : AcptTest
    {
        private int _userId2 = -1;
        private string _user2Name;
        private string _user2Pw;
        private string _user2EmailGood;
        private int _userId3 = -1;
        private string _user3Name;
        private string _user3Pw;
        private string _user3EmailGood;

        //setup: (called from base)
        protected override void SubClassInit()
        {
            //delete all games and all users, then register user1
            //RestartSystem();
            _userId2 = new Random().Next() + 9292;
            _user2Name = "yarden";
            _user2EmailGood = "yarden@gmail.com";
            _user2Pw = "123456789";
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            _userId3 = new Random().Next() + 91;
            _user3Name = "Aviv";
            _user3EmailGood = "Aviv@gmail.com";
            _user3Pw = "123456789";


        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            if (DeleteUser(_userId2))
                _userId2 = -1;
            Assert.True(_userId2 == -1);

        }

        private bool DeleteUser(int id)
        {
            if (id != -1)
            {
                List<int> user2Games = UserBridge.GetUsersGameRooms(id);
                foreach (var roomId in user2Games)
                {
                    UserBridge.RemoveUserFromRoom(id, RoomId);
                }

                UserBridge.DeleteUser(id);
                return true;
            }
            return false;

        }

        [TestCase]
        public void GetReplayableGamesTestGood()
        {
            //create a game to be replayd
            //RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            GameBridge.StartGame(UserId, RoomId);
            System.Threading.Thread.Sleep(5000);
            UserBridge.RemoveUserFromRoom(UserId, RoomId);
            UserBridge.RemoveUserFromRoom(_userId2, RoomId);
            UserBridge.RemoveUserFromRoom(_userId3, RoomId);
            Assert.True(ReplayBridge.GetReplayableGames(RoomId, 0, _userId3)._actions.Count >= 1);
        }


        [TestCase]
        public void GetReplayableGamesTestSad()
        {
            //RestartSystem();
            SetupUser1();
            Assert.True(RoomId == -1);
            Assert.True(ReplayBridge.GetReplayableGames(RoomId, 0, UserId) == null);
        }

        [TestCase]
        public void ViewReplayTestGood()
        {
            //create a game to be replayd
            //RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            GameBridge.StartGame(UserId, RoomId);
            //   UserBridge.RemoveUserFromRoom(UserId, RoomId);

            var replays = ReplayBridge.ViewReplay(RoomId, 0, UserId);
            Assert.GreaterOrEqual(6, replays.Count); //join, 2 call, leave, lose, win
        }

        [TestCase]
        public void ViewReplayTestBad()
        {
            //RestartSystem();
            SetupUser1();
            Assert.IsEmpty(ReplayBridge.ViewReplay(RoomId, 0, UserId));
        }

        private void RegisterUser(int userId, string name, string pass, string mail)
        {
            if (!UserBridge.IsThereUser(userId))
            {
                UserBridge.RegisterUser(userId, name, pass, mail);

                Users.Add(userId);
            }
            else if (!UserBridge.IsUserLoggedIn(userId))
            {
                UserBridge.LoginUser(name, pass);
            }
        }


    }
}
