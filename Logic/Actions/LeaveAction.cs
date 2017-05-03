using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class LeaveAction : Action
    {
        public Player _player { get; set; }

        public LeaveAction(Player player)
        {
            _player = player;
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, left the game\n",
                        _player.user.MemberName());
        }
    }
}
