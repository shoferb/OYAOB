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
        protected int UserId;
        protected int User2Id = 0;
        protected string User1Name;
        protected string User1Pw;
        protected const int RoomId = 0; //room1 must NOT exist when tests start.
        protected int NewRoomId = 0;
        protected List<int> Users; //list holding all user ids used for testing
        protected string UserEmailGood1;

        protected AcptTest()
        {
            //todo - return briges
            UserBridge = new UserBridge();
            GameBridge = new GameBridge();
            ReplayBridge = new ReplayBridge();
            Users = new List<int>();

            User1Name = "Oded";
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";

            SetupUser1();
        }

        [SetUp]
        protected void Init()
        {
            if (!GameBridge.DoesRoomExist(RoomId))
            {
                CreateGameWithUser();
            }

            SubClassInit();
        }

        [TearDown]
        protected void Dispose()
        {
            RestartSystem();

            SubClassDispose();
            User1Name = null;
            User1Pw = null;
            UserId = -1;
        }

        //subclass' setup method
        protected abstract void SubClassInit();

        //subclass' setup method
        protected abstract void SubClassDispose();

       //make sure user1 exists and has at least 1 replayable game
        protected void SetupUser1()
        {
            this.UserId = new Random().Next();
            RegisterUser1();

         /*   //create a new game and run it 1 move, then leave it
            if (ReplayBridge.GetReplayableGames(UserId).Count == 0)
            {
                int newRoomId = GameBridge.CreateGameRoom(UserId);
            }*/
        }

        protected void Setup2Users1Game()
        {
            RegisterUser1();

            //create a new game and run it 1 move, then leave it
            if (ReplayBridge.GetReplayableGames(UserId).Count == 0)
            {
                NewRoomId = GameBridge.CreateGameRoom(UserId);
                User2Id = GetNextUser();
                int money = UserBridge.GetUserMoney(User2Id);
                UserBridge.AddUserToGameRoomAsPlayer(User2Id, NewRoomId, money);
                GameBridge.StartGame(NewRoomId);
            }
        }

        protected void RegisterUser1()
        {
            if (!UserBridge.IsThereUser(UserId))
            {
                UserBridge.RegisterUser(UserId, User1Name, User1Pw, UserEmailGood1);
                
                Users.Add(UserId);
            }
            else if (!UserBridge.IsUserLoggedIn(UserId))
            {
                UserBridge.LoginUser(User1Name, User1Pw);
            }
        }

        private void CleanUserAndHisGames(int userId)
        {
            if (UserBridge.IsUserLoggedIn(userId))
            {
                UserBridge.LogoutUser(UserId);
            }

            List<int> usersGames = UserBridge.GetUsersGameRooms(userId);
            usersGames.ForEach(usersRoom =>
            {
                UserBridge.RemoveUserFromRoom(userId, usersRoom);
            });
            UserBridge.DeleteUser(userId);
        }

        //delete all users and all games, then register user1
        protected void RestartSystem()
        {
            //delete all users:
            List<int> allUsers = UserBridge.GetAllUsers();
            Users.ForEach(u =>
            {
                if (!allUsers.Contains(u))
                {
                    CleanUserAndHisGames(u);
                }
            });
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

        }

        //create a new game with user2 as only player, return user2's Id
        protected void CreateGameWithUser()
        {
            //delete room1 if exists
            if (GameBridge.DoesRoomExist(RoomId))
            {
                GameBridge.GetPlayersInRoom(RoomId).ForEach(uid =>
                {
                    UserBridge.RemoveUserFromRoom(uid, RoomId);
                });
            }
            //make sure the room is deleted
            Assert.True(!GameBridge.DoesRoomExist(RoomId));
            Assert.True(GameBridge.CreateGameRoom(this.UserId)!= -1);
        }
    }
}
