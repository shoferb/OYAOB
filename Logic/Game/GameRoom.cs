using System;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GameRoom
    {
 
        private string _name { set; get; }
        private int _smallBlind { set; get; }
        private int _bigBlind { set; get; }
        private int _minEnterMoney { set; get; }
        private int _maxEnterMoney { set; get; }
        private int _gameNumber { set; get; }

        protected GameRoom(string name, int sb, int bb, int minMoney, int maxMoney, int gameNumber)
        {
            this._name = name;
            this._smallBlind = sb;
            this._bigBlind = bb;
            this._minEnterMoney = minMoney;
            this._maxEnterMoney = maxMoney;
            this._gameNumber = gameNumber;
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
