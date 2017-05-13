using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class SystemControl
    {
        private List<IUser> users;

        private static SystemControl systemControlInstance = null;
        private static readonly object padlock = new object();


        public static SystemControl SystemControlInstance
        {
            get
            {
                lock (padlock)
                {
                    if (systemControlInstance == null)
                    {
                        systemControlInstance = new SystemControl();
                    }
                    return systemControlInstance;
                }
            }
        }

        private SystemControl()
        {
            this.users = new List<IUser>();
            var ServiceTimer = new System.Timers.Timer();
            ServiceTimer.Enabled = true;
            ServiceTimer.Interval = (1000 * 60 * 60 * 24 * 7);//once a week
            ServiceTimer.Elapsed += new System.Timers.ElapsedEventHandler(DivideLeague);
        }

        //getter seeter user list
        public List<IUser> Users
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

        




        //remove user from user list byID - syncronized
        public bool RemoveUserById(int id)
        {
            lock (padlock)
            {
                bool toReturn = false;
                IUser original = GetUserWithId(id);
                if (!IsValidInputNotSmallerZero(id))
                {
                    return toReturn;
                }
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
        }


        //remove user by name and password  - syncronized
        public bool RemoveUserByUserNameAndPassword(string username, string password)
        {
            bool toReturn;
            IUser toRemove = null;
            bool found = false;
            lock (padlock)
            {
                foreach (IUser u in users)
                {
                    if ((u.Password().Equals(password)) && (u.MemberName().Equals(username)))
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
        }


        //remove user - syncronized
        public bool RemoveUser(IUser toRemove)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (toRemove == null)
                {
                    return toReturn;
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
        }


        //find user by user name - syncronized (due to foreatch)
        //return null if not found or user name is "" || " "
        public IUser GetIUSerByUsername(string username)
        {
            lock (padlock)
            {
                IUser toRerutn = null;
                
                if (username.Equals("")|| username.Equals(" "))
                {
                    return toRerutn;
                }
                try
                {
                    foreach (User u in users)
                    {
                        if (u.MemberName().Equals(username))
                        {
                            toRerutn = u;
                            return toRerutn;
                        }
                    }
                }
                catch
                {
                    return toRerutn;
                }
                
                return toRerutn;
            }
        }

        //register to system - return bool that tell is success or fail - syncronized
        public bool RegisterToSystem(int id, string name, string memberName, string password, int money, string email)
        {
            bool toReturn = false;
            lock (padlock)
            {
                if (!CanCreateNewUser(id, memberName, password, email))
                {
                    return toReturn;
                }
                if (name.Equals(" ") || name.Equals(""))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(money))
                {
                    return toReturn;
                }
                
                User newUser = new User(id, name, memberName, password, 0, money, email);
                users.Add(newUser);
                toReturn = true;
                return toReturn;
            }
        }



        public bool CanCreateNewUser(int id, string memberName,
            string password, string email)
        {
            
            bool toReturn = IsUsernameFree(memberName) && IsIdFree(id) &&
                IsValidPassword(password) && IsValidEmail(email) && !memberName.Equals("") && !memberName.Equals(" ");
            if (!IsUsernameFree(memberName))
            {
                
            }
            if (!IsIdFree(id))
            {
                
            }
            if (!IsValidPassword(password))
            {

            }
            if (!IsValidEmail(email))
            {
                
            }
            return toReturn;
        }

        //return true - if user name free, false otherwise 
        //syncrinized - due to foreath
        public bool IsUsernameFree(string username)
        {
            lock (padlock)
            {
                bool toReturn = true;
                if (username.Equals("") || username.Equals(" "))
                {
                    toReturn = false;
                    return toReturn;
                }
                foreach (IUser u in users)
                {
                    if (u.MemberName().Equals(username))
                    {
                        toReturn = false;
                        return toReturn;
                    }
                }
                return toReturn;
            }
        }

        //return true - if user Id free, false otherwise 
        //syncrinized - due to foreath
        public bool IsIdFree(int ID)
        {
            lock (padlock)
            {
                bool toReturn = true;
                
                if (!IsValidInputNotSmallerZero(ID))
                {
                    toReturn = false;
                    return toReturn;
                }
                foreach (IUser u in users)
                {
                    if (u.Id() ==  ID)
                    {
                        toReturn = false;
                        return toReturn;
                    }
                }
                return toReturn;
            }
        }

        //return true if user with Id exist 
        //syncronized - due to foreatch
        public bool IsUserExist(int id)
        {
            lock (padlock)
            {
                bool toReturn = false;
                foreach (IUser u in users)
                {
                    if (u.Id() == id)
                    {
                        toReturn = true;
                        return toReturn;
                    }

                }
                return toReturn;
            }
        }


        //get user by Id - null if not exist / invalid Id
        //syncronized - due to for
        public IUser GetUserWithId(int id)
        {
            lock (padlock)
            {
                IUser toReturn = null;
                if (!IsValidInputNotSmallerZero(id))
                {
                    return toReturn;
                }
                
                foreach (IUser u in users)
                {
                    if (u.Id() == id)
                    {
                        toReturn = u;
                        return toReturn;
                    }
                }
                return toReturn;
            }
        }


        //check if email valid according to .NET convention
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

        private bool IsValidPassword(string password)
        {
            bool toReturn = false;
            try
            {
                int len = password.Length;
                if (len > 7 && len < 13)
                {
                    toReturn = true;
                }
            }
            catch (Exception e)
            {
                toReturn = false;
                return toReturn;
            }

            return toReturn;
        }

    
        //get all active games of user 
        //syncronizef due to for
        public List<IGame> GetActiveGamesByUserName(string userName)
        {
            lock (padlock)
            {
                List<IGame> toReturn = null;
                if (userName.Equals("")||userName.Equals(" ")|| IsUsernameFree(userName))
                {
                    return toReturn;
                }
               
                IUser user = GetIUSerByUsername(userName);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = new List<IGame>();
                foreach (IGame room in user.ActiveGameList())
                {
                    if (room.IsGameActive())
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }


        //get all the game user spectete
        //syncronized due to for
        public List<IGame> GetSpectetorGamesByUserName(string userName)
        {
            lock (padlock)
            {
                List<IGame> toReturn = null;
                if (userName.Equals("") || userName.Equals(" ") || IsUsernameFree(userName))
                {
                    return toReturn;
                }

                IUser user = GetIUSerByUsername(userName);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = new List<IGame>();
                foreach (IGame room in user.SpectateGameList())
                {
                    if (room.IsSpectatable())
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }

      

        public List<IUser> GetAllUser()
        {
            lock (padlock)
            {
                return users;
            }
        }
        
 
//no use any more
        public List<IUser> SortByRank()
        {
            lock (padlock)
            {
                List<IUser> sort = GetAllUser();
                sort.Sort(delegate(IUser x, IUser y)
                {
                    return y.Points().CompareTo(x.Points());
                });
                return sort;
            }
        }

        
     
        public List<IUser> GetAllUnKnowUsers()
        {
            List<IUser> toReturn = new List<IUser>();
            lock (padlock)
            {
                
                foreach (IUser u in users)
                {
                    if (u.IsUnKnow())
                    {
                        toReturn.Add(u);
                    }   
                }
            }
            return toReturn;
        }

        private bool IsValidInputNotSmallerZero(int toCheck)
        {
            return toCheck >= 0;
        }

        

        public List<IUser> SortByPoint()
        {
            lock (padlock)
            {
                List<IUser> sort = GetAllUser();
                sort.Sort(delegate (IUser x, IUser y)
                {
                    return y.Points().CompareTo(x.Points());
                });
                return sort;
            }
        }

        public void DivideLeague(object sender, ElapsedEventArgs e)
        {
            
            lock (padlock)
            {
                List<IUser> sorted = SortByPoint();
                int userCount = sorted.Count;
                int divideTo;
                if (userCount == 0)
                {
                    return;
                }
                double temp = (userCount / 5);
                divideTo = (int)Math.Round(temp);
                if (divideTo < 2)
                {
                    divideTo = 2;
                }
                int i = 0;
                int k = 0;
                LeagueName curr = LeagueName.A;
                while (i < userCount)
                {
                    while (k <= divideTo && i < userCount)
                    {
                        sorted.ElementAt(i).SetLeague(curr);
                        i++;
                        k++;
                    }
                    k = 0;
                    curr = GetNextLeague(curr);
                }
            }
        }

        private LeagueName GetNextLeague(LeagueName curr)
        {
            LeagueName toReturn;
            switch (curr)
            {
                case LeagueName.A:
                    toReturn = LeagueName.B;
                    break;
                case LeagueName.B:
                    toReturn = LeagueName.C;
                    break;
                case LeagueName.C:
                    toReturn = LeagueName.D;
                    break;
                case LeagueName.D:
                    toReturn = LeagueName.E;
                    break;
                case LeagueName.E:
                    toReturn = LeagueName.E;
                    break;
                default:
                    toReturn = LeagueName.A;
                    break;
            }
            return toReturn;
        }
    }
}
