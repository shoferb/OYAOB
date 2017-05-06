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

        //return true if user has money for buyIn + starting chip and his point are in:
        //room.minRank <= point <= room.maxPoints +
        //check that game is no active and has more room for another player
        //if isUNknow = true --> no need th check if point are good
        bool CanUserJoinGame(int userMoney, int userPoints, bool ISUnKnow);

        List<Player> GetPlayersInRoom();


        List<Spectetor> GetSpectetorInRoom();

        int GetMinRank();

        int GetMaxRank();

    }
}
