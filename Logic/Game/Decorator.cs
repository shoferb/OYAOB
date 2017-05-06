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

        public abstract bool CanStartTheGame(int numOfPlayers);
        public abstract bool CanSpectatble();
        public abstract int GetMinBetInRoom();
        public abstract int GetEnterPayingMoney();
        public abstract int GetStartingChip();
        public abstract int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step);
        public abstract int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step);
        public abstract bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step);
        public abstract bool CanJoin(int count, int amount);


     



        //return true the game mode is the same
        public abstract bool IsGameModeEqual(GameMode gm);

        //return true the buyIn is the same
        public abstract bool IsGameBuyInPolicyEqual(int buyIn);

        //return true the min player is the same
        public abstract bool IsGameMinPlayerEqual(int min);

        //return true the max player is the same
        public abstract bool IsGameMaxPlayerEqual(int max);

        //return true the min bet in room is the same
        public abstract bool IsGameMinBetEqual(int nimBet);

        //return true the stsrtingchip of room is the same
        public abstract bool IsGameStartingChipEqual(int startingChip);

        //return true if user has money for buyIn + starting chip and his point are in:
        public abstract bool CanUserJoinGameWithMoney(int userMoney);

        //return true can add another player
        public abstract bool CanAddAnotherPlayer(int currNumOfPlayer);
    }
}
