using System;
using System.Collections.Generic;
using TexasHoldem.Logic.User;

namespace TexasHoldem.Logic.Game
{
    class ConcreteGameRoom : GameRoom
    {
        private int _id { get; set; }
        private bool _isActive { get; set; }
        private int _blind { get; set; }
        private int _potSize { get; set; }
        private Player _currentPlayer { get; set; }
        private Player _currentDealer { get; set; }
        private int _highBetInTurn { get; set; }
        private List<Card> _cardsOnTable { get; set; }
         
        public ConcreteGameRoom(int id, bool isActive, int blind, int potSize, Player curr, Player dealer, int turn, List<Card> cards,  string name, int sb, int bb, int minMoney, int maxMoney, int gameNumber) : base(name, sb, bb, minMoney, maxMoney, gameNumber)
        {
            this._id = id;
            this._isActive = isActive;
            this._potSize = potSize;
            this._currentPlayer = curr;
            this._currentDealer = dealer;
            this._highBetInTurn = turn;
            this._cardsOnTable = cards;
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
        {
            throw new NotImplementedException();
        }



    }
}
