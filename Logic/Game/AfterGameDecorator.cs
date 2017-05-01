using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game
{
    class AfterGameDecorator : Decorator
    {
        public AfterGameDecorator( Decorator d) : base(d)
        {
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


    }


}
