using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class JoinAction : Action
    {
        public Player _player { get; set; }
        public int _position { get; set; }

        public JoinAction(Player player, int position)
        {
            _player = player;
            _position = position;
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("PlayerName: {0}, joined position number {1} in the game\n",
                        _player.user.MemberName(), _position);
        }
    }
}
