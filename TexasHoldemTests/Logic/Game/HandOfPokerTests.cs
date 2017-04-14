using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;
using TexasHoldem.Logic.Game.Evaluator;

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
        List<Player> _players;
        List<HandEvaluator> _winners;

        [TestInitialize()]
        public void Initialize()   
        {
            _player1 = new Player(1000,100,1,"Y","C","",0,0,"",0,false);
            _player2 = new Player(1000, 200, 1, "L", "M", "", 0, 0, "", 0, false);
            _player3 = new Player(1000, 300, 1, "Z", "X", "", 0, 0, "", 0, false);
            _players = new List<Player>();
            _players.Add(_player1);
            _players.Add(_player2);
            _players.Add(_player3);
            _room = new ConcreteGameRoom(_players, 1);
            _hand = new HandOfPoker(_room);
        }

        [TestMethod()]
        public void NewHandTest()
        {
            _hand.NewHand(_room);
            Assert.IsTrue(_hand._dealerPlayer!= _hand._bbPlayer && _hand._dealerPlayer != _hand._sbPlayer);
        }

        [TestMethod()]
        public void PlayTest()
        {
            _hand.NewHand(_room);
            _hand.Play(_room);
            Assert.IsTrue(_hand._forTest>4);
        }

        [TestMethod()]
        public void ProgressHandTest()
        {
            _hand.NewHand(_room);
            _hand.Play(_room);
            Assert.IsTrue(_hand._verifyAction == 4);
        }

        [TestMethod()]
        public void EndHandTest()
        {
            _hand.NewHand(_room);
            _hand.Play(_room);
           Assert.IsTrue(_hand._winners.Count>=1);
        }

        [TestMethod()]
        public void FindWinnerTieTest()
        {
            List<Card> table = new List<Card>();
            Card card1 = new Card(Suits.Clubs, 1);
            Card card2 = new Card(Suits.Clubs, 2);
            Card card3 = new Card(Suits.Clubs, 3);
            Card card4 = new Card(Suits.Clubs, 4);
            Card card5 = new Card(Suits.Clubs, 5);
            Card card6 = new Card(Suits.Clubs, 6);
            Card card7 = new Card(Suits.Clubs, 7);
            table.Add(card1);
            table.Add(card2);
            table.Add(card3);
            table.Add(card4);
            table.Add(card5);
            _player1.AddCard(card6);
            _player1.AddCard(card7);
            _player2.AddCard(card6);
            _player2.AddCard(card7);
            _player3.AddCard(card6);
            _player3.AddCard(card7);
            _winners = _hand.FindWinner(table, _players);
            Assert.IsTrue(_winners.Count == 3);
        }

        [TestMethod()]
        public void EvalTiesTest()
        {
            Assert.Fail();
        }
    }
}