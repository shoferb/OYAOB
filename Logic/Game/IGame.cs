using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Logic.Game
{
    public interface IGame
    {
        int Id { get; set; }
        bool DoAction(IUser user, TexasHoldemShared.CommMessages.CommunicationMessage.ActionType action, int amount);
        bool AddSpectetorToRoom(IUser user);
        bool RemoveSpectetorFromRoom(IUser user);
        bool CanJoin(IUser user);
        bool IsGameActive();
        bool IsSpectatable();
        bool IsPotSizEqual(int potSize);
        bool IsGameModeEqual(GameMode gm);
        bool IsGameBuyInPolicyEqual(int buyIn);
        bool IsGameMinPlayerEqual(int min);
        bool IsGameMaxPlayerEqual(int max);
        bool IsGameMinBetEqual(int minBet);
        bool IsGameStartingChipEqual(int startingChip);
        int GetMinRank();
        int GetMaxRank();

        //Getter for search display 
        int GetMinPlayer();
        int GetMinBet();
        int GetMaxPlayer();
        int GetPotSize();
        int GetBuyInPolicy();
        int GetStartingChip();
        GameMode GetGameGameMode();
        LeagueName GetLeagueName();
    }
}
