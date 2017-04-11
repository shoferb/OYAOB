using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game.Evaluator;

namespace TexasHoldem.Logic.Game
{
    public class Hand : IComparable
    {
        Card _holeOne;
        Card _holeTwo;
        List<Card> _publicCards = new List<Card>();
        public HandEvaluator _bestHand;
        public Hand()
        {
            this._holeOne = null;
            this._holeTwo = null;
        }

        public Tuple<HandRank, List<Card>> findBestHand()
        {
            //TODO AvivG
            return null;
        }

        public List<Card> GetHoleCards()
        {
            List<Card> holeCards = new List<Card>()
                {_holeOne,
                 _holeTwo,
                };
            return holeCards;

        }

        public void ClearCards()
        {
            _holeOne = null;
            _holeTwo = null;
            _publicCards.Clear();

        }
        public void AddCard(Card newCard)
        {
            if (_publicCards.Count < 5)
                _publicCards.Add(newCard);
            else
                throw new System.ArgumentException("Too many cards.", "newCard");
        }
        public void AddHoleCards(Card newCardA, Card newCardB)
        {
            if (_holeOne == null && _holeTwo == null)
            {
                _holeOne = newCardA;
                _holeTwo = newCardB;
            }
            else
                throw new System.ArgumentException("Hole cards already determined.", "newCard");
        }

        public int CompareTo(Object otherObject)
        {
            //TODO: myabe I'll delete it later - maybe AvivG will use it.
            return 0;

        }
    }
}
