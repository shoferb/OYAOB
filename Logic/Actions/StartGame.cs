using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class StartGame : Action
    {
        public List<Player> _players;
        public Player _dealer;
        public Player _sb;
        public Player _bb;

        public StartGame(List<Player> players, Player dealer, Player sb, Player bb)
        {
            _players = players;
            _dealer = dealer;
            _sb = sb;
            _bb = bb;
        }

        public override String ReplayAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("Game Started, participants: {0}\n" +
                "Dealer: {1}, Small Blind: {2}, Big Blind: {3}\n",
                PrintPlayers(), _dealer.MemberName, _sb.MemberName, _bb.MemberName);
        }

        public String PrintPlayers()
        {
            String str = "";
            foreach (Player p in _players)
            {
                str += p.MemberName + " ";
            }
            return str;
        }
    }
}
