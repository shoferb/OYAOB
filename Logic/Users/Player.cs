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
        private bool isActive;
        public bool inHand { get; set; }
        public int seatNumber { get; set; }
        public string name { get; set; }
        public int chipCount { get; set; }
        public int chipsCommitted { get; set; }

        public bool human { get; set; }


        public string lastAction { get; set; }
        public Hand hand = new Hand();

        public Player(int id, string name, string memberName, string password, int points, int money, String email,
            int gameId, bool isActive) : base(id, name, memberName, password, points, money, email, gameId)
        {
            this.isActive = isActive;
            this.seatNumber = seatNumber;
            this.name = name;
            this.chipsCommitted = chipsCommitted;
            this.chipCount = chipCount;
            inHand = false;

        }

        //getter setter
        public bool IsActive
        {
            get { return isActive; }

            set { isActive = value; }
        }
        public bool IsAllIn()
        {
            if (chipCount == 0 && inHand)
                return true;
            return false;
        }

        public void ClearCards()
        {
            hand.ClearCards();
        }
        public List<Card> GetHoleCards()
        {
            return hand.GetHoleCards();

        }
        public bool OutOfMoney()
        {
            if (chipCount == 0)
                return true;
            return false;
        }

        public void CommitChips(int chips)
        {
            chipCount -= chips;
            chipsCommitted += chips;
        }

        public void AddCard(Card newCard)
        {
            hand.AddCard(newCard);
        }
        public bool CanCheck(ConcreteGameRoom state)
        {
            if (state.maxCommitted == chipsCommitted)
                return true;
            return false;
        }
        public void AddHoleCards(Card newCardA, Card newCardB)
        {
            hand.AddHoleCards(newCardA, newCardB);
        }
        public void Fold()
        {
            lastAction = "fold";
            inHand = false;
        }
        public void Check()
        {
            lastAction = "check";
        }

        public void Call(int additionalChips)
        {
            lastAction = "call";
            additionalChips = Math.Min(additionalChips, chipCount); // if can't afford that many chips in a call, go all in           
            CommitChips(additionalChips);
        }

        public void Call(ConcreteGameRoom state)
        {
            Call(state.ToCall());

        }
        public void Bet(int additionalChips, ConcreteGameRoom state)
        {
            lastAction = "bet";
            additionalChips = Math.Max(additionalChips, state.bb); // have to bet at least the bb
            additionalChips = Math.Min(additionalChips, chipCount); // if can't afford that many chips in a call, go all in            
            CommitChips(additionalChips);
            state.lastRaise = additionalChips;

        }
        public void Raise(int additionalChips, int toCall, ConcreteGameRoom state)
        {
            if (toCall >= chipCount)
            { // if has less than or equal number of chips to call (ie cannot raise)
                Call(chipCount);
                state.lastRaise = chipCount;
            }
            else
            {
                lastAction = "raise";
                int totalChips = additionalChips + toCall;
                totalChips = Math.Min(totalChips, chipCount); // if can't afford that many chips to raise, go all in

                CommitChips(totalChips);
                state.lastRaise = totalChips;
            }

        }
        public void Raise(int additionalChips, ConcreteGameRoom state)
        {
            additionalChips = Math.Max(additionalChips, state.bb); // have to raise at least the bb
            additionalChips = Math.Max(additionalChips, state.lastRaise); // have to raise at least the last bet/raise
            Raise(additionalChips, state.ToCall(), state);

        }

        /* public int CompareTo(Object otherObject)
         {
             return hand.CompareTo((((Player)otherObject).hand));
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
                    Bet(state.bb * 2, state);
            }
            else
            {
                if (choice == 0)
                    Fold();
                else if (choice == 1)
                    Call(state);
                else if (choice == 2)
                    Raise(state.bb * 2, state);
            }




        }

    }
}
