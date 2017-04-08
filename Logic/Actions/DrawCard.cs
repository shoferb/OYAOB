using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class DrawCard : Action
    {
        private Card _card { set; get; }
        private List<Card> _cardsOnTable { set; get; }
        public int _pot { set; get; }

        public DrawCard(Card card, List<Card> cardsOnTable, int pot)
        {
            _card = card;
            _cardsOnTable = cardsOnTable;
            _pot = pot;
        }


        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("Dealer Draw new card: {0}, Cards on Table: {1}\n" +
                "Pot: {2}\n",
                        _card.ToString(), PrintCards(), _pot);
        }

        public String PrintCards()
        {
            String str = "";
            foreach (Card c in _cardsOnTable)
            {
                str += c.ToString() + " ";
            }
            return str;
        }

    }
}
