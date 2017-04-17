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


        //return user with id, if doesn't exist return null
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

        //return all the users
        public List<User> GetAllUsers()
        {
            List<User> toReturn = sc.Users;
            return toReturn;
        }

        public int GetMaxUserPoints()
        {
            User higher = gc.HigherRank;
            int toReturn = higher.Points;
            return toReturn;
        }


        public abstract Player GetPlayer(int userId, int roomId);


        //? to renove?
        public abstract int GetNextUserId();

        // public abstract int IncUserId();
        public bool LoginUser(string name, string password)
        {
            bool toReturn = sc.Login(name, password);
            if (toReturn)
            {
                Console.WriteLine("User with username: " + name + " is now loged in");
            }
            else
            {
                Console.WriteLine("User with username: " + name + " was NOT loged in");
            }
            return toReturn;
        }

        public bool LogoutUser(int userId)
        {
            bool toReturn = sc.Logout(userId);
            if (toReturn)
            {
                Console.WriteLine("User with userId: " + userId + " is now Logout");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT logout in");
            }
            return toReturn;
        }

        public User CreateNewUser(int id, string name, string memberName,
            string password, string email)
        {
            User toReturn = new User(id, name, memberName, password, 0, 0, email);
            Console.WriteLine("User was created with info:" + toReturn.ToString());
            return toReturn;
        }



        //by name and password
        public bool DeleteUser(string name, string password)
        {
            bool toReturn = sc.RemoveUserByUserNameAndPassword(name, password);
            if (toReturn)
            {
                Console.WriteLine("User with userName " + name + " was succesfuly deleted");
            }
            else
            {
                Console.WriteLine("User with username: " + name + " was NOT deleted");
            }
            return toReturn;
        }

        //by id 
        public bool DeleteUserById(int id)
        {
            bool toReturn = sc.RemoveUserById(id);
            return toReturn;
        }

        public bool EditUserPassword(int userId, string newPassword)
        {
            bool toReturn = sc.EditPassword(userId, newPassword);
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed password");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change password");
            }
            return toReturn;
        }

        public bool EditUserEmail(int userId, string newEmail)
        {
            bool toReturn = sc.EditEmail(userId, newEmail);
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed email");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change email");
            }
            return toReturn;
        }

        public bool EditUserName(int userId, string newName)
        {
            bool toReturn = sc.EditUserName(userId, newName);
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed username to: " + newName);
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change username to: " + newName);
            }
            return toReturn;
        }

        public List<Notification> GetUserNotifications(int userId)
        {
            User user = sc.GetUserWithId(userId);
            List<Notification> toReturn = user.WaitListNotification;
            return toReturn;
        }

        //todo impl
        public bool EditUserAvatar(int id, string newAvatarPath)
        {
            throw new NotImplementedException();
        }
    }
}