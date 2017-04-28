using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class WinAction : PlayerAction
    {
        public List<Card> _cardsOnTable { get; set; }
        public List<Card> _winningHand { get; set; }
        public int _pot;

        public WinAction(Player player, Card card1, Card card2, int pot, List<Card> table, List<Card> winningHand ) :
            base(player, card1, card2, pot)
        {
            _cardsOnTable = table;
            _winningHand = winningHand;
            _pot = pot;
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, Holding cards: {1} and {2}, Won the game and got {3} jetons\n" +
                "Cards on table: {4},   Winning _hand: {5}\n",
                _player.user.MemberName(), _card1.ToString(), _card2.ToString(), _amount, _cardsOnTable, _winningHand);
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
