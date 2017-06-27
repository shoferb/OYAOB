using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;

namespace TexasHoldem.Logic.Users.Tests
{
    [TestClass()]
    public class UserTests
    {

   
        private static readonly LogControl LogControl = new LogControl();
        private static readonly SystemControl SysControl = new SystemControl(LogControl);
        private static readonly ReplayManager ReplayManager = new ReplayManager();

        
        private static readonly SessionIdHandler Sender = new SessionIdHandler();
        private static readonly GameCenter GameCenter = new GameCenter(SysControl, LogControl, ReplayManager,Sender);
        private static readonly UserDataProxy _userDataProxy = new UserDataProxy();
        private static readonly GameDataProxy _gameDataProxy = new GameDataProxy(GameCenter);


        [TestMethod()]
        public void IsUnKnowTestGood_on_Create()
        {
            IUser user = new User(95950052, "orelie", "orelie95950052", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            Assert.IsTrue(user.IsUnKnow());
            _userDataProxy.DeleteUserById(95950052);
        }


        [TestMethod()]
        public void IsUnKnowTestGood_on_at_10_Games()
        {
            IUser user = new User(95002524, "orelie", "orelie95002524", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            for (int i = 0; i < 10; i++)
            {
                user.IncGamesPlay();
            }
            Assert.IsTrue(user.IsUnKnow());
            _userDataProxy.DeleteUserById(95002524);
        }


        [TestMethod()]
        public void IsUnKnowTestGood_More_Than_10()
        {
            IUser user = new User(85850054, "orelie", "orelie85850054", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            for (int i = 0; i < 11; i++)
            {
                user.IncGamesPlay();
            }
            Assert.IsFalse(user.IsUnKnow());
            _userDataProxy.DeleteUserById(85850054);
        }



   

        [TestMethod()]
        public void EmailTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Email(), "orelie@post.bgu.ac.il");
        }

        [TestMethod()]
        public void EmailTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Email(), " ");
        }
      

        [TestMethod()]
        public void ActiveGameListTest_good_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.ActiveGameList().Count, 0);
        }



        [TestMethod()]
        public void SpectateGameListTest_good_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.SpectateGameList().Count, 0);
        }

        [TestMethod()]
        public void WinNumTest_good_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.WinNum, 0);
        }

        [TestMethod()]
        public void WinNumTest_Bad_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.WinNum, 10);
        }

        [TestMethod()]
        public void IncWinNumTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.IncWinNum();
            Assert.AreEqual(user.WinNum, 1);
        }

     

        [TestMethod()]
        public void LoginTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.Login());
        }

        [TestMethod()]
        public void LogoutTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.Login();

            Assert.IsTrue(user.Logout());
        }

        [TestMethod()]
        public void EditIdTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditId(305077902));
        }

        [TestMethod()]
        public void EditIdTest_Bad_Smaller_than_Zero()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditId(-2));
        }



        [TestMethod()]
        public void EditEmailTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditEmail("orelie@walla.co.il"));
        }

        [TestMethod()]
        public void EditEmailTest_Bad_IValid_address()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditEmail("orelie.walla.co.il"));
        }

        [TestMethod()]
        public void EditEmailTest_Bad_Empty()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditEmail(""));
        }

        [TestMethod()]
        public void EditPasswordTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditPassword("12345698"));
        }


        [TestMethod()]
        public void EditPasswordTest_Bad_Small()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditPassword("12"));
        }

        [TestMethod()]
        public void EditPasswordTest_Bad_big()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditPassword("121212121212121212112"));
        }


        [TestMethod()]
        public void EditPasswordTest_Bad_empty()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditPassword(""));
        }

        [TestMethod()]
        public void EditUserNameTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditUserName("orelie2222"));
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_empty()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditUserName(""));
        }



        [TestMethod()]
        public void EditUserNameTest_Bad_empty2()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditUserName(" "));
        }

        [TestMethod()]
        public void EditNameTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditName("or"));
        }


        [TestMethod()]
        public void EditNameTest_Bad_empty()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditName(""));
        }

        [TestMethod()]
        public void EditNameTest_Bad_empty2()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditName(" "));
        }

        [TestMethod()]
        public void EditAvatarTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditAvatar("/GuiScreen/Photos/Avatar/Test.png"));
        }


        [TestMethod()]
        public void EditAvatarTest_bad_empty1()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditAvatar(""));
        }

        [TestMethod()]
        public void EditAvatarTest_bad_empty2()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditAvatar(" "));
        }

        [TestMethod()]
        public void EditUserPointsTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditUserPoints(100));
        }


        [TestMethod()]
        public void EditUserPointsTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditUserPoints(-100));
        }


        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_good_bigger_than_zero_bool()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.ReduceMoneyIfPossible(100));
        }

        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_good_bigger_than_zero()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.ReduceMoneyIfPossible(100);
            Assert.AreEqual(user.Money(), 400);
        }


        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_good_Equal_to_zero()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.ReduceMoneyIfPossible(500);
            Assert.AreEqual(user.Money(), 0);
        }

        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_good_Equal_to_zero_bool()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.ReduceMoneyIfPossible(500));
        }

        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_Bad_Smaller_than_zero()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.ReduceMoneyIfPossible(600);
            Assert.AreEqual(user.Money(), 500);
        }

        [TestMethod()]
        public void ReduceMoneyIfPossibleTest_Bad_Smaller_than_zero_bool()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.ReduceMoneyIfPossible(600));
        }

        [TestMethod()]
        public void AddMoneyTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.AddMoney(50);
            Assert.AreEqual(user.Money(), 550);
        }

        [TestMethod()]
        public void EditUserMoneyTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.EditUserMoney(100));
        }


        [TestMethod()]
        public void EditUserMoneyTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsFalse(user.EditUserMoney(-100));
        }

      

        [TestMethod()]
        public void RemoveRoomFromActiveGameListTest_good()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user = _userDataProxy.GetUserById(userId);
            user.AddRoomToActiveGameList(toAddg);
            Assert.IsTrue(user.RemoveRoomFromActiveGameList(toAddg));
            _userDataProxy.DeleteActiveGameOfUser(userId, roomid, gameNum);
            Cleanup(gameNum, roomid, userId);

        }

        [TestMethod()]
        public void RemoveRoomFromActiveGameListTest_bad_dont_contain()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user = _userDataProxy.GetUserById(userId);
            Assert.IsFalse (user.RemoveRoomFromActiveGameList(toAddg));
            _userDataProxy.DeleteActiveGameOfUser(userId, roomid, gameNum);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void RemoveRoomFromActiveGameListTest_bad_game_null()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.RemoveRoomFromActiveGameList(gameRoom));
        }

        [TestMethod()]
        public void RemoveRoomFromSpectetorGameListTest_good()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();
            RegisterUser(userId2);
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user2 = _userDataProxy.GetUserById(userId2);
            user2.AddRoomToSpectetorGameList(toAddg);
            Assert.IsTrue(user2.RemoveRoomFromSpectetorGameList(toAddg));
            _userDataProxy.DeleteSpectetorGameOfUSer(userId, roomid, gameNum);
            _userDataProxy.DeleteUserById(userId2);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void RemoveRoomFromSpectetorGameListTest_Bad_dont_contain()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IUser user2 = new User(305077902, "orelie2", "orelie", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            int roomID = 9999;
            List<Player> players = new List<Player>();
            Player player1 = new Player(user, 1000, roomID);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            IGame gameRoom = new GameRoom(players, roomID, deco, GameCenter, LogControl, ReplayManager, Sender);
            Assert.IsFalse(user2.RemoveRoomFromSpectetorGameList(gameRoom));
        }

        [TestMethod()]
        public void RemoveRoomFromSpectetorGameListTest_Bad_game_null()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.RemoveRoomFromSpectetorGameList(gameRoom));
        }

        [TestMethod()]
        public void HasThisActiveGameTest_good_contain()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user = _userDataProxy.GetUserById(userId);
            user.AddRoomToActiveGameList(toAddg);
            Assert.IsTrue(user.HasThisActiveGame(toAddg));
            _userDataProxy.DeleteActiveGameOfUser(userId, roomid, gameNum);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void HasThisActiveGameTest_good_dont_contain()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user = _userDataProxy.GetUserById(userId);
            Assert.IsFalse(user.HasThisActiveGame(toAddg));
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void HasThisActiveGameTest_Bad_game_null()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.HasThisActiveGame(gameRoom));
        }


        [TestMethod()]
        public void HasThisSpectetorGameTest_good_contain()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            RegisterUser(userId2);
            IUser user2 = _userDataProxy.GetUserById(userId2);
            user2.AddRoomToSpectetorGameList(toAddg);
            Assert.IsTrue(user2.HasThisSpectetorGame(toAddg));
            _userDataProxy.DeleteSpectetorGameOfUSer(userId2,roomid,gameNum);
            _userDataProxy.DeleteUserById(userId2);
            Cleanup(gameNum,roomid,userId);
        }

        [TestMethod()]
        public void HasThisSpectetorGameTest_good_dont_contain()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            RegisterUser(userId2);
            IUser user2 = _userDataProxy.GetUserById(userId2);
            Assert.IsFalse(user2.HasThisSpectetorGame(toAddg));
            _userDataProxy.DeleteUserById(userId2);
            Cleanup(gameNum, roomid, userId);
        }


        [TestMethod()]
        public void HasThisSpectetorGameTest_bad_game_null()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.HasThisSpectetorGame(gameRoom));
        }

        [TestMethod()]
        public void AddRoomToActiveGameListTest_good()
        {
            int userId = new Random().Next();
            IUser user = new User(userId, "orelie", "orelie26" + userId, "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            int roomId = new Random().Next();
            List<Player> players = new List<Player>();
            Player player1 = new Player(user, 1000, roomId);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            GameRoom gameRoom = new GameRoom(players, roomId, deco, GameCenter, LogControl,
                ReplayManager, Sender);
            gameRoom.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(gameRoom);
            user.AddRoomToActiveGameList(gameRoom);
            var activeGames = _gameDataProxy.GetAllUserActiveGames(userId);
            var gameIds = activeGames.ConvertAll(g => g.Id);
            Assert.IsTrue(gameIds.Contains(gameRoom.Id));
            RemoveUserAndGamesFromDb(userId);
        }

        [TestMethod()]
        public void AddRoomToActiveGameListTest_Bad_game_null()
        {

            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.AddRoomToActiveGameList(gameRoom));
        }


        [TestMethod()]
        public void AddRoomToActiveGameListTest_Bad_already_contain()
        {
            int userId = new Random().Next();
            IUser user = new User(userId, "orelie", "orelie26" + userId, "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
             int roomId = new Random().Next(); 
            List<Player> players = new List<Player>();
            Player player1 = new Player(user, 1000, roomId);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            GameRoom gameRoom = new GameRoom(players, roomId, deco, GameCenter, LogControl, 
                ReplayManager, Sender);
            gameRoom.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(gameRoom);
            user.AddRoomToActiveGameList(gameRoom);
            Assert.IsFalse(user.AddRoomToActiveGameList(gameRoom));
            RemoveUserAndGamesFromDb(userId);
        }


        [TestMethod()]
        public void AddRoomToSpectetorGameListTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IUser user2 = new User(305077902, "orelie2", "orelie", "123456789", 0, 1500, "orelie@post.bgu.ac.il");

            int roomID = 9999;
            List<Player> players = new List<Player>();
            Player player1 = new Player(user, 1000, roomID);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            IGame gameRoom = new GameRoom(players, roomID, deco, GameCenter, LogControl, ReplayManager, Sender);
           // Spectetor spectetor = new Spectetor(user2, roomID);
            Assert.IsTrue(user2.AddRoomToSpectetorGameList(gameRoom));
        }

        [TestMethod()]
        public void AddRoomToSpectetorGameListTest_Bad_Room_null()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 1500, "orelie@post.bgu.ac.il");
            IGame gameRoom = null;
            Assert.IsFalse(user.AddRoomToSpectetorGameList(gameRoom));
        }

        [TestMethod()]
        public void AddRoomToSpectetorGameListTest_Bad_Room_already_contains()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();
            RegisterUser(userId2);
            GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            toAddg.SetIsActive(true);
            _gameDataProxy.InsertNewGameRoom(toAddg);
            IUser user2 = _userDataProxy.GetUserById(userId2);
            user2.AddRoomToSpectetorGameList(toAddg);
            Assert.IsFalse(user2.AddRoomToSpectetorGameList(toAddg));
            _userDataProxy.DeleteSpectetorGameOfUSer(userId, roomid, gameNum);
            _userDataProxy.DeleteUserById(userId2);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void IsLoginTest_good_on_Create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
          
            Assert.IsTrue(user.IsLogin());
        }

        [TestMethod()]
        public void IsLoginTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            user.Login();
            Assert.IsTrue(user.IsLogin());
        }



        [TestMethod()]
        public void GetLeagueTest_Good_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.GetLeague(), LeagueName.Unknow);
        }

        [TestMethod()]
        public void GetLeagueTest_Good_after_10_games()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            for (int i = 0; i < 20; i++)
            {
                user.IncGamesPlay();
            }
            Assert.AreEqual(user.GetLeague(), LeagueName.E);
        }

        [TestMethod()]
        public void GetLeagueTest_Bad_on_create()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.GetLeague(), LeagueName.A);
        }

        [TestMethod()]
        public void GetLeagueTest_Bad_after_10_games()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            for (int i = 0; i < 20; i++)
            {
                user.IncGamesPlay();
            }
            Assert.AreNotEqual(user.GetLeague(), LeagueName.Unknow);
        }



        [TestMethod()]
        public void HasEnoughMoneyTest_good_bool()
        {
            IUser user = new User(20005250, "orelie", "orelie", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            Assert.IsTrue(user.HasEnoughMoney(100, 50));
            _userDataProxy.DeleteUserById(20005250);

        }


        [TestMethod()]
        public void HasEnoughMoneyTest_Bad_bool()
        {
            
            IUser user = new User(456000525, "orelie", "orelie456000525", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(user);
            Assert.IsFalse(user.HasEnoughMoney(490, 50));
            _userDataProxy.DeleteUserById(456000525);
        }



        [TestMethod()]
        public void IdTest_good()
        {
            
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Id(), 305077901);
        }

        [TestMethod()]
        public void IdTest_bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Id(), 305077902);
        }

        [TestMethod()]
        public void NameTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Name(), "orelie");
        }


        [TestMethod()]
        public void NameTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Name(), " ");
        }

        [TestMethod()]
        public void MemberNameTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.MemberName(), "orelie26");
        }


        [TestMethod()]
        public void MemberNameTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.MemberName(), "orelie18");
        }


        [TestMethod()]
        public void PasswordTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Password(), "123456789");
        }


        [TestMethod()]
        public void PasswordTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Password(), "12");
        }


       


        [TestMethod()]
        public void AvatarTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Avatar(), " ");

        }

        [TestMethod()]
        public void PointsTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Points(), 0);
        }

        [TestMethod()]
        public void MoneyTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Money(), 500);
        }


        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0, 15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private Logic.Game.GameRoom CreateRoomWithId(int gameNum, int roomId, int userId1)
        {

            RegisterUser(userId1);
            bool useCommunication = false;

            List<Player> toAddPlayers = new List<Player>();
            IUser user = _userDataProxy.GetUserById(userId1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            Player player1 = new Player(user, 1000, roomId);
            toAddPlayers.Add(player1);
            Logic.Game.GameRoom gm = new Logic.Game.GameRoom(toAddPlayers, roomId, deco, GameCenter, LogControl, ReplayManager, Sender);
            gm.GameNumber = gameNum;
            return gm;
        }


        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.Unknow);
            before.SetNextDecorator(mid);
            return before;
        }

        private void RemoveUserAndGamesFromDb(int userId)
        {
            var games = _gameDataProxy.GetAllUserActiveGames(userId);
            games.ForEach(g =>
            {
                var players = g.GetPlayersInRoom();
                players.ForEach(p =>
                {
                    IUser user = p.user;
                    _userDataProxy.DeleteActiveGameOfUser(user.Id(), g.Id, g.GameNumber);
                    _userDataProxy.DeleteUserById(user.Id());
                });

                var specs = g.GetSpectetorInRoom();
                specs.ForEach(s =>
                {
                    IUser user = s.user;
                    _userDataProxy.DeleteSpectetorGameOfUSer(user.Id(), g.Id, g.GameNumber);
                    _userDataProxy.DeleteUserById(user.Id());
                });
                ReplayManager.DeleteGameReplay(g.Id, g.GameNumber);
                ReplayManager.DeleteGameReplay(g.Id, g.GameNumber);
                _gameDataProxy.DeleteGameRoomPref(g.Id);
                _gameDataProxy.DeleteGameRoom(g.Id, g.GameNumber);
                
            });
        }
        public void Cleanup(int gameNum, int roomId, int userId1)
        {
            _userDataProxy.DeleteUserById(userId1);

            ReplayManager.DeleteGameReplay(roomId, gameNum);
            ReplayManager.DeleteGameReplay(roomId, gameNum);
            _gameDataProxy.DeleteGameRoomPref(roomId);
            _gameDataProxy.DeleteGameRoom(roomId, gameNum);
        }
    }
}