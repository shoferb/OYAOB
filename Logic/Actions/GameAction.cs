using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.User;

namespace TexasHoldem.Logic.Actions
{
    public abstract class GameAction: Action
    {
        private int _pot;
        private List<Card> _cardsOnTable;

        public GameAction(int pot, List<Card> cardsOnTable, Player player, int roomID, int gameNumber) :
            base(player, roomID, gameNumber)
        {
            _pot = pot;
            _cardsOnTable = cardsOnTable;
        }

        public int Pot { get; set; }
        public List<Card> CardsOnTable { get => _cardsOnTable; set => _cardsOnTable = value; }
    }
}
