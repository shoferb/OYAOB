using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Actions
{
    public abstract class GameAction: Action
    {
        public int _pot { set; get; }
        public List<Card> _cardsOnTable { set; get; }

        public GameAction(int pot, List<Card> cardsOnTable, Player player, int roomID, int gameNumber) :
            base(player, roomID, gameNumber)
        {
            _pot = pot;
            _cardsOnTable = cardsOnTable;
        }
    }
}
