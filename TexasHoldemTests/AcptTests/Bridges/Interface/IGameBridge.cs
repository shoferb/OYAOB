using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IGameBridge
    {

        bool CreateGameRoom(int userId, int roomId);
        int CreateGameRoom(int userId); //create game room and return it's Id 
        bool RemoveGameRoom(int id);
        int GetNextFreeRoomId();
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool IsRoomActive(int roomId);
        bool StartGame(int roomId);
        List<int> GetPlayersInRoom(int roomId);
        List<int> ListAvailableGamesByUserRank(int userRank);
        List<int> ListSpectateableRooms();
        List<int> GetAllGames();

        //(if only 1 player in room, return he's Id)
        int GetDealerId(int roomId);
        int GetBbId(int roomId);
        int GetSbId(int roomId);
        
        //game related:
        int GetDeckSize(int gameId);
        int GetCurrPlayer(int gameId);
        int GetSbSize(int gameId);
        int GetPotSize(int gameId);
        //List<int> GetWinner(int gameId);

        //game player oporations:
        //bool Fold(int userId, int roomId);
        //bool Check(int userId, int roomId);
        //bool Call(int userId, int roomId, int amount);
        //bool Raise(int userId, int roomId, int amount);

    }
}
