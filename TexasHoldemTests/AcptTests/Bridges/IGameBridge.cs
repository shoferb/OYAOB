using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    interface IGameBridge
    {
        bool CreateGameRoom(int userId, int roomId); //no preferences because can't get preference class
        bool RemoveGameRoom(int id);
        int GetNextFreeRoomId();
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool IsRoomActive(int roomId);
        bool StartGame(int roomId);
        List<int> GetPlayersInRoom(int roomId);

        //if only 1 player in room, return he's id
        int GetDealerId(int roomId);
        int GetBbId(int roomId);
        int GetSbId(int roomId);

        //TODO: test room becomes inactive after less then 2 players

    }
}
