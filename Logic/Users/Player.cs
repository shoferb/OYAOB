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
      

        public Hand _hand;

        //new Fields
        public IUser user { get; }
        public int roomId { get; }

        public Player(IUser User, int totalChip, int roundChipBetComitted, int RoomId)
        {
            this.user = User;
            this.roomId = RoomId;
            this.TotalChip = totalChip;
            this.RoundChipBet = roundChipBetComitted;
          
            this.RoundChipBet = RoundChipBet;
            this.TotalChip = totalChip;
            isPlayerActive = false;
            _hand = new Hand();

            this._payInThisRound = 0;
            this.moveForTest = 0;
        }
        
        public Player(int totalChip, int roundChipBetComitted, int id, string name, string memberName, string password, int points, int money, String email,
            int roomId) : base(id, name, memberName, password, points, money, email, roomId)
        {
            
            this.TotalChip = totalChip;
            this.RoundChipBet = roundChipBetComitted;
            this.name = name;
            this.RoundChipBet = RoundChipBet;
            this.TotalChip = totalChip;
            isPlayerActive = false;
            _hand = new Hand();
            
            this._payInThisRound = 0;
            this.moveForTest = 0;
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
            if (TotalChip == 0)
                return true;
            return false;
        }


        public void InitPayInRound()
        {
            _payInThisRound = 0;
        }

        public void AddCard(Card newCard)
        {
            _hand.AddPublicCardToPlayer(newCard);
        }
        public bool CanCheck(GameRoom state)
        {
            if (state.MaxCommitted == RoundChipBet)
                return true;
            return false;
        }
        public void AddHoleCards(Card newCardA, Card newCardB)
        {
            _hand.Add2Cards(newCardA, newCardB);
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
            this.TotalChip -= chips;
            this.RoundChipBet += chips;
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
        




        public int GetNewPoint()
        {
            int calc = (int)(user.Money() / 100);
            int newPoint = (20 * ((5 * user.WinNum()))) + calc;
            return newPoint;
        }
       
    }

}
