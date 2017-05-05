using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;

namespace TexasHoldem.Logic.Users
{
    public class Player 
    {

        public bool isPlayerActive { get; set; }
        public string name { get; set; }
        public int TotalChip { get; set; }
        public int RoundChipBet { get; set; } // the number of chips player have each round
        public bool PlayedAnActionInTheRound { get; set; }
        public bool _isInRoom { get; set; }
        public int _payInThisRound { get; set; } //כמות שבזבז בסיבוב 
        public int moveForTest { get; set; } //-1 fold, 0 check,raise / call / bet by how mutch
        public Card _firstCard;
        public Card _secondCard;
        public List<Card> _publicCards = new List<Card>();

        //new Fields
        public IUser user { get; set; }
        public int roomId { get; set; }

        public Player(IUser User, int totalChip, int roundChipBetComitted, int RoomId)
        {
            this.user = User;
            this.roomId = RoomId;
            this.TotalChip = totalChip;
            this.RoundChipBet = roundChipBetComitted;
       
            this.RoundChipBet = RoundChipBet;
            this.TotalChip = totalChip;
            isPlayerActive = false;
            this._firstCard = null;
            this._secondCard = null;
            this._payInThisRound = 0;
            this.moveForTest = 0;
            this.isPlayerActive = false;
            this.PlayedAnActionInTheRound = false;
        }
        
       //getter setter

        public bool IsAllIn()
        {
            if (TotalChip == 0 && isPlayerActive)
            {
                return true;
            }
            return false;
        }

       
       public bool OutOfMoney()
        {
            if (TotalChip == 0)
                return true;
            return false;
        }


        public void InitPayInRound()
        {
            _payInThisRound = 0;
        }

        public bool CanCheck(GameRoom state)
        {
            if (state.MaxCommitted == RoundChipBet)
                return true;
            return false;
        }
        public void AddPublicCardToPlayer(Card newCard)
        {
            if (_publicCards.Count < 5)
                _publicCards.Add(newCard);
            else
                throw new System.ArgumentException("Too many cards.", "newCard");
        }


        //for test propuse only !!!'

        /// return -1 if player select fold
        /// return -2 if player selsct exit
        /// return positive number bigger than 0 for call / raise 
        /// return 0 for check
        /*
        public int Play(int amount, GameRoom.HandStep h)
        {
            List<playerMoves> validMoves;
            if (h == GameRoom.HandStep.PreFlop) //first round - can call/fold/raise
            {

            }
            else if ((h == GameRoom.HandStep.Flop) || (h == GameRoom.HandStep.Turn) || (h == GameRoom.HandStep.River)) //round 2 to 4 - can check/bet/fold
            {

            }

            return 0;
        }*/

        public void CommitChips(int chips)
        {
            TotalChip -= chips;
            RoundChipBet += chips;
        }


        //get amout won, inc winCounter + update point + check if the user is now the highest player
        public bool Win(int amount)
        {
            bool toReturn;
            try
            {
                user.IncWinNum();
                TotalChip += amount;
                int newPoint = GetNewPoint();
                user.EditUserPoints(newPoint) ;
             //   SystemControl sc = SystemControl.SystemControlInstance;
               /* int highestRank = GameCenter.Instance.HigherRank.Points;
                if (this.Points > highestRank)
                {
                    GameCenter.Instance.HigherRank.IsHigherRank = false;
                    GameCenter.Instance.HigherRank = SystemControl.SystemControlInstance.GetUserWithId(this.Id);
                    this.IsHigherRank = true;
                }*/
                
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public List<Card> GetCards()
        {
            List<Card> holeCards = new List<Card>()
                {_firstCard,
                 _secondCard,
                };
            return holeCards;

        }

        public void ClearCards()
        {
            _firstCard = null;
            _secondCard = null;
            _publicCards.Clear();

        }

        public void Add2Cards(Card newCardA, Card newCardB)
        {
            if (_firstCard == null && _secondCard == null)
            {
                _firstCard = newCardA;
                _secondCard = newCardB;
            }
            else
                throw new System.ArgumentException("Player cards already determined.", "newCard");
        }



        public int GetNewPoint()
        {
            int calc = (int)(user.Money() / 100);
            int newPoint = (20 * ((5 * user.WinNum()))) + calc;
            return newPoint;
        }
       
    }

}
