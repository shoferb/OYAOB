#region

using System;
using System.Collections.Generic;

#endregion

namespace TexasHoldemTests.AcptTests.Bridges
{
    internal class UserBridgeProxy : IUserBridge
    {
        //private UserBridgeReal _realBridge;

        public UserBridgeProxy()
        {
            //_realBridge = new UserBridgeReal();
        }

        public bool IsUserLoggedIn(int userId)
        {
            return true;
        }

        public string GetUserName(int id)
        {
            throw new NotImplementedException();
        }

        public string GetUserPw(int id)
        {
            throw new NotImplementedException();
        }

        public string GetUserEmail(int id)
        {
            throw new NotImplementedException();
        }

        public int GetUserMoney(int id)
        {
            return 0;
        }

        public List<int> GetUsersGameRooms(int userId)
        {
            return new List<int>();
        }

        public List<string> GetUserNotificationMsgs(int userId)
        {
            throw new NotImplementedException();
        }

        public bool LoginUser(string name, string password)
        {
            return true;
        }

        public bool LogoutUser(int userId)
        {
            return true;
        }

        public bool EditName(int id, string newName)
        {
            return true;
        }

        public bool EditPw(int id, string oldPw, string newPw)
        {
            return true;
        }

        public bool EditEmail(int id, string newEmail)
        {
            return true;
        }

        public bool AddUserToGameRoomAsPlayer(int userId, int roomId, int chipAmount)
        {
            return true;
        }

        public bool RemoveUserFromRoom(int userId, int roomId)
        {
            return true;
        }

        public bool ReduceUserMoney(int userId, int amount)
        {
            return true;
        }

        public bool AddUserMoney(int userId, int amount)
        {
            return true;
        }

        public bool RegisterUser(string name, string pw, string pw2)
        {
            return true;
        }

        public bool DeleteUser(string name, string pw)
        {
            return true;
        }

        public bool AddUserToGameRoomAsSpectator(int userId, int roomId)
        {
            return true;
        }

        public int GetNextFreeUserId()
        {
            return 1;
        }

        public bool DeleteUser(int id)
        {
            return true;
        }

        public int GetUserChips(int userId)
        {
            return 0;
        }

        public int GetUserChips(int userId, int roomId)
        {
            return 0;
        }

        public List<int> GetReplayableGames(int userId)
        {
            return new List<int>();
        }

        public int GetUserWins(int userId)
        {
            return 0;
        }

        public int GetUserLosses(int userId)
        {
            return 0;
        }

        public bool AddUserChips(int userId, int roomId, int amount)
        {
            return true;
        }

        public int GetUserRank(int userId)
        {
            throw new NotImplementedException();
        }

        public bool IsThereUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}