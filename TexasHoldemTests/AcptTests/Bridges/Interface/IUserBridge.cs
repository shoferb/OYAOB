#region

using System.Collections.Generic;
using TexasHoldem.Logic.Users;

#endregion

namespace TexasHoldemTests.AcptTests.Bridges.Interface
{
    public interface IUserBridge
    {
        bool IsUserLoggedIn(int userId);
        string GetUserName(int id);
        string GetUserPw(int id);
        string GetUserEmail(int id);
        string GetUserAvatar(int userId);
        int GetUserMoney(int id);
        int GetUserChips(int userId);
        int GetUserChips(int userId, int roomId);
        List<int> GetUsersGameRooms(int userId);
        int GetNextFreeUserId();
        int GetUserPoints(int userId);
        void SetUserPoints(int userId, int points); //change user's rank BY SYSTEM

        bool IsThereUser(int id);
        List<int> GetAllUsers();
        IUser getUserById(int userId);
        bool LoginUser(string name, string password);
        bool LogoutUser(int userId);
        int RegisterUser(string name, string pw1, string email); //register and login. return Id
        bool RegisterUser(int id, string name, string pw1, string email); //register and login
        bool DeleteUser(string name, string pw); //used only for tests. deletes user from system if exists
        bool DeleteUser(int id); //used only for tests. deletes user from system if exists
        bool EditName(int id, string newName);
        bool EditPw(int id, string newPw);
        bool EditEmail(int id, string newEmail);
        bool EditAvatar(int id, string newAvatarPath);
        bool AddUserToGameRoomAsPlayer(int userId, int roomId, int chipAmount);
        bool AddUserToGameRoomAsSpectator(int userId, int roomId);
        bool RemoveUserFromRoom(int userId, int roomId);
        bool ReduceUserMoney(int userId, int amount);
        bool AddUserMoney(int userId, int amount);
        bool RemoveSpectatorFromRoom(int userId, int roomId);
        bool DividLeage();
        List<IUser> GetUsersByNumOfGames();
        List<IUser> GetUsersByHighestCash();
        List<IUser> GetUsersByTotalProfit();
        List<IUser> GetUsersByHighestCashn();
    }
}