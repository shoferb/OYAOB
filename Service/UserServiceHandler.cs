using System.Collections.Generic;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class UserServiceHandler : ServiceHandler
    {
        public const int InitialMoneyAmount = 100;
        public const int InitialPointsAmount = 0;

        public abstract User GetUserFromId(int userId);
        public abstract List<User> GetAllUsers();
        public abstract int GetMaxUserPoints();
        public abstract Player GetPlayer(int userId, int roomId);
        public abstract int GetNextUserId();
        public abstract int IncUserId();
        public abstract bool LoginUser(string name, string password);
        public abstract bool LogoutUser(int userId);

        public abstract User CreateNewUser(int id, string name, string memberName,
            string password, string email);

        public abstract bool DeleteUser(string name, string password);

        public abstract bool EditUserPassword(int userId, string oldPassword, string newPassword);
        public abstract bool EditUserEmail(int userId, string newEmail);
        public abstract bool EditUserName(int userId, string newName);

        public abstract List<Notification> GetUserNotifications(int userId);
    }
}