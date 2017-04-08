using System;
using System.Collections.Generic;
using TexasHoldemTests.AcptTests.Bridges.Real;

namespace TexasHoldemTests.AcptTests.Bridges.Proxy
{
    //this class just delegates to RealBridge now...
    //TODO: maybe delete it?
    class GameBridgeProxy : IGameBridge
    {
        private static readonly GameBridgeReal RealBridge = new GameBridgeReal();

        public bool CreateGameRoom(int userId, int roomId)
        {
            return RealBridge.CreateGameRoom(userId, roomId);
        }

        public int CreateGameRoom(int id)
        {
            return RealBridge.CreateGameRoom(id);
        }

        public int GetNextFreeRoomId()
        {
            return RealBridge.GetNextFreeRoomId();
        }

        public bool RemoveGameRoom(int id)
        {
            return RealBridge.RemoveGameRoom(id);
        }

        public bool DoesRoomExist(int id)
        {
            return RealBridge.DoesRoomExist(id);
        }

        public bool IsUserInRoom(int userId, int roomId)
        {
            return RealBridge.IsUserInRoom(userId, roomId);
        }

        public bool StartGame(int roomId)
        {
            return RealBridge.StartGame(roomId);
        }

        public bool IsRoomActive(int roomId)
        {
            return RealBridge.IsRoomActive(roomId);
        }

        public int GetDealerId(int roomId)
        {
            return RealBridge.GetDealerId(roomId);
        }

        public int GetBbId(int roomId)
        {
            return RealBridge.GetBbId(roomId);
        }

        public int GetSbId(int roomId)
        {
            return RealBridge.GetSbId(roomId);
        }

        public List<int> GetPlayersInRoom(int roomId)
        {
            return RealBridge.GetPlayersInRoom(roomId);
        }

        public List<int> ListAvailableGamesByUserRank(int userRank)
        {
            return RealBridge.ListAvailableGamesByUserRank(userRank);
        }

        public List<int> ListSpectateableRooms()
        {
            return RealBridge.ListSpectateableRooms();
        }

        public bool Fold(int userId, int roomId)
        {
            return RealBridge.Fold(userId, roomId);
        }

        public bool Check(int userId, int roomId)
        {
            return RealBridge.Check(userId, roomId);
        }

        public bool Call(int userId, int roomId, int amount)
        {
            return RealBridge.Call(userId, roomId, amount);
        }

        public bool Raise(int userId, int roomId, int amount)
        {
            return RealBridge.Raise(userId, roomId, amount);
        }

        public List<int> GetAllGames()
        {
            return RealBridge.GetAllGames();
        }

        public int GetDeckSize(int gameId)
        {
            return RealBridge.GetDeckSize(gameId);
        }

        public int GetCurrPlayer(int gameId)
        {
            return RealBridge.GetCurrPlayer(gameId);
        }

        public int GetSbSize(int gameId)
        {
            return RealBridge.GetSbSize(gameId);
        }

        public int GetPotSize(int gameId)
        {
            return RealBridge.GetPotSize(gameId);
        }

        public bool DealFirstCards(int gameId)
        {
            return RealBridge.DealFirstCards(gameId);
        }

        public bool DealFlop(int gameId)
        {
            return RealBridge.DealFlop(gameId);
        }

        public bool DealSingleCardToTable(int gameId)
        {
            return RealBridge.DealSingleCardToTable(gameId);
        }

        public int GetWinner(int gameId)
        {
            return RealBridge.GetWinner(gameId);
        }
    }
}
