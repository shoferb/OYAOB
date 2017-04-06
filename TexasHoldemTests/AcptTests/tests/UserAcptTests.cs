using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class UserAcptTests
    {

        private IUserBridge _userBridge;
        private IGameBridge _gameBridge;

        private readonly int _userId1 = ConstVarDefs.UserId1;
        private int _userId2;
        private readonly int _roomId = ConstVarDefs.RoomId;
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
            if (_userBridge.IsUserLoggedIn(_userId1))
            {
                _userBridge.LogoutUser(_userId1);
            }

            //remove user1 from all Rooms
            if (_userBridge.GetUsersGameRooms(_userId1).Count > 0)
            {
                _userBridge.GetUsersGameRooms(_userId1).ForEach(gId => { _userBridge.RemoveUserFromRoom(_userId1, gId); });
            }
            
            //remove user2 from all Rooms
            if (_userId2 != -1 && _userBridge.GetUsersGameRooms(_userId2).Count > 0)
            {
                _userBridge.GetUsersGameRooms(_userId1).ForEach(gId => { _userBridge.RemoveUserFromRoom(_userId2, gId); });
            }

            //delete user2
            if (_userId2 != -1)
            {
                _userBridge.DeleteUser(_userId2);
            }

            //delete room1
            if (_gameBridge.DoesRoomExist(_roomId))
            {
                _gameBridge.RemoveGameRoom(_roomId);
            }

            _userBridge = null;
            _gameBridge = null;

            _userId2 = -1;

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

        //create a new game with user2 as only player
        private void CreateGameWithUser2()
        {
            _userId2 = _userBridge.GetNextFreeUserId();
            Assert.True(_gameBridge.CreateGameRoom(_userId2, _roomId));
        }

        private void LoginUser1()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));
        }

        //login
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

        //logout
        [TestCase]
        public void UserLogoutTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));
            Assert.True(_userBridge.LogoutUser(_userId1));
        }
        
        [TestCase]
        public void UserLogoutTestSad()
        {
            //user is not logged in!
            Assert.False(_userBridge.LogoutUser(_userId1));
        }
        
        [TestCase]
        public void UserLogoutTestBad()
        {
            Assert.False(_userBridge.LogoutUser(-1));
        }

        //register
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

        //edit
        [TestCase]
        public void UserEditNameTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditName(_userId1, "newName"));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), "newName");
            Assert.True(_userBridge.EditName(_userId1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(_userId1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditName(_userId1, "newName"));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(-1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), _userNameGood);
            Assert.False(_userBridge.EditName(_userId1, ""));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), _userNameGood);
            Assert.False(_userBridge.EditName(_userId1, _userNameBad));
            Assert.AreEqual(_userBridge.GetUserName(_userId1), _userNameGood);
        }

        [TestCase]
        public void UserEditPwTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditPw(_userId1, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood2);
            Assert.True(_userBridge.EditPw(_userId1, _userPwGood2, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(_userId1, _userPwGood1, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(_userId1, _userPwSad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(_userId1, _userPwSad, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(_userId1, _userPwSad, _userPwSad));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditPw(_userId1, _userPwGood1, _userPwGood2));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(-1, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(_userId1, _userPwBad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
            Assert.False(_userBridge.EditPw(_userId1, _userPwBad, _userPwBad));
            Assert.AreEqual(_userBridge.GetUserPw(_userId1), _userPwGood1);
        }

        [TestCase]
        public void UserEditEmailTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditEmail(_userId1, _userEmailGood2));
            Assert.AreEqual(_userBridge.GetUserEmail(_userId1), _userEmailGood2);
            Assert.True(_userBridge.EditEmail(_userId1, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(_userId1), _userEmailGood1);

        }
        
        [TestCase]
        public void UserEditEmailTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(_userId1, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(_userId1), _userEmailGood1);
        }
        
        [TestCase]
        public void UserEditEmailTestBad()
        {
            //user is not logged in:
            Assert.False(_userBridge.EditEmail(_userId1, _userEmailGood1));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(_userId1, _userEmailBad1));
            Assert.AreEqual(_userBridge.GetUserEmail(_userId1), _userEmailGood1);
            Assert.False(_userBridge.EditEmail(_userId1, _userEmailBad2));
            Assert.AreEqual(_userBridge.GetUserEmail(_userId1), _userEmailGood1);
        }

        //TODO: test edit avatar

        [TestCase]
        public void UserAddUserMoneyTestGood()
        {
            const int amountToChange = 100;
            int prevAmount = _userBridge.GetUserMoney(_userId1);
            Assert.True(_userBridge.AddUserMoney(_userId1, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(_userId1) - amountToChange);
            Assert.True(_userBridge.ReduceUserMoney(_userId1, -1 * amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(_userId1));
        }
        
        [TestCase]
        public void UserAddUserMoneyTestBad()
        {
            const int amountToChange = -100;
            int prevAmount = _userBridge.GetUserMoney(_userId1);
            Assert.False(_userBridge.AddUserMoney(_userId1, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(_userId1));
        }

        //add player to room
        [TestCase]
        public void UserAddToRoomAsPlayerAllMoneyTestGood()
        {
            int money = _userBridge.GetUserMoney(_userId1);

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, money));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));
            Assert.AreEqual(money, _userBridge.GetUserMoney(_userId1));
            Assert.AreEqual(money, _userBridge.GetUserChips(_userId1));
            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));

        }

        [TestCase]
        public void UserAddToRoomAsPlayerNoMoneyTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));
            Assert.AreEqual(0, _userBridge.GetUserMoney(_userId1));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerTestSad()
        {
            int nonExistantRoomId = _gameBridge.GetNextFreeRoomId();

            LoginUser1();

            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(_userId1, nonExistantRoomId, 0));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, nonExistantRoomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(nonExistantRoomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegMoneyTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negative amount of money
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, -1));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegRoomTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negtive room id
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(_userId1, -1, 0));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
        }
        
        [TestCase]
        public void UserAddToRoomAsPlayerNegUserTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();   

            //negtive user id
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(-1, _roomId, 0));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerAllreadySpectatorInRoomTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(_userId1, _roomId));
            Assert.False(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 1));
        }


        //TODO: test add to room in different league

        //add spectator to room
        [TestCase]
        public void UserAddToRoomAsSpectatorGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.AreEqual(0, _userBridge.GetUserMoney(_userId1));
            Assert.AreEqual(0, _userBridge.GetUserChips(_userId1));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorNonExsistantRoomTestSad()
        {
            int nonExistantRoomId = _gameBridge.GetNextFreeRoomId();

            LoginUser1();

            Assert.False(_userBridge.AddUserToGameRoomAsSpectator(_userId1, nonExistantRoomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, nonExistantRoomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(nonExistantRoomId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorAllreadyPlayerInRoomTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();
            
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 1));
            Assert.False(_userBridge.AddUserToGameRoomAsSpectator(_userId1, _roomId));
        }
        
        //remove from game
        [TestCase]
        public void UserRemoveFromGamePlayerDealerMovesTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            int dealerId = _gameBridge.GetDealerId(_roomId);

            Assert.True(_userBridge.RemoveUserFromRoom(dealerId, _roomId));

            Assert.AreNotEqual(dealerId, _gameBridge.GetDealerId(_roomId));

        }
        
        [TestCase]
        public void UserRemoveFromGamePlayerBbMovesTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            int bbId = _gameBridge.GetBbId(_roomId);

            Assert.True(_userBridge.RemoveUserFromRoom(bbId, _roomId));

            Assert.AreNotEqual(bbId, _gameBridge.GetDealerId(_roomId));

        }
        
        [TestCase]
        public void UserRemoveFromGamePlayerSbMovesTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            int sbId = _gameBridge.GetSbId(_roomId);

            Assert.True(_userBridge.RemoveUserFromRoom(sbId, _roomId));

            Assert.AreNotEqual(sbId, _gameBridge.GetDealerId(_roomId));

        }

        [TestCase]
        public void UserRemoveFromGamePlayerNotifiesTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));

            //foreach player in the room, check that he got a notification that contains the word "left"
            List<int> playersInRoom = _gameBridge.GetPlayersInRoom(_roomId);
            playersInRoom.ForEach(id =>
            {
                List<String> notificationMsgs = _userBridge.GetUserNotificationMsgs(id);
                Assert.True(notificationMsgs.Exists(msg => msg.Contains("left")));
            });
        }

        //TODO: test log created after player leaves. need to contain some moves to make sense
        
        [TestCase]
        public void UserRemoveFromRoomSpectatorTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            int money = _userBridge.GetUserMoney(_userId1);
            int chips = _userBridge.GetUserChips(_userId1);

            //add user to Room as spectator
            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(_userId1, _roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Equals(money, _userBridge.GetUserMoney(_userId1));
            Assert.Equals(chips, _userBridge.GetUserChips(_userId1));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, _roomId)); //user2 should still be in Room
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerNonActiveRoomTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            int money = _userBridge.GetUserMoney(_userId1);
            int chips = _userBridge.GetUserChips(_userId1);

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Equals(money, _userBridge.GetUserMoney(_userId1));
            Assert.Equals(chips, _userBridge.GetUserChips(_userId1));

            Assert.True(_gameBridge.IsUserInRoom(_userId2, _roomId)); //user2 should still be in Room
            Assert.Contains(_roomId, _userBridge.GetReplayableGames(_userId1));
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerActiveRoomTestGood()
        {
            CreateGameWithUser2();

            LoginUser1();

            int money = _userBridge.GetUserMoney(_userId1);
            int chips = _userBridge.GetUserChips(_userId1);
            int losses = _userBridge.GetUserLosses(_userId1);

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            //make room active
            Assert.True(_gameBridge.StartGame(_roomId));

            //remove user from Room
            Assert.True(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
            Assert.False(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Equals(money, _userBridge.GetUserMoney(_userId1));
            Assert.Equals(chips, _userBridge.GetUserChips(_userId1));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, _roomId)); //user2 should still be in Room
            Assert.Contains(_roomId, _userBridge.GetReplayableGames(_userId1));

            //now has more losses
            Assert.Greater(_userBridge.GetUserLosses(_userId1), losses);
        }
        
        [TestCase]
        public void UserRemoveFromRoomPlayerNotInRoomTestBad()
        {
            //remove player from a non existant Room
            Assert.False(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
            
            CreateGameWithUser2();

            //remove user from Room he is not in
            Assert.False(_userBridge.RemoveUserFromRoom(_userId1, _roomId));
            Assert.False(_gameBridge.IsUserInRoom(_userId1, _roomId));
        }
        
        [TestCase]
        public void UserRemoveFromRoomPlayerWrongGameIdTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as player
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId1, _roomId, 0));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            //remove user from Room he is not in (fails)
            Assert.False(_userBridge.RemoveUserFromRoom(_userId1, _roomId + 1));
            Assert.True(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, _roomId)); //user2 should still be in Room            
        }
        
        [TestCase]
        public void UserRemoveFromRoomSpectatorTestBad()
        {
            CreateGameWithUser2();

            LoginUser1();

            //add user to Room as spectator
            Assert.True(_userBridge.AddUserToGameRoomAsSpectator(_userId1, _roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.Contains(_roomId, _userBridge.GetUsersGameRooms(_userId1));

            //remove user from Room he is not in
            Assert.False(_userBridge.RemoveUserFromRoom(_userId1, _roomId + 1));
            Assert.True(_userBridge.GetUsersGameRooms(_userId1).Contains(_roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId1, _roomId));
            Assert.True(_gameBridge.IsUserInRoom(_userId2, _roomId)); //user2 should still be in Room
        }

    }
}
