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
    public class GameManagerTests
    {
        // private GameManager _hand;
        private ConcreteGameRoom _room;
        private Player _player1;
        private Player _player2;
        private Player _player3;
        private List<Player> _players;
        private List<HandEvaluator> _winners;

        [TestInitialize()]
        public void Initialize()   
        {
            _player1 = new Player(1000,100,1,"Y","C","",0,0,"",0);
            _player2 = new Player(1000, 200, 1, "L", "M", "", 0, 0, "", 0);
            _player3 = new Player(1000, 300, 1, "Z", "X", "", 0, 0, "", 0);
            _players = new List<Player>();
            _players.Add(_player1);
            _players.Add(_player2);
            _players.Add(_player3);
           _room = new ConcreteGameRoom(_players, 50);
        }
        //TODO: move the random play of player to here
        [TestMethod()]
        public void SetRoleTest()
        {   //check game with only two.
            Console.WriteLine(_room._gm._sbPlayer.name);
            Console.WriteLine(_room._gm._dealerPlayer.name);
            Console.WriteLine(_room._gm._bbPlayer.name);
            Assert.IsTrue(_room._gm._dealerPlayer!= _room._gm._bbPlayer && _room._gm._dealerPlayer != _room._gm._sbPlayer);
        }

        [TestMethod()]
        public void PlayTest()
        {
            Console.WriteLine(_room._gm._forTest);
            Assert.IsTrue(_room._gm._forTest>=3);
        }

        [TestMethod()]
        public void ProgressHandTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void EndHandTest()
        {
            Assert.IsTrue(_room._gm._winners.Count >= 1);
        }

        [TestMethod()]
        public void FindWinnerTieTest()
        {
            _room.ClearPublicCards();
           _player1.ClearCards();
            _player2.ClearCards();
            _player3.ClearCards();
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
            _player1.AddHoleCards(card6, card7);
            _player2.AddHoleCards(card6, card7);
            _player3.AddHoleCards(card6, card7);
            _winners = _room._gm.FindWinner(table, _players);
            Assert.IsTrue(_winners.Count == 3);
        }

        [TestMethod()]
        public void FindWinnerAlmostTieTest()
        {
            _room.ClearPublicCards();
            _player1.ClearCards();
            _player2.ClearCards();
            _player3.ClearCards();
            List<Card> table = new List<Card>();
            Card card1 = new Card(Suits.Clubs, 1);
            Card card2 = new Card(Suits.Clubs, 13);
            Card card3 = new Card(Suits.Clubs, 2);
            Card card4 = new Card(Suits.Clubs, 3);
            Card card5 = new Card(Suits.Clubs, 12);
            Card card6 = new Card(Suits.Clubs, 4);
            Card card7 = new Card(Suits.Clubs, 5);
            Card card8 = new Card(Suits.Clubs, 11);
            Card card9 = new Card(Suits.Clubs, 10);
            Card card10 = new Card(Suits.Spades, 11);
            Card card11 = new Card(Suits.Spades, 10);
            table.Add(card1);
            table.Add(card2);
            table.Add(card3);
            table.Add(card4);
            table.Add(card5);
            _player1.AddHoleCards(card10, card11);
            _player2.AddHoleCards(card6, card7);
            _player3.AddHoleCards(card8, card9);
            _winners = _room._gm.FindWinner(table, _players);
            Assert.IsTrue(_winners.Count == 1);
            Assert.IsTrue(_winners[0]._player == _player3);
        }

        [TestMethod()]
        public void FindWinnerAlmostTieTest2()
        {
            _room.ClearPublicCards();
            _player1.ClearCards();
            _player2.ClearCards();
            _player3.ClearCards();
            List<Card> table = new List<Card>();
            Card card1 = new Card(Suits.Clubs, 1);
            Card card2 = new Card(Suits.Clubs, 13);
            Card card3 = new Card(Suits.Clubs, 2);
            Card card4 = new Card(Suits.Spades, 3);
            Card card5 = new Card(Suits.Clubs, 12);
            Card card6 = new Card(Suits.Spades, 4);
            Card card7 = new Card(Suits.Spades, 5);
            Card card8 = new Card(Suits.Hearts, 11);
            Card card9 = new Card(Suits.Hearts, 10);
            Card card10 = new Card(Suits.Spades, 11);
            Card card11 = new Card(Suits.Spades, 10);
            table.Add(card1);
            table.Add(card2);
            table.Add(card3);
            table.Add(card4);
            table.Add(card5);
            _player1.AddHoleCards(card10, card11);
            _player2.AddHoleCards(card6, card7);
            _player3.AddHoleCards(card8, card9);
            _winners = _room._gm.FindWinner(table, _players);
            Assert.IsTrue(_winners.Count == 2);
            Assert.IsTrue(_winners[0]._player == _player1 ||
                _winners[0]._player == _player3);
            Assert.IsTrue(_winners[1]._player == _player1 ||
                _winners[1]._player == _player3);
        }

        [TestMethod()]
        public void FindWinnerTest3()
        {
            _room.ClearPublicCards();
            _player1.ClearCards();
            _player2.ClearCards();
            _player3.ClearCards();
            List<Card> table = new List<Card>();
            Card card1 = new Card(Suits.Clubs, 1);
            Card card2 = new Card(Suits.Hearts, 12);
            Card card3 = new Card(Suits.Clubs, 6);
            Card card4 = new Card(Suits.Spades, 1);
            Card card5 = new Card(Suits.Diamonds, 6);
            Card card6 = new Card(Suits.Hearts, 1);
            Card card7 = new Card(Suits.Spades, 5);
            Card card8 = new Card(Suits.Hearts, 7);
            Card card9 = new Card(Suits.Spades, 6);
            Card card10 = new Card(Suits.Spades, 13);
            Card card11 = new Card(Suits.Clubs, 13);
            table.Add(card1);
            table.Add(card2);
            table.Add(card3);
            table.Add(card4);
            table.Add(card5);
            _player1.AddHoleCards(card6, card7);
            _player2.AddHoleCards(card8, card9);
            _player3.AddHoleCards(card10, card11);
            _winners = _room._gm.FindWinner(table, _players);
            Assert.IsTrue(_winners.Count == 1);
            Assert.IsTrue(_winners[0]._player == _player1);
        }
    }
}