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

        public override int  GetMaxAllowedRaise(int BB, int maxCommited, GameRoom.HandStep step)
        {
            if (this.GameMode == GameMode.Limit && (step == GameRoom.HandStep.Flop ||
                                            step == GameRoom.HandStep.PreFlop))
            {
                return BB;
            }
            if (this.GameMode == GameMode.Limit && (step == GameRoom.HandStep.River ||
                                                       step == GameRoom.HandStep.Turn))
            {
                return BB * 2;
            }
            else // no limit
            {
                return int.MaxValue;
            }

        }

        public override int GetMinAllowedRaise(int BB, int maxCommited, GameRoom.HandStep step)
        {
            if (this.GameMode == GameMode.NoLimit)
            {
                return maxCommited;
            }
            return 0;
        }

        public override bool CanBeSpectatble()
        {
            throw new NotImplementedException();
        }
    }
}
