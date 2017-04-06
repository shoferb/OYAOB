using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemTests.AcptTests.Bridges
{
    interface IGameBridge
    {
        bool CreateGameRoom(int userId);
        bool CreateGameRoom(int userId, int roomId);
        bool RemoveGameRoom(int id);
        int GetNextFreeRoomId();
        bool DoesRoomExist(int id);
        bool IsUserInRoom(int userId, int roomId);
        bool StartGame(int roomId);

    }
}
