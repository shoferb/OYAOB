using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldem.Logic.GameControl;
using static TexasHoldemShared.CommMessages.CommunicationMessage;
using TexasHoldem.Logic.Replay;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic;

namespace TexasHoldem.DatabaseProxy.Tests

{
    [TestClass()]
    public class GameDataProxyGameDataProxyTests
    {

        private GameDataProxy proxy = new GameDataProxy();
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
            user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            user2 = new User(2, "test2", "no", "1234", 0, 5000, "test2@gmail.com");
            user3 = new User(3, "test3", "3test", "1234", 0, 5000, "test3@mailnator.com");
            useCommunication = false;
            roomID = 9999;
            players = new List<Player>();
            player1 = new Player(user1, 1000, roomID);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            gameRoom = new GameRoom(players, roomID, deco, gameCenter, logControl, replayManager, ses);
        }

        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            return before;
        }

        [TestCleanup()]
        public void Cleanup()
        {
            user1 = null;
            user2 = null;
            players = null;
            player1 = null;
            gameRoom = null;
            replayManager.DeleteGameReplay(roomID, 0);
            replayManager.DeleteGameReplay(roomID, 1);
        }

        [TestMethod()]
        public void AddNewGameToDBTest()
        {
            Assert.IsTrue(proxy.AddNewGameToDB(gameRoom));
            proxy.DeleteGameRoom(gameRoom.Id, gameRoom.GetGameNum());
        }

        [TestMethod()]
        public void UpdateGameRoomPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateGameRoomTest()
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
        public void GetAllGamesTest()
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
        public void GetGameRoomsByMaxPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinBetTest()
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
        public void GetGameRoombyIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomReplyByIdTest()
        {
            Assert.Fail();
        }
    }
}