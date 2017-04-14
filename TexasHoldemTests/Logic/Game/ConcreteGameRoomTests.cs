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
    public class ConcreteGameRoomTests
    {
        private ConcreteGameRoom _gameRoom;
        private bool _isGameOver = false;
        private int _potCount = 0;
       // private static int _buttonPos = 8;
        private static List<Player> _players = new List<Player>(2);
       // private int _actionPos = (_buttonPos + 3) % _players.Count;
        private int _maxCommitted = 0;
        private List<Card> _publicCards;
        private int _sb = 0;
        private List<Tuple<int, List<Player>>> _sidePots;
        private ConcreteGameRoom.HandStep _handStep;
        private int _bb;
        private Deck _deck;

        [TestInitialize()]
        public void Initialize()
        {
            _gameRoom = new ConcreteGameRoom(_players, 2);
            _publicCards = new List<Card>();
            _sidePots = new List<Tuple<int, List<Player>>>();
            _deck = new Deck();
        }
      

        [TestMethod()]
        public void AddNewPublicCardTest()
        {
            _gameRoom.AddNewPublicCard();
            Assert.IsTrue(_gameRoom._publicCards.Count>0);
        }

        [TestMethod()]
        public void NextToPlayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DelaerPositionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToCallTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateGameStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearPublicCardsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateMaxCommittedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndTurnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetActionPosTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MoveChipsToPotTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayersInHandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayersAllInTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AllDoneWithTurnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void newSplitPotTest()
        {
            Assert.Fail();
        }
    }
}