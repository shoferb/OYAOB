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
            return this.NextDecorator.CanStartTheGame(numOfPlayers);
        }

        public bool CanRaise()
        {
            return this.NextDecorator.CanRaise();
        }

        public bool CanCheck()
        {
            return  this.NextDecorator.CanCheck();
        }

        public bool CanFold()
        {
            return this.NextDecorator.CanFold();
        }

       public bool CanSpectatble()
        {
            return this.NextDecorator.CanSpectatble();
        }

        public int GetMinBetInRoom()
        {
            return this.NextDecorator.GetMinBetInRoom();
        }

        public int GetEnterPayingMoney()
        {
            return this.NextDecorator.GetEnterPayingMoney();
        }

        public int GetStartingChip()
        {
            return this.NextDecorator.GetStartingChip();
        }

        public bool CanAddMorePlayer(int currNumOfPlayers)
        {
            return this.NextDecorator.CanAddMorePlayer(currNumOfPlayers);
        }

    
    }
}
