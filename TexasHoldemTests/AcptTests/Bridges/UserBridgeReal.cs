using System;
using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class UserBridgeReal : IUserBridge
    {
        public bool IsUserLoggedIn(int userId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool LogoutUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool EditName(int id, string newName)
        {
            throw new NotImplementedException();
        }

        public bool EditPw(int id, string oldPw, string newPw)
        {
            throw new NotImplementedException();
        }

        public bool EditEmail(int id, string newEmail)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToGameAsPlayer(int userId, int gameId, int chipAmount)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserFromGame(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        public bool ReduceUserMoney(int userId, int amount)
        {
            throw new NotImplementedException();
        }

        public bool AddUserMoney(int userId, int amount)
        {
            throw new NotImplementedException();
        }

        public bool RegisterUser(string name, string pw, string pw2)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string name, string pw)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToGameAsSpectator(int userId, int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
