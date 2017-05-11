using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldem.Logic.GameControl;
using static TexasHoldemShared.CommMessages.CommunicationMessage;

namespace TexasHoldem.Logic.Game.Tests
{
    [TestClass()]
    public class GameRoomTests
    {
        private User user1, user2;
        List<Player> players;
        Player player1, player2;
        int roomID ;
        GameRoom gameRoom;
        [TestInitialize()]
        public void Initialize()
        {
            user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            user2 = new User(2, "test2", "no", "1234", 0, 5000, "test2@gmail.com");
            roomID = 9999;
            players = new List<Player>();
            player1 = new Player(user1, 1000, roomID);
            players.Add(player1);
            gameRoom = new GameRoom(players, roomID);
            AddDecoratores1();
        }

        private void AddDecoratores1()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(5, 1000, true, 2, 4, 10, LeagueName.A);
            before.SetNextDecorator(mid);
            gameRoom.AddDecorator(before);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            user1 = null;
            user2 = null;
            players = null;
            player1 = null;
            gameRoom = null;
        }

        [TestMethod()]
        public void DoActionLeaveTest()
        {
            //irrelevant user
            Assert.IsFalse(gameRoom.DoAction(user2, ActionType.Leave, 0));
           
            //relevant user
            Assert.IsTrue(gameRoom.DoAction(user1, ActionType.Leave, 0));
        }

        [TestMethod()]
        public void DoActionLeaveTest2()
        {
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Join, 1000));
            Assert.IsTrue(gameRoom.DoAction(user1, ActionType.Leave, 0));
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Leave, 0));
        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            //user that is a player in the room
            Assert.IsFalse(gameRoom.AddSpectetorToRoom(user1));
            //relevant user
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            //user that is a player in the room but not a spectator
            Assert.IsFalse(gameRoom.RemoveSpectetorFromRoom(user1));
            //irrelevant user
            Assert.IsFalse(gameRoom.RemoveSpectetorFromRoom(user2));
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest2()
        {
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            //relevant user
            Assert.IsTrue(gameRoom.RemoveSpectetorFromRoom(user2));
        }


        [TestMethod()]
        public void CanJoinTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsSpectatableTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsPotSizEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameModeEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameBuyInPolicyEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameMinPlayerEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameMaxPlayerEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameMinBetEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameStartingChipEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPlayersInRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSpectetorInRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMinPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMinBetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMaxPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetBuyInPolicyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetStartingChipTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameGameModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetLeagueNameTest()
        {
            Assert.Fail();
        }
    }
}