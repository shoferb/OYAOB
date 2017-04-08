using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public class LeaveAction : GameAction
    {
        public LeaveAction(int pot, List<Card> cardsOnTable, Player player, int roomID, int gameNumber) :
            base(pot, cardsOnTable, player, roomID, gameNumber)
        {
        }

        public override String DoAction()
        {
            throw new NotImplementedException();
        }
    }
}
