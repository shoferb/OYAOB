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

        public abstract bool CanStartTheGame(int numOfPlayers);
        public abstract bool CanRaise();
        public abstract bool CanCheck();
        public abstract bool CanFold();
        public abstract bool CanSpectatble();
        public abstract int GetMinBetInRoom();
        public abstract int GetEnterPayingMoney();
        public abstract int GetStartingChip();
        public abstract bool CanAddMorePlayer(int currNumOfPlayers);
        public abstract int GetMaxAllowedRaise(int bb, int maxCommited, GameRoom.HandStep step);
        public abstract int GetMinAllowedRaise(int bb, int maxCommited, GameRoom.HandStep step);
        public abstract bool CanBeSpectatble();

    }
}
