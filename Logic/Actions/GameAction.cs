using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Actions
{
    public abstract class GameAction: Action
    {
        private int pot;
        private List<Card> cardsOnTable;

        public int Pot { get => pot; set => pot = value; }
        public List<Card> CardsOnTable { get => cardsOnTable; set => cardsOnTable = value; }
    }
}
