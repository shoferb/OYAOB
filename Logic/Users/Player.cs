using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic.Users
{
    public class Player : Spectetor
    {
        
        public bool _isActive { get; set; }
        public string name { get; set; }
        public int _chipCount { get; set; }
        public int _chipsCommitted { get; set; }

        public string _lastAction { get; set; }
        public Hand _hand;

        public Player(int chipCount, int chipsComitted, int id, string name, string memberName, string password, int points, int money, String email,
            int gameId) : base(id, name, memberName, password, points, money, email, gameId)
        {
            //TODO: Orellie, I deleted the "IsActive" field and refactor isHand to it. it's not
            //TODO: should pass in the constructor, I'm changing it from the game.
            //TODO: pls delete the money field - bc we have the cheaps already, and the gameID
            this._chipCount = chipCount;
            this._chipsCommitted = chipsComitted;
           this.name = name;
            this._chipsCommitted = _chipsCommitted;
            this._chipCount = chipCount;
            _isActive = false;
            _hand = new Hand();

        }

        //getter setter
        
        public bool IsAllIn()
        {
            if (_chipCount == 0 && _isActive)
                return true;
            return false;
        }

        public void ClearCards()
        {
            _hand.ClearCards();
        }
        public List<Card> GetHoleCards()
        {
            return _hand.GetCards();

        }
        public bool OutOfMoney()
        {
            if (_chipCount == 0)
                return true;
            return false;
        }

        public void CommitChips(int chips)
        {
            _chipCount -= chips;
            _chipsCommitted += chips;
        }

        public void AddCard(Card newCard)
        {
            _hand.AddPublicCardToPlayer(newCard);
        }
        public bool CanCheck(ConcreteGameRoom state)
        {
            if (state._maxCommitted == _chipsCommitted)
                return true;
            return false;
        }
        public void AddHoleCards(Card newCardA, Card newCardB)
        {
            _hand.Add2Cards(newCardA, newCardB);
        }
        public void Fold()
        {
            _lastAction = "fold";
            _isActive = false;
        }
        public void Check()
        {
            _lastAction = "check";
        }

        public void Call(int additionalChips)
        {
            _lastAction = "call";
            additionalChips = Math.Min(additionalChips, _chipCount); // if can't afford that many chips in a call, go all in           
            CommitChips(additionalChips);
        }

        public void Call(ConcreteGameRoom state)
        {
            Call(state.ToCall());

        }
        public void Bet(int additionalChips, ConcreteGameRoom state)
        {
            _lastAction = "bet";
            additionalChips = Math.Max(additionalChips, state._bb); // have to bet at least the _bb
            additionalChips = Math.Min(additionalChips, _chipCount); // if can't afford that many chips in a call, go all in            
            CommitChips(additionalChips);
            state._sb = additionalChips;

        }
        public void Raise(int additionalChips, int toCall, ConcreteGameRoom state)
        {
            if (toCall >= _chipCount)
            { // if has less than or equal number of chips to call (ie cannot raise)
                Call(_chipCount);
                state._sb = _chipCount;
            }
            else
            {
                _lastAction = "raise";
                int totalChips = additionalChips + toCall;
                totalChips = Math.Min(totalChips, _chipCount); // if can't afford that many chips to raise, go all in

                CommitChips(totalChips);
                state._sb = totalChips;
            }

        }
        public void Raise(int additionalChips, ConcreteGameRoom state)
        {
            additionalChips = Math.Max(additionalChips, state._bb); // have to raise at least the _bb
            additionalChips = Math.Max(additionalChips, state._sb); // have to raise at least the last bet/raise
            Raise(additionalChips, state.ToCall(), state);

        }

        /* public int CompareTo(Object otherObject)
         {
             return _hand.CompareTo((((Player)otherObject)._hand));
         }*/

            //for test propuse only !!!
        public void Play(ConcreteGameRoom state)
        {
            bool canCheck = this.CanCheck(state);
            Random random = new Random();
            int choice;
            if (canCheck)
                choice = random.Next(1, 3);
            else
                choice = random.Next(0, 3);

            if (canCheck)
            {
                if (choice == 1)
                    Check();
                else if (choice == 2)
                    Bet(state._bb * 2, state);
            }
            else
            {
                if (choice == 0)
                    Fold();
                else if (choice == 1)
                    Call(state);
                else if (choice == 2)
                    Raise(state._bb * 2, state);
            }




        }
        //TODO: orellie
         public void Win(int amount)
        {
           
        }
        //TODO: orellie
        public static void Lose()
        {
            
        }
    }
}
