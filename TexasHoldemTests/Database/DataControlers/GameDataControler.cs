using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database.DataControlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.communication.Impl;

namespace TexasHoldem.Database.DataControlers.Tests
{
    [TestClass()]
    public class GameDataControler
    {
        private GameDataProxy proxy;
        private UserDataProxy _userDataProxy;
        private IUser user1, user2, user3;
        private List<Player> players;
        private Player player1;
        private int roomID;
        private GameRoom gameRoom;
        private bool useCommunication;
        private static LogControl logControl = new LogControl();
        private static SystemControl sysControl = new SystemControl(logControl);
        private static ReplayManager replayManager = new ReplayManager();
        private static SessionIdHandler ses = new SessionIdHandler();
        private static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager, ses);

        [TestInitialize()]
        public void Initialize()
        {
            proxy = new GameDataProxy(gameCenter);
            _userDataProxy = new UserDataProxy();
            user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            user2 = new User(2, "test2", "no", "1234", 0, 5000, "test2@gmail.com");
            user3 = new User(3, "test3", "3test", "1234", 0, 5000, "test3@mailnator.com");
            _userDataProxy.AddNewUser(user1);
            _userDataProxy.AddNewUser(user2);
            _userDataProxy.AddNewUser(user3);
            useCommunication = false;
            roomID = 9999;
            players = new List<Player>();
            player1 = new Player(user1, 1000, roomID);
            player1.RoundChipBet = 22;
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            gameRoom = new GameRoom(players, roomID, deco, gameCenter, logControl, replayManager, ses);
        }

        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0, 15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private GameRoom CreateRoomWithId(int gameNum, int roomId, int userId1, int userId2, int userId3)
        {

            RegisterUser(userId1);
            RegisterUser(userId2);
            RegisterUser(userId3);
            useCommunication = false;

            List<Player> toAddPlayers = new List<Player>();
            IUser user = _userDataProxy.GetUserById(userId1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            player1.RoundChipBet = 22;
            players.Add(player1);
            player1 = new Player(user, 1000, roomId);
            GameRoom gm = new GameRoom(toAddPlayers, roomId, deco, gameCenter, logControl, replayManager, ses);
            gm.GameNumber = gameNum;
            return gm;
        }
        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            return before;
        }


        public void Cleanup(int gameNum, int roomId, int userId1, int userId2, int userId3)
        {
            _userDataProxy.DeleteUserById(userId1);
            _userDataProxy.DeleteUserById(userId2);
            _userDataProxy.DeleteUserById(userId3);
            replayManager.DeleteGameReplay(roomID, 0);
            replayManager.DeleteGameReplay(roomID, 1);
            proxy.DeleteGameRoomPref(roomId);
            proxy.DeleteGameRoom(roomId, gameNum);
        }


        [TestMethod()]
        public void getAllGamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertGameRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateGameRoomPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteGameRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteGameRoomPrefTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllActiveGameRoomsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllSpectatebleGameRoomsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByBuyInPolicyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByGameModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByStartingChipTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinBetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMaxPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomReplyByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomPrefByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateGameRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertPrefTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameModeValByNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetLeagueValByNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUserActiveGamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserSpectetorsGameResultTest()
        {
            Assert.Fail();
        }
    }
}