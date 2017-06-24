using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemTests.AcptTests.Bridges;
using Moq;

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
        private int _userId5 = -1;
        private string _user5Name;
        private string _user5Pw;
        private string _user5EmailGood;
        private int _userId6 = -1;
        private string _user6Name;
        private string _user6Pw;
        private string _user6EmailGood;
        private string _userPwBad;
        private string _userEmailBad;

        //setup: (called from case)
        protected override void SubClassInit()
        {
            _userEmailBad = "אבי חיון איז היר";
            _userPwBad = "-~~~~~~~~~~~~~~~~~~~~~~~~~~~~`";
            _userId2 = new Random().Next() + 9292;
            _user2Name = "yarden";
            _user2EmailGood = "yarden@gmail.com";
            _user2Pw = "123456789";

            _userId3 = new Random().Next() + 91;
            _user3Name = "Aviv";
            _user3EmailGood = "Aviv@gmail.com";
            _user3Pw = "123456789";

            _userId4 = new Random().Next() + 234;
            _user4Name = "Yarden2";
            _user4EmailGood = "YRD@gmail.com";
            _user4Pw = "123456789";

            _userId5 = 305509069;
            _user5Name = "Orellie";
            _user5EmailGood = "o@gmail.com";
            _user5Pw = "12345555";

            _userId6 = 305509000;
            _user6Name = "bar";
            _user6EmailGood = "b@gmail.com";
            _user6Pw = "9191919191";


            //RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            //RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            //RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);


        }


        //tear down: (called from case)
        protected override void SubClassDispose()
        {

            //if (DeleteUser(_userId2))
            //    _userId2 = -1;
            //if (DeleteUser(_userId3))
            //    _userId3 = -1;
            //if (DeleteUser(_userId4))
            //    _userId4 = -1;

            //Assert.True(_userId2 == -1);
            //Assert.True(_userId3 == -1);
            //Assert.True(_userId4 == -1);
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
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            Assert.True(UserBridge.LoginUser(User1Name,User1Pw));
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserLoginTestSad_bad_password()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "11";
            UserEmailGood1 = "gooduser1@gmail.com";
         
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, "w2@.com") == -1);
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
            //RegisterUser1();
            int id = SetupUser1();

            //Assert.True(UserBridge.LoginUser(User1Name, User1Pw));
            Assert.True(UserBridge.LogoutUser(id));
        }

        [TestCase]
        public void UserLogoutTestSad()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
           
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
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1) != -1);
            UserBridge.DeleteUser(User1Name, User1Pw);
        }

        [TestCase]
        public void UserRegisterTest_Sad_username_taken()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";

            //user name already exists in system 
            UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1);
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, UserEmailGood1) == -1);
            UserBridge.DeleteUser(User1Name, User1Pw);

        }

        [TestCase]
        public void UserRegisterTest_Sad_invalid_email_hebrew()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, "היי") == -1);
  
        }

        [TestCase]
        public void UserRegisterTest_Sad_invalid_email_number()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, "-1") == -1);
        }


        [TestCase]
        public void UserRegisterTest_Bad_PwBad()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _userPwBad = "5";
           // RegisterUser1();
            Assert.True(UserBridge.RegisterUser(User1Name, _userPwBad, UserEmailGood1) == -1);
            UserBridge.DeleteUser(User1Name, _userPwBad);

           
        }

        [TestCase]
        public void UserRegisterTest_bad_Empty_userName()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _userPwBad = "5";

            Assert.True(UserBridge.RegisterUser("", User1Pw, UserEmailGood1) == -1);
           
        }
        [TestCase]
        public void UserRegisterTest_bad_Empty_password()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _userPwBad = "5";

         
            Assert.True(UserBridge.RegisterUser(User1Name, "", UserEmailGood1) == -1);
            
        }

        [TestCase]
        public void UserRegisterTest_bad_Empty_Email()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _userPwBad = "5";
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, "") == -1);
        }

        [TestCase]
        public void UserRegisterTest_bad_Nulls_username()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            Assert.True(UserBridge.RegisterUser(null, User1Pw, UserEmailGood1) == -1);
         
        }

        [TestCase]
        public void UserRegisterTest_bad_Nulls_password()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            Assert.True(UserBridge.RegisterUser(User1Name, null, UserEmailGood1) == -1);
            
        }

        [TestCase]
        public void UserRegisterTest_bad_Nulls_email()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, null) == -1);
        }
        //edit
        [TestCase]
        public void UserEditNameTest_Good()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

            UserBridge.EditName(UserId, "newName"+UserId);
            Assert.AreEqual(UserBridge.GetUserName(UserId), "newName"+UserId);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditNameTest_Sad_samename()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            Assert.False(UserBridge.EditName(UserId, User1Name));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
            UserBridge.DeleteUser(UserId);
        }
        [TestCase]
        public void UserEditNameTest_Bad_empty()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

            Assert.False(UserBridge.EditName(UserId, ""));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
            UserBridge.DeleteUser(UserId);
        }
        [TestCase]
        public void UserEditNameTest_Bad_invalidId()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

            Assert.False(UserBridge.EditName(-1, User1Name));
            Assert.AreEqual(UserBridge.GetUserName(UserId), User1Name);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditPwTest_Good()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

            Assert.True(UserBridge.EditPw(UserId, _user2Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), _user2Pw);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditPwTest_Sad()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            _userPwBad = "bad";
            Assert.False(UserBridge.EditPw(UserId, _userPwBad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditPwTest_Bad_invalidId()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _user2Pw = "newPassword";
            RegisterUser1();
            Assert.False(UserBridge.EditPw(-1, _user2Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditPwTest_Bad_invalid_passwords()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            _userPwBad = "bad";
            RegisterUser1();
            Assert.False(UserBridge.EditPw(UserId, _userPwBad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
            UserBridge.DeleteUser(UserId);
        }
        [TestCase]
        public void UserEditEmailTestGood()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            Assert.True(UserBridge.EditEmail(UserId, _user2EmailGood));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), _user2EmailGood);
            UserBridge.DeleteUser(UserId);

        }

        [TestCase]
        public void UserEditEmailTestBad_logout()
        {
            Assert.False(UserBridge.EditEmail(5658558, UserEmailGood1));
        }

        [TestCase]
        public void UserEditEmailTestBad()
        {

            UserId = new Random().Next();
            User1Name = "Oded" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);

            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void UserEditAvatarTestGood()
        {
            //RegisterUser1();
            int id = SetupUser1();

            Assert.True(UserBridge.EditAvatar(id, "yarden"));
            Assert.AreEqual(UserBridge.GetUserAvatar(id), "yarden");

            ////set back
            //Assert.True(UserBridge.EditEmail(UserId, UserEmailGood1));
            //Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
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
            ////RestartSystem();
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            const int amountToChange = -10000;
            SetupUser1();
            int prevAmount = UserBridge.GetUserMoney(UserId);
            Assert.False(UserBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == UserBridge.GetUserMoney(UserId));
            UserBridge.DeleteUser(UserId);
        }

        //add player to room
        [TestCase]
        public void UserAddToRoomAsPlayerAllMoneyTestGood()
        {
            //RestartSystem();
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
            //RestartSystem();
            //RegisterUser1();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerTestSad()
        {
            //RestartSystem();
            //RegisterUser1();
            Assert.True(RoomId == -1);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.True(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerNegUserTestBad()
        {
            //RestartSystem();
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
            //RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            var u2 = UserBridge.getUserById(_userId2);
            var u1 = UserBridge.getUserById(UserId);
            u2.AddMoney(10000);
            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(_userId2, RoomId));
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 100));
        }

        //add spectator to room
        [TestCase]
        public void UserAddToRoomAsSpectatorGood()
        {
            //RestartSystem();
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
            //RestartSystem();
            RegisterUser1();
            Assert.True(RoomId == -1);

            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorAllreadyPlayerInRoomTestBad()
        {
            //RestartSystem();
            RegisterUser1();
            CreateGameWithUser1();
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
        }

        [TestCase]
        public void UserRemoveFromRoomSpectatorTestGood()
        {
            //RestartSystem();
            //SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);

            int money = UserBridge.GetUserMoney(_userId2);
            int chips = UserBridge.GetUserChips(_userId2);

            //add user to Room as spectator
            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(_userId2, RoomId));
            Assert.True(GameBridge.IsUserInRoom(_userId2, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(_userId2));

            //remove user from Room
            Assert.True(UserBridge.RemoveSpectatorFromRoom(_userId2, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(_userId2).Contains(RoomId));
            Assert.AreEqual(money, UserBridge.GetUserMoney(_userId2));
            Assert.AreEqual(chips, UserBridge.GetUserChips(_userId2));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId)); //user1 should still be in Room
        }



        [TestCase]
        public void PlacingBlindBetsForPlayersTestGood()
        {
            //RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            int user2MoneyBefore = user2.Money();
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            int user3MoneyBefore = user3.Money();
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            IGame game = GameBridge.GetAllGames().First();
            GameBridge.StartGame(UserId, RoomId);
            Player player2 = GetInGamePlayerFromUser(user2, RoomId);
            int sb = game.GetMinBet();
            Player player3 = GetInGamePlayerFromUser(user3, RoomId);
            Assert.True(player2.TotalChip == user2MoneyBefore - sb || player3.TotalChip == user3MoneyBefore - sb);
        }

        [TestCase]
        public void FoldTestBad()
        {
            //RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            int user2MoneyBefore = user2.Money();
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            int user3MoneyBefore = user3.Money();
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            IGame game = GameBridge.GetAllGames().First();
            GameBridge.StartGame(UserId, RoomId);
            Player player2 = GetInGamePlayerFromUser(user2, RoomId);
            // Player player3 = GetInGamePlayerFromUser(user3, RoomId);
            GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Fold, -1, RoomId);
            Assert.True(GameBridge.GetPlayersInRoom(RoomId).Contains(player2));
        }


        //TODO -- fail - player dont enter the room
        [TestCase]
        public void FoldTestGood()
        {
            ////RestartSystem();
            //SetupUser1();
           // CreateGameWithUser1();
            int userId = new Random().Next();
            int userId2 = new Random().Next();
            int userId3 = new Random().Next();
            int roomId = new Random().Next();
            CreateGame(roomId,userId,100, true,GameMode.NoLimit, 2, 8, 0, 10);
            RegisterUserToDB(userId2);
           // RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(userId2);
            user2.AddMoney(1000);
            UserBridge.AddUserToGameRoomAsPlayer(userId2, roomId, user2.Money());
           // Assert.True(UserBridge.AddUserToGameRoomAsPlayer(userId2, roomId, user2.Money()));
            // RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            RegisterUserToDB(userId3);
            IUser user3 = UserBridge.getUserById(userId3);
            user3.AddMoney(1000);
            UserBridge.AddUserToGameRoomAsPlayer(userId3, roomId, user3.Money());

            //Assert.True(UserBridge.AddUserToGameRoomAsPlayer(userId3, roomId, user3.Money()));
            GameBridge.StartGame(userId, roomId);
            Console.WriteLine("room Id "+ roomId + "users 1, 2, 3: " + userId +" , "+userId2+" , "+userId3);
            IUser user1 = UserBridge.getUserById(userId);
            Player player1 = GetInGamePlayerFromUser(user1, roomId);
            Player player2 = GetInGamePlayerFromUser(user2, roomId);
            Player player3 = GetInGamePlayerFromUser(user3, roomId);
            Console.WriteLine("player 1 isActive: " + player1.isPlayerActive);
            Console.WriteLine("player 2 isActive: " + player2.isPlayerActive);
            Console.WriteLine("player 3 isActive: " + player3.isPlayerActive);
            GameBridge.DoAction(userId, CommunicationMessage.ActionType.Fold, -1, roomId);
            //Assert.True(GameBridge.GetPlayersInRoom(roomId).Contains(player1));
            //Assert.True(GameBridge.GetPlayersInRoom(roomId).Contains(player2));
            //Assert.True(GameBridge.GetPlayersInRoom(roomId).Contains(player3));
            Assert.False(player1.isPlayerActive);
            Assert.True(player2.isPlayerActive);
            Assert.True(player3.isPlayerActive);
            CleanUp(roomId);
        }

        [TestCase]
        public void CheckTestSad()
        {
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
            //cant raise not eungh money       
            Assert.False(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 0, RoomId));
        }

        [TestCase]
        public void CheckTestBad()
        {
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
            //cant raise not his turn       
            Assert.False(GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Bet, 0, RoomId));
        }

        [TestCase]
        public void CheckTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
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
            Assert.True(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 100, RoomId));
            Assert.True(GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Fold, -1, RoomId));
            Assert.True(GameBridge.DoAction(_userId3, CommunicationMessage.ActionType.Bet, 1000, RoomId));
            Assert.True(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 0, RoomId));

        }

        [TestCase]
        public void RaiseTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            GameBridge.StartGame(UserId, RoomId);
            Assert.True(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 100, RoomId));
            Assert.True(GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Bet, 1000, RoomId));
            Assert.True(GameBridge.DoAction(_userId3, CommunicationMessage.ActionType.Bet, 10000, RoomId));

        }

        [TestCase]
        public void RaiseTestSad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            GameBridge.StartGame(UserId, RoomId);
            Assert.False(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 9, RoomId));
        }

        [TestCase]
        public void RaiseTestBad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(100000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            GameBridge.StartGame(UserId, RoomId);
            Assert.False(GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Bet, 100, RoomId));
        }

        [TestCase]
        public void CallTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
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
            Assert.True(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 10, RoomId));
        }

        [TestCase]
        public void CallTestSad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
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
            Assert.False(GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 5, RoomId));
        }

        [TestCase]
        public void CallTestBad()
        {
            //RestartSystem();
            UserId = SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            int roomId = new Random().Next();
            CreateGame(roomId, UserId, 100, true, GameMode.NoLimit, 2, 8, 0, 10);
            _userId2 = new Random().Next();
            RegisterUser(_userId2, _user2Name + _userId2, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, roomId, user2.Money()));
            _userId3 = new Random().Next();
            RegisterUser(_userId3, _user3Name + _userId3, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, roomId, user3.Money()));
            GameBridge.StartGame(UserId, roomId);
            Assert.False(GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Bet, -10, roomId));
            CleanUp(roomId);
        }

        [TestCase]
        public void UknownUserTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            Assert.True(user2.IsUnKnow());
            Assert.True(user3.IsUnKnow());
            Assert.True(user1.IsUnKnow());
        }

        [TestCase]
        public void UknownUserTestBad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            user1.AddMoney(100000000);
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            Assert.True(user2.IsUnKnow());
            Assert.True(user3.IsUnKnow());
            Assert.True(user1.IsUnKnow());
            Assert.False(user2.Points() > 0);
            Assert.False(user3.Points() > 0);
            Assert.False(user1.Points() > 0);
        }

        [TestCase]
        public void redistributesThePlayersAmongTheLeaguesGood()
        {
            //RestartSystem();
            //SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
            IUser user4 = UserBridge.getUserById(_userId4);
            RegisterUser(_userId5, _user5Name, _user5Pw, _user5EmailGood);
            IUser user5 = UserBridge.getUserById(_userId5);
            RegisterUser(_userId6, _user6Name, _user6Pw, _user6EmailGood);
            IUser user6 = UserBridge.getUserById(_userId6);
            for (int i = 0; i < 11; i++)
            {
                user6.IncGamesPlay();
                user5.IncGamesPlay();
                user3.IncGamesPlay();
                user4.IncGamesPlay();
                user2.IncGamesPlay();
                user1.IncGamesPlay();
            }

            Assert.True(user2.EditUserPoints(100));
            Assert.True(user1.EditUserPoints(10000));
            Assert.True(UserBridge.DividLeage());
            Assert.True(user1.GetLeague() == LeagueName.A);
            Assert.True(user2.GetLeague() == LeagueName.A);

        }

        [TestCase]
        public void redistributesThePlayersAmongTheLeaguesSad()
        {
            //RestartSystem();
            //SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
            IUser user4 = UserBridge.getUserById(_userId4);
            RegisterUser(_userId5, _user5Name, _user5Pw, _user5EmailGood);
            IUser user5 = UserBridge.getUserById(_userId5);
            RegisterUser(_userId6, _user6Name, _user6Pw, _user6EmailGood);
            IUser user6 = UserBridge.getUserById(_userId6);
            for (int i = 0; i < 11; i++)
            {
                user6.IncGamesPlay();
                user5.IncGamesPlay();
                user3.IncGamesPlay();
                user4.IncGamesPlay();
                user2.IncGamesPlay();
                user1.IncGamesPlay();
            }

            Assert.True(user2.EditUserPoints(100));
            Assert.True(user1.EditUserPoints(10000));
            Assert.True(UserBridge.DividLeage());
            //both spoused to be in leage A
            Assert.True(user1.GetLeague() == LeagueName.A);
            Assert.True(user2.GetLeague() == LeagueName.A);

        }

        [TestCase]
        public void redistributesThePlayersAmongTheLeaguesBad()
        {
            //RestartSystem();
            //SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
            IUser user4 = UserBridge.getUserById(_userId4);
            RegisterUser(_userId5, _user5Name, _user5Pw, _user5EmailGood);
            IUser user5 = UserBridge.getUserById(_userId5);
            RegisterUser(_userId6, _user6Name, _user6Pw, _user6EmailGood);
            IUser user6 = UserBridge.getUserById(_userId6);
            for (int i = 0; i < 11; i++)
            {
                user6.IncGamesPlay();
                user5.IncGamesPlay();
                user3.IncGamesPlay();
                user4.IncGamesPlay();
                user2.IncGamesPlay();
                user1.IncGamesPlay();
            }

            Assert.True(user2.EditUserPoints(100));
            Assert.True(user1.EditUserPoints(10000));
            Assert.True(UserBridge.DividLeage());
            Assert.True(user1.GetLeague() == LeagueName.A);
            Assert.True(user2.GetLeague() == LeagueName.A);
            Assert.True(user3.GetLeague() == LeagueName.B);
            Assert.True(user4.GetLeague() == LeagueName.B);
            Assert.True(user5.GetLeague() == LeagueName.C);
            Assert.True(user6.GetLeague() == LeagueName.C);


        }

        [TestCase]
        public void LeaderBoardByNumOfGamesTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
            IUser user4 = UserBridge.getUserById(_userId4);
            RegisterUser(_userId5, _user5Name, _user5Pw, _user5EmailGood);
            IUser user5 = UserBridge.getUserById(_userId5);
            RegisterUser(_userId6, _user6Name, _user6Pw, _user6EmailGood);
            IUser user6 = UserBridge.getUserById(_userId6);

            IncWinAndPoints(user1, 100, 1100, 1);
            IncWinAndPoints(user2, 200, 1200, 2);
            IncWinAndPoints(user3, 300, 1300, 3);
            IncWinAndPoints(user4, 400, 1400, 4);
            IncWinAndPoints(user5, 500, 1500, 5);
            IncWinAndPoints(user6, 600, 1600, 6);

            List<IUser> users = UserBridge.GetUsersByNumOfGames();
            Assert.IsTrue(users[0] == user6);
            Assert.IsTrue(users[1] == user5);
            Assert.IsTrue(users[2] == user4);
            Assert.IsTrue(users[3] == user3);
            Assert.IsTrue(users[4] == user2);
            Assert.IsTrue(users[5] == user1);
        }

        [TestCase]
        public void LeaderBoardByNumOfGamesTestSad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            IncWinAndPoints(user1, 100, 1100, 1);

            List<IUser> users = UserBridge.GetUsersByNumOfGames();
            Assert.IsTrue(users[0] == user1);
        }

        [TestCase]
        public void LeaderBoardByNumOfGamesTestBad()
        {
            //RestartSystem();
            List<IUser> users = UserBridge.GetUsersByNumOfGames();
            Assert.IsEmpty(users);
        }

        [TestCase]
        public void LeaderBoardByHighestCashTestGood()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

            
            IUser user1 = UserBridge.getUserById(UserId);
            _userId2 = new Random().Next();
            _user2Name = "orelie" + _userId2;
            UserBridge.RegisterUser(_userId2, _user2Name, User1Pw, UserEmailGood1);
     //       RegisterUser(_userId2, _user2Name, User1Pw, UserEmailGood1);
            IUser user2 = UserBridge.getUserById(_userId2);
            _userId3 = new Random().Next();
            _user3Name = "orelie" + _userId3;
            RegisterUser(_userId3, _user3Name, User1Pw, UserEmailGood1);
            IUser user3 = UserBridge.getUserById(_userId3);
            _userId4 = new Random().Next();
            _user4Name = "orelie" + _userId4;
            RegisterUser(_userId4, _user4Name, User1Pw, UserEmailGood1);
            IUser user4 = UserBridge.getUserById(_userId4);
            _userId5 = new Random().Next();
            _user5Name = "orelie" + _userId5;
            RegisterUser(_userId5, _user5Name, User1Pw, UserEmailGood1);
            IUser user5 = UserBridge.getUserById(_userId5);
            _userId6 = new Random().Next();
            _user6Name = "orelie" + _userId6;
            RegisterUser(_userId6, _user6Name, User1Pw, UserEmailGood1);
            IUser user6 = UserBridge.getUserById(_userId6);


            //IncWinAndPoints(user1, 1000000, 1100, 10);
            //IncWinAndPoints(user2, 2000000, 1200, 20);
            //IncWinAndPoints(user3, 3000000, 1300, 30);
            //IncWinAndPoints(user4, 4000000, 1400, 40);
            //IncWinAndPoints(user5, 5000000, 1500, 50);
            //IncWinAndPoints(user6, 6000000, 1600, 60);

            user1.UpdateHighestCashInGame(1000000);
            user2.UpdateHighestCashInGame(2000000);
            user3.UpdateHighestCashInGame(3000000);
            user4.UpdateHighestCashInGame(4000000);
            user5.UpdateHighestCashInGame(5000000);
            user6.UpdateHighestCashInGame(6000000);

            List<IUser> users = UserBridge.GetUsersByHighestCash();
            Assert.IsTrue(users[0].Id() == user6.Id());
            Assert.IsTrue(users[1].Id() == user5.Id());
            Assert.IsTrue(users[2].Id() == user4.Id());
            Assert.IsTrue(users[3].Id() == user3.Id());
            Assert.IsTrue(users[4].Id() == user2.Id());
            Assert.IsTrue(users[5].Id() == user1.Id());
            UserBridge.DeleteUser(UserId);
            UserBridge.DeleteUser(_userId2);
            UserBridge.DeleteUser(_userId3);
            UserBridge.DeleteUser(_userId4);
            UserBridge.DeleteUser(_userId5);
            UserBridge.DeleteUser(_userId6);
            
        }

        [TestCase]
        public void LeaderBoardByHighestCashTestSad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            IncWinAndPoints(user1, 100, 1100, 1);

            List<IUser> users = UserBridge.GetUsersByHighestCash();
            Assert.IsTrue(users[0] == user1);
        }

        [TestCase]
        public void LeaderBoardByHighestCashTestBad()
        {
            List<IUser> users = UserBridge.GetUsersByHighestCashn();
            Assert.IsEmpty(users);
        }

        [TestCase]
        public void LeaderBoardByTotalProfitTestGood()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
            IUser user4 = UserBridge.getUserById(_userId4);
            RegisterUser(_userId5, _user5Name, _user5Pw, _user5EmailGood);
            IUser user5 = UserBridge.getUserById(_userId5);
            RegisterUser(_userId6, _user6Name, _user6Pw, _user6EmailGood);
            IUser user6 = UserBridge.getUserById(_userId6);

            IncWinAndPoints(user1, 100, 1100, 1);
            IncWinAndPoints(user2, 200, 1200, 2);
            IncWinAndPoints(user3, 300, 1300, 3);
            IncWinAndPoints(user4, 400, 1400, 4);
            IncWinAndPoints(user5, 500, 1500, 5);
            IncWinAndPoints(user6, 600, 1600, 6);

            List<IUser> users = UserBridge.GetUsersByTotalProfit();
            Assert.IsTrue(users[0] == user6);
            Assert.IsTrue(users[1] == user5);
            Assert.IsTrue(users[2] == user4);
            Assert.IsTrue(users[3] == user3);
            Assert.IsTrue(users[4] == user2);
            Assert.IsTrue(users[5] == user1);
        }

        [TestCase]
        public void LeaderBoardByTotalProfitTestSad()
        {
            //RestartSystem();
            SetupUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            IncWinAndPoints(user1, 100, 1100, 1);

            List<IUser> users = UserBridge.GetUsersByTotalProfit();
            Assert.IsTrue(users[0] == user1);
        }

        [TestCase]
        public void LeaderBoardByTotalProfitTestBad()
        {
            //RestartSystem();
            List<IUser> users = UserBridge.GetUsersByTotalProfit();
            Assert.IsEmpty(users);
        }

        [TestCase]
        public void AverageCashTestGood()
        {
            // //RestartSystem();
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            UserBridge.RegisterUser(UserId, User1Name, User1Pw, UserEmailGood1);

            IUser user1 = UserBridge.getUserById(UserId);
            Assert.AreNotEqual(user1,null);
            //RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            //IUser user2 = UserBridge.getUserById(_userId2);
            //user1.IncGamesPlay();
            //user2.IncGamesPlay();
            //IncWinAndPoints(user1, 100, 1100, 1);
            //IncWinAndPoints(user2, 500, 1200, 2);
            //Assert.IsTrue(user1.GetAvgCashGainPerGame() == 100);
            //Assert.IsTrue(user2.GetAvgCashGainPerGame() == 500 / 2);
        }

        [TestCase]
        public void AverageCashTest_Bad_noGmesPlayed()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
           
            RegisterUser1();
            IUser user1 = UserBridge.getUserById(UserId);
         //   Assert.AreNotEqual(user1,null);
            Assert.IsTrue(user1.GetAvgCashGainPerGame() == 0.0);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void AverageGrossTest_Good()
        {
            UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            IncWinAndPoints(user1, 100, 1100, 1);
            Assert.IsTrue(user1.GetAvgProfit() == 100.0);
            UserBridge.DeleteUser(UserId);
        }

        [TestCase]
        public void AverageGrossTestBad()
        {
             UserId = new Random().Next();
            User1Name = "orelie" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
           
            RegisterUser1();
            IUser user1 = UserBridge.getUserById(UserId);
            IUser user2 = UserBridge.getUserById(_userId2);
            Assert.IsTrue(user1.GetAvgProfit() == 0);
            UserBridge.DeleteUser(UserId);
        }

        private void IncWinAndPoints(IUser user, int amount, int points, int numOfWins)
        {
            for (int i = 0; i < numOfWins; i++)
            {
                user.IncWinNum();
            }
            user.UpdateHighestCashInGame(amount);
            user.UpdateTotalProfit(amount);
            user.EditUserPoints(points);
        }

        private Player GetInGamePlayerFromUser(IUser user, int roomId)
        {

            foreach (Player player in GameBridge.GetPlayersInRoom(roomId))
            {
                if (player.user.Id() == user.Id())
                {
                    return player;
                }
            }
            return null;
        }
    }
}
