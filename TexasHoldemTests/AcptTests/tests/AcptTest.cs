﻿using System;
using System.Collections.Generic;
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
        protected string User1Name;
        protected string User1Pw;
        protected const int RoomId = 0; //room1 must NOT exist when tests start.
        protected List<int> OtherUsers; //list holding all user ids used for testing except UserId1

        protected AcptTest()
        {
            UserBridge = new UserBridgeProxy();
            GameBridge = new GameBridgeProxy();
            OtherUsers = new List<int>();

            User1Name = "Oded";
            User1Pw = "goodPw1234";

            SetupUser1();
        }

        [SetUp]
        protected void Init()
        {
            User1Name = "Oded";
            User1Pw = "goodPw1234";

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

            UserBridge = null;
            GameBridge = null;
            User1Name = null;
            User1Pw = null;

            SubClassDispose();
        }

        //subclass' setup method
        protected abstract void SubClassInit();

        //subclass' setup method
        protected abstract void SubClassDispose();

        //create a new user, add to OtherUsers list and return the user's id
        protected int GetNextUserId()
        {
            int randInt = new Random().Next();

            int someUser = UserBridge.GetNextFreeUserId();
            UserBridge.RegisterUser(randInt.ToString(), User1Pw, User1Pw);

            OtherUsers.Add(someUser);

            return someUser;
        }

        //make sure user1 exists and has at least 1 replayable game
        protected void SetupUser1()
        {
            RegisterUser1();

            //create a new game and run it 1 move, then leave it
            if (UserBridge.GetReplayableGames(UserId).Count == 0)
            {
                int newRoomId = GameBridge.CreateGameRoom(UserId);
                int userId2 = GetNextUserId();
                int money = UserBridge.GetUserMoney(userId2);
                UserBridge.AddUserToGameRoomAsPlayer(userId2, newRoomId, money);
                GameBridge.StartGame(newRoomId);

                //maybe not good?
                GameBridge.Check(UserId, newRoomId);
                GameBridge.Check(userId2, newRoomId);

                UserBridge.RemoveUserFromRoom(userId2, newRoomId);
                UserBridge.RemoveUserFromRoom(UserId, newRoomId);

                UserBridge.DeleteUser(userId2);
                OtherUsers.Remove(userId2);
            }
        }

        protected void RegisterUser1()
        {
            if (!UserBridge.IsThereUser(UserId))
            {
                UserBridge.RegisterUser(User1Name, User1Pw, User1Name);
            }
            else if (!UserBridge.IsUserLoggedIn(UserId))
            {
                UserBridge.LoginUser(User1Name, User1Pw);
            }
        }

    }
}
