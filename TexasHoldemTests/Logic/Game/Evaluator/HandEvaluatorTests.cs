using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game.Evaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game.Evaluator.Tests
{
    [TestClass()]
    public class HandEvaluatorTests
    {
        private HandEvaluator _evaluator;
        private Card[] _cards;
        private List<Card> _evalCards;
        private Card _card1 = new Card(Suits.Clubs, 1);
        private Card _card2 = new Card(Suits.Hearts, 2);
        private Card _card3 = new Card(Suits.Clubs, 3);
        private Card _card4 = new Card(Suits.Diamonds, 5);
        private Card _card5 = new Card(Suits.Hearts, 1);
        private Card _card6 = new Card(Suits.Hearts, 7);
        private Card _card7 = new Card(Suits.Clubs, 8);

        [TestInitialize()]
        public void Initialize()
        {
            _evaluator = new HandEvaluator();
            _cards = new Card[7];
        }

        [TestMethod()]
        public void DetermineHandRankTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsAStraightFlushTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShapeOfStraightTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsAStraightTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsAFlushTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsThreeOfAKindTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsTwoPairTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsPairTest()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 5);
            _card5 = new Card(Suits.Hearts, 1);
            _card6 = new Card(Suits.Hearts, 7);
            _card7 = new Card(Suits.Clubs, 8);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsPair(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
        }

        [TestMethod()]
        public void IsPairTest2()
        {
            _card1 = new Card(Suits.Clubs, 4);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 5);
            _card5 = new Card(Suits.Hearts, 3);
            _card6 = new Card(Suits.Hearts, 7);
            _card7 = new Card(Suits.Clubs, 8);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsPair(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card5));
        }

        [TestMethod()]
        public void IsAFullHouseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsAFourOfAKindTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsRoyalFlushTest()
        {
            Assert.Fail();
        }
    }
}