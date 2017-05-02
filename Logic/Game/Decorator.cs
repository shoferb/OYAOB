using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic
{
    public abstract class Decorator
    {
       internal Decorator NextDecorator;

        public Decorator(Decorator d)
        {
            this.NextDecorator = d;
        }

        public bool CanStartTheGame(int numOfPlayers)
        {
            return false;
        }

        public bool CanRaise()
        {
            return false;
        }

        public bool CanCheck()
        {
            return false;
        }

        public bool CanFold()
        {
            return false;
        }

       public bool CanSpectatble()
        {
            return false;
        }

        public int GetMinBetInRoom()
        {
            return 0;
        }
    }
}
