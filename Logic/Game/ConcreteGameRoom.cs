 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        private int _id { get; set; } //base has ID allready
        private Pot p = new Pot();
        private PlayersList _roomPlayers = new PlayersList();
        private List<Spectetor> _roomSpectetors = new List<Spectetor>();
        private GameHand _hand;
        private Player _currentPlayer;
        private Player _currentDealer;
        private List<Card> _cardsOnTable { get; set; }
        private Deck _deck;
        private Player _currentSB;
        private Player _currentBB;
        private int _roundCounter { get; set; }

        public ConcreteGameRoom(int id, int blind, Player curr, Player dealer, int turn, List<Card> cards, string name, int minMoney, int maxMoney, int gameNumber) : base(name, minMoney, maxMoney, gameNumber)
        {
            this._id = id;
            this.IsActive = true;
            this._currentPlayer = curr;
            this._currentDealer = dealer;
            this._cardsOnTable = cards;
            this._deck = new Deck();
            this._hand = new GameHand(_deck);
            this._roundCounter = 0;
        }

        public int GetDeckSize()
        {
            return _deck.NumOfCards;
        }

        public Player CurrentDealer
        {
            get { return _currentDealer; }
        }

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        public Player CurrentSb
        {
            get { return _currentSB; }
        }

        public Player CurrentBb
        {
            get { return _currentBB; }
        }

        public Pot Pot
        {
            get { return p; }
        }

        private bool addPlayerToGame(Player p)
        {
            if (this._roomPlayers.Count > 8) return false;
            else
            {
               this. _roomPlayers.Add(p);
                return true;
            }
        }

        private bool removePlayerFromGame(Player p)
        {
            //TODO
            return true;
        }

        private bool addSpectetorToGame(Spectetor s)
        { 
           this._roomSpectetors.Add(s);
            return true;      
        }
        private bool removeSpectetorFromGame(Spectetor s)
        {
            if (_roomSpectetors.Count == 0) return false;
            else _roomSpectetors.Remove(s);
            return true;
        }

        private bool setRoles()
        {
            if (this._roomPlayers.Count < 2) return false;
           else if (this._roomPlayers.Count == 2)
            {
                this._currentDealer = _roomPlayers[0];
                this._currentBB = _roomPlayers[0];
                this._currentSB = _roomPlayers[1];
                this._currentPlayer = _roomPlayers[1];
                return true;
            }
           else if (this._roomPlayers.Count > 2)
            {
                this._currentDealer = _roomPlayers[0];                
                this._currentSB = _roomPlayers[1];
                this._currentBB = _roomPlayers[2];
                this._currentPlayer = _roomPlayers[3];
                return true;
            }
            return false;
        }
        public void Fold()
        {
            throw new NotImplementedException();
        }

        public void Raise(int sum)
        {
            throw new NotImplementedException();
        }

        public void Check()
        {
            throw new NotImplementedException();
        }

        public void Call()
        {
            throw new NotImplementedException();
        }

        private Player findWinner(int sum)
        {   //TODO : byAvivG
            throw new NotImplementedException();
        }

        private void Play()
        {
            bool flag = false;
            while (!flag)
            {
                flag = setRoles();
            }

            foreach (Player p in _roomPlayers)
            {
                
            }
        }

    }
}
