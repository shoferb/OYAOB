using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GameRoom
    {
 
        private string _name { set; get; }       
        private int _minEnterMoney { set; get; }
        private int _maxEnterMoney { set; get; }

        private int _gameNumber { set; get; }
        private List<Player> _roomPlayers = new List<Player>();
        private List<Spectetor> _roomSpectetors = new List<Spectetor>();

        protected GameRoom(string name, int minMoney, int maxMoney, int gameNumber )
        {
            this._name = name;
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

        private bool Play()
        {
            throw new NotImplementedException();
        }

    }
}
