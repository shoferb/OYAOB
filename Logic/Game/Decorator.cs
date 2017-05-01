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
        public enum DecState
        {
            Before,
            Middle,
            After
        };

        internal Decorator NextDecorator;

        public Decorator(Decorator d)
        {
            this.NextDecorator = d;
        }

        public bool CanStartTheGame()
        {
            return false;
        }

        public bool CanRaise()
        {
            return false;
        }

        public void Check() { }

        public void Fold() { }

        public int GetMinPlayersInRoom()
        {
            return 0;
        }

        public bool CanBeSpectatble()
        {
            return false;
        }

        public GameMode? GetGameMode()
        {
            return null;
        }

    }
}
