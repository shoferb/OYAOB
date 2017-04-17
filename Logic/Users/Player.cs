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
        public int _totalChip { get; set; }
        public int _gameChip { get; set; }

        public string _lastAction { get; set; }
        public Hand _hand;
        public enum playerMoves { Call, Check, Bet, Fold, Raise }
        public Player(int totalChip, int gameChipComitted, int id, string name, string memberName, string password, int points, int money, String email,
            int gameId) : base(id, name, memberName, password, points, money, email, gameId)
        {
            //TODO: Orellie, I deleted the "IsActive" field and refactor isHand to it. it's not
            //TODO: should pass in the constructor, I'm changing it from the game.
            //TODO: pls delete the money field - bc we have the cheaps already, and the gameID
            this._totalChip = totalChip;
            this._gameChip = gameChipComitted;
            this.name = name;
            this._gameChip = _gameChip;
            this._totalChip = totalChip;
            _isActive = false;
            _hand = new Hand();

        }

        //getter setter

        public bool IsAllIn()
        {
            if (_totalChip == 0 && _isActive)
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
