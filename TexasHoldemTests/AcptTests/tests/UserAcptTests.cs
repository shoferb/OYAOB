using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class UserAcptTests
    {

        private IUserBridge _userBridge;
        private IGameBridge _gameBridge;

        private const int UserId1 = 0;
        private int _userId2;
        private const int RoomId = 0; //Room that exists
        private string _userNameGood;
        private string _userNameBad;
        private string _userPwGood1;
        private string _userPwGood2;
        private string _userPwBad;
        private string _userPwSad;
        private string _registerNameGood;
        private string _registerNameBad;
        private string _userEmailGood1;
        private string _userEmailGood2;
        private string _userEmailBad1;
        private string _userEmailBad2;

        [SetUp]
        public void Init()
        {
            _userBridge = new UserBridgeProxy();
            _gameBridge = new GameBridgeProxy();

            _userNameGood = "Oded";
            _userNameBad = "שם משתמש";

            _userPwGood1 = "goodPw1234";
            _userPwGood2 = "goodPw5678";
            _userPwSad = "sadPw1234";
            _userPwBad = "סיסמא";

            _registerNameGood = "registerNameGood";
            _registerNameBad = "שם משתמש רע לרישום";

            _userEmailGood1 = "gooduser1@gmail.com";
            _userEmailGood2 = "gooduser2@gmail.com";
            _userEmailBad1 = "מייל בעברית";
            _userEmailBad2 = "baduser"; //no @ sign
        }

        [TearDown]
        public void Dispose()
        {
            //logout user1
            if (_userBridge.IsUserLoggedIn(UserId1))
            {
                _userBridge.LogoutUser(UserId1);
            }

            //remove user1 from all Rooms
            if (_userBridge.GetUsersGameRooms(UserId1).Count > 0)
            {
                _userBridge.GetUsersGameRooms(UserId1).ForEach(gId => { _userBridge.RemoveUserFromRoom(UserId1, gId); });
            }

            //delete room1
            if (_gameBridge.DoesRoomExist(RoomId))
            {
                _gameBridge.RemoveGameRoom(RoomId);
            }

            _userBridge = null;
            _gameBridge = null;

            _userNameGood = null;
            _userNameBad = null;

            _userPwGood1 = null;
            _userPwGood2 = null;
            _userPwBad = null;
            _userPwSad = null;

            _registerNameGood = null;
            _registerNameBad = null;

            _userEmailGood1 = null;
            _userEmailGood2 = null;

            _userEmailGood1 = null;
            _userEmailGood2 = null;
            _userEmailBad1 = null;
            _userEmailBad2 = null;
        }

        [TestCase]
        public void UserLoginTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));
        }
        
        [TestCase]
        public void UserLoginTestSad()
        {
            Assert.False(_userBridge.LoginUser(_userNameBad, _userPwSad));
            Assert.False(_userBridge.LoginUser(_userNameGood, _userPwSad));
        }
        
        [TestCase]
        public void UserLoginTestBad()
        {
            Assert.False(_userBridge.LoginUser("", ""));
            Assert.False(_userBridge.LoginUser("אני שם בעברית", _userNameGood));
        }

        [TestCase]
        public void UserLogoutTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));
            Assert.True(_userBridge.LogoutUser(UserId1));
        }
        
        [TestCase]
        public void UserLogoutTestSad()
        {
            //user is not logged in!
            Assert.False(_userBridge.LogoutUser(UserId1));
        }
        
        [TestCase]
        public void UserLogoutTestBad()
        {
            Assert.False(_userBridge.LogoutUser(-1));
        }

        [TestCase]
        public void UserRegisterTestGood()
        {
            Assert.True(_userBridge.RegisterUser(_registerNameGood, _userPwGood1, _userPwGood1));
            _userBridge.DeleteUser(_registerNameGood, _userPwGood1);
        }
        
        [TestCase]
        public void UserRegisterTestSad()
        {
            //pw do not match
            Assert.True(_userBridge.RegisterUser(_registerNameGood, _userPwGood1, _userPwGood2));
            _userBridge.DeleteUser(_registerNameGood, _userPwGood1);

            //user name already exists in system 
            Assert.False(_userBridge.RegisterUser(_userNameGood, _userPwGood1, _userPwGood1));
            _userBridge.DeleteUser(_registerNameGood, _userPwGood1);

            //pw not good
            Assert.False(_userBridge.RegisterUser(_registerNameGood, _userPwSad, _userPwSad));
            _userBridge.DeleteUser(_registerNameGood, _userPwSad);
        }
        
        [TestCase]
        public void UserRegisterTestBad()
        {
            Assert.False(_userBridge.RegisterUser(_registerNameBad, _userPwGood1, _userPwGood1));
            _userBridge.DeleteUser(_registerNameBad, _userPwGood1);
            Assert.False(_userBridge.RegisterUser(_registerNameGood, _userPwBad, _userPwBad));
            _userBridge.DeleteUser(_registerNameGood, _userPwBad);
            Assert.False(_userBridge.RegisterUser(_registerNameGood, _userPwBad, _userPwGood1));
            _userBridge.DeleteUser(_registerNameGood, _userPwBad);
            _userBridge.DeleteUser(_registerNameGood, _userPwGood1);
            Assert.False(_userBridge.RegisterUser(_registerNameBad, _userPwGood1, _userPwGood1));
            _userBridge.DeleteUser(_registerNameBad, _userPwGood1);
            Assert.False(_userBridge.RegisterUser(_registerNameBad, _userPwBad, _userPwGood1));
            _userBridge.DeleteUser(_registerNameBad, _userPwGood1);
            _userBridge.DeleteUser(_registerNameBad, _userPwBad);
            Assert.False(_userBridge.RegisterUser(_registerNameBad, _userPwBad, _userPwBad));
            _userBridge.DeleteUser(_registerNameBad, _userPwBad);
            Assert.False(_userBridge.RegisterUser("", _userPwGood1, _userPwGood1));
            _userBridge.DeleteUser("", _userPwGood1);
            Assert.False(_userBridge.RegisterUser(_registerNameGood, "", _userPwBad));
            _userBridge.DeleteUser(_registerNameGood, _userPwBad);
            Assert.False(_userBridge.RegisterUser(_registerNameGood, _userPwGood1, ""));
            _userBridge.DeleteUser(_registerNameGood, _userPwGood1);
        }

        [TestCase]
        public void UserEditNameTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditName(UserId1, "newName"));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), "newName");
            Assert.True(_userBridge.EditName(UserId1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(UserId1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditName(UserId1, "newName"));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(-1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), _userNameGood);
            Assert.False(_userBridge.EditName(UserId1, ""));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), _userNameGood);
            Assert.False(_userBridge.EditName(UserId1, _userNameBad));
            Assert.AreEqual(_userBridge.GetUserName(UserId1), _userNameGood);
        }

        [TestCase]
        public void UserEditPwTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditPw(UserId1, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood2);
            Assert.True(_userBridge.EditPw(UserId1, _userPwGood2, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(UserId1, _userPwGood1, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId1, _userPwSad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId1, _userPwSad, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId1, _userPwSad, _userPwSad));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditPw(UserId1, _userPwGood1, _userPwGood2));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(-1, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId1, _userPwBad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId1, _userPwBad, _userPwBad));
            Assert.AreEqual(_userBridge.GetUserPw(UserId1), _userPwGood1);
        }

        [TestCase]
        public void UserEditEmailTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditEmail(UserId1, _userEmailGood2));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId1), _userEmailGood2);
            Assert.True(_userBridge.EditEmail(UserId1, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId1), _userEmailGood1);

        }
        
        [TestCase]
        public void UserEditEmailTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(UserId1, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId1), _userEmailGood1);
        }
        
        [TestCase]
        public void UserEditEmailTestBad()
        {
            //user is not logged in:
            Assert.False(_userBridge.EditEmail(UserId1, _userEmailGood1));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(UserId1, _userEmailBad1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId1), _userEmailGood1);
            Assert.False(_userBridge.EditEmail(UserId1, _userEmailBad2));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId1), _userEmailGood1);
        }

        //TODO: test edit avatar

        [TestCase]
        public void UserAddUserMoneyTestGood()
        {
            const int amountToChange = 100;
            int prevAmount = _userBridge.GetUserMoney(UserId1);
            Assert.True(_userBridge.AddUserMoney(UserId1, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId1) - amountToChange);
            Assert.True(_userBridge.ReduceUserMoney(UserId1, -1 * amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId1));
        }
        
        [TestCase]
        public void UserAddUserMoneyTestBad()
        {
            const int amountToChange = -100;
            int prevAmount = _userBridge.GetUserMoney(UserId1);
            Assert.False(_userBridge.AddUserMoney(UserId1, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId1));
        }

        //create a new game with user2 as only player
        private void CreateGameWithUser2()
        {
            _userId2 = _userBridge.GetNextFreeUserId();
            Assert.True(_gameBridge.CreateGameRoom(_userId2, RoomId));
        }

        private void LoginUser1()
        {
           Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerAllMoneyTestGood()
        {
            int money = _userBridge.GetUserMoney(UserId1);

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(UserId1, RoomId, money));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));
            Assert.AreEqual(money, _userBridge.GetUserMoney(UserId1));
            Assert.AreEqual(money, _userBridge.GetUserChips(UserId1));
            Assert.True(_userBridge.RemoveUserFromRoom(UserId1, RoomId));

        }

        [TestCase]
        public void UserAddToRoomAsPlayerNoMoneyTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(UserId1, RoomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));
            Assert.AreEqual(0, _userBridge.GetUserMoney(UserId1));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
            Assert.True(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
        }


        [TestCase]
        public void UserAddToRoomAsPlayerTestSad()
        {
            int nonExistantRoomId = _gameBridge.GetNextFreeRoomId();

            LoginUser1();

            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(UserId1, nonExistantRoomId, 0));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, nonExistantRoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(nonExistantRoomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegMoneyTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negative amount of money
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(UserId1, RoomId, -1));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegRoomTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negtive room id
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(UserId1, -1, 0));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegUserTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negtive user id
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(-1, RoomId, 0));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
        }



        [TestCase]
        public void UserAddToRoomAsSpectatorGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.AreEqual(0, _userBridge.GetUserMoney(UserId1));
            Assert.AreEqual(0, _userBridge.GetUserChips(UserId1));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorTestSad()
        {
            int nonExistantRoomId = _gameBridge.GetNextFreeRoomId();

            LoginUser1();

            Assert.False(_userBridge.AddUserToGameRoomAsSpectator(UserId1, nonExistantRoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, nonExistantRoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(nonExistantRoomId));
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(UserId1, RoomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room
            Assert.Contains(RoomId, _userBridge.GetReplayableGames(UserId1));

            //add user to Room as spectator
            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room
        }
        
        [TestCase]
        public void UserRemoveFromRoomSpectatorTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as spectator
            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
            Assert.False(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room
        }
        
        [TestCase]
        public void UserRemoveFromRoomPlayerNotInRoomTestBad()
        {
            //remove player from a non existant Room
            Assert.False(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
            
            CreateGameWithUser2();

            //remove user from Room he is not in
            Assert.False(_userBridge.RemoveUserFromRoom(UserId1, RoomId));
            Assert.False(_gameBridge.IsUserInRoom(UserId1, RoomId));
        }
        
        [TestCase]
        public void UserRemoveFromRoomPlayerWrongGameIdTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(UserId1, RoomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));

            //remove user from Room he is not in (fails)
            Assert.False(_userBridge.RemoveUserFromRoom(UserId1, RoomId + 1));
            Assert.True(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room            
        }
        
        [TestCase]
        public void UserRemoveFromRoomSpectatorTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as spectator
            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.Contains(RoomId, _userBridge.GetUsersGameRooms(UserId1));

            //remove user from Room he is not in
            Assert.False(_userBridge.RemoveUserFromRoom(UserId1, RoomId + 1));
            Assert.True(_userBridge.GetUsersGameRooms(UserId1).Contains(RoomId));
            Assert.True(_gameBridge.IsUserInRoom(UserId1, RoomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, RoomId)); //user2 should still be in Room
        }

    }
}
