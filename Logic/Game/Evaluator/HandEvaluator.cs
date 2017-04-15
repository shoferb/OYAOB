using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game.Evaluator
{
    public class HandEvaluator
    {
        public HandRank _rank;
        public List<Card> _relevantCards = new List<Card>();

        public HandEvaluator(Player player)
        {
            _player = player;
        }

        public Player _player { get; set;}

        public void DetermineHandRank(Card[] cards)
        {
            FixAceTo14(cards);
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
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
                _relevantCards.Clear();
                FixAceTo14(cards);
                Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
                int i = 0;
                while (i < cards.Count() && _relevantCards.Count < 5)
                {
                    if (!_relevantCards.Contains(cards[i]))
                    {
                        _relevantCards.Add(cards[i]);
                    }
                    i++;
                }
            }
            FixAceTo1(cards);
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
            FixAceTo14(cards);
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
                FixAceTo1(cards);
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
                        FixAceTo1(cards);
                        return flush;
                    }
                }
                else
                {
                    _relevantCards.Clear();
                    noOfCardsInARow = 0;
                }
                pos++;
            }
            FixAceTo1(cards);
            _relevantCards.Clear();
            Array.Sort(relevantArr, (x, y) => y._value.CompareTo(x._value)); //decending
            noOfCardsInARow = 0;
            pos = 0;
            while (pos < relevantArr.Count() - 1)
            {
                if (relevantArr[pos]._value - relevantArr[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(relevantArr[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(relevantArr[pos + 1]);
                        FixAceTo1(cards);
                        return relevantArr[pos + 1]._suit;
                    }
                }
                else
                {
                    _relevantCards.Clear();
                    noOfCardsInARow = 0;
                }
                pos++;
            }
            return Suits.None;
        }

        public Suits IsAStraight(Card[] cards)
        {
            _relevantCards.Clear();
            FixAceTo14(cards);
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
                        FixAceTo1(cards);
                        return cards[pos+1]._suit;
                    }
                }
                else
                {
                    _relevantCards.Clear();
                    noOfCardsInARow = 0;
                }
                pos++;
            }
            FixAceTo1(cards);
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            noOfCardsInARow = 0;
            pos = 0;
            while (pos < cards.Count() - 1)
            {
                if (cards[pos]._value - cards[pos + 1]._value == 1)
                {
                    noOfCardsInARow++;
                    _relevantCards.Add(cards[pos]);
                    if (noOfCardsInARow == 4)
                    {
                        _relevantCards.Add(cards[pos + 1]);
                        FixAceTo1(cards);
                        return cards[pos + 1]._suit;
                    }
                }
                else
                {
                    _relevantCards.Clear();
                    noOfCardsInARow = 0;
                }
                pos++;
            }
            return Suits.None;
        }

        public Suits IsAFlush(Card[] cards)
        {
            _relevantCards.Clear();
            FixAceTo14(cards);
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
            if (flush == Suits.None)
            {
                FixAceTo1(cards);
                return flush;
            }

            int counter = 0;
            foreach (Card c in cards)
            {
                if (c._suit == flush && counter < 5)
                {
                    _relevantCards.Add(c);
                    counter++;
                }
            }
            FixAceTo1(cards);
            return flush;
        }

        public bool IsThreeOfAKind(Card[] cards)
        {
            bool found = false;
            FixAceTo14(cards);
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i < cards.Count() - 2 && !found)
            {
                if (cards[i]._value == cards[i + 1]._value && cards[i + 1]._value == cards[i + 2]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    _relevantCards.Add(cards[i + 2]);
                    found = true;
                }
                i++;
            }
            if (!found)
            {
                FixAceTo1(cards);
                return false;
            }
            return GetBestHand(cards);
        }

        public bool IsTwoPair(Card[] cards)
        {
            FixAceTo14(cards);
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
            if (pairs < 2)
            {
                FixAceTo1(cards);
                return false;
            }
            return GetBestHand(cards);
        }

        public bool IsPair(Card[] cards)
        {
            FixAceTo14(cards);
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            bool found = false;
            while (i < cards.Count() - 1 && !found)
            {
                if (cards[i]._value == cards[i + 1]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    found = true;
                }
                i++;
            }
            if (!found)
            {
                FixAceTo1(cards);
                return false;
            }
            return GetBestHand(cards);
        }

        public bool IsAFullHouse(Card[] cards)
        {
            _relevantCards.Clear();
            bool found = false;
            FixAceTo14(cards);
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i < cards.Count() - 2 && !found)
            {
                if (cards[i]._value == cards[i + 1]._value && cards[i + 1]._value == cards[i + 2]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    _relevantCards.Add(cards[i + 2]);
                    found = true;
                }
                i++;
            }
            if (!found)
            {
                FixAceTo1(cards);
                return false;
            }
            Card[] relevantArr = cards.Where(x => !_relevantCards.Contains(x)).ToArray(); //intersect
            Array.Sort(relevantArr, (x, y) => y._value.CompareTo(x._value)); //decending
            i = 0;
            while (i < relevantArr.Count() - 1)
            {
                if (relevantArr[i]._value == relevantArr[i + 1]._value)
                {
                    _relevantCards.Add(relevantArr[i]);
                    _relevantCards.Add(relevantArr[i + 1]);
                    return true;
                }
                i++;
            }
            return false;
        }

        public bool IsAFourOfAKind(Card[] cards)
        {
            FixAceTo14(cards);
            _relevantCards.Clear();
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            bool found = false;
            while (i < cards.Count() - 3)
            {
                if (cards[i]._value == cards[i + 1]._value &&
                    cards[i + 1]._value == cards[i + 2]._value &&
                    cards[i + 2]._value == cards[i + 3]._value)
                {
                    _relevantCards.Add(cards[i]);
                    _relevantCards.Add(cards[i + 1]);
                    _relevantCards.Add(cards[i + 2]);
                    _relevantCards.Add(cards[i + 3]);
                    found = true;
                }
                i++;
            }
            if (!found)
            {
                FixAceTo1(cards);
                return false;
            }
            return GetBestHand(cards);
        }

        public bool IsRoyalFlush(Card[] cards)
        {
            bool ace = false;
            bool two = false;
            if (IsAStraightFlush(cards))
            {
                foreach (Card c in _relevantCards)
                {
                    if (c._value == (1)) //ACE
                    {
                        ace = true;
                    }
                    if (c._value == (2)) //two
                    {
                        two = true;
                    }
                }
            }
            return (ace && !two); //we got flush straight with ace and with no two (1,2,3,4,5)
        }

        private void FixAceTo14(Card[] cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == 1) //ACE
                {
                    c._value = 14;
                }
            }
        }

        private void FixAceTo1(Card[] cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == (14)) //ACE
                {
                    c._value = 1;
                }
            }
            foreach (Card c in _relevantCards)
            {
                if (c._value == (14)) //ACE
                {
                    c._value = 1;
                }
            }

        }

        private bool GetBestHand(Card[] cards)
        {
            Array.Sort(cards, (x, y) => y._value.CompareTo(x._value)); //decending
            int i = 0;
            while (i < cards.Count() && _relevantCards.Count < 5)
            {
                if (!_relevantCards.Contains(cards[i]))
                {
                    _relevantCards.Add(cards[i]);
                }
                i++;
            }
            FixAceTo1(cards);
            return true;
        }

    }
}

