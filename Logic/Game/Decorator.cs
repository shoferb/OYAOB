using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Logic
{
    public interface Decorator
    {

        void SetNextDecorator(Decorator d);
        bool CanStartTheGame(int numOfPlayers);
        bool CanSpectatble();
        int GetMinBetInRoom();
        int GetEnterPayingMoney();
        int GetStartingChip();
        bool CanRaise(int lastRaiseInRound ,int currentPlayerRaise, int maxBetInRound, int RoundChipBet, int PotCount, GameRoom.HandStep step);
        bool CanJoin(int count, int amount, IUser user);
   
        //return true the game mode is the same
        bool IsGameModeEqual(GameMode gm);

        //return true the buyIn is the same
        bool IsGameBuyInPolicyEqual(int buyIn);

        //return true the min player is the same
        bool IsGameMinPlayerEqual(int min);

        //return true the max player is the same
        bool IsGameMaxPlayerEqual(int max);

        //return true the min bet in room is the same
        bool IsGameMinBetEqual(int nimBet);

        //return true the stsrtingchip of room is the same
        bool IsGameStartingChipEqual(int startingChip);

        int GetMinPlayerInRoom();
        int GetMaxPlayerInRoom();
        GameMode GetGameMode();
        LeagueName GetLeagueName();


    }
}
