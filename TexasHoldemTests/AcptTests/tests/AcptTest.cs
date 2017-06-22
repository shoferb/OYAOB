using System;
using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldemTests.AcptTests.Bridges;
using TexasHoldemTests.AcptTests.Bridges.Interface;
using TexasHoldemTests.AcptTests.Bridges;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;

namespace TexasHoldemTests.AcptTests.tests
{

    //parent class for all acceptance test classes.
    public abstract class AcptTest
    {
        protected IUserBridge UserBridge;
        protected IGameBridge GameBridge;
        protected IReplayBridge ReplayBridge;
        protected int UserId;
        protected string User1Name;
        protected string User1Pw;
        protected int RoomId; //room1 must NOT exist when tests start.
        protected List<int> Users; //list holding all user ids used for testing
        protected string UserEmailGood1;
        protected static LogControl logControl = new LogControl();
        protected static SystemControl sysControl = new SystemControl(logControl);
        protected static ReplayManager replayManager = new ReplayManager();
        protected static SessionIdHandler ses = new SessionIdHandler();
        protected static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager,ses);

        protected AcptTest()
        {
            UserBridge = new UserBridge(gameCenter, sysControl, logControl, replayManager);
            GameBridge = new GameBridge(gameCenter, sysControl, logControl, replayManager);
            ReplayBridge = new ReplayBridge(gameCenter, sysControl, logControl, replayManager);
            Users = new List<int>();

            User1Name = "Oded";
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";

            SetupUser1();
        }

        [SetUp]
        protected void Init()
        {
            /* if (!GameBridge.DoesRoomExist(RoomId))
             {
                 CreateGameWithUser1();
             }
             */
            SetupUser1();
            SubClassInit();
        }

        [TearDown]
        protected void Dispose()
        {
            //RestartSystem();

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
            UserId = new Random().Next();
            User1Name = "Oded" + UserId;
            User1Pw = "goodPw1234";
            UserEmailGood1 = "gooduser1@gmail.com";
            RegisterUser1();

        }

        protected void RegisterUser1()
        {
            if (!UserBridge.IsThereUser(UserId))
            {
                UserBridge.RegisterUser(UserId, User1Name, User1Pw, UserEmailGood1);

                Users.Add(UserId);
            }
            if (!UserBridge.IsUserLoggedIn(UserId))
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

        //delete all users and all games
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

            List<int> allGames = GameBridge.GetAllGamesId();
            if (allGames == null)
            {
                allGames = new List<int>();
            }
            else
            {
                foreach (int id in allGames)
                {
                    GameBridge.RemoveRoom(id);
                }
            }
            allGames = GameBridge.GetAllGamesId();
       //     Assert.True(allGames.Count == 0);
            Assert.False(GameBridge.DoesRoomExist(RoomId));
            RoomId = -1;

        }

        //create a new game with user2 as only player, return user2's Id
        protected void CreateGameWithUser1()
        {
            //delete room1 if exists
            if (GameBridge.DoesRoomExist(RoomId))
            {
                GameBridge.GetPlayersInRoom(RoomId).ForEach(p =>
                {
                    UserBridge.RemoveUserFromRoom(p.user.Id(), RoomId);
                });
            }
            Assert.True(!GameBridge.DoesRoomExist(RoomId));

            RoomId = GameBridge.CreateGameRoom(UserId, 100);

            //make sure the room is deleted
            Assert.True(RoomId != -1);
        }

        protected void RegisterUserToDB(int userId)
        {
            UserBridge.RegisterUser(userId, "orelie" + userId, "123456789", "orelie@post.bgu.ac.il");

        }

        protected void UserCleanUpFromDB(int userId)
        {
            UserBridge.DeleteUser( "orelie" + userId, "123456789");

        }
        //make room with 1 player 
        protected void CreateGame(int roomId, int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            UserBridge.RegisterUser(userId, "orelie" + userId, "123456789", "orelie@post.bgu.ac.il");
           
            GameBridge.CreateNewRoomWithRoomId(roomId, userId, startingChip, isSpectetor, gameModeChosen,
                minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
        }

        protected void CleanUp(int roomId)
        {
            List<IGame> games = GameBridge.GetAllGames();
            foreach (IGame game in games)
            {
                if (game.Id == roomId)
                {
                    foreach (Player p in game.GetPlayersInRoom())
                    {
                        UserBridge.RemoveUserFromRoom(p.user.Id(), game.Id);
                        UserBridge.DeleteUser(p.user.MemberName(), p.user.Password());
                    }
                    foreach (Spectetor s in game.GetSpectetorInRoom())
                    {
                        UserBridge.RemoveSpectatorFromRoom(s.user.Id(), game.Id);
                        UserBridge.DeleteUser(s.user.MemberName(), s.user.Password());
                    }
               
                GameBridge.RemoveRoom(roomId);
                }
            }
        }
    }
}
