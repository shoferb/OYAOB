using System;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class GameBridgeProxy : IGameBridge
    {
        public bool CreateGameRoom(int userId, int roomId)
        {
            return true;
        }

        public bool CreateGameRoom(int id)
        {
            return true;
        }

        public int GetNextFreeRoomId()
        {
            return 1;
        }

        public bool RemoveGameRoom(int id)
        {
            return true;
        }

        public bool DoesRoomExist(int id)
        {
            return true;
        }

        public bool IsUserInRoom(int userId, int roomId)
        {
            return true;
        }

        public bool StartGame(int roomId)
        {
            throw new NotImplementedException();
        }
    }
}
