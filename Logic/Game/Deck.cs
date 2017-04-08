using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using System.Collections;


namespace TexasHoldem
{
    public class Deck
    {
        private int _numOfCards { get; set; }
        private ArrayList _cards = new ArrayList();
    private int[] _numbers = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        static Random r = new Random();

        public Deck()
        {
            giveValuesToCards();
        }

        private void giveValuesToCards()
        {
            int i = 0;
            foreach (int s in _numbers)
            {
                _cards.Add(new Card(Suits.Clubs, s));
            }
            foreach (int s in _numbers)
            {
                _cards.Add(new Card(Suits.Spades, s));
            }
            foreach (int s in _numbers)
            {
                _cards.Add(new Card(Suits.Hearts, s));
            }
            foreach (int s in _numbers)
            {
                _cards.Add(new Card(Suits.Diamonds, s));
            }

            this._numOfCards = _cards.Count;
        }

        private void Shuffle()
        {
            for (int n = _cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = _cards[n] as Card;
                _cards[n] = _cards[k];
                _cards[k] = temp;
            }
        }

        private Card Draw()
        {    
            Shuffle();
            Card c = _cards[r.Next(0)] as Card; 
            _cards.Remove(c);
            this._numOfCards--;
            return c;
        }

        private bool isEmpty()
        {
            return _numOfCards == 0 ? true: false;
        }
    }
}
