using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public abstract class PlayerAction : Action
    {
        public Player _player { get; set; }
        public Card _card1 { get; set; }
        public Card _card2 { get; set; }
        public int _amount { get; set; }

        public PlayerAction(Player player, Card card1, Card card2, int amount)
        {
            _player = player;
            _card1 = card1;
            _card2 = card2;
            _amount = amount;
        }
    }
}
