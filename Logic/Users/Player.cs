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
        public int _enteredChip { get; set; }
        public int _totalChips { get; set; }

        public string _lastAction { get; set; }
        public Hand _hand;

        public Player(int enteredChip, int totalChipsComitted, int id, string name, string memberName, string password, int points, int money, String email,
            int gameId) : base(id, name, memberName, password, points, money, email, gameId)
        {
            //TODO: Orellie, I deleted the "IsActive" field and refactor isHand to it. it's not
            //TODO: should pass in the constructor, I'm changing it from the game.
            //TODO: pls delete the money field - bc we have the cheaps already, and the gameID
            this._enteredChip = enteredChip;
            this._totalChips = totalChipsComitted;
           this.name = name;
            this._totalChips = _totalChips;
            this._enteredChip = enteredChip;
            _isActive = false;
            _hand = new Hand();

        }

        //getter setter
        
        public bool IsAllIn()
        {
            if (_enteredChip == 0 && _isActive)
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
            if (_enteredChip == 0)
                return true;
            return false;
        }

       

        public void AddCard(Card newCard)
        {
            _hand.AddPublicCardToPlayer(newCard);
        }
        public bool CanCheck(ConcreteGameRoom state)
        {
            if (state._maxCommitted == _totalChips)
                return true;
            return false;
        }
        public void AddHoleCards(Card newCardA, Card newCardB)
        {
            _hand.Add2Cards(newCardA, newCardB);
        }
        

       //for test propuse only !!!
        public void Play(ConcreteGameRoom state)
        {
        }

        public void CommitChips(int chips)
        {
            this._enteredChip -= chips;
            this._totalChips += chips;
        }
        //TODO: orellie
        public void Win(int amount)
        {
           
        }
        //TODO: orellie
        public static void Lose()
        {
            
        }
        //TODO : orellie 
        internal int Play(int _sb)
        {
            return 0;
        }
    }
}
