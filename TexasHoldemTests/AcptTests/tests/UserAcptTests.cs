using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class UserAcptTests : AcptTest
    {
        //private int _userId2;
        private string _userPwGood2;
        private string _userPwBad;
        private string _userPwSad;
        private string _registerNameGood;
        private string _userEmailGood2;
        private string _userEmailBad1;
        private string _userEmailBad2;
        private string _userAvatarGood;
        private string _userAvatarBad;
        private const string _userAvatarDefault = "default path. CHANGE ME WHEN THERE IS REAL PATH!"; //TODO: fill this up

        //setup: (called from case)
        protected override void SubClassInit()
        {
            _userPwGood2 = "goodPw5678";
            _userPwSad = "sadPw1234";
            _userPwBad = "short";

            _registerNameGood = "registerNameGood";

            _userEmailGood2 = "gooduser2@gmail.com";
            _userEmailBad1 = "מייל בעברית";
            _userEmailBad2 = "baduser"; //no @ sign

            _userAvatarGood = "/path/to/good/avatar"; //TODO: change this to real path
            _userAvatarBad = "I ain't no path!";
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            //delete registered users (except user1)
            string[] pwArr = { _userPwGood2, _userPwBad, _userPwSad, User1Pw };
            foreach (var s in pwArr)
            {
                if (!s.Equals(User1Pw))
                {
                    UserBridge.DeleteUser(User1Name, s);
                }
            }

            _userPwGood2 = null;
            _userPwBad = null;
            _userPwSad = null;

            _registerNameGood = null;

            UserEmailGood1 = null;
            _userEmailGood2 = null;

            UserEmailGood1 = null;
            _userEmailGood2 = null;
            _userEmailBad1 = null;
            _userEmailBad2 = null;
        }

        [TestCase]
        public void UserLoginTestGood()
        {
            Assert.True(UserBridge.LoginUser(User1Name, User1Pw));
        }

        [TestCase]
        public void UserLoginTestSad()
        {
            int rand = new Random().Next();
            Assert.False(UserBridge.LoginUser(rand.ToString(), _userPwSad));
            Assert.False(UserBridge.LoginUser(User1Name, _userPwSad));
        }

        [TestCase]
        public void UserLoginTestBad()
        {
            Assert.False(UserBridge.LoginUser("", ""));
            Assert.False(UserBridge.LoginUser("אני שם בעברית", User1Name));
        }

        //logout
        [TestCase]
        public void UserLogoutTestGood()
        {
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

            Assert.True(UserBridge.RegisterUser(_registerNameGood, User1Pw, UserEmailGood1) != -1);
            UserBridge.DeleteUser(_registerNameGood, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestSad()
        {
            RestartSystem();
            
            //user name already exists in system 
            Assert.True(UserBridge.RegisterUser(User1Name, User1Pw, User1Pw) != -1);
            Assert.False(UserBridge.RegisterUser(User1Name, User1Pw, User1Pw) != -1);
            UserBridge.DeleteUser(_registerNameGood, User1Pw);

            //pw not good
            Assert.False(UserBridge.RegisterUser(_registerNameGood, _userPwSad, UserEmailGood1) != -1);
            UserBridge.DeleteUser(_registerNameGood, _userPwSad);

            //email not good1:
            Assert.False(UserBridge.RegisterUser(_registerNameGood, User1Pw, _userEmailBad1) != -1);
            UserBridge.DeleteUser(_registerNameGood, User1Pw);
            
            //email not good2:
            Assert.False(UserBridge.RegisterUser(_registerNameGood, User1Pw, _userEmailBad2) != -1);
            UserBridge.DeleteUser(_registerNameGood, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestBadPwBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser(_registerNameGood, _userPwBad, _userPwBad) != -1);
            UserBridge.DeleteUser(_registerNameGood, _userPwBad);

            Assert.False(UserBridge.RegisterUser(_registerNameGood, _userPwBad, User1Pw) != -1);
            UserBridge.DeleteUser(_registerNameGood, _userPwBad);
            UserBridge.DeleteUser(_registerNameGood, User1Pw);
        }

        [TestCase]
        public void UserRegisterTestEmptysBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser("", User1Pw, User1Pw) != -1);
            UserBridge.DeleteUser("", User1Pw);

            Assert.False(UserBridge.RegisterUser(_registerNameGood, "", _userPwBad) != -1);
            UserBridge.DeleteUser(_registerNameGood, _userPwBad);

            Assert.False(UserBridge.RegisterUser(_registerNameGood, User1Pw, "") != -1);
        }

        [TestCase]
        public void UserRegisterTestNullsBad()
        {
            RestartSystem();

            Assert.False(UserBridge.RegisterUser(null, User1Pw, User1Pw) != -1);
            UserBridge.DeleteUser("", User1Pw);

            Assert.False(UserBridge.RegisterUser(_registerNameGood, null, _userPwBad) != -1);
            UserBridge.DeleteUser(_registerNameGood, _userPwBad);

            Assert.False(UserBridge.RegisterUser(_registerNameGood, User1Pw, null) != -1);
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
            //user is not logged in
            Assert.False(UserBridge.EditName(UserId, "newName"));

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

            Assert.True(UserBridge.EditPw(UserId, _userPwGood2));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), _userPwGood2);

            //set back
            Assert.True(UserBridge.EditPw(UserId, User1Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditPwTestSad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditPw(UserId, User1Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);

            Assert.False(UserBridge.EditPw(UserId, _userPwGood2));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);

            Assert.False(UserBridge.EditPw(UserId, _userPwSad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditPwTestBad()
        {
            //user is not logged in
            Assert.False(UserBridge.EditPw(UserId, _userPwGood2));

            RegisterUser1();

            Assert.False(UserBridge.EditPw(-1, _userPwGood2));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);

            Assert.False(UserBridge.EditPw(UserId, User1Pw));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);

            Assert.False(UserBridge.EditPw(UserId, _userPwBad));
            Assert.AreEqual(UserBridge.GetUserPw(UserId), User1Pw);
        }

        [TestCase]
        public void UserEditEmailTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditEmail(UserId, _userEmailGood2));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), _userEmailGood2);

            //set back
            Assert.True(UserBridge.EditAvatar(UserId, _userAvatarDefault));
            Assert.AreEqual(UserBridge.GetUserAvatar(UserId), _userAvatarDefault);
        }

        [TestCase]
        public void UserEditEmailTestSad()
        {
            RegisterUser1();

            Assert.False(UserBridge.EditEmail(UserId, UserEmailGood1));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
        }

        [TestCase]
        public void UserEditEmailTestBad()
        {
            //user is not logged in:
            Assert.False(UserBridge.EditEmail(UserId, UserEmailGood1));

            RegisterUser1();

            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad1));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);

            Assert.False(UserBridge.EditEmail(UserId, _userEmailBad2));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
        }

        [TestCase]
        public void UserEditAvatarTestGood()
        {
            RegisterUser1();

            Assert.True(UserBridge.EditAvatar(UserId, _userAvatarGood));
            Assert.AreEqual(UserBridge.GetUserAvatar(UserId), _userAvatarGood);

            //set back
            Assert.True(UserBridge.EditEmail(UserId, UserEmailGood1));
            Assert.AreEqual(UserBridge.GetUserEmail(UserId), UserEmailGood1);
        }

        [TestCase]
        public void UserSetUserRankByTopUserGood()
        {
            RegisterUser1();

            //make sure user1 is top user
            UserBridge.SetUserPoints(UserId, 999999999);

            int someUser = GetNextUser();
            Assert.True(UserBridge.SetUserPoints(someUser, 10, UserId));
        }

        [TestCase]
        public void UserSetUserRankByTopUserBad()
        {
            RegisterUser1();

            int someUser = GetNextUser();

            //make sure someUser is top user
            UserBridge.SetUserPoints(someUser, 999999999);

            //user1 is NOT top user
            Assert.False(UserBridge.SetUserPoints(someUser, 10, UserId));
        }

        [TestCase]
        public void UserAddUserMoneyTestGood()
        {
            const int amountToChange = 100;
            int prevAmount = UserBridge.GetUserMoney(UserId);
            Assert.True(UserBridge.AddUserMoney(UserId, amountToChange));
            Assert.True(prevAmount == UserBridge.GetUserMoney(UserId) - amountToChange);
            Assert.True(UserBridge.ReduceUserMoney(UserId, -1 * amountToChange));
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

        [TestCase]
        public void SetLeagueCriteriaTestGood()
        {
            RegisterUser1();

            //make sure user1 is top user
            UserBridge.SetUserPoints(UserId, 999999999);

            Assert.True(UserBridge.SetLeagueCriteria(UserId, 10));
        }

        [TestCase]
        public void SetLeagueCriteriaNotTopUserTestBad()
        {
            RegisterUser1();

            int someUser = GetNextUser();

            //make sure someUser is top user
            UserBridge.SetUserPoints(someUser, 99999999);

            Assert.False(UserBridge.SetLeagueCriteria(UserId, 10));
        }

        [TestCase]
        public void SetLeagueCriteriaBadCriteriaTestBad()
        {
            RegisterUser1();

            //make sure user1 is top user
            UserBridge.SetUserPoints(UserId, 999999999);

            Assert.False(UserBridge.SetLeagueCriteria(UserId, -1));
            Assert.False(UserBridge.SetLeagueCriteria(UserId, 0));
        }

        //add player to room
        [TestCase]
        public void UserAddToRoomAsPlayerAllMoneyTestGood()
        {
            int money = UserBridge.GetUserMoney(UserId);

            RegisterUser1();

            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, money));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerNoMoneyTestGood()
        {
            CreateGameWithUser();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);

            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
            Assert.GreaterOrEqual(money, UserBridge.GetUserMoney(UserId));
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerTestSad()
        {
            int nonExistantRoomId = GameBridge.GetNextFreeRoomId();

            RegisterUser1();

            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(UserId, nonExistantRoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(UserId, nonExistantRoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(nonExistantRoomId));
            Assert.AreEqual(0, UserBridge.GetUserChips(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerNegUserTestBad()
        {
            CreateGameWithUser();

            RegisterUser1();

            //negtive user id
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(-1, RoomId, 0));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.AreEqual(0, UserBridge.GetUserChips(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerAllreadySpectatorInRoomTestBad()
        {
            CreateGameWithUser();

            RegisterUser1();

            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 1));
        }

        [TestCase]
        public void UserAddToRoomAsPlayerDifferentLeagueTestBad()
        {
            int someUser = CreateGameWithUser();

            RegisterUser1();
            int rank = UserBridge.GetUserPoints(someUser);

            //make sure user1 and someUser are not in same league
            UserBridge.SetUserPoints(UserId, rank + 1000);
            Assert.False(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 1));
        }

        //add spectator to room
        [TestCase]
        public void UserAddToRoomAsSpectatorGood()
        {
            CreateGameWithUser();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);

            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.AreEqual(money, UserBridge.GetUserMoney(UserId));
            Assert.AreEqual(0, UserBridge.GetUserChips(UserId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorNonExsistantRoomTestSad()
        {
            int nonExistantRoomId = GameBridge.GetNextFreeRoomId();

            RegisterUser1();

            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, nonExistantRoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, nonExistantRoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(nonExistantRoomId));
        }

        [TestCase]
        public void UserAddToRoomAsSpectatorAllreadyPlayerInRoomTestBad()
        {
            CreateGameWithUser();

            RegisterUser1();

            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 1));
            Assert.False(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
        }

        //remove from game
        [TestCase]
        public void UserRemoveFromGamePlayerDealerMovesTestGood()
        {
            CreateGameWithUser();

            RegisterUser1();

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            int dealerId = GameBridge.GetDealerId(RoomId);

            Assert.True(UserBridge.RemoveUserFromRoom(dealerId, RoomId));

            Assert.AreNotEqual(dealerId, GameBridge.GetDealerId(RoomId));

        }

        [TestCase]
        public void UserRemoveFromGamePlayerBbMovesTestGood()
        {
            CreateGameWithUser();

            RegisterUser1();

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            int bbId = GameBridge.GetBbId(RoomId);

            Assert.True(UserBridge.RemoveUserFromRoom(bbId, RoomId));

            Assert.AreNotEqual(bbId, GameBridge.GetDealerId(RoomId));

        }

        [TestCase]
        public void UserRemoveFromGamePlayerSbMovesTestGood()
        {
            CreateGameWithUser();

            RegisterUser1();

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            int sbId = GameBridge.GetSbId(RoomId);

            Assert.True(UserBridge.RemoveUserFromRoom(sbId, RoomId));

            Assert.AreNotEqual(sbId, GameBridge.GetDealerId(RoomId));

        }

        [TestCase]
        public void UserRemoveFromGamePlayerGameIsSavedTestGood()
        {
            SetupUser1();
            Assert.IsNotEmpty(ReplayBridge.ViewReplay(RoomId, 1));
            Assert.AreEqual(2, ReplayBridge.ViewReplay(RoomId, 1).Count);
        }

        [TestCase]
        public void UserRemoveFromRoomSpectatorTestGood()
        {
            int userId2 = CreateGameWithUser();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);
            int chips = UserBridge.GetUserChips(UserId);

            //add user to Room as spectator
            Assert.True(UserBridge.AddUserToGameRoomAsSpectator(UserId, RoomId));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //remove user from Room
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Equals(money, UserBridge.GetUserMoney(UserId));
            Assert.Equals(chips, UserBridge.GetUserChips(UserId));
            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerNonActiveRoomTestGood()
        {
            int userId2 = CreateGameWithUser();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);
            int chips = UserBridge.GetUserChips(UserId);

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //remove user from Room
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Equals(money, UserBridge.GetUserMoney(UserId));
            Assert.Equals(chips, UserBridge.GetUserChips(UserId));

            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room
            Assert.Contains(RoomId, ReplayBridge.GetReplayableGames(UserId));
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerActiveRoomTestGood()
        {
            int userId2 = CreateGameWithUser();

            RegisterUser1();

            int money = UserBridge.GetUserMoney(UserId);
            int chips = UserBridge.GetUserChips(UserId);
            int rank = UserBridge.GetUserPoints(UserId);

            //add user to Room as player
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(UserId, RoomId, 0));
            Assert.True(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Contains(RoomId, UserBridge.GetUsersGameRooms(UserId));

            //make room active
            Assert.True(GameBridge.StartGame(RoomId));

            //remove user from Room
            Assert.True(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(UserBridge.GetUsersGameRooms(UserId).Contains(RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
            Assert.Equals(money, UserBridge.GetUserMoney(UserId));
            Assert.Equals(chips, UserBridge.GetUserChips(UserId));
            Assert.True(GameBridge.IsUserInRoom(userId2, RoomId)); //user2 should still be in Room
            Assert.Contains(RoomId, ReplayBridge.GetReplayableGames(UserId));

            //now rank <= prev rank
            Assert.Greater(UserBridge.GetUserPoints(UserId), rank);
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerNotInRoomTestBad()
        {
            //remove player from a non existant Room
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));

            CreateGameWithUser();

            //remove user from Room he is not in
            Assert.False(UserBridge.RemoveUserFromRoom(UserId, RoomId));
            Assert.False(GameBridge.IsUserInRoom(UserId, RoomId));
        }

        [TestCase]
        public void UserRemoveFromRoomPlayerWrongGameIdTestBad()
        {
            int userId2 = CreateGameWithUser();

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
            int userId2 = CreateGameWithUser();

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

    }
}
