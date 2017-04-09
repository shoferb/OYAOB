using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GamePrefDecorator : GameRoom
    {
        protected GamePrefDecorator(string name, int minMoney, int maxMoney, int gameNumber) : base(name, minMoney, maxMoney, gameNumber)
        {
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
