using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game.Evaluator
{
    public class HandEvaluator
    {
        public HandRank _rank;
        public List<Card> _relevantCards = new List<Card>();

        public void determineHandRank(Card[] cards)
        {
            Array.Sort(cards, (x, y) => x._value.CompareTo(y._value));
            if (isARoyalFlush(cards))
            {
                _rank = HandRank.ROYAL_FLUSH;
            }
            else if (isAStraightFlush(cards))
            {
                _rank = HandRank.STRAIGHT_FLUSH;

            }
            else if (isAFourOfAKind(cards))
            {
                _rank = HandRank.FOUR_OF_A_KIND;
            }
            else if (isAFullHouse(cards))
            {
                _rank = HandRank.FULL_HOUSE;
            }
            else if (isAFlush(cards))
            {
                _rank = HandRank.FLUSH;
            }
            else if (isAStraight(cards))
            {
                _rank = HandRank.STRAIGHT;
            }
            else if (isThreeOfAKind(cards))
            {
                _rank = HandRank.THREE_OF_A_KIND;
            }
            else if (isTwoPair(cards))
            {
                _rank = HandRank.TWO_PAIR;
            }
            else if (isPair(cards))
            {
                _rank = HandRank.PAIR;
            }
            else
            {
                _rank = HandRank.HIGH_CARD;
            }

        }

        public bool isARoyalFlush(Card[] cards)
        {
            _relevantCards.Clear();
            Suits flush = isAFlush(cards);
            if (flush == Suits.None) return false;
            Suits straight = isRoyalStraight(cards, flush);
            if (!(flush == straight)) return false;
            return true;
            /*
            bool aceExists = false, kingExists = false, queenExists = false, jackExists = false, tenExists = false;
            foreach (Card c in cards)
            {
                switch (c._value)
                {
                    case 1:
                        aceExists = true;
                        _relevantCards.Add(c);
                        break;
                    case 13:
                        kingExists = true;
                        _relevantCards.Add(c);
                        break;
                    case 12:
                        queenExists = true;
                        _relevantCards.Add(c);
                        break;
                    case 11:
                        jackExists = true;
                        _relevantCards.Add(c);
                        break;
                    case 10:
                        tenExists = true;
                        _relevantCards.Add(c);
                        break;
                }
            }
            return (aceExists && kingExists && queenExists && jackExists && tenExists);
        */
    
         }

        private Suits isRoyalStraight(Card[] cards, Suits flush)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            List<Card> relevant = new List<Card>();
            foreach (Card c in cards)
            {
                if (c._suit == flush)
                {
                    relevant.Add(c);
                }
            }   
            if (relevant.Count < 5)
            {
                return Suits.None;
            }
            Card[] relevantArr = relevant.ToArray();
            Array.Sort(relevantArr, (x, y) => y._value.CompareTo(x._value)); //decending

            int noOfCardsInARow = 0;
            int pos = 0;
            bool isAStraight = false;
            while (pos < relevantArr.Count() - 1 && !isAStraight)
            {
                if (relevantArr[pos]._value - relevantArr[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(relevantArr[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(relevantArr[pos + 1]);
                        isAStraight = true;
                    }
                    else
                    {
                        pos++;
                        _relevantCards.Clear();
                    }
                }
                else
                {
                    noOfCardsInARow = 0;
                    pos++;
                }
            }
            if (isAStraight) return flush;
            return Suits.None;
        }

        public Suits isAStraight(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int noOfCardsInARow = 0;
            int pos = 0;
            bool isAStraight = false;
            while (pos < cards.Count() - 1 && !isAStraight)
            {
                if (cards[pos]._value - cards[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(cards[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(cards[pos+1]);
                        isAStraight = true;
                    }
                    else
                    {
                        pos++;
                        _relevantCards.Clear();
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

        public Suits isAFlush(Card[] cards)
        {
            _relevantCards.Clear();
            Suits flush = Suits.None;
            int noOfClubs = 0;
            int noOfSpades = 0;
            int noOfHearts = 0;
            int noOfDiamonds = 0;
            foreach (Card c in cards)
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
            if (noOfClubs >= 5) flush = Suits.Clubs;
            if (noOfSpades >= 5) flush = Suits.Clubs;
            if (noOfHearts >= 5) flush = Suits.Clubs;
            if (noOfDiamonds >= 5) flush = Suits.Clubs;
            if (flush == Suits.None) return flush;

            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int counter = 0;
            foreach (Card c in cards)
            {
                if (c._suit == flush && counter < 5)
                {
                    _relevantCards.Add(c);
                    counter++;
                }
            }
            return flush;
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
                            return true;
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

