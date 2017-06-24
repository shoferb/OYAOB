using System.Collections.Generic;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.Logic.Game
{
    public interface IGame
    {
        int Id { get; set; } 
        int GameNumber { get; set; }
        IEnumerator<ActionResultInfo> DoAction(IUser user, CommunicationMessage.ActionType action, int amount, bool useCommunication);
        IEnumerator<ActionResultInfo> AddSpectetorToRoom(IUser user);
        bool CanJoin(IUser user);
        bool IsGameActive();
        bool IsSpectatable();
        bool IsPotSizeEqual(int potSize);
        bool IsGameModeEqual(GameMode gm);
        bool IsGameBuyInPolicyEqual(int buyIn);
        bool IsGameMinPlayerEqual(int min);
        bool IsGameMaxPlayerEqual(int max);
        bool IsGameMinBetEqual(int minBet);
        bool IsGameStartingChipEqual(int startingChip);
        void SetDecorator(Decorator decorator);

        //Getter for search display 
        int GetMinPlayer();
        int GetMinBet();
        int GetMaxPlayer();
        int GetPotSize();
        int GetBuyInPolicy();
        int GetStartingChip();
        GameMode GetGameMode();
        LeagueName GetLeagueName();
        GameRoom.HandStep GetStep();
        List<Player> GetPlayersInRoom();
        List<Spectetor> GetSpectetorInRoom();
        List<Card> GetPublicCards();
        Player GetDealer();
        Player GetBb();
        Player GetSb();
        Player GetCurrPlayer();
        int GetCurrPosition();

        //methods for chat
        bool IsPlayerInRoom(IUser user);
        bool IsSpectetorInRoom(IUser user);
        IEnumerator<ActionResultInfo> ReturnToGameAsPlayer(IUser user);
        IEnumerator<ActionResultInfo> ReturnToGameAsSpec(IUser user);
    }
}
