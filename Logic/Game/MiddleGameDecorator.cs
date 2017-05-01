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

        public bool CanRaise()
        {
           //TODO
            return false;
        }

        public void Check()
        {
            //TODO
           
        }

        public void Fold()
        {
           //TODO
        }

        public int GetMinPlayersInRoom()
        {
            //TODO
            return 0;
        }
        public GameMode? GetGameMode()
        {
            return this.GameMode;
        }
    }
}
