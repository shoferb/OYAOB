using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

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
        public void GameRoomTest()
        {
            
            Assert.Fail();
        }

        [TestMethod()]
        public void AddDecoratorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DoActionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindWinnerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            Assert.Fail();
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