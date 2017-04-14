using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using System.Collections;
using System.Security.Cryptography;


namespace TexasHoldem
{
    public class Deck
    {
        public int _numOfCards;
        public List<Card> _deck;
        public int[] _cardRank = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        static Random r = new Random();

        public Deck()
        {
            _deck = new List<Card>(52);
            for (int suit = 0; suit <= 3; suit++)
            {
                for (int rank = 0; rank <= 12; rank++)
                {
                    Card card = new Card((Suits) suit, _cardRank[rank]);
                    _deck.Add(card);
                }
            }


            Shuffle<Card>(_deck);
        }

        public int NumOfCards
        {
            get { return _numOfCards; }
        }

        public static void Shuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public Card ShowCard()
        {
            return _deck[0];
        }
        public void RemoveCard()
        {
            _deck.RemoveAt(0);
        }
        public Card Draw()
        {
            Card card = _deck[0];
            _deck.RemoveAt(0);
            return card;
        }

    }
}
