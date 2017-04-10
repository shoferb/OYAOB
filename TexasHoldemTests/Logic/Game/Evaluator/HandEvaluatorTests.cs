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
        public void DetermineHandRankRoyalTest()
        {
            _card1 = new Card(Suits.Diamonds, 11);
            _card2 = new Card(Suits.Diamonds, 12);
            _card3 = new Card(Suits.Diamonds, 1);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Diamonds, 10);
            _card7 = new Card(Suits.Diamonds, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;
            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.ROYAL_FLUSH);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card6));
        }

        [TestMethod()]
        public void DetermineHandRankSFTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 12);
            _card3 = new Card(Suits.Clubs, 13);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 10);
            _card7 = new Card(Suits.Clubs, 11);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.STRAIGHT_FLUSH);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void DetermineHandRankFourTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Diamonds, 9);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 1);
            _card7 = new Card(Suits.Spades, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.FOUR_OF_A_KIND);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
        }

        [TestMethod()]
        public void DetermineHandRankFullTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 3);
            _card7 = new Card(Suits.Diamonds, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.FULL_HOUSE);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card3));
        }

        [TestMethod()]
        public void DetermineHandRankFlushTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 1);
            _card7 = new Card(Suits.Clubs, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.FLUSH);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void DetermineHandRankStraightTest()
        {
            _card1 = new Card(Suits.Clubs, 6);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.STRAIGHT);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card2));
        }

        [TestMethod()]
        public void DetermineHandRankThreeTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 1);
            _card7 = new Card(Suits.Diamonds, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.THREE_OF_A_KIND);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card4));
        }

        [TestMethod()]
        public void DetermineHandRankTwoPairsTest()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 3);
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

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.TWO_PAIR);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void DetermineHandRankPairTest()
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

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.PAIR);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card4));
        }

        [TestMethod()]
        public void DetermineHandRankHighTest()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 5);
            _card5 = new Card(Suits.Hearts, 13);
            _card6 = new Card(Suits.Hearts, 7);
            _card7 = new Card(Suits.Clubs, 12);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            _evaluator.DetermineHandRank(_cards);
            Assert.IsTrue(_evaluator._rank == HandRank.HIGH_CARD);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card4));
        }

        [TestMethod()]
        public void IsAStraightFlushTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 12);
            _card3 = new Card(Suits.Clubs, 13);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 10);
            _card7 = new Card(Suits.Clubs, 11);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraightFlush(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsAStraightFlushTest2()
        {
            _card1 = new Card(Suits.Diamonds, 11);
            _card2 = new Card(Suits.Diamonds, 12);
            _card3 = new Card(Suits.Diamonds, 1);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Diamonds, 10);
            _card7 = new Card(Suits.Diamonds, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraightFlush(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsAStraightFlushTest3()
        {
            _card1 = new Card(Suits.Spades, 5);
            _card2 = new Card(Suits.Spades, 2);
            _card3 = new Card(Suits.Spades, 1);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Spades, 9);
            _card6 = new Card(Suits.Spades, 4);
            _card7 = new Card(Suits.Spades, 3);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraightFlush(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsAStraightFlushTest4()
        {
            _card1 = new Card(Suits.Spades, 8);
            _card2 = new Card(Suits.Spades, 2);
            _card3 = new Card(Suits.Spades, 3);
            _card4 = new Card(Suits.Spades, 1);
            _card5 = new Card(Suits.Spades, 5);
            _card6 = new Card(Suits.Spades, 4);
            _card7 = new Card(Suits.Spades, 6);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraightFlush(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void ShapeOfStraightTest()
        {
            _card1 = new Card(Suits.Clubs, 6);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Clubs, 4);
            _card5 = new Card(Suits.Clubs, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.ShapeOfStraight(_cards, Suits.Clubs) == Suits.Clubs);
        }

        [TestMethod()]
        public void ShapeOfStraightTest2()
        {
            _card1 = new Card(Suits.Diamonds, 6);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Clubs, 4);
            _card5 = new Card(Suits.Clubs, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Clubs, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.ShapeOfStraight(_cards, Suits.Clubs) == Suits.Clubs);
        }

        [TestMethod()]
        public void ShapeOfStraightTest3()
        {
            _card1 = new Card(Suits.Diamonds, 11);
            _card2 = new Card(Suits.Diamonds, 10);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 12);
            _card5 = new Card(Suits.Clubs, 5);
            _card6 = new Card(Suits.Diamonds, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.ShapeOfStraight(_cards, Suits.Diamonds) == Suits.Diamonds);
        }

        [TestMethod()]
        public void IsAStraightTest()
        {
            _card1 = new Card(Suits.Clubs, 6);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraight(_cards) != Suits.None);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card2));
        }

        [TestMethod()]
        public void IsAStraightTest2()
        {
            _card1 = new Card(Suits.Clubs, 10);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 11);
            _card4 = new Card(Suits.Diamonds, 12);
            _card5 = new Card(Suits.Hearts, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraight(_cards) != Suits.None);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));

        }

        [TestMethod()]
        public void IsAStraightTest3()
        {
            _card1 = new Card(Suits.Clubs, 7);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraight(_cards) != Suits.None);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsAStraightTest4()
        {
            _card1 = new Card(Suits.Clubs, 7);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 6);
            _card5 = new Card(Suits.Hearts, 5);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Diamonds, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAStraight(_cards) == Suits.None);
        }

        [TestMethod()]
        public void IsAFlushTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 1);
            _card7 = new Card(Suits.Clubs, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFlush(_cards) == Suits.Clubs);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsAFlushTest2()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Clubs, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 1);
            _card7 = new Card(Suits.Clubs, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFlush(_cards) == Suits.Clubs);
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));

        }

        [TestMethod()]
        public void IsAFlushTest3()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Clubs, 2);
            _card3 = new Card(Suits.Hearts, 3);
            _card4 = new Card(Suits.Clubs, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Clubs, 1);
            _card7 = new Card(Suits.Hearts, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFlush(_cards) == Suits.None);
        }

        [TestMethod()]
        public void IsThreeOfAKindTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 1);
            _card7 = new Card(Suits.Diamonds, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsThreeOfAKind(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card4));
        }

        [TestMethod()]
        public void IsThreeOfAKindTest2()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 12);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 7);
            _card7 = new Card(Suits.Diamonds, 8);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsFalse(_evaluator.IsThreeOfAKind(_cards));
        }

        [TestMethod()]
        public void IsTwoPairTest()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 3);
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

            Assert.IsTrue(_evaluator.IsTwoPair(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsTwoPairTest2()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 4);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 2);
            _card5 = new Card(Suits.Hearts, 12);
            _card6 = new Card(Suits.Hearts, 2);
            _card7 = new Card(Suits.Clubs, 12);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsTwoPair(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card1));
        }

        [TestMethod()]
        public void IsTwoPairTest3()
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

            Assert.IsFalse(_evaluator.IsTwoPair(_cards));
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
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card4));
        }

        [TestMethod()]
        public void IsPairTest2()
        {
            _card1 = new Card(Suits.Clubs, 4);
            _card2 = new Card(Suits.Hearts, 1); 
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
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }

        [TestMethod()]
        public void IsPairTest3()
        {
            _card1 = new Card(Suits.Clubs, 1);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 12);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Clubs, 7);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsFalse(_evaluator.IsPair(_cards));
        }

        [TestMethod()]
        public void IsAFourOfAKindTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Diamonds, 9);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 1);
            _card7 = new Card(Suits.Spades, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFourOfAKind(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card6));
        }

        [TestMethod()]
        public void IsAFourOfAKindTest2()
        {
            _card1 = new Card(Suits.Clubs, 12);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Diamonds, 12);
            _card4 = new Card(Suits.Diamonds, 3);
            _card5 = new Card(Suits.Clubs, 3);
            _card6 = new Card(Suits.Hearts, 3);
            _card7 = new Card(Suits.Spades, 3);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFourOfAKind(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1) || _evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card6));
        }

        [TestMethod()]
        public void IsAFourOfAKindTest3()
        {
            _card1 = new Card(Suits.Clubs, 12);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Diamonds, 12);
            _card4 = new Card(Suits.Diamonds, 3);
            _card5 = new Card(Suits.Clubs, 3);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Spades, 3);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsFalse(_evaluator.IsAFourOfAKind(_cards));
        }

        [TestMethod()]
        public void IsRoyalFlushTest()
        {
            _card1 = new Card(Suits.Diamonds, 11);
            _card2 = new Card(Suits.Diamonds, 12);
            _card3 = new Card(Suits.Diamonds, 1);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Diamonds, 10);
            _card7 = new Card(Suits.Diamonds, 13);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsRoyalFlush(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card2));
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card6));
        }

        [TestMethod()]
        public void IsRoyalFlushTest2()
        {
            _card1 = new Card(Suits.Diamonds, 8);
            _card2 = new Card(Suits.Diamonds, 2);
            _card3 = new Card(Suits.Diamonds, 1);
            _card4 = new Card(Suits.Diamonds, 5);
            _card5 = new Card(Suits.Diamonds, 9);
            _card6 = new Card(Suits.Diamonds, 4);
            _card7 = new Card(Suits.Diamonds, 3);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsFalse(_evaluator.IsRoyalFlush(_cards));
        }

        [TestMethod()]
        public void IsRoyalFlushTest3()
        {
            _card1 = new Card(Suits.Diamonds, 8);
            _card2 = new Card(Suits.Diamonds, 2);
            _card3 = new Card(Suits.Diamonds, 7);
            _card4 = new Card(Suits.Diamonds, 5);
            _card5 = new Card(Suits.Diamonds, 9);
            _card6 = new Card(Suits.Diamonds, 4);
            _card7 = new Card(Suits.Diamonds, 3);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsFalse(_evaluator.IsRoyalFlush(_cards));
        }

        [TestMethod()]
        public void IsAFullHouseTest()
        {
            _card1 = new Card(Suits.Clubs, 9);
            _card2 = new Card(Suits.Hearts, 2);
            _card3 = new Card(Suits.Clubs, 3);
            _card4 = new Card(Suits.Diamonds, 4);
            _card5 = new Card(Suits.Hearts, 9);
            _card6 = new Card(Suits.Hearts, 3);
            _card7 = new Card(Suits.Diamonds, 9);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFullHouse(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card7));
            Assert.IsTrue(_evalCards.Contains(_card6));
            Assert.IsTrue(_evalCards.Contains(_card3));
        }

        [TestMethod()]
        public void IsAFullHouseTest2()
        {
            _card1 = new Card(Suits.Spades, 13);
            _card2 = new Card(Suits.Hearts, 9);
            _card3 = new Card(Suits.Clubs, 1);
            _card4 = new Card(Suits.Diamonds, 1);
            _card5 = new Card(Suits.Clubs, 13);
            _card6 = new Card(Suits.Hearts, 13);
            _card7 = new Card(Suits.Hearts, 1);

            _cards[0] = _card1;
            _cards[1] = _card2;
            _cards[2] = _card3;
            _cards[3] = _card4;
            _cards[4] = _card5;
            _cards[5] = _card6;
            _cards[6] = _card7;

            Assert.IsTrue(_evaluator.IsAFullHouse(_cards));
            _evalCards = _evaluator._relevantCards;
            Assert.IsTrue(_evalCards.Contains(_card1));
            Assert.IsTrue(_evalCards.Contains(_card5));
            Assert.IsTrue(_evalCards.Contains(_card3));
            Assert.IsTrue(_evalCards.Contains(_card4));
            Assert.IsTrue(_evalCards.Contains(_card7));
        }
    }
}