using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class Deck
    {
        private int _numOfCards { set; get; }
        private Card _cards { set; get;}

        public Deck(int num, Card cards)
        {
            this._numOfCards = num;
            this._cards = cards;
        }

        private void Shuffle()
        {
            throw new NotImplementedException();
        }

        private void Draw()
        {
            throw new NotImplementedException();
        }

        private bool isEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
