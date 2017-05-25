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
        public int RoundChipBet { get; set; } // the number of chips player use in this round
        public bool PlayedAnActionInTheRound { get; set; }
        public Card _firstCard { get; set; } 
        public Card _secondCard { get; set; }
        public List<Card> _publicCards = new List<Card>();

        //new Fields
        public IUser user { get; set; }
        public int roomId { get; set; }

        public Player(IUser User, int totalChip, int RoomId)
        {
            this.user = User;
            this.roomId = RoomId;
            this.TotalChip = totalChip;
            this.RoundChipBet = 0;     
            this.TotalChip = totalChip;
            isPlayerActive = false;
            this._firstCard = null;
            this._secondCard = null;
            this.isPlayerActive = false;
            this.PlayedAnActionInTheRound = false;
        }
        
        public void InitForNewGame()
        {
            isPlayerActive = true;
            RoundChipBet = 0;
            PlayedAnActionInTheRound = false;
            _firstCard = null;
            _secondCard = null;
        }

        public void InitForNewRound()
        {
            RoundChipBet = 0;
            PlayedAnActionInTheRound = false;
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

        public Card getFirstCard()
        {
            return this._firstCard;
        }

        public Card getSeconedCard()
        {
            return this._secondCard;
        }


        public void InitPayInRound()
        {
            RoundChipBet = 0;
            PlayedAnActionInTheRound = false;
        }

        public void AddPublicCardToPlayer(Card newCard)
        {
            if (_publicCards.Count < 5)
                _publicCards.Add(newCard);
            else
                throw new System.ArgumentException("Too many cards.", "newCard");
        }



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
            int newPoint = (20 * ((5 * user.WinNum))) + calc;
            return newPoint;
        }
       
    }

}
