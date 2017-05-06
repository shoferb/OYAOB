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
        public int BB { get; set; }
        public int SB { get; set; }


        public MiddleGameDecorator(GameMode gameModeChosen, int bb, int sb, Decorator d) : base(d)
        {
            this.GameMode = gameModeChosen;
            this.BB = bb;
            this.SB = sb;
        }

        public override bool CanStartTheGame(int numOfPlayers)
        {
            throw new NotImplementedException();
        }

        public override bool CanSpectatble()
        {
            throw new NotImplementedException();
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


        public override int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            switch (GameMode)
            {
                case GameMode.Limit:
                    switch (step)
                    {
                        case GameRoom.HandStep.Flop:
                        case GameRoom.HandStep.PreFlop:
                            return BB;
                        case GameRoom.HandStep.River:
                        case GameRoom.HandStep.Turn:
                            return BB * 2;
                    }
                    break;
                case GameMode.NoLimit:
                    return int.MaxValue;
                case GameMode.PotLimit:
                    return maxCommited;
                default:
                    break;
            }
            //not spouse to arrive here
            return -1;
        }

        public override int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            if (this.GameMode == GameMode.NoLimit)
            {
                return maxCommited;
            }
            return 0;
        }

        public override bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            if (currentPlayerBet <= GetMaxAllowedRaise(maxBetInRound, step) && currentPlayerBet > 0)
                return true;
            return false;
        }

        //check if the amount is in the range
        public override bool CanJoin(int playersCount, int amount)
        {
            throw new NotImplementedException();
        }



        public override bool IsGameModeEqual(GameMode gm)
        {
            return this.GameMode == gm;
        }

        public override bool IsGameBuyInPolicyEqual(int buyIn)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameMinPlayerEqual(int min)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameMaxPlayerEqual(int max)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameMinBetEqual(int nimBet)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameStartingChipEqual(int startingChip)
        {
            throw new NotImplementedException();
        }

        public override bool CanUserJoinGameWithMoney(int userMoney)
        {
            throw new NotImplementedException();
        }

        public override bool CanAddAnotherPlayer(int currNumOfPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
