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

        [SetUp]
        protected void Init()
        {
            UserBridge = new UserBridgeProxy();
            GameBridge = new GameBridgeProxy();

            UserNameGood = "Oded";
            UserPwGood = "goodPw1234";

            SubMethodInit();
        }

        //subclass' setup method
        protected abstract void SubMethodInit();

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
                UserBridge.GetUsersGameRooms(UserId).ForEach(gId => { UserBridge.RemoveUserFromRoom(UserId, gId); });
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

            SubMethodDispose();
        }

        //subclass' setup method
        protected abstract void SubMethodDispose();

        protected void LoginUser1()
        {
            Assert.True(UserBridge.LoginUser(UserNameGood, UserPwGood));
        }
    }
}
