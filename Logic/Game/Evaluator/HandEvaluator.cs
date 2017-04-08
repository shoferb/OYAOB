using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game.Evaluator
{
    public class HandEvaluator
    {
        public HandRank determineHandRank(Card[] cards)
        {
            if (isARoyalFlush(cards))
            {
                return HandRank.ROYAL_FLUSH;
            }
            else if (isAStraightFlush(cards))
            {
                return HandRank.STRAIGHT_FLUSH;

            }
            else if (isAFourOfAKind(cards))
            {
                return HandRank.FOUR_OF_A_KIND;
            }
            else if (isAFullHouse(cards))
            {
                return HandRank.FULL_HOUSE;
            }
            else if (isAFlush(cards))
            {
                return HandRank.FLUSH;
            }
            else if (isAStraight(cards))
            {
                return HandRank.STRAIGHT;
            }
            else if (isThreeOfAKind(cards))
            {
                return HandRank.THREE_OF_A_KIND;
            }
            else if (isTwoPair(cards))
            {
                return HandRank.TWO_PAIR;
            }
            else if (isPair(cards))
            {
                return HandRank.PAIR;
            }
            else
            {
                return HandRank.HIGH_CARD;
            }

        }

        public bool isARoyalFlush(Card[] flop)
        {
            if (isAStraight(flop) && isAFlush(flop))
            {
                bool aceExists = false, kingExists = false, queenExists = false, jackExists = false, tenExists = false;
                foreach (Card c in flop)
                {
                    switch (c._value)
                    {
                        case 1:
                            aceExists = true;
                            break;
                        case 13:
                            kingExists = true;
                            break;
                        case 12:
                            queenExists = true;
                            break;
                        case 11:
                            jackExists = true;
                            break;
                        case 10:
                            tenExists = true;
                            break;

                    }
                }
                return (aceExists && kingExists && queenExists && jackExists && tenExists);
            }
            else
            {
                return false;
            }
        }

        public bool isAStraight(Card[] flop)
        {
            Array.Sort(flop, (x, y) => x._value.CompareTo(y._value));
            int noOfCardsInARow = 0;
            int pos = 0;
            bool isAStraight = false;
            while (pos < flop.Count() - 1 && !isAStraight)
            {
                if (flop[pos + 1]._value - flop[pos]._value == 1)
                {
                    noOfCardsInARow++;
                    if (noOfCardsInARow == 4)
                    {
                        isAStraight = true;
                    }
                    else
                    {
                        pos++;
                    }
                }
                else
                {
                    noOfCardsInARow = 0;
                    pos++;
                }
            }
            return isAStraight;
        }

        public bool isAFlush(Card[] flop)
        {
            int noOfClubs = 0;
            int noOfSpades = 0;
            int noOfHearts = 0;
            int noOfDiamonds = 0;
            foreach (Card c in flop)
            {
                switch (c._suit)
                {
                    case Suits.Hearts:
                        noOfHearts++;
                        break;
                    case Suits.Spades:
                        noOfSpades++;
                        break;
                    case Suits.Clubs:
                        noOfClubs++;
                        break;
                    case Suits.Diamonds:
                        noOfDiamonds++;
                        break;
                }
            }
            return (noOfClubs >= 5 || noOfSpades >= 5 || noOfHearts >= 5 || noOfDiamonds >= 5);
        }

        private bool isThreeOfAKind(Card[] flop)
        {
            int cardRepeats = 1;
            bool isThreeOfAKind = false;
            int i = 0;
            int k = i + 1;
            while (i < flop.Count() && !isThreeOfAKind)
            {
                cardRepeats = 1;
                while (k < flop.Count() && !isThreeOfAKind)
                {
                    if (flop[i]._value == flop[k]._value)
                    {
                        cardRepeats++;
                        if (cardRepeats == 3)
                        {
                            isThreeOfAKind = true;
                        }
                    }
                    k++;
                }
                i++;
            }
            return isThreeOfAKind;
        }

        private bool isTwoPair(Card[] flop)
        {
            int cardRepeats = 1;
            int noOfCardRepeats = 0;
            bool isTwoPair = false;
            int i = 0;
            int k = i + 1;
            while (i < flop.Count() && !isTwoPair)
            {
                cardRepeats = 1;
                while (k < flop.Count() && !isTwoPair)
                {
                    if (flop[i]._value == flop[k]._value)
                    {
                        cardRepeats++;
                        if (cardRepeats == 2)
                        {
                            cardRepeats = 1;
                            noOfCardRepeats++;
                            if (noOfCardRepeats == 2)
                            {
                                isTwoPair = true;

                            }
                        }

                    }
                    k++;
                }
                i++;
            }
            return isTwoPair;
        }

        private bool isPair(Card[] flop)
        {
            int cardRepeats = 1;
            bool isPair = false;
            int i = 0;
            int k = i + 1;
            while (i < flop.Count() && !isPair)
            {
                cardRepeats = 1;
                while (k < flop.Count() && !isPair)
                {
                    if (flop[i]._value == flop[k]._value)
                    {
                        cardRepeats++;
                        if (cardRepeats == 2)
                        {
                            isPair = true;
                        }
                    }
                    k++;
                }
                i++;
            }
            return isPair;
        }

        private bool isAFullHouse(Card[] flop)
        {
            Array.Sort(flop, (x, y) => x._value.CompareTo(y._value));
            int noOfRepeats = 1;
            bool isThreeOfAKind = false;
            bool isTwoOfAKind = false;
            for (int i = 0; i < flop.Count() - 1; i++)
            {
                if (flop[i]._value == flop[i + 1]._value)
                {
                    noOfRepeats++;
                    if (noOfRepeats == 3)
                    {
                        isThreeOfAKind = true;
                        noOfRepeats = 1;
                    }
                    else if (noOfRepeats == 2)
                    {
                        isTwoOfAKind = true;
                        noOfRepeats = 1;
                    }
                }
                else
                {
                    noOfRepeats = 1;
                }
            }
            return (isTwoOfAKind && isThreeOfAKind);

        }

        public bool isAFourOfAKind(Card[] flop)
        {
            int cardRepeats = 1;
            bool isFourOfAKind = false;
            int i = 0;
            int k = i + 1;
            while (i < flop.Count() && !isFourOfAKind)
            {
                cardRepeats = 1;
                while (k < flop.Count() && !isFourOfAKind)
                {
                    if (flop[i]._value == flop[k]._value)
                    {
                        cardRepeats++;
                        if (cardRepeats == 4)
                        {
                            isFourOfAKind = true;
                        }
                    }
                    k++;
                }
                i++;
            }
            return isFourOfAKind;
        }

        private bool isAStraightFlush(Card[] flop)
        {
            return isAFlush(flop) && isAStraight(flop);
        }

        public Card getHighCard(Card[] flop)
        {
            Array.Sort(flop, (x, y) => x._value.CompareTo(y._value));
            return flop[0];
        }
/*
        public Card getHandHighCard()
        {
            //Array.Sort(flop, (x, y) => x._value.CompareTo(y._value));
            Arrays.sort(hand, byRank);
            return hand[0];
        }
        */
    }
}

