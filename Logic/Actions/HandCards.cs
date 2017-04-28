using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class HandCards : PlayerAction
    {
        public HandCards(Player player, Card card1, Card card2) :
            base(player, card1, card2, 0)
        {
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, Got cards: {1} and {2}\n",
                        _player.user.MemberName(), _card1.ToString(), _card2.ToString());
        }
    }
}
