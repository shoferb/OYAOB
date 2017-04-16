#region

using System.Collections.Generic;

#endregion

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IUserBridge
    {
        bool IsUserLoggedIn(int userId);
        string GetUserName(int id);
        string GetUserPw(int id);
        string GetUserEmail(int id);
        int GetUserMoney(int id);
        int GetUserChips(int userId);
        int GetUserChips(int userId, int roomId);
        List<int> GetUsersGameRooms(int userId);
        List<string> GetUserNotificationMsgs(int userId);
        int GetNextFreeUserId();
        int GetUserRank(int userId);
        void SetUserRank(int userId, int rank); //change user's rank BY SYSTEM
        bool SetUserRank(int userIdToChange, int rank, int changingUserId); //change user's rank BY LEADING USER
        bool SetLeagueCriteria(int userId, int criteria); //change the rank diff between leagus, BY LEADING USER

        bool IsThereUser(int id);
        List<int> GetAllUsers();

        bool LoginUser(string name, string password);
        bool LogoutUser(int userId);
        bool RegisterUser(string name, string pw1, string email); //register and login
        bool DeleteUser(string name, string pw); //used only for tests. deletes user from system if exists
        bool DeleteUser(int id); //used only for tests. deletes user from system if exists
        bool EditName(int id, string newName);
        bool EditPw(int id, string oldPw, string newPw);
        bool EditEmail(int id, string newEmail);
        //TODO: add edit avatar
        bool AddUserToGameRoomAsPlayer(int userId, int roomId, int chipAmount);
        bool AddUserToGameRoomAsSpectator(int userId, int roomId);
        bool RemoveUserFromRoom(int userId, int roomId);
        bool ReduceUserMoney(int userId, int amount);
        bool AddUserMoney(int userId, int amount);
        bool AddUserChips(int userId, int roomId, int amount);
    }
}