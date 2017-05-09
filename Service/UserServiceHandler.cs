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
    public class UserServiceHandler 
    {
        private SystemControl sc = SystemControl.SystemControlInstance;
        private GameCenter gc = GameCenter.Instance;

        
        //Use-Case: user can login to system
        //return -1 if fail
        public int LoginUser(string username, string password)
        {
            int toReturn = -1;
            IUser user = sc.GetIUSerByUsername(username);
            if (user == null || !user.Password().Equals(password))
            {
                return toReturn;
            }
            if (user.Login())
            {
                toReturn = user.Id();
            }
            return toReturn;
        }


        //Use-Case: user can logput from system
        public bool LogoutUser(int userId)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !user.IsLogin())
            {
                return toReturn;
            }
            toReturn = user.Logout();
            return toReturn;
        }

        
        //register to system - return bool that tell is success or fail - syncronized
        public int RegisterToSystem(int id, string name, string memberName, string password, int money, string email)
        {
            int toReturn = -1;

            if (sc.RegisterToSystem(id, name, memberName, password, money, email))
            {
                toReturn = id;
            }
            return toReturn;
        }


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
            return toReturn;
        }

        //by Id 
        public bool DeleteUserById(int id)
        {
            
            bool toReturn = sc.RemoveUserById(id);
            return toReturn;
        }


        //use-case: user can edit is password
        public bool EditUserPassword(int userId, string newPassword)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditPassword(newPassword);
            return toReturn;
        }


        //use-case: user can edit is email
        public bool EditUserEmail(int userId, string newEmail)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditEmail(newEmail);
            return toReturn;
        }

        //use-case: user can edit is userName
        public bool EditUserName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !sc.IsUsernameFree(newName))
            {
                return toReturn;
            }
            toReturn = user.EditUserName(newName);
            return toReturn;
        }

        //use-case: user can edit is name
        public bool EditName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditName(newName);
            return toReturn;
        }


        //use-case: user can edit is Id
        public bool EditId(int userId, int newId)
        {
            bool toReturn = false;
            IUser user = sc.GetUserWithId(userId);
            if (user == null || !sc.IsIdFree(newId))
            {
                return toReturn;
            }
            toReturn = user.EditId(newId);
            return toReturn;
        }


        //use-case: user can get his rank
        public int GetUserRank(int userId)
        {
            int toReturn = -1;
            toReturn = sc.GetUserRank(userId);
            return toReturn;
        }


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

        //use-case: user can edit is avatar
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


        public List<IUser> GetAllUser()
        {
            return sc.GetAllUser();
        }

        
    }
}