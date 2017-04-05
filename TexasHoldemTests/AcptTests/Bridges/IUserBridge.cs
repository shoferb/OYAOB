using System.Collections.Generic;

namespace TexasHoldemTests.AcptTests.Bridges
{
    interface IUserBridge
    {
        bool IsUserLoggedIn(int userId);
        string GetUserName(int id);
        string GetUserPw(int id);
        string GetUserEmail(int id);
        int GetUserMoney(int id);
        List<int> GetUsersGames(int userId);
        List<string> GetUserNotifications(int userId);

        bool LoginUser(string name, string password);
        bool LogoutUser(int userId);
        bool RegisterUser(string name, string pw1, string pw2);
        bool DeleteUser(string name, string pw); //used only for tests. deletes user from system if exists
        bool EditName(int id, string newName);
        bool EditPw(int id, string oldPw, string newPw);
        bool EditEmail(int id, string newEmail);
        //TODO: add edit avatar
        bool AddUserToGameAsPlayer(int userId, int gameId, int chipAmount);
        bool AddUserToGameAsSpectator(int userId, int gameId);
        bool RemoveUserFromGame(int userId, int gameId);
        bool ReduceUserMoney(int userId, int amount);
        bool AddUserMoney(int userId, int amount);

    }
}
