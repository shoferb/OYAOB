using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game.Evaluator
{
    public enum HandRank
    {
        ROYAL_FLUSH = 9,
        STRAIGHT_FLUSH = 8,
        FOUR_OF_A_KIND = 7,
        FULL_HOUSE = 6,
        FLUSH = 5,
        STRAIGHT = 4,
        THREE_OF_A_KIND = 3,
        TWO_PAIR = 2,
        PAIR = 1,
        HIGH_CARD = 0
    }
}
