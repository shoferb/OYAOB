using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic
{

    class MiddleGameDecorator : Decorator
    {
        public GameMode GameMode { get; set; }

        public MiddleGameDecorator(GameMode gameModeChosen, Decorator d) : base(d)
        {
            this.GameMode = gameModeChosen;
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
