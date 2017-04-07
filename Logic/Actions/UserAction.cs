using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.User;

namespace TexasHoldem.Logic.Actions
{
    public abstract class UserAction : Action
    {
        private Card _card1;
        private Card _card2;
        private int _playerPosition;
        private Role _playerRole;
        private int _amount;

        public UserAction(Card card1, Card card2, int playerPosition, Role playerRole, int amount,
            Player player, int roomID, int gameNumber) : base(player, roomID, gameNumber)
        {
            _card1 = card1;
            _card2 = card2;
            _playerPosition = playerPosition;
            _playerRole = playerRole;
            _amount = amount;
        }

        public Card Card1 { get => _card1; set => _card1 = value; }
        public Card Card2 { get => _card2; set => _card2 = value; }
        public int PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
        public Role PlayerRole { get => _playerRole; set => _playerRole = value; }
        public int Amount { get => _amount; set => _amount = value; }
    }
}
