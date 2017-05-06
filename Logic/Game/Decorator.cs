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
        public abstract bool CanSpectatble();
        public abstract int GetMinBetInRoom();
        public abstract int GetEnterPayingMoney();
        public abstract int GetStartingChip();
        public abstract int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step);
        public abstract int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step);
        public abstract bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step);
        public abstract bool CanJoin(int count, int amount);
    }
}
