using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    public interface IGameBridge
    {
        bool CreateGameRoom(int userId, int roomId); //no preferences because can't get preference class
        bool RemoveGameRoom(int id);
        int GetNextFreeRoomId();
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool IsRoomActive(int roomId);
        bool StartGame(int roomId);
        List<int> GetPlayersInRoom(int roomId);
        List<int> ListAvailableGamesByUserRank(int userRank);
        List<int> ListSpecateableRooms();

        //if only 1 player in room, return he's id
        int GetDealerId(int roomId);
        int GetBbId(int roomId);
        int GetSbId(int roomId);
    }
}
