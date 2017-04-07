using System;
using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class GameBridgeProxy : IGameBridge
    {
        public bool CreateGameRoom(int userId, int roomId)
        {
            return true;
        }

        public int CreateGameRoom(int id)
        {
            return -1;
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

        public bool IsRoomActive(int roomId)
        {
            throw new NotImplementedException();
        }

        public int GetDealerId(int roomId)
        {
            throw new NotImplementedException();
        }

        public int GetBbId(int roomId)
        {
            throw new NotImplementedException();
        }

        public int GetSbId(int roomId)
        {
            throw new NotImplementedException();
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        public List<int> ListAvailableGamesByUserRank(int userRank)
        {
            throw new NotImplementedException();
        }

        public List<int> ListSpectateableRooms()
        {
            throw new NotImplementedException();
        }

        public bool Fold(int userId, int roomId)
        {
            throw new NotImplementedException();
        }

        public bool Check(int userId, int roomId)
        {
            throw new NotImplementedException();
        }

        public bool Call(int userId, int roomId, int amount)
        {
            throw new NotImplementedException();
        }

        public bool Raise(int userId, int roomId, int amount)
        {
            throw new NotImplementedException();
        }

        public List<int> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public int GetDeckSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetCurrPlayer(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetSbSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public int GetPotSize(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool DealFirstCards(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool DealFlop(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool DealSingleCardToTable(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
