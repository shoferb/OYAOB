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
        public Card _card1 { set; get; }
        public Card _card2 { set; get; }
        public int _playerPosition { set; get; }
        public Role _playerRole { set; get; }
        public int _amount { set; get; }

        public UserAction(Card card1, Card card2, int playerPosition, Role playerRole, int amount,
            Player player, int roomID, int gameNumber) : base(player, roomID, gameNumber)
        {
            _card1 = card1;
            _card2 = card2;
            _playerPosition = playerPosition;
            _playerRole = playerRole;
            _amount = amount;
        }

    }
}
