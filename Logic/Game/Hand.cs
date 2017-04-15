using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game.Evaluator;

namespace TexasHoldem.Logic.Game
{
    public class Hand 
    {
        public Card _firstCard;
        public Card _seconedCard;
        public List<Card> _publicCards = new List<Card>();
        public Hand()
        {
            this._firstCard = null;
            this._seconedCard = null;
        }

        public List<Card> GetCards()
        {
            List<Card> holeCards = new List<Card>()
                {_firstCard,
                 _seconedCard,
                };
            return holeCards;

        }

        public void ClearCards()
        {
            _firstCard = null;
            _seconedCard = null;
            _publicCards.Clear();

        }
        public void AddPublicCardToPlayer(Card newCard)
        {
            if (_publicCards.Count < 5)
                _publicCards.Add(newCard);
            else
                throw new System.ArgumentException("Too many cards.", "newCard");
        }
        public void Add2Cards(Card newCardA, Card newCardB)
        {
            if (_firstCard == null && _seconedCard == null)
            {
                _firstCard = newCardA;
                _seconedCard = newCardB;
            }
            else
                throw new System.ArgumentException("Player cards already determined.", "newCard");
        }

        }
}
