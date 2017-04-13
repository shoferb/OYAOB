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
    public class HandOfPokerTests
    {
        private HandOfPoker _hand;
        private ConcreteGameRoom _room;
        private Player _player1;
        private Player _player2;
        private Player _player3;

        [TestInitialize()]
        public void Initialize()   
        {
            _player1 = new Player(1, "one", "one", "one", 10, 100, "one@", 1, true);
            _player2 = new Player(2, "two", "two", "two", 10, 100, "two@", 1, true);
            _player3 = new Player(3, "three", "three", "three", 10, 100, "three@", 1, true);
            List<Player> players = new List<Player>();
            players.Add(_player1);
            players.Add(_player2);
            players.Add(_player3);
            _room = new ConcreteGameRoom(players, 1);
            _hand = new HandOfPoker(_room);
        }

        [TestMethod()]
        public void HandOfPokerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NewHandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ProgressHandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndHandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindWinnerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EvalTiesTest()
        {
            Assert.Fail();
        }
    }
}