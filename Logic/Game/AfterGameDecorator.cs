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
        public override bool CanStartTheGame(int numOfPlayers)
        {
            return false;
        }

        public override bool CanRaise()
        {
            return false;
        }

        public override bool CanCheck()
        {
            return false;
        }

        public override bool CanFold()
        {
            return false;
        }

        public override bool CanSpectatble()
        {
            return false;
        }

        public override int GetMinBetInRoom()
        {
            throw new NotImplementedException();
        }

        public override int GetEnterPayingMoney()
        {
            throw new NotImplementedException();
        }

        public override int GetStartingChip()
        {
            throw new NotImplementedException();
        }

        public override bool CanAddMorePlayer(int currNumOfPlayers)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxAllowedRaise(int bb, int maxCommited, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public override int GetMinAllowedRaise(int bb, int maxCommited, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public override bool CanBeSpectatble()
        {
            throw new NotImplementedException();
        }
    }


}
