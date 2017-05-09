using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game
{
    class AfterGameDecorator : Decorator
    {
        private Decorator NextDecorator;

        public AfterGameDecorator() 
        {
        }

        public void SetNextDecorator(Decorator d)
        {
            NextDecorator = d;
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
            throw new NotImplementedException();
        }

        public int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public bool CanJoin(int count, int amount)
        {
            throw new NotImplementedException();
        }



        public bool IsGameModeEqual(GameMode gm)
        {
            throw new NotImplementedException();
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
