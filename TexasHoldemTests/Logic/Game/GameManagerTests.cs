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
           _room = new ConcreteGameRoom(_players, 50, 0);
        }

        [TestMethod()]
        public void SetRoleTest3Players()
        {
            _room._gm.SetRoles();
            Assert.IsTrue(_room._gm._dealerPlayer!= _room._gm._bbPlayer && _room._gm._dealerPlayer != _room._gm._sbPlayer);
        }

        [TestMethod()]
        public void SetRoleTest2Players()
        {   
            _room._players.Remove(_player1);
            _room._gm.SetRoles();
            Assert.IsTrue(_room._gm._dealerPlayer != _room._gm._bbPlayer && _room._gm._dealerPlayer == _room._gm._sbPlayer);
        }

        [TestMethod()]
        public void PlayerDesicionFoldTest()
        {
            _room._gm._currentPlayer = _player1;
            _room._gm.PlayerDesicion(-1);
            Assert.IsTrue(_player1.isPlayerActive == false);
        }

        [TestMethod()]
        public void PlayerDesicionFoldCheck()
        {
            _room._gm._currentPlayer = _player1;
            _room._gm.PlayerDesicion(0);
            Assert.IsTrue(_player1._lastAction == "check");
        }

        [TestMethod()]
        public void PlayerDesicionFoldRaise()
        {
            _room._deck = new Deck();
            _room._gm._currentPlayer = _player1;
            _room._maxCommitted = 50;
            _player1.isPlayerActive = true;
            _player2.isPlayerActive = true;
            _room._gm.PlayerDesicion(100);
            Assert.IsTrue(_room._maxCommitted >= 100);
        }

        [TestMethod()]
        public void ProgressHandTest()
        {
            _room._deck = new Deck();
            _player1.isPlayerActive = true;
            _player2.isPlayerActive = true;
            _player3.isPlayerActive = true;
            _room._gm.ProgressHand(ConcreteGameRoom.HandStep.PreFlop);
            Assert.IsTrue(_room._handStep == ConcreteGameRoom.HandStep.Flop);
            Assert.IsTrue(_room._publicCards.Count==3);

        }

        [TestMethod()]
        public void EndHandTest()
        {
           //TODO
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