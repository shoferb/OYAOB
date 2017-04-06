using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    public interface IGameBridge
    {
        bool CreateGameRoom(int userId, int roomId); //no preferences because can't get preference class
        int CreateGameRoom(int userId); //create game room and return it's id 
        bool RemoveGameRoom(int id);
        int GetNextFreeRoomId();
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool IsRoomActive(int roomId);
        bool StartGame(int roomId);
        List<int> GetPlayersInRoom(int roomId);
        List<int> ListAvailableGamesByUserRank(int userRank);
        List<int> ListSpectateableRooms();

        //if only 1 player in room, return he's id
        int GetDealerId(int roomId);
        int GetBbId(int roomId);
        int GetSbId(int roomId);

        //game oporations:
        bool Fold(int userId, int roomId);
        bool Check(int userId, int roomId);
        bool Call(int userId, int roomId, int amount);
        bool Raise(int userId, int roomId, int amount);
        //bool Win(int userId, int roomId); //maybe not needed
        //bool Lose(int userId, int roomId); //maybe not needed

    }
}
