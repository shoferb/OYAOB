using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class UserServiceHandler : ServiceHandler
    {
        private SystemControl sc = new SystemControl();
        private GameCenter gc = new GameCenter();


        //public const int InitialMoneyAmount = 100;
//        public const int InitialPointsAmount = 0;


       //return user with id, if doesent exist return null
        public User GetUserFromId(int userId)
        {
            User toReturn = null;
            bool isThereUser = sc.IsUserWithId(userId);
            if (!isThereUser)
            {
                Console.WriteLine("There is no user with id: " + userId);
            }
            else
            {
                toReturn = sc.GetUserWithId(userId);
            }
            return toReturn;

        }
        public abstract List<User> GetAllUsers();
        public abstract int GetMaxUserPoints();
        public abstract Player GetPlayer(int userId, int roomId);
        public abstract int GetNextUserId();
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