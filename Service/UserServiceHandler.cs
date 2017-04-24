using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public class UserServiceHandler : ServiceHandler
    {
        private SystemControl sc = SystemControl.SystemControlInstance;
        private GameCenter gc = GameCenter.Instance;


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

        //TODO
        public Player GetPlayer(int userId, int roomId)
        {
            return null;
            ;
        }


        //TODO
        //? to renove?
        public int GetNextUserId()
        {
            return 1;
        }

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


        //return null if one of the field is not valid
        public User CreateNewUser(int id, string name, string memberName,
            string password, string email)
        {
            User toReturn = null;
            if (CanCreateNewUser(id , memberName, password, email))
            {
                toReturn = new User(id, name, memberName, password, 0, 0, email);
            }
            
            //Console.WriteLine("User was created with info:" + toReturn.ToString());
            return toReturn;
        }



        public bool CanCreateNewUser(int id , string memberName,
            string password, string email)
        {

            bool toReturn = sc.IsUsernameFree(memberName) && sc.IsIdFree(id) && sc.IsValidPassword(password) &&
                            sc.IsValidEmail(email);
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

        public bool EditUserPoints(int userId, int points)
        {
            return SystemControl.SystemControlInstance.EditUserPoints(userId, points);
        }

        public List<Notification> GetUserNotifications(int userId)
        {
            User user = sc.GetUserWithId(userId);
            List<Notification> toReturn = user.WaitListNotification;
            return toReturn;
        }


        public bool EditUserAvatar(int id, string newAvatarPath)
        {
            bool toReturn = sc.EditAvatar(id, newAvatarPath);
            return toReturn;
        }

        public GameReplay GetGameReplay(int roomID, int gameID)
        {
            return gc.GetGameReplay(roomID, gameID);
        }


        public List<GameRoom> GetActiveGamesByUserName(string userName)
        {
            List<GameRoom> toReturn = sc.GetActiveGamesByUserName(userName);
            return toReturn;
        }


        public List<GameRoom> GetSpectetorGamesByUserName(string userName)
        {
            List<GameRoom> toReturn = sc.GetSpectetorGamesByUserName(userName);
            return toReturn;
        }


        public bool IsHigestRankUser(int userId)
        {
            bool toReturn = sc.IsHigestRankUser(userId);
            return toReturn;
        }

        public List<User> SortUserByRank()
        {
            return sc.SortByRank();
        }

        public List<User> SortUserByPoint()
        {
            return sc.SortUserByPoint();
        }

        public bool SetDefultLeauseToNewUsers(int highestId, int newPoint)
        {
            return SystemControl.SystemControlInstance.SetDefultLeauseToNewUsers(highestId, newPoint);
        }

        public bool MovePlayerBetweenLeague(int highestId, int userToMove, int newPoint)
        {
            return SystemControl.SystemControlInstance.MovePlayerBetweenLeague(highestId, userToMove, newPoint);
        }
    }
}