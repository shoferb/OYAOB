using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;

namespace TexasHoldem.Logic.Users
{
    public class Player : Spectetor
    {

        public bool isPlayerActive { get; set; }
        public string name { get; set; }
        public int _totalChip { get; set; }
        public int _gameChip { get; set; }

        public string _lastAction { get; set; }

        

        public Hand _hand;
        public enum playerMoves { Call, Check, Bet, Fold, Raise }

        private int _winNum;
        private int _loseNum;

        public Player(int totalChip, int gameChipComitted, int id, string name, string memberName, string password, int points, int money, String email,
            int roomId) : base(id, name, memberName, password, points, money, email, roomId)
        {
            
            this._totalChip = totalChip;
            this._gameChip = gameChipComitted;
            this.name = name;
            this._gameChip = _gameChip;
            this._totalChip = totalChip;
            isPlayerActive = false;
            _hand = new Hand();
            this._winNum = 0;
            this._loseNum = 0;

        }

        //getter setter

        public bool IsAllIn()
        {
            if (_totalChip == 0 && isPlayerActive)
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
            if (_totalChip == 0)
                return true;
            return false;
        }



        public void AddCard(Card newCard)
        {
            _hand.AddPublicCardToPlayer(newCard);
        }
        public bool CanCheck(ConcreteGameRoom state)
        {
            if (state._maxCommitted == _gameChip)
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

        public int Play(int amount, ConcreteGameRoom.HandStep h)
        {
            List<playerMoves> validMoves;
            if (h == ConcreteGameRoom.HandStep.PreFlop) //first round - can call/fold/raise
            {

            }
            else if ((h == ConcreteGameRoom.HandStep.Flop) || (h == ConcreteGameRoom.HandStep.Turn) || (h == ConcreteGameRoom.HandStep.River)) //round 2 to 4 - can check/bet/fold
            {

            }

            return 0;
        }

        public void CommitChips(int chips)
        {
            this._totalChip -= chips;
            this._gameChip += chips;
        }


        //get amout won, inc winCounter + update point + check if the user is now the highest player
        public bool Win(int amount)
        {
            bool toReturn;
            try
            {
                WinNum = WinNum + 1;
                int newPoint = GetNewPoint();
                Points = newPoint;
                SystemControl sc = new SystemControl();
                //int highestRank = sc.
                //todo - need to chck if user is noe the hugest rank
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }
        //todo - yarden - why static?? remove for now
        public  bool Lose()
        {
            bool toReturn;
            try
            {
                LoseNum = LoseNum + 1;
                int newPoint = GetNewPoint();
                Points = newPoint;
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
            int calc = (int)(Money / 100);
            int newPoint = (20 * ((5 * WinNum) - LoseNum)) + calc;
            return newPoint;
        }
        public int WinNum
        {
            get
            {
                return _winNum;
            }

            set
            {
                _winNum = value;
            }
        }

        public int LoseNum
        {
            get
            {
                return _loseNum;
            }

            set
            {
                _loseNum = value;
            }
        }
    }

}
