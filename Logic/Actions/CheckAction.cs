using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    class CheckAction : UserAction
    {
        public CheckAction(Card card1, Card card2, int playerPosition, Role playerRole, int amount,
            Player player, int roomID, int gameNumber) :
            base(card1, card2, playerPosition, playerRole, amount, player, roomID, gameNumber)
        {
        }

        public override String DoAction()
        {
            throw new NotImplementedException();
        }
    }
}
