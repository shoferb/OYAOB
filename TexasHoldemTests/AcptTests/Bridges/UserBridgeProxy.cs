using System;
using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class UserBridgeProxy : IUserBridge
    {
        private UserBridgeReal _realBridge;

        public UserBridgeProxy()
        {
            _realBridge = new UserBridgeReal();
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
            throw new NotImplementedException();
        }

        public List<int> GetUsersGames(int userId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserNotifications(int userId)
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

        public bool AddUserToGameAsPlayer(int userId, int gameId, int chipAmount)
        {
            return true;
        }

        public bool RemoveUserFromGame(int userId, int gameId)
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

        public bool AddUserToGameAsSpectator(int userId, int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
