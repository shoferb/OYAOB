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
            SetDecoratores1();
        }

        private void SetDecoratores1()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            gameRoom.AddDecorator(before);
        }

        private void SetDecoratoresLimitNoSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.Limit, 20, 5);
            Decorator before = new BeforeGameDecorator(20, 1500, false, 2, 5, 25, LeagueName.A);
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
            //new user
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Join, 1000));
            Assert.IsTrue(user2.Money() == 5000 - 1000 - 20);
            //an already player user
            Assert.IsFalse(gameRoom.DoAction(user1, ActionType.Join, 1000));
        }

        [TestMethod()]
        public void CanJoinTest2()
        {
            //not enough money enter
            Assert.IsFalse(gameRoom.DoAction(user2, ActionType.Join, 1));
            Assert.IsTrue(user2.Money() == 5000);
            //an already player user
            Assert.IsFalse(gameRoom.DoAction(user1, ActionType.Join, 1000));
        }


        [TestMethod()]
        public void CanJoinTestWithSpectator()
        {
            //relevant user
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            //an already spectator user
            Assert.IsFalse(gameRoom.DoAction(user2, ActionType.Join, 1000));
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            //non started game
            Assert.IsFalse(gameRoom.IsGameActive());
            //join another player and start game
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Join, 1000));
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.StartGame, 0));
            Assert.IsTrue(gameRoom.IsGameActive());
        }

        [TestMethod()]
        public void IsSpectatableTest()
        {
            Assert.IsTrue(gameRoom.IsSpectatable());
            SetDecoratoresLimitNoSpectatores();
            Assert.IsFalse(gameRoom.IsSpectatable());
        }

        [TestMethod()]
        public void IsPotSizEqualTest()
        {
            Assert.IsTrue(gameRoom.IsPotSizeEqual(0));

            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Join, 1000));
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.StartGame, 0));
            Assert.IsTrue(gameRoom.IsPotSizeEqual(10+5)); //small+big
        }

        [TestMethod()]
        public void IsGameModeEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameModeEqual(GameMode.NoLimit));
            SetDecoratoresLimitNoSpectatores();
            Assert.IsTrue(gameRoom.IsGameModeEqual(GameMode.Limit));
        }

        [TestMethod()]
        public void IsGameBuyInPolicyEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameBuyInPolicyEqual(20));
            Assert.IsFalse(gameRoom.IsGameBuyInPolicyEqual(25));
            SetDecoratoresLimitNoSpectatores(); // change to 25 fee
            Assert.IsTrue(gameRoom.IsGameBuyInPolicyEqual(25));
            Assert.IsFalse(gameRoom.IsGameBuyInPolicyEqual(20));
        }

        [TestMethod()]
        public void IsGameMinPlayerEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMinPlayerEqual(2));
            Assert.IsFalse(gameRoom.IsGameMinPlayerEqual(3));

            SetDecoratoresLimitNoSpectatores(); // same min players
            Assert.IsTrue(gameRoom.IsGameMinPlayerEqual(2));
            Assert.IsFalse(gameRoom.IsGameMinPlayerEqual(3));
        }

        [TestMethod()]
        public void IsGameMaxPlayerEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMaxPlayerEqual(4));
            Assert.IsFalse(gameRoom.IsGameMaxPlayerEqual(5));

            SetDecoratoresLimitNoSpectatores(); // max 5 player
            Assert.IsTrue(gameRoom.IsGameMaxPlayerEqual(5));
            Assert.IsFalse(gameRoom.IsGameMaxPlayerEqual(4));
        }

        [TestMethod()]
        public void IsGameMinBetEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMinBetEqual(10));
            Assert.IsFalse(gameRoom.IsGameMinBetEqual(20));

            SetDecoratoresLimitNoSpectatores(); // BB (equal to min bet) is now 20
            Assert.IsTrue(gameRoom.IsGameMinBetEqual(20));
            Assert.IsFalse(gameRoom.IsGameMinBetEqual(10));
        }

        [TestMethod()]
        public void IsGameStartingChipEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameStartingChipEqual(1000));
            Assert.IsFalse(gameRoom.IsGameStartingChipEqual(1500));

            SetDecoratoresLimitNoSpectatores(); // starting cheap equal to 1500
            Assert.IsTrue(gameRoom.IsGameStartingChipEqual(1500));
            Assert.IsFalse(gameRoom.IsGameStartingChipEqual(1000));
        }

        [TestMethod()]
        public void GetPlayersInRoomTest()
        {
            Assert.IsTrue(gameRoom.GetPlayersInRoom().Count == 1);
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(0).user.Equals(user1));
            // add 1 more player
            Assert.IsTrue(gameRoom.DoAction(user2, ActionType.Join, 1000)); 
            Assert.IsTrue(gameRoom.GetPlayersInRoom().Count == 2);
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(0).user.Equals(user1));
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(1).user.Equals(user2));
        }

        [TestMethod()]
        public void GetSpectetorInRoomTest()
        {
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().Count == 0);
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().Count == 1);
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().ElementAt(0).user.Equals(user2));
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