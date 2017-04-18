using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class SystemControl
    {
        private List<User> users;

        public SystemControl()
        {
            this.users = new List<User>();
        }
        public List<User> Users
        {
            get
            {
                return users;
            }

            set
            {
                users = value;
            }
        }
        public bool AddNewUser(User newUser)
        {
            bool toReturn = false;
            try
            {
                users.Add(newUser);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool RemoveUserById(int id)
        {
            bool toReturn = false;
            User original = GetUserWithId(id);
            try
            {
                users.Remove(original);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool RemoveUserByUserNameAndPassword(string username, string password)
        {
            bool toReturn = false;
            User toRemove = null;
            bool found = false;
            foreach (User u in users)
            {
                if ((u.Password.Equals(password)) && (u.MemberName.Equals(username)))
                {
                    toRemove = u;
                    found = true;
                }
            }
            try
            {
                users.Remove(toRemove);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }


        public bool RemoveUser(User toRemove)
        {
            bool toReturn = false;
            try
            {
                users.Remove(toRemove);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool ReplaceUser(User oldUser, User newUser)
        {
            bool toReturn = false;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] == oldUser)
                {
                    users[i] = newUser;
                    toReturn = true;
                }
            }
            return toReturn;
        }
        private User FindUser(string username)
        {
            User toRerutn = null;
            foreach (User u in users)
            {
                if (u.MemberName.Equals(username))
                {
                    toRerutn = u;
                }
            }
            return toRerutn;
        }

        //login to system
        public bool Login(string UserName, string password)
        {
            bool toReturn = false;
            User original = FindUser(UserName);
            if (original == null)
            {
                return toReturn;

            }
            foreach (User u in users)
            {
                if ((u.Password.Equals(password)) && (u.MemberName.Equals(UserName)))
                {
                    User toAdd = original;
                    toAdd.IsActive = true;
                    ReplaceUser(original, toAdd);
                    toReturn = true;
                }
            }
            return toReturn;
        }

        public bool Logout(int id)
        {
            bool toReturn = false;
            User original = GetUserWithId(id);
            User changed = original;
            if (original == null)
            {
                return toReturn;

            }
            changed.IsActive = false;
            toReturn = ReplaceUser(original, changed);
            return toReturn;
        }


        //register to system - return bool that tell is success or fail and why
        public bool RegisterToSystem(int id, string name, string memberName, string password, int money, string email)
        {
            bool toReturn = false;
            foreach (User u in users)
            {
                if (u.MemberName.Equals(memberName))
                {
                    //was string toReturn = "Registration failed - this user name is taken!";
                    return toReturn; // fail username taken
                }
            }
            User newUser = new User(id, name, memberName, password, 0, money, email);
            toReturn = AddNewUser(newUser);
            return toReturn;
        }


        //return true - if user name free, false otherwise
        public bool IsUsernameFree(string username)
        {
            bool toReturn = true;
            foreach (User u in users)
            {
                if (u.MemberName.Equals(username))
                {
                    toReturn = false;
                }
            }
            return toReturn;
        }

        public bool IsUserWithId(int id)
        {
            bool toReturn = false;
            foreach (User u in users)
            {
                if (u.Id == id)
                {
                    toReturn = true;
                }

            }
            return toReturn;
        }
        public User GetUserWithId(int id)
        {
            User toReturn = null;
            foreach (User u in users)
            {
                if (u.Id == id)
                {
                    toReturn = u;
                }
            }
            return toReturn;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        //use case - allow edit email 
        public bool EditEmail(int id, string newEmail)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            bool valid = IsValidEmail(newEmail);
            if (valid)
            {
                changed.Email = newEmail;
                toReturn = ReplaceUser(toEdit, changed);
            }
            else
            {
                toReturn = false;
            }
            return toReturn;
        }

        //use case allow to change password
        public bool EditPassword(int id, string newPassword)
        {
            User toEdit = GetUserWithId(id);
            bool toReturn;
            User changed = toEdit;
            int len = newPassword.Length;
            if (len > 7 && len < 13)
            {

                changed.Password = newPassword;
                toReturn = ReplaceUser(toEdit, changed);
            }
            else
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool EditUserName(int id, string newUserName)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            bool validname = IsUsernameFree(newUserName);
            if (validname)
            {
                changed.MemberName = newUserName;
                toReturn = ReplaceUser(toEdit, changed);
            }
            else
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool EditUserID(int id, int newId)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            changed.Id = newId;
            toReturn = ReplaceUser(toEdit, changed);
            return toReturn;
        }

        public bool EditUserPoints(int id, int newPoints)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            changed.Points = newPoints;
            toReturn = ReplaceUser(toEdit, changed);
            return toReturn;
        }

        public bool EditUserMoney(int id, int newMoney)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            changed.Money = newMoney;
            toReturn = ReplaceUser(toEdit, changed);
            return toReturn;
        }

        public bool EditActiveGame(int id, bool activemode)
        {
            User toEdit = GetUserWithId(id);
            User changed = toEdit;
            bool toReturn;
            changed.IsActive = activemode;
            toReturn = ReplaceUser(toEdit, changed);
            return toReturn;
        }

        public bool RemoveRoomFromActiveRoom(int roomId, int userId)
        {
            bool toReturn = false;
            try
            {
                ConcreteGameRoom toRemove = GameCenter.Instance.GetRoomById(roomId);
                User user = GetUserWithId(userId);
                user.ActiveGameList.Remove(toRemove);
                toReturn = true;
            }
            catch(Exception e)
            {
                toReturn = false;
            }
            return toReturn;
         }

        public bool RemoveRoomFromSpectetRoom(int roomId, int userId)
        {
            bool toReturn = false;
            try
            {
                ConcreteGameRoom toRemove = GameCenter.Instance.GetRoomById(roomId);
                User user = GetUserWithId(userId);
                user.SpectateGameList.Remove(toRemove);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool HasThisActiveGame(int roomId, int userId)
        {
            bool toReturn = false;
            ConcreteGameRoom toCheck = GameCenter.Instance.GetRoomById(roomId);
            User user = GetUserWithId(userId);
            toReturn = user.ActiveGameList.Contains(toCheck);
            return toReturn;
        }

        public bool HasThisSpectetorGame(int roomId, int userId)
        {
            bool toReturn = false;
            ConcreteGameRoom toCheck = GameCenter.Instance.GetRoomById(roomId);
            User user = GetUserWithId(userId);
            toReturn = user.SpectateGameList.Contains(toCheck);
            return toReturn;
        }

        public List<ConcreteGameRoom> GetActiveGamesByUserName(string userName)
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            User user = FindUser(userName);
            foreach (ConcreteGameRoom room in user.ActiveGameList)
            {
                if (room._isActiveGame)
                {
                    toReturn.Add(room);
                }
            }
            return toReturn;
        }

        public List<ConcreteGameRoom> GetSpectetorGamesByUserName(string userName)
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            User user = FindUser(userName);
            foreach (ConcreteGameRoom room in user.ActiveGameList)
            {
                if (room._isSpectetor)
                {
                    toReturn.Add(room);
                }
            }
            return toReturn;
        }
    }
}
