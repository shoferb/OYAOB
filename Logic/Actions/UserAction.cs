using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Actions
{
    public abstract class UserAction : Action
    {
        private Card _card1;
        private Card _card2;
        private int _playerPosition;
        private Role playerRole;
        private int _amount;

        public Card Card1 { get => _card1; set => _card1 = value; }
        public Card Card2 { get => _card2; set => _card2 = value; }
        public int PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
        public Role PlayerRole { get => playerRole; set => playerRole = value; }
        public int Amount { get => _amount; set => _amount = value; }
    }
}
