using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game
{
    class PokerLogic //TODO check if it seven
    {
        private static bool isFlush(GameHand h)
        {
            if (h[0]._suit == h[1]._suit &&
                h[1]._suit == h[2]._suit &&
                h[2]._suit == h[3]._suit &&
                h[3]._suit == h[4]._suit)
                return true;
            return false;
        }

        // make sure the rank differs by one
        // we can do this since the Hand is 
        // sorted by this point
        private static bool isStraight(GameHand h)
        {
            if (h[0]._value == h[1]._value - 1 &&
                h[1]._value == h[2]._value - 1 &&
                h[2]._value == h[3]._value - 1 &&
                h[3]._value == h[4]._value - 1)
                return true;
            // special case cause ace ranks lower
            // than 10 or higher
            if (h[1]._value == 10 &&
                h[2]._value == 11 &&
                h[3]._value == 12 &&
                h[4]._value == 13 &&
                h[0]._value == 0)
                return true;
            return false;
        }

        // must be flush and straight and
        // be certain cards. No wonder I have
        private static bool isRoyalFlush(GameHand h)
        {
            if (isStraight(h) && isFlush(h) &&
                  h[0]._value == 0 &&
                  h[1]._value == 10 &&
                  h[2]._value == 11 &&
                  h[3]._value == 12 &&
                  h[4]._value == 13)
                return true;
            return false;
        }

        private static bool isStraightFlush(GameHand h)
        {
            if (isStraight(h) && isFlush(h))
                return true;
            return false;
        }

        /*
         * Two choices here, the first four cards
         * must match in rank, or the second four
         * must match in rank. Only because the hand
         * is sorted
         */
        private static bool isFourOfAKind(GameHand h)
        {
            if (h[0]._value == h[1]._value &&
                h[1]._value == h[2]._value &&
                h[2]._value == h[3]._value)
                return true;
            if (h[1]._value == h[2]._value &&
                h[2]._value == h[3]._value &&
                h[3]._value == h[4]._value)
                return true;
            return false;
        }

        /*
         * two choices here, the pair is in the
         * front of the hand or in the back of the
         * hand, because it is sorted
         */
        private static bool isFullHouse(GameHand h)
        {
            if (h[0]._value == h[1]._value &&
                h[2]._value == h[3]._value &&
                h[3]._value == h[4]._value)
                return true;
            if (h[0]._value == h[1]._value &&
                h[1]._value == h[2]._value &&
                h[3]._value == h[4]._value)
                return true;
            return false;
        }

        /*
         * three choices here, first three cards match
         * middle three cards match or last three cards
         * match
         */
        private static bool isThreeOfAKind(GameHand h)
        {
            if (h[0]._value == h[1]._value &&
                h[1]._value == h[2]._value)
                return true;
            if (h[1]._value == h[2]._value &&
                h[2]._value == h[3]._value)
                return true;
            if (h[2]._value == h[3]._value &&
                h[3]._value == h[4]._value)
                return true;
            return false;
        }

        /*
         * three choices, two pair in the front,
         * separated by a single card or
         * two pair in the back
         */
        private static bool isTwoPair(GameHand h)
        {
            if (h[0]._value == h[1]._value &&
                h[2]._value == h[3]._value)
                return true;
            if (h[0]._value == h[1]._value &&
                h[3]._value == h[4]._value)
                return true;
            if (h[1]._value == h[2]._value &&
                h[3]._value == h[4]._value)
                return true;
            return false;
        }

        /*
         * 4 choices here
         */
        private static bool isOnePair(GameHand h)
        {
            if (h[0]._value == h[1]._value)
                return true;
            if (h[0]._value == h[1]._value)
                return true;
            if (h[1]._value == h[2]._value)
                return true;
            return false;
        }

        // must be in order of hands and must be
        // mutually exclusive choices
        public static POKERSCORE score(GameHand h)
        {
            if (isRoyalFlush(h))
                return POKERSCORE.RoyalFlush;
            else if (isStraightFlush(h))
                return POKERSCORE.StraightFlush;
            else if (isFourOfAKind(h))
                return POKERSCORE.FourOfAKind;
            else if (isFullHouse(h))
                return POKERSCORE.FullHouse;
            else if (isFlush(h))
                return POKERSCORE.Flush;
            else if (isStraight(h))
                return POKERSCORE.Straight;
            else if (isThreeOfAKind(h))
                return POKERSCORE.ThreeOfAKind;
            else if (isTwoPair(h))
                return POKERSCORE.TwoPair;
            else if (isOnePair(h))
                return POKERSCORE.OnePair;
            else
                return POKERSCORE.NoHand;
        }

}
}
