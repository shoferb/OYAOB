using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.Service.interfaces
{
    public interface IGameService
    {
        IEnumerator<ActionResultInfo> DoAction(int userId, CommunicationMessage.ActionType action,
            int amount, int roomId);

        IEnumerator<ActionResultInfo> ReturnToGameAsPlayer(int userId, int roomId);
        IEnumerator<ActionResultInfo> ReturnToGameAsSpec(int userId, int roomId);
        List<Player> GetPlayersInRoom(int roomId);
        List<Spectetor> GetSpectatorsInRoom(int roomId);
        IGame GetGameFromId(int gameId);

        bool CreateNewRoomWithRoomId(int roomId, int userId, int startingChip, bool isSpectetor,
            GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet);

        int CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet);

        List<string> GetGameReplay(int roomId, int gameNum, int userId);
        IEnumerator<ActionResultInfo> RemoveSpectatorFromRoom(int userId, int roomId);
        IEnumerator<ActionResultInfo> AddSpectatorToRoom(int userId, int roomId);
        IGame GetGameById(int id);
        List<IGame> GetAllActiveGames();
        List<IGame> GetAllActiveGamesAUserCanJoin(int userId);
        List<IGame> GetAllGames();
        List<IGame> GetSpectateableGames();
        List<IGame> GetGamesByPotSize(int potSize);
        List<IGame> GetGamesByGameMode(GameMode gm);
        List<IGame> GetGamesByBuyInPolicy(int buyIn);
        List<IGame> GetGamesByMinPlayer(int min);
        List<IGame> GetGamesByMinBet(int minBet);
        List<IGame> GetGamesByStartingChip(int startingChip);
        List<IGame> GetGamesByMaxPlayer(int max);
        IEnumerator<int> CanSendPlayerBrodcast(int playerId, int roomId);
        IEnumerator<int> CanSendSpectetorBrodcast(int idSpectetor, int roomId);
        bool CanSendPlayerWhisper(int idSender, string reciverUsername, int roomId);
        bool CanSendSpectetorWhisper(int idSender, string reciverUsername, int roomId);
        List<IGame> GetActiveGamesByUserName(string userName);
        List<IGame> GetSpectetorGamesByUserName(string userName);
    }
}
