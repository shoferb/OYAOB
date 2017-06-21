
using System.Collections.Generic;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;

namespace TexasHoldem.Service.interfaces
{
    interface IUserService
    {
        IUser LoginUser(string username, string password);
        IUser LogoutUser(int userId);
        bool RegisterToSystem(int id, string name, string memberName, string password, int money, string email);
        bool DeleteUser(string name, string password);
        bool DeleteUserById(int id);
        bool EditUserPoints(int userId, int newPoints);
        bool EditUserPassword(int userId, string newPassword);
        bool EditUserEmail(int userId, string newEmail);
        bool EditUserName(int userId, string newName);
        bool EditName(int userId, string newName);
        bool EditId(int userId, int newId);
        bool EditMoney(int userId, int newmoney);
        bool EditUserAvatar(int id, string newAvatarPath);
        IUser GetIUserByUserName(string userName);
        List<IUser> GetAllUser();
        IUser GetUserById(int id);
        LeagueName GetUserLeague(int userId);
        bool DevideLeague();
        List<IUser> GetUsersByTotalProfit();
        List<IUser> GetUsersByHighestCash();
        List<IUser> GetUsersByNumOfGames();
        UserStatistics GetUserStatistics(int userId);
    }
}
