 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        private int _id { get; set; }
        private bool _isActive { get; set; }
        private Pot p = new Pot();
        private List<Player> _roomPlayers = new List<Player>(); //change to list of playres
        private List<Spectetor> _roomSpectetors = new List<Spectetor>();
        private Dictionary<Player, Card[]> _playerNcards;
        private GameHand _hand;
        private Player _currentPlayer { get; set; }
        private Player _currentDealer { get; set; }
        private List<Card> _cardsOnTable { get; set; }
        public Card [] _playerCards = new Card[2];
        private Deck _deck;
        private Player _currentSB { get; set; }
        private Player _currentBB { get; set; }
        private int _roundCounter { get; set; }

public ConcreteGameRoom(int id, int blind, Player curr, Player dealer, int turn, List<Card> cards, string name, int minMoney, int maxMoney, int gameNumber) : base(name, minMoney, maxMoney, gameNumber)
        {
            this._id = id;
            this._isActive = true;
            this._currentPlayer = curr;
            this._currentDealer = dealer;
            this._cardsOnTable = cards;
            this._deck = new Deck();
            this._hand = new GameHand(_deck);
            this._roundCounter = 0;
            this._playerNcards =  new Dictionary<Player, Card[]>();

        }

        private bool AddPlayerToGame(Player p)
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
        //TODO not be according to the indexes
        private bool SetRoles()
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
            //check the position of the dealer and take the next one
            return false;
        }
        private void Fold()
        {
            throw new NotImplementedException();
        }

        private void Raise(int sum)
        {
            throw new NotImplementedException();
        }

        private void Check()
        {
            throw new NotImplementedException();
        }

        private void Call()
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
                flag = SetRoles();
            }

            foreach (Player p in _roomPlayers)
            {
                
            }
        }

    }
}
