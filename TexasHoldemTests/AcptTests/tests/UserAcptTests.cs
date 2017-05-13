using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldem.Logic.Users;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class UserAcptTests : AcptTest
    {
        private int _userId2 = -1;
        private string _user2Name;
        private string _user2Pw;
        private string _user2EmailGood;
        private int _userId3 = -1;
        private string _user3Name;
        private string _user3Pw;
        private string _user3EmailGood;
        private int _userId4 = -1;
        private string _user4Name;
        private string _user4Pw;
        private string _user4EmailGood;
        private string _userPwBad;
        private string _userEmailBad;

        //setup: (called from case)
        protected override void SubClassInit()
        {
            _userEmailBad = "אבי חיון איז היר";
            _userPwBad = "-~~~~~~~~~~~~~~~~~~~~~~~~~~~~`";
            _userId2 = new Random().Next();
            _user2Name = "yarden";
            _user2EmailGood = "yarden@gmail.com";
            _user2Pw = "123456789";

            _userId3 = new Random().Next();
            _user3Name = "Aviv";
            _user3EmailGood = "Aviv@gmail.com";
            _user3Pw = "123456789";

            _userId4 = new Random().Next();
            _user4Name = "Yarden2";
            _user4EmailGood = "YRD@gmail.com";
            _user4Pw = "123456789";

            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);


        }


        //tear down: (called from case)
        protected override void SubClassDispose()
        {

            if (DeleteUser(_userId2))
                _userId2 = -1;
            if (DeleteUser(_userId3))
                _userId3 = -1;
            if (DeleteUser(_userId4))
                _userId4 = -1;

            Assert.True(_userId2==-1);
            Assert.True(_userId3 == -1);
            Assert.True(_userId4 == -1);
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

        protected void RegisterUser(int userId, string name, string pass, string mail)
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

      [TestCase]
        public void UserLoginTestGood()
        {
            RestartSystem();
            Assert.True(UserBridge.LogoutUser(UserId) || UserBridge.getUserById(UserId)==null);
            SetupUser1();
            Assert.True(UserBridge.LogoutUser(UserId));
        }

        [TestCase]
        public void UserLoginTestSad()
        {
            Assert.True(UserBridge.RegisterUser(_user2Name, "11", "w2@.com") == -1);
        }

        [TestCase]
        public void UserLoginTestBad()
        {
            Assert.False(UserBridge.LoginUser("", ""));
            Assert.False(UserBridge.LoginUser("אני שם בעברית", User1Pw));
        }

        //logout
        [TestCase]
        public void UserLogoutTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.LoginUser(User1Name, User1Pw));
            Assert.True(UserBridge.LogoutUser(UserId));
        }

        [TestCase]
        public void UserLogoutTestSad()
        {
            //user is not logged in!
            Assert.False(UserBridge.LogoutUser(UserId));
        }

        [TestCase]
        public void UserLogoutTestBad()
        {
            Assert.False(UserBridge.LogoutUser(-1));
        }

        //register
        [TestCase]
        public void UserRegisterTestGood()
        {
            RestartSystem();

            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1) != -1);
            UserBridge.DeleteUser(User1Name, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestSad()
        {
            RestartSystem();
            
            //user name already exists in system 
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1) != -1);
            Assert.False(UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1) != -1);
            UserBridge.DeleteUser(User1Name, User1Pw);

            //pw not good
            Assert.False(UserBridge.RegisterUser(User1Name, "", UserEmailGood1) != -1);
            UserBridge.DeleteUser(User1Name, "");

            //email not good1:
            Assert.False(UserBridge.RegisterUser("", User1Pw, "היי") != -1);
            UserBridge.DeleteUser(User1Name, User1Pw);
            
            //email not good2:
            Assert.False(UserBridge.RegisterUser(User1Name, User1Pw, "-1") != -1);
            UserBridge.DeleteUser(User1Name, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestBadPwBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser(User1Name, _userPwBad, UserEmailGood1) != -1);
            UserBridge.DeleteUser(User1Name, _userPwBad);

            Assert.False(UserBridge.RegisterUser(User1Name, _userPwBad, User1Pw) != -1);
            UserBridge.DeleteUser(User1Name, _userPwBad);
            UserBridge.DeleteUser(User1Name, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestEmptysBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser("", User1Pw, User1Pw) != -1);
            UserBridge.DeleteUser("", User1Pw);

            Assert.False(UserBridge.RegisterUser(User1Name, "", _userPwBad) != -1);
            UserBridge.DeleteUser(User1Name, _userPwBad);

            Assert.False(UserBridge.RegisterUser(User1Name, User1Pw, "") != -1);
        }

        [TestCase]
        public void UserRegisterTestNullsBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser(null, User1Pw, User1Pw) != -1);
            UserBridge.DeleteUser("", User1Pw);

            Assert.False(UserBridge.RegisterUser(User1Name, null, _userPwBad) != -1);
            UserBridge.DeleteUser(User1Name, _userPwBad);

            Assert.False(UserBridge.RegisterUser(User1Name, User1Pw, null) != -1);
        }

        //edit
        [TestCase]
        public void UserEditNameTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditName(UserId, "newName"));
            Assert.AreEqual(UserBridge.GetUserName(UserId), "newName");
            Assert.True(UserBridge.EditName(UserId, User1Name));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
        }

        [TestCase]
        public void UserEditNameTestSad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditName(UserId, User1Name));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
        }

        [TestCase]
        public void UserEditNameTestBad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditName(-1, User1Name));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);

            Assert.False(UserBridge.EditName(UserId, ""));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
        }

        [TestCase]
        public void UserEditPwTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditPw(UserId, _user2Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), _user2Pw);

            //set back
            Assert.True(UserBridge.EditPw(UserId, User1Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditPwTestSad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditPw(UserId, _userPwBad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditPwTestBad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditPw(-1, _user2Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);

            Assert.False(UserBridge.EditPw(UserId, _userPwBad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditEmailTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditEmail(UserId, _user2EmailGood));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), _user2EmailGood);

            //set back
            Assert.True(UserBridge.EditAvatar(UserId, ""));
            Assert.AreEqual(UserBridge.GetUserAvatar(UserId), "yarden");
        }

        [TestCase]
        public void UserEditEmailTestBad()
        {
            //user is not logged in:
            Assert.False(UserBridge.EditEmail(UserId, UserEmailGood1));

            RegisterUser1();

            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);

            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
        }

        [TestCase]
        public void UserEditAvatarTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditAvatar(UserId, "yarden"));
            Assert.AreEqual(UserBridge.GetUserAvatar(UserId), "yarden");

            //set back
            Assert.True(UserBridge.EditEmail(UserId, UserEmailGood1));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
        }
      

        [TestCase]
        public void UserAddUserMoneyTestGood()
        {
            RegisterUser1();

            const int amountToChange = 100;
            int prevAmount = UserBridge.GetUserMoney(UserId);
            Assert.True(UserBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == UserBridge.GetUserMoney(UserId) - amountToChange);
            Assert.True(UserBridge.ReduceUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == UserBridge.GetUserMoney(UserId));
        }

        [TestCase]
        public void UserAddUserMoneyTestBad()
        {
            const int amountToChange = -100;
            int prevAmount = UserBridge.GetUserMoney(UserId);
            Assert.False(UserBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == UserBridge.GetUserMoney(UserId));
        }

       //add player to room
        [TestCase]
        public void UserAddToRoomAsPlayerAllMoneyTestGood()
        {
            RestartSystem();
            SetupUser1();
            Assert.True(RoomId == -1);
            Assert.False(UserBridge.getUserById(UserId) == null);
            CreateGameWithUser1();

            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            Assert.True(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerNoMoneyTestGood()
        {
            RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
            Assert.GreaterOrEqual(0, UserBridge.GetUserMoney(UserId));
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerTestSad()
        {
            RestartSystem();
            RegisterUser1();

            Assert.True(RoomId==-1);

            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(UserId,RoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.AreEqual(0, UserBridge.GetUserChips(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerNegUserTestBad()
        {
            RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            //negtive user Id
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(-1, RoomId, 0));
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, -1));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            
        }
        // talk with Aviv about that
        [TestCase]
        public void UserAddToRoomAsPlayerAllreadySpectatorInRoomTestBad()
        {
            RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(_userId2, RoomId));
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 1));
        }

        //add spectator to room
        [TestCase]
        public void UserAddToRoomAsSpectatorGood()
        {
            RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            int money = UserBridge.GetUserMoney(_userId2);

            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(_userId2, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(_userId2));
            Assert.True(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.AreEqual(money, UserBridge.GetUserMoney(_userId2));
            Assert.AreEqual(0, UserBridge.GetUserChips(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorNonExsistantRoomTestSad()
        {
            RestartSystem();
            RegisterUser1();
            Assert.True(RoomId == -1);

            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorAllreadyPlayerInRoomTestBad()
        {
            RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
        }

       [TestCase]
        public void UserRemoveFromRoomSpectatorTestGood()
        {
            RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            int money = UserBridge.GetUserMoney(_userId2);
            int chips = UserBridge.GetUserChips(_userId2);

            //add user to Room as spectator
            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(_userId2, RoomId));
            Assert.True(GameBridge.IsUserInRoom(_userId2, RoomId)); /////////////ADD search for spectator there!!
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(_userId2));

            //remove user from Room
            Assert.True(UserBridge.RemoveUserFromRoom(_userId2, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(_userId2).Contains(RoomId));
            Assert.AreEqual(money, UserBridge.GetUserMoney(_userId2));
            Assert.AreEqual(chips, UserBridge.GetUserChips(_userId2));
            Assert.True(GameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room
        }
/*
        [TestCase]
        public void UserRemoveFromRoomPlayerNonActiveRoomTestGood()
        {
            int userId2 = CreateGameWithUser1();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);
            int chips = UserBridge.GetUserChips(UserId);

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //remove user from Room
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
//            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.AreEqual(money, UserBridge.GetUserMoney(UserId));
            Assert.AreEqual(chips, UserBridge.GetUserChips(UserId));

            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerNotInRoomTestBad()
        {
            //remove player from a non existant Room
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));

            CreateGameWithUser1();

            //remove user from Room he is not in
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerWrongGameIdTestBad()
        {
            int userId2 = CreateGameWithUser1();

            RegisterUser1();

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //remove user from Room he is not in (fails)
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId + 1));
            Assert.True(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room            
        }

        [TestCase]
        public void UserRemoveFromRoomSpectatorTestBad()
        {
            int userId2 = CreateGameWithUser1();

            RegisterUser1();

            //add user to Room as spectator
            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //remove user from Room he is not in
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId + 1));
            Assert.True(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room
        }
        */
    }
}
