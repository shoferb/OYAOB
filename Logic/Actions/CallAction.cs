using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class CallAction : PlayerAction
    {
        public CallAction(Player player, Card card1, Card card2, int amount) :
            base(player, card1, card2, amount)
        {
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, Holding cards: {1} and {2}, Performed Call with {3} jetons\n",
                        _player.MemberName, _card1.ToString(), _card2.ToString(), _amount);  
        }

    }
}
