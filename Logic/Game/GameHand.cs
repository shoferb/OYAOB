using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game
{
    public class GameHand
    {
        private Deck deck;
        private Card[] hand;

        public GameHand(Deck deck)
        {
            this.deck = deck;
            this.hand = new Card[5];
        }

        public void pullCards()
        {
            for (int i = 0; i < 5; ++i) //TODO change to 2 
                hand[i] = deck.Draw();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Card c in hand)
            {
                sb.Append(c);
                sb.Append(", ");
            }
            return sb.ToString();
        }

        public Card this[int index]
        {
            get
            {
                return hand[index];
            }
        }
        public void Sort()
        {
            Array.Sort(hand);
        }
    }

}
