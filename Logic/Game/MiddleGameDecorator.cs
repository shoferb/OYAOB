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
        private Decorator NextDecorator;
        public GameMode GameMode { get; set; }
        public int BB { get; set; }
        public int SB { get; set; }

        public void SetDecorator(Decorator d)
        {
            NextDecorator = d;
        }

        public MiddleGameDecorator(GameMode gameModeChosen, int bb, int sb, Decorator d)
        {
            this.GameMode = gameModeChosen;
            this.BB = bb;
            this.SB = sb;
        }

        public bool CanStartTheGame(int numOfPlayers)
        {
            throw new NotImplementedException();
        }

        public bool CanSpectatble()
        {
            throw new NotImplementedException();
        }

        public int GetMinBetInRoom()
        {
            throw new NotImplementedException();
        }

        public int GetEnterPayingMoney()
        {
            throw new NotImplementedException();
        }

        public int GetStartingChip()
        {
            throw new NotImplementedException();
        }


        public int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
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

        public int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            if (this.GameMode == GameMode.NoLimit)
            {
                return maxCommited;
            }
            return 0;
        }

        public bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            if (currentPlayerBet <= GetMaxAllowedRaise(maxBetInRound, step) && currentPlayerBet > 0)
                return true;
            return false;
        }

        //check if the amount is in the range
        public bool CanJoin(int playersCount, int amount)
        {
            throw new NotImplementedException();
        }



        public bool IsGameModeEqual(GameMode gm)
        {
            return this.GameMode == gm;
        }

        public bool IsGameBuyInPolicyEqual(int buyIn)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMinPlayerEqual(int min)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMaxPlayerEqual(int max)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMinBetEqual(int nimBet)
        {
            throw new NotImplementedException();
        }

        public bool IsGameStartingChipEqual(int startingChip)
        {
            throw new NotImplementedException();
        }

        public bool CanUserJoinGameWithMoney(int userMoney)
        {
            throw new NotImplementedException();
        }


        public bool CanAddAnotherPlayer(int currNumOfPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
