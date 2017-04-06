using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{

    //parent class for all acceptance test classes.
    public abstract class AcptTest
    {
        protected IUserBridge UserBridge;
        protected IGameBridge GameBridge;
        protected const int UserId = 0; //user1 must all ready be in system when tests start.
        protected string UserNameGood;
        protected string UserPwGood;
        protected const int RoomId = 0; //room1 must NOT exist when tests start.

        protected AcptTest()
        {
            UserBridge = new UserBridgeProxy();
            GameBridge = new GameBridgeProxy();

            UserNameGood = "Oded";
            UserPwGood = "goodPw1234";

            SetupUser1();
        }

        [SetUp]
        protected void Init()
        {
            UserNameGood = "Oded";
            UserPwGood = "goodPw1234";

            SubClassInit();
        }

        [TearDown]
        protected void Dispose()
        {
            //logout user1
            if (UserBridge.IsUserLoggedIn(UserId))
            {
                UserBridge.LogoutUser(UserId);
            }

            //remove user1 from all Rooms
            if (UserBridge.GetUsersGameRooms(UserId).Count > 0)
            {
                UserBridge.GetUsersGameRooms(UserId).ForEach(gId =>
                {
                    UserBridge.RemoveUserFromRoom(UserId, gId);
                });
            }

            //delete room1
            if (GameBridge.DoesRoomExist(RoomId))
            {
                GameBridge.RemoveGameRoom(RoomId);
            }

            UserBridge = null;
            GameBridge = null;
            UserNameGood = null;
            UserPwGood = null;

            SubClassDispose();
        }

        //subclass' setup method
        protected abstract void SubClassInit();

        //subclass' setup method
        protected abstract void SubClassDispose();

        //make sure user1 exists and has at least 1 replayable game
        protected void SetupUser1()
        {
            if (!UserBridge.IsThereUser(UserId))
            {
                UserBridge.RegisterUser(UserNameGood, UserPwGood, UserNameGood);
            }

            //create a new game and run it 1 move, then leave it
            if (UserBridge.GetReplayableGames(UserId).Count == 0)
            {
                int newRoomId = GameBridge.CreateGameRoom(UserId);
                int userId2 = UserBridge.GetNextFreeUserId();
                UserBridge.RegisterUser(UserNameGood + '!', UserPwGood, UserNameGood);
                int money = UserBridge.GetUserMoney(userId2);
                UserBridge.AddUserToGameRoomAsPlayer(userId2, newRoomId, money);
                GameBridge.StartGame(newRoomId);

                //maybe not good?
                GameBridge.Check(UserId, newRoomId);
                GameBridge.Check(userId2, newRoomId);

                UserBridge.RemoveUserFromRoom(userId2, newRoomId);
                UserBridge.RemoveUserFromRoom(UserId, newRoomId);

                UserBridge.DeleteUser(userId2);
            }
        }

        protected void LoginUser1()
        {
            Assert.True(UserBridge.LoginUser(UserNameGood, UserPwGood));
        }
    }
}
