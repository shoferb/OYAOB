using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game
{
    class AfterGameDecorator : Decorator
    {
        public AfterGameDecorator( Decorator d) : base(d)
        {
        }
        public override bool CanStartTheGame(int numOfPlayers)
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

        public override int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public override int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public override bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public override bool CanJoin(int count, int amount)
        {
            throw new NotImplementedException();
        }

        public override bool IsPotSizEqual(int potSize)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameModeEqual(GameMode gm)
        {
            throw new NotImplementedException();
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

        public override bool CanUserJoinGame(int userMoney, int userPoints, bool isUnKnow)
        {
            throw new NotImplementedException();
        }
    }


}
