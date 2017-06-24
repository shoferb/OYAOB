using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service.interfaces;
using TexasHoldemShared;

namespace TexasHoldem.Service
{
    public class UserServiceHandler : IUserService
    {
        private readonly SystemControl _sc;
        private readonly UserDataProxy _userDataProxy;

        public UserServiceHandler (GameCenter game, SystemControl system)
        {
            _sc = system;
            _userDataProxy = new UserDataProxy();
        }

        //Use-Case: user can login to system

        public IUser LoginUser(string username, string password)
        {
            IUser user = _sc.GetIUSerByUsername(username);
            if (user == null || !user.Password().Equals(password))
            {
                return user;
            }
            Console.WriteLine("in login user login?:"+user.IsLogin());

            if (user.Login())
            {
                Console.WriteLine("before login db");
                _userDataProxy.Login(user);
                Console.WriteLine("after login db");
            }
            return user;
        }


        //Use-Case: user can logput from system
        public IUser LogoutUser(int userId)
        {
            IUser user = _sc.GetUserWithId(userId);
            if (user == null || !user.IsLogin())
            {
                return user;
            }

            var toReturn = user.Logout();
            if (toReturn)
            {
                _userDataProxy.Logout(user);
                return user;
            }
            return null;
        }

        
        //register to system - return bool that tell is success or fail - syncronized
        public bool RegisterToSystem(int id, string name, string memberName, string password, int money, string email)
        {
            return _sc.RegisterToSystem(id, name, memberName, password, money, email);
        }


        //by name and password
        public bool DeleteUser(string name, string password)
        {
            bool toReturn = false;
            IUser user = _sc.GetIUSerByUsername(name);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = _sc.RemoveUserByUserNameAndPassword(name, password);
            return toReturn;
        }

        //by Id 
        public bool DeleteUserById(int id)
        {
            
            bool toReturn = _sc.RemoveUserById(id);
            return toReturn;
        }


        //for test only
        //use-case: user can edit is points
        public bool EditUserPoints(int userId, int newPoints)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditUserPoints(newPoints);
            if (toReturn)
            {
                _userDataProxy.EditUserPoints(userId,newPoints);
            }
            return toReturn;
        }

        //use-case: user can edit is password
        public bool EditUserPassword(int userId, string newPassword)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditPassword(newPassword);
            if (toReturn)
            {
                _userDataProxy.EditPassword(userId,newPassword);
            }
            return toReturn;
        }


        //use-case: user can edit is email
        public bool EditUserEmail(int userId, string newEmail)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditEmail(newEmail);
            if (toReturn)
            {
                _userDataProxy.EditEmail(userId,newEmail);
            }
            return toReturn;
        }

        //use-case: user can edit is userName
        public bool EditUserName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null || !_sc.IsUsernameFree(newName))
            {
                return toReturn;
            }
            toReturn = user.EditUserName(newName);
            if (toReturn)
            {
                _userDataProxy.EditUserName(userId, newName);
            }
            return toReturn;
        }

        //use-case: user can edit is name
        public bool EditName(int userId, string newName)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditName(newName);
            if (toReturn)
            {
                _userDataProxy.EditName(userId, newName);
            }
            return toReturn;
        }

        //use-case: user can edit is Id
        public bool EditId(int userId, int newId)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null || !_sc.IsIdFree(newId))
            {
                return toReturn;
            }
            toReturn = user.EditId(newId);
            if (toReturn)
            {
                _userDataProxy.EditUserId(userId, newId);
            }
            return toReturn;
        }

        //use-case: user can edit is money
        public bool EditMoney(int userId, int newmoney)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(userId);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditUserMoney(newmoney);
            if (toReturn)
            {
                _userDataProxy.EditUserMoney(userId, newmoney);
            }
            return toReturn;
        }
 
        //use-case: user can edit is avatar
        public bool EditUserAvatar(int id, string newAvatarPath)
        {
            bool toReturn = false;
            IUser user = _sc.GetUserWithId(id);
            if (user == null)
            {
                return toReturn;
            }
            toReturn = user.EditAvatar(newAvatarPath);
            if (toReturn)
            {
                _userDataProxy.EditUserAvatar(id, newAvatarPath);
            }
            return toReturn;
        }

        public IUser GetIUserByUserName(string userName)
        {
            IUser toReturn = _sc.GetIUSerByUsername(userName);
            return toReturn;
        }

        public List<IUser> GetAllUser()
        {
            return _sc.GetAllUser();
        }

        public IUser GetUserById(int id)
        {
            return _sc.GetUserWithId(id);
        }

        public LeagueName GetUserLeague(int userId)
        {
            return _sc.GetUserWithId(userId).GetLeague();
        }

        public bool DevideLeague()
        {
            return _sc.DivideLeague();
        }

        public List<IUser> GetUsersByTotalProfit()
        {
            return _sc.GetUsersByTotalProfit();
        }

        public List<IUser> GetUsersByHighestCash()
        {
            return _sc.GetUsersByHighestCash();
        }

        public List<IUser> GetUsersByNumOfGames()
        {
            return _sc.GetUsersByNumOfGames();
        }

        public UserStatistics GetUserStatistics(int userId)
        {
            IUser user = GetUserById(userId);
            if (user != null)
            {
                return new UserStatistics(user.GetAvgCashGainPerGame(), user.GetAvgProfit());
            }
            return null;
        }
    }
}