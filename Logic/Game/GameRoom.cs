using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInfra
{
    abstract class GameRoom
    {
        private string _name { set; get; }
        private int _smallBlind { set; get; }
        private int _bigBlind { set; get; }
        private int _minEnterMoney { set; get; }
        private int _maxEnterMoney { set; get; }

        public GameRoom(string name, int sb, int bb, int minMoney, int maxMoney)
        {
            this._name = name;
            this._smallBlind = sb;
            this._bigBlind = bb;
            this._minEnterMoney = minMoney;
            this._maxEnterMoney = maxMoney;
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
        //TODO: change to return USER type and not void.
        private void findWinner(int sum)
        {
            throw new NotImplementedException();
        }

    }
}
