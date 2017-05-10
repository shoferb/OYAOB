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
    public class HandTests
    {
        private Card _one;
        private Card _two;
        private List<Card> _publicCards;
        private Hand _hand;
        private Deck _deck;
        private Player _player;

        [TestInitialize()]
        public void Initialize()
        {
            _publicCards =  new List<Card>();
            _hand = new Hand();
            _one = new Card(Suits.Hearts, 4);
            _two = new Card(Suits.Clubs, 5);
            _deck = new Deck();
            _player = new Player(1000, 100, 1,"Yarden","Chen","",0,0,"",0);
        }

       [TestMethod()]
        public void GetCardsTest()
       {
            _hand.Add2Cards(_deck.Draw(), _deck.Draw());
           List<Card> _res = _hand.GetCards();
            Assert.IsTrue(_res[0] != _res[1]);
       }

        [TestMethod()]
        public void ClearCardsTest()
        {
            _player._hand.Add2Cards(_deck.Draw(), _deck.Draw());
            _player._hand.ClearCards();
            Assert.IsTrue(_player._hand._firstCard==null && _player._hand._seconedCard == null);
        }

       [TestMethod()]
        public void AddCardsTest()
        {
            _player._hand.Add2Cards(_deck.Draw(), _deck.Draw());
            List<Card> _res = _player._hand.GetCards();
            Assert.IsTrue(_res[0] != _res[1]);
        }
    }
}