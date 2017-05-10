using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TexasHoldem.Tests
{
    [TestClass()]
    public class DeckTests
    {
        private Deck _deckA;
        private Deck _deckB;


        [TestInitialize()]
        public void Initialize()
        {
            _deckA = new Deck();
            _deckB = new Deck();
        }

        [TestMethod()]
        public void ShuffleTest()
        {
            for (int i = 0; i < _deckA._deck.Count; i++)
            {
                Assert.IsTrue(_deckA._deck[i] != _deckB._deck[i]);
            }
        }

        [TestMethod()]
        public void RemoveCardTest()
        {
            int before = _deckA._deck.Count;
            _deckA.RemoveCard();

            Assert.IsTrue(before > _deckA._deck.Count);
        }

        [TestMethod()]
        public void DrawTest()
        {
            HashSet<Card> _c = new HashSet<Card>();
           for (int i = 0; i < 52; i++)
           {
               Card card = _deckA.Draw();
                if (!_c.Contains(card))
                _c.Add(card);
            }
            Assert.IsTrue(_c.Count==52);
        }
    }
}