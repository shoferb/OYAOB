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


/*
        //return user with Id, if doesn't exist return null
        public User GetUserFromId(int userId)
        {
            User toReturn = null;
            bool isThereUser = sc.IsUserExist(userId);
            if (!isThereUser)
            {
                Console.WriteLine("There is no user with Id: " + userId);
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
        }*/

       


       

        
        public bool LoginUser(string username, string password)
        {
            bool toReturn = false;
            IUser user = sc.GetIUSerByUsername(username);
            if (user == null || !user.Password().Equals(password))
            {
                return toReturn;
            }
            toReturn = user.Login();
            /*if (toReturn)
            {
                Console.WriteLine("User with username: " + username + " is now loged in");
            }
            else
            {
                Console.WriteLine("User with username: " + username + " was NOT loged in");
            }*/
            return toReturn;
        }

        public bool LogoutUser(int userId)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !user.IsLogin())
            {
                return toReturn;
            }
            toReturn = user.Logout();
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId: " + userId + " is now Logout");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT logout in");
            }*/
            return toReturn;
        }

        
        //register to system - return bool that tell is success or fail - syncronized
        public bool RegisterToSystem(int id, string name, string memberName, string password, int money, string email)
        {
            return sc.RegisterToSystem(id, name, memberName, password, money, email);
        }

        /*
        public User FindUser(string username)
        {
            return SystemControl.SystemControlInstance.GetIUSerByUsername(username);
        }

        
        public bool CanCreateNewUser(int Id , string memberName,
            string password, string email)
        {

            bool toReturn = sc.CanCreateNewUser(Id, memberName, password, email);
            return toReturn;
        }*/

        //by name and password
        public bool DeleteUser(string name, string password)
        {
            bool toReturn = false;
            IUser user = sc.GetIUSerByUsername(name);
            if (user == null || !user.Password().Equals(password))
            {
                return toReturn;
            }
            toReturn = sc.RemoveUserByUserNameAndPassword(name, password);
            /*if (toReturn)
            {
                Console.WriteLine("User with userName " + name + " was succesfuly deleted");
            }
            else
            {
                Console.WriteLine("User with username: " + name + " was NOT deleted");
            }*/
            return toReturn;
        }

        //by Id 
        public bool DeleteUserById(int id)
        {
            
            bool toReturn = sc.RemoveUserById(id);
            return toReturn;
        }

        public bool EditUserPassword(int userId, string newPassword)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditPassword(newPassword);
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed password");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change password");
            }*/
            return toReturn;
        }

        public bool EditUserEmail(int userId, string newEmail)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditEmail(newEmail);
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed email");
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change email");
            }*/
            return toReturn;
        }

        public bool EditUserName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !sc.IsUsernameFree(newName))
            {
                return toReturn;
            }
            toReturn = user.EditUserName(newName);
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed username to: " + newName);
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change username to: " + newName);
            }*/
            return toReturn;
        }


        public bool EditName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditName(newName);
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed username to: " + newName);
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change username to: " + newName);
            }*/
            return toReturn;
        }

        public bool EditId(int userId, int newId)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !sc.IsIdFree(newId))
            {
                return toReturn;
            }
            toReturn = user.EditId(newId);
            /*
            if (toReturn)
            {
                Console.WriteLine("User with userId " + userId + " was succesfuly changed username to: " + newName);
            }
            else
            {
                Console.WriteLine("User with username: " + userId + " was NOT able to change username to: " + newName);
            }*/
            return toReturn;
        }

        public int GetUserRank(int userId)
        {
            int toReturn = -1;
            toReturn = sc.GetUserRank(userId);
            return toReturn;
        }
        /*
        public bool EditUserPoints(int userId, int points)
        {
            return SystemControl.SystemControlInstance.EditUserPoints(userId, points);
        }*/

        public List<Notification> GetUserNotifications(int userId)
        {
            List<Notification> toReturn = null;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
 
            toReturn = user.WaitListNotification();
            return toReturn;
        }


        public bool EditUserAvatar(int id, string newAvatarPath)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(id);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditAvatar(newAvatarPath);
            return toReturn;
        }


        public List<IGame> GetActiveGamesByUserName(string userName)
        {
            List<IGame> toReturn = sc.GetActiveGamesByUserName(userName);
            return toReturn;
        }

        
        public List<IGame> GetSpectetorGamesByUserName(string userName)
        {
            List<IGame> toReturn = sc.GetSpectetorGamesByUserName(userName);
            return toReturn;
        }


     /*   public bool IsHigestRankUser(int userId)
        {
            bool toReturn = sc.IsHigestRankUser(userId);
            return toReturn;
        }*/
/*
        public List<IUser> SortUserByRank()
        {
            return sc.SortByRank();
        }*/

       
/*
        public bool SetDefultLeauseToNewUsers(int highestId, int newPoint)
        {
            return SystemControl.SystemControlInstance.SetDefultLeauseToNewUsers(highestId, newPoint);
        }

        public bool MovePlayerBetweenLeague(int highestId, int userToMove, int newPoint)
        {
            return SystemControl.SystemControlInstance.MovePlayerBetweenLeague(highestId, userToMove, newPoint);
        }

        public bool ChangeGapByHighestUserAndCreateNewLeague(int userId, int newGap)
        {
            return SystemControl.SystemControlInstance.ChangeGapByHighestUserAndCreateNewLeague(userId, newGap);
        }*/

        public List<IUser> GetAllUser()
        {
            return sc.GetAllUser();
        }

        
    }
}