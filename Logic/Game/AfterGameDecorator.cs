using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Logic.Game
{
    public class AfterGameDecorator : Decorator
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

        public bool CanRaise(int lastRaiseInRound, int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            throw new NotImplementedException();
        }

        public bool CanJoin(int count, int amount,IUser user)
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

        public int GetMinPlayerInRoom()
        {
            throw new NotImplementedException();
        }

        public int GetMaxPlayerInRoom()
        {
            throw new NotImplementedException();
        }

        public GameMode GetGameMode()
        {
            throw new NotImplementedException();
        }

        public LeagueName GetLeagueName()
        {
            throw new NotImplementedException();
        }

    }


}
