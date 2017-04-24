using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;
using TexasHoldemTests.AcptTests.Bridges.Interface;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{

    //parent class for all acceptance test classes.
    public abstract class AcptTest
    {
        protected IUserBridge UserBridge;
        protected IGameBridge GameBridge;
        protected IReplayBridge ReplayBridge;
        protected const int UserId = 0; //user1 must all ready be in system when tests start.
        protected string User1Name;
        protected string User1Pw;
        protected const int RoomId = 0; //room1 must NOT exist when tests start.
        protected List<int> OtherUsers; //list holding all user ids used for testing except UserId1
        protected string UserEmailGood1;

        protected AcptTest()
        {
            UserBridge = new UserBridge();
            GameBridge = new GameBridge();
            ReplayBridge = new ReplayBridge();
            OtherUsers = new List<int>();

            User1Name = "Oded";
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";

            SetupUser1();
        }

        [SetUp]
        protected void Init()
        {
            User1Name = "Oded";
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";

            if (!GameBridge.DoesRoomExist(RoomId))
            {
                CreateGameWithUser();
            }

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

            //delete all other users
            OtherUsers.ForEach(uid =>
            {
                if (UserBridge.IsThereUser(uid))
                {
                    UserBridge.GetUsersGameRooms(uid).ForEach(gid =>
                    {
                        UserBridge.RemoveUserFromRoom(uid, gid);
                    });
                    UserBridge.DeleteUser(uid);
                }
            });

            SubClassDispose();

            UserBridge = null;
            GameBridge = null;
            User1Name = null;
            User1Pw = null;
        }

        //subclass' setup method
        protected abstract void SubClassInit();

        //subclass' setup method
        protected abstract void SubClassDispose();

        //create a new user, add to OtherUsers list and return the user's id
        protected int GetNextUser()
        {
            int randInt = new Random().Next();

            //int someUser = UserBridge.GetNextFreeUserId();
            UserBridge.RegisterUser(randInt.ToString(), User1Pw, User1Pw);

            OtherUsers.Add(randInt);

            return randInt;
        }

        //make sure user1 exists and has at least 1 replayable game
        protected void SetupUser1()
        {
            RegisterUser1();

            //create a new game and run it 1 move, then leave it
            if (ReplayBridge.GetReplayableGames(UserId).Count == 0)
            {
                int newRoomId = GameBridge.CreateGameRoom(UserId);
                int userId2 = GetNextUser();
                int money = UserBridge.GetUserMoney(userId2);
                UserBridge.AddUserToGameRoomAsPlayer(userId2, newRoomId, money);
                GameBridge.StartGame(newRoomId);

                ////maybe not good?
                //GameBridge.Call(UserId, newRoomId, 2);
                //GameBridge.Call(userId2, newRoomId, 2);

                UserBridge.RemoveUserFromRoom(userId2, newRoomId);
                //now user1 is only player in room => user1 wins
                //=> game is done => save replay
                UserBridge.RemoveUserFromRoom(UserId, newRoomId);

                UserBridge.DeleteUser(userId2);
                OtherUsers.Remove(userId2);
            }
        }

        protected void RegisterUser1()
        {
            if (!UserBridge.IsThereUser(UserId))
            {
                UserBridge.RegisterUser(UserId, User1Name, User1Pw, UserEmailGood1);
            }
            else if (!UserBridge.IsUserLoggedIn(UserId))
            {
                UserBridge.LoginUser(User1Name, User1Pw);
            }
        }

        //delete all users and all games, then register user1
        protected void RestartSystem()
        {
            //delete all users:
            List<int> allUsers = UserBridge.GetAllUsers();
            allUsers.ForEach(user =>
            {
                List<int> usersGames = UserBridge.GetUsersGameRooms(user);
                usersGames.ForEach(usersRoom =>
                {
                    UserBridge.RemoveUserFromRoom(user, usersRoom);
                });
                UserBridge.DeleteUser(user);
            });

            //delete all rooms
            List<int> allGames = GameBridge.GetAllGames();
            allGames.ForEach(room =>
            {
                GameBridge.RemoveGameRoom(room);
            });

            RegisterUser1();
        }

        //create a new game with user2 as only player, return user2's id
        protected int CreateGameWithUser()
        {
            //delete room1 if exists
            if (GameBridge.DoesRoomExist(RoomId))
            {
                GameBridge.GetPlayersInRoom(RoomId).ForEach(uid =>
                {
                    UserBridge.RemoveUserFromRoom(uid, RoomId);
                });
                GameBridge.RemoveGameRoom(RoomId);
            }

            int userId2 = GetNextUser();
            Assert.True(GameBridge.CreateGameRoom(userId2, RoomId));

            return userId2;
        }
    }
}
