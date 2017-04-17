 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGamePrefDecorator : GamePrefDecorator
    {
        public ConcreteGamePrefDecorator(List<Player> players, int startingChip, int ID, ReplayManager rm) : base(players, startingChip, ID, rm)
        {
        }

        public List<Player> _players { get; set; }
        public Guid _id { get; set; }
        public List<Spectetor> _spectatores { get; set; }
        public int _dealerPos { get; set; }
        public int _maxCommitted { get; set; }
        public int _actionPos { get; set; }
        public int _potCount { get; set; }
        public int _bb { get; set; }
        public int _sb { get; set; }
        public Deck _deck { get; set; }
        public List<Card> _publicCards { get; set; }
        public bool _isGameOver { get; set; }
        public List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public int _gameRoles { get; set; }


    }

}
