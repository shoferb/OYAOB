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

        public void DetermineHandRank(Card[] cards)
        {
            Array.Sort(cards, (x, y) => x._value.CompareTo(y._value));
            if (IsRoyalFlush(cards))
            {
                _rank = HandRank.ROYAL_FLUSH;
            }
            else if (IsAStraightFlush(cards))
            {
                _rank = HandRank.STRAIGHT_FLUSH;

            }
            else if (IsAFourOfAKind(cards))
            {
                _rank = HandRank.FOUR_OF_A_KIND;
            }
            else if (IsAFullHouse(cards))
            {
                _rank = HandRank.FULL_HOUSE;
            }
            else if (IsAFlush(cards) != Suits.None)
            {
                _rank = HandRank.FLUSH;
            }
            else if (IsAStraight(cards) != Suits.None)
            {
                _rank = HandRank.STRAIGHT;
            }
            else if (IsThreeOfAKind(cards))
            {
                _rank = HandRank.THREE_OF_A_KIND;
            }
            else if (IsTwoPair(cards))
            {
                _rank = HandRank.TWO_PAIR;
            }
            else if (IsPair(cards))
            {
                _rank = HandRank.PAIR;
            }
            else
            {
                _rank = HandRank.HIGH_CARD;
            }

        }

        public bool IsAStraightFlush(Card[] cards)
        {     
            _relevantCards.Clear();
            Suits flush = IsAFlush(cards);
            if (flush == Suits.None) return false;
            Suits straight = ShapeOfStraight(cards, flush);
            if (!(flush == straight)) return false;
            return true;
         }

        public Suits ShapeOfStraight(Card[] cards, Suits flush)
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
            while (pos < relevantArr.Count() - 1 )
            {
                if (relevantArr[pos]._value - relevantArr[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(relevantArr[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(relevantArr[pos + 1]);
                        return flush;
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
            return Suits.None;
        }

        public Suits IsAStraight(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int noOfCardsInARow = 0;
            int pos = 0;
            while (pos < cards.Count() - 1 )
            {
                if (cards[pos]._value - cards[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(cards[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(cards[pos+1]);
                        return cards[pos+1]._suit;
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
            return Suits.None;
        }

        public Suits IsAFlush(Card[] cards)
        {
            _relevantCards.Clear();
            Suits flush = Suits.None;
            int noOfClubs = 0;
            int noOfSpades = 0;
            int noOfHearts = 0;
            int noOfDiamonds = 0;
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending

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
            if (noOfSpades >= 5) flush = Suits.Spades;
            if (noOfHearts >= 5) flush = Suits.Hearts;
            if (noOfDiamonds >= 5) flush = Suits.Diamonds;
            if (flush == Suits.None) return flush;

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

        private bool IsThreeOfAKind(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i <= cards.Count() - 2 )
            {
                if (cards[i]._value == cards[i+1]._value && cards[i + 1]._value == cards[i+2]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i+1]);
                    _relevantCards.Add(cards[i+2]);
                    return true;
                }
                i++;
            }
            return false;
        }

        private bool IsTwoPair(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int pairs = 0;
            int i = 0;
            while (i < cards.Count()-1 && pairs<2)
            {
                if (cards[i]._value == cards[i + 1]._value)
                {
                    pairs++;
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i+1]);
                }
                i++;
            }
            return (pairs == 2);
        }

        private bool IsPair(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i < cards.Count() - 1 )
            {
                if (cards[i]._value == cards[i + 1]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    return true;
                }
                i++;
            }
            return false;
        }

        private bool IsAFullHouse(Card[] cards)
        {
            _relevantCards.Clear();
            if (!IsThreeOfAKind(cards))
            {
                return false;
            }
            Card[] relevantArr = cards.Where(x => !_relevantCards.Contains(x)).ToArray(); //intersect
            Array.Sort(relevantArr, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i < relevantArr.Count() - 1)
            {
                if (relevantArr[i]._value == relevantArr[i + 1]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    return true;
                }
                i++;
            }
            return false;
        }

        public bool IsAFourOfAKind(Card[] cards)
        {
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i <= cards.Count() - 3)
            {
                if (cards[i]._value == cards[i + 1]._value &&
                    cards[i + 1]._value == cards[i + 2]._value &&
                    cards[i + 2]._value == cards[i + 3]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    _relevantCards.Add(cards[i + 2]);
                    _relevantCards.Add(cards[i + 3]);
                    return true;
                }
                i++;
            }
            return false;
        }

        public bool IsRoyalFlush(Card[] cards)
        {
            if (IsAStraightFlush(cards))
            {
                foreach (Card c in _relevantCards)
                {
                    if (c._value == (14)) //ACE
                    {
                        return true;
                    }
                } 
            }
            return false;
        }
    }
}

