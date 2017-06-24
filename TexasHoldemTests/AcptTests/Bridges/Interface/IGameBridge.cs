using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IGameBridge
    {

        int CreateGameRoom(int userId, int startingCheap);

        bool CreateNewRoomWithRoomId(int roomId, int userId, int startingChip, bool isSpectetor,
            GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet);
        int CreateGameRoomWithPref(int userId, int startingCheap, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet);
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool IsRoomActive(int roomId);
        bool StartGame(int userId, int roomId);
        List<Player> GetPlayersInRoom(int roomId);
        List<int> GetIdPlayersInRoom(int roomId);
        List<int> ListAvailableGamesByUserRank(int userRank);
        List<IGame> ListSpectateableRooms();
        List<int> GetAllGamesId();
        List<IGame> GetAllGames();
        bool RemoveRoom(int roodId);
        //game related:
        List<IGame> GetGamesByGameMode(GameMode mode);
        bool DoAction(int userId, CommunicationMessage.ActionType action, int amount, int roomId);
    }
}
