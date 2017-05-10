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

        //return true if this is an active game
        bool IsGameActive();

        //return true if this is a game that can hold spectetors.
        bool IsSpectetorGame();

        //return true the pot size is equal 
        bool IsPotSizEqual(int potSize);

        //return true the game mode is the same
        bool IsGameModeEqual(GameMode gm);

        //return true the buyIn is the same
        bool IsGameBuyInPolicyEqual(int buyIn);

        //return true the min player is the same
        bool IsGameMinPlayerEqual(int min);

        //return true the max player is the same
        bool IsGameMaxPlayerEqual(int max);

        //return true the min bet in room is the same
        bool IsGameMinBetEqual(int minBet);

        //return true the stsrtingchip of room is the same
        bool IsGameStartingChipEqual(int startingChip);

        List<Player> GetPlayersInRoom();


        List<Spectetor> GetSpectetorInRoom();

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
