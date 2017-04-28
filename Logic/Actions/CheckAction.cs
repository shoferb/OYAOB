using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    class CheckAction : PlayerAction
    {
        public CheckAction(Player player, Card card1, Card card2) :
            base(player, card1, card2, 0)
        {
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, Holding cards: {1} and {2}, Performed Check \n",
                        _player.user.MemberName(), _card1.ToString(), _card2.ToString());
        }

    }
}
