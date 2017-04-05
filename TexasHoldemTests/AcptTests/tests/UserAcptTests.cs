using System;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class UserAcptTests
    {

        private IUserBridge _userBridge;
        private IGameBridge _gameBridge;

        private static readonly int UserId = 0;
        private static readonly int GameId = 0; //game that exists
        private string _userNameGood;
        private string _userNameBad;
        private string _userPwGood1;
        private string _userPwGood2;
        private string _userPwBad;
        private string _registerNameGood;
        private string _registerNameBad;
        private string _userPwSad;
        private string _userEmailGood1;
        private string _userEmailGood2;
        private string _userEmailBad1;
        private string _userEmailBad2;

        [SetUp]
        public void Init()
        {
            //setup vars:
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
            if (_userBridge.IsUserLoggedIn(UserId))
            {
                _userBridge.LogoutUser(UserId);
            }

            if (_userBridge.GetUsersGames(UserId).Contains(GameId))
            {
                _userBridge.RemoveUserFromGame(UserId, GameId);
            }
            if (_gameBridge.DoesGameExist(GameId))
            {
                _gameBridge.RemoveGame(GameId);
            }

            _userBridge = null;

            _userNameGood = null;
            _userNameBad = null;

            _userPwGood1 = null;
            _userPwBad = null;
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
            Assert.True(_userBridge.LogoutUser(UserId));
        }
        
        [TestCase]
        public void UserLogoutTestSad()
        {
            //user is not logged in!
            Assert.False(_userBridge.LogoutUser(UserId));
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

            Assert.True(_userBridge.EditName(UserId, "newName"));
            Assert.AreEqual(_userBridge.GetUserName(UserId), "newName");
            Assert.True(_userBridge.EditName(UserId, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(UserId, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId), _userNameGood);
        }
        
        [TestCase]
        public void UserEditNameTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditName(UserId, "newName"));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditName(-1, _userNameGood));
            Assert.AreEqual(_userBridge.GetUserName(UserId), _userNameGood);
            Assert.False(_userBridge.EditName(UserId, ""));
            Assert.AreEqual(_userBridge.GetUserName(UserId), _userNameGood);
            Assert.False(_userBridge.EditName(UserId, _userNameBad));
            Assert.AreEqual(_userBridge.GetUserName(UserId), _userNameGood);
        }

        [TestCase]
        public void UserEditPwTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditPw(UserId, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood2);
            Assert.True(_userBridge.EditPw(UserId, _userPwGood2, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(UserId, _userPwGood1, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId, _userPwSad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId, _userPwSad, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId, _userPwSad, _userPwSad));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
        }
        
        [TestCase]
        public void UserEditPwTestBad()
        {
            //user is not logged in
            Assert.False(_userBridge.EditPw(UserId, _userPwGood1, _userPwGood2));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditPw(-1, _userPwGood1, _userPwGood2));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId, _userPwBad, _userPwGood1));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
            Assert.False(_userBridge.EditPw(UserId, _userPwBad, _userPwBad));
            Assert.AreEqual(_userBridge.GetUserPw(UserId), _userPwGood1);
        }

        [TestCase]
        public void UserEditEmailTestGood()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.True(_userBridge.EditEmail(UserId, _userEmailGood2));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId), _userEmailGood2);
            Assert.True(_userBridge.EditEmail(UserId, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId), _userEmailGood1);

        }
        
        [TestCase]
        public void UserEditEmailTestSad()
        {
            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(UserId, _userEmailGood1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId), _userEmailGood1);
        }
        
        [TestCase]
        public void UserEditEmailTestBad()
        {
            //user is not logged in:
            Assert.False(_userBridge.EditEmail(UserId, _userEmailGood1));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.EditEmail(UserId, _userEmailBad1));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId), _userEmailGood1);
            Assert.False(_userBridge.EditEmail(UserId, _userEmailBad2));
            Assert.AreEqual(_userBridge.GetUserEmail(UserId), _userEmailGood1);
        }

        //TODO: test edit avatar

        [TestCase]
        public void UserAddUserMoneyTestGood()
        {
            const int amountToChange = 100;
            int prevAmount = _userBridge.GetUserMoney(UserId);
            Assert.True(_userBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId) - amountToChange);
            Assert.True(_userBridge.ReduceUserMoney(UserId, -1 * amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId));
        }
        
        [TestCase]
        public void UserAddUserMoneyTestBad()
        {
            const int amountToChange = -100;
            int prevAmount = _userBridge.GetUserMoney(UserId);
            Assert.False(_userBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == _userBridge.GetUserMoney(UserId));
        }

        [TestCase]
        public void UserAddToGameAsPlayerTestGood()
        {
            Assert.True(_gameBridge.CreateGame(GameId));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            int money = _userBridge.GetUserMoney(UserId);

            Assert.True(_userBridge.AddUserToGameAsPlayer(UserId, GameId, money));
            Assert.Contains(GameId, _userBridge.GetUsersGames(UserId));
            Assert.AreEqual(0, _userBridge.GetUserMoney(UserId));
            Assert.True(_userBridge.RemoveUserFromGame(UserId, GameId));

            Assert.True(_userBridge.AddUserToGameAsPlayer(UserId, GameId, 0));
            Assert.Contains(GameId, _userBridge.GetUsersGames(UserId));
            Assert.AreEqual(money, _userBridge.GetUserMoney(UserId));
            Assert.True(_userBridge.RemoveUserFromGame(UserId, GameId));
        }
        
        [TestCase]
        public void UserAddToGameAsPlayerTestSad()
        {
            int nonExistantGameId = _gameBridge.GetNextFreeId();

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            Assert.False(_userBridge.AddUserToGameAsPlayer(UserId, nonExistantGameId, 0));
            Assert.False(_userBridge.GetUsersGames(UserId).Contains(nonExistantGameId));
        }
        
        [TestCase]
        public void UserAddToGameAsPlayerTestBad()
        {
            Assert.True(_gameBridge.CreateGame(GameId));

            Assert.True(_userBridge.LoginUser(_userNameGood, _userPwGood1));

            //negative amount of money
            Assert.False(_userBridge.AddUserToGameAsPlayer(UserId, GameId, -1));
            Assert.False(_userBridge.GetUsersGames(UserId).Contains(GameId));

            //negtive game id
            Assert.False(_userBridge.AddUserToGameAsPlayer(UserId, -1, 0));
            Assert.False(_userBridge.GetUsersGames(UserId).Contains(GameId));

            //negtive user id
            Assert.False(_userBridge.AddUserToGameAsPlayer(-1, GameId, 0));
            Assert.False(_userBridge.GetUsersGames(UserId).Contains(GameId));
        }

    }
}
