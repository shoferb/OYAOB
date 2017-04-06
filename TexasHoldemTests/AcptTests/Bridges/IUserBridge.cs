#region

using System.Collections.Generic;

#endregion

namespace TexasHoldemTests.AcptTests.Bridges
{
    internal interface IUserBridge
    {
        bool IsUserLoggedIn(int userId);
        string GetUserName(int id);
        string GetUserPw(int id);
        string GetUserEmail(int id);
        int GetUserMoney(int id);
        int GetUserChips(int userId);
        int GetUserChips(int userId, int gameId);
        List<int> GetUsersGames(int userId);
        List<string> GetUserNotifications(int userId);
        int GetNextFreeUserId();
        List<int> GetReplayableGames(int userId);
        int GetUserWins(int userId);
        int GetUserLosses(int userId);

        bool LoginUser(string name, string password);
        bool LogoutUser(int userId);
        bool RegisterUser(string name, string pw1, string pw2);
        bool DeleteUser(string name, string pw); //used only for tests. deletes user from system if exists
        bool DeleteUser(int id); //used only for tests. deletes user from system if exists
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