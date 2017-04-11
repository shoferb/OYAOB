using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class SystemControl
    {
        List<User> users;

        public SystemControl()
        {
            this.users = new List<User>();
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

        
        
    }
}
