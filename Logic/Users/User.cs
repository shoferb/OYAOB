using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Game;
using Action = TexasHoldem.Logic.Actions.Action;

namespace TexasHoldem.Logic.Users
{
    public class User : IUser
    {
        private int id;
        private String name;
        private String memberName;
        private string password;
        private String avatar;//- image path
        private int points;
        private int money;
        private List<Notification> waitListNotification;
        private string email;
        private bool isActive;
        public List<Tuple<int, int>> _gamesAvailableToReplay { get; set; }
        private List<IGame> activeGameList;
        private List<IGame> spectateGameList;

        public int rank { get; set; }

        public int winNum { get; set; }
        //for syncronize
        private static readonly object padlock = new object();


        public User(int id, string name, string memberName, string password, int points, int money, string email)
        {
            this.id = id;
            this.name = name;
            this.memberName = memberName;
            this.password = password;
            this.points = points;
            this.money = money;
            this.email = email;
            this.waitListNotification = new List<Notification>();
            this.isActive = false;
            this.avatar = "path?";
            _gamesAvailableToReplay = new List<Tuple<int,int>>();
            activeGameList = new List<IGame>();
            spectateGameList = new List<IGame>();
            this.winNum = 0;
          
        }

        //function to recive notificaion - return the notification.
        public bool SendNotification(Notification toSend)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (AddNotificationToList(toSend))
                {
                    toReturn = true;
                }
                else
                {
                    return toReturn;
                }
                return toReturn;
            }
        }


        //private method - add the notification to list so can print when not in game
        public bool AddNotificationToList(Notification toAdd)
        {
            lock (padlock)
            {
                this.waitListNotification.Add(toAdd);
                return true;
            }
        }

        public int Id()
        {
            return id;
        }

        public string Name()
        {
            return name;
        }

        public string MemberName()
        {
            return memberName;
        }

        public string Password()
        {
            return this.password;
        }

        public string Avatar()
        {
            return avatar;
        }

        public int Points()
        {
            return points;
        }

        public int Money()
        {
            return money;
        }

        public List<Notification> WaitListNotification()
        {
            return waitListNotification;
        }

        public string Email()
        {
            return email;
        }

        public List<Tuple<int, int>> GamesAvailableToReplay()
        {
            return _gamesAvailableToReplay;
        }

        public List<IGame> ActiveGameList()
        {
            return activeGameList;
        }

        public List<IGame> SpectateGameList()
        {
            return spectateGameList;
        }

        public int WinNum()
        {
            return this.winNum;
        }

        public bool IncWinNum()
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    this.winNum++;
                    toReturn = true;
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public int Rank()
        {
            return rank;
        }

        public bool Login()
        {
            bool toReturn = false;
            try
            {
                this.isActive = true;
                toReturn = true;
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
            return toReturn;
        }

        public bool Logout()
        {
            bool toReturn = false;
            try
            {
                if (isActive == true)
                {
                    this.isActive = false;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
            
        }

        public bool EditId(int Id)
        {
            bool toReturn = false;
            try
            {
                if (IsValidInputNotSmallerZero(Id))
                {
                    this.id = Id;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditEmail(string email)
        {
            bool toReturn = false;
            try
            {
                if(IsValidEmail(email))
                {
                    this.email = email;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditPassword(string password)
        {
            bool toReturn = false;
            try
            {
                if (IsValidPassword(password))
                {
                    this.password = password;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditUserName(string username)
        {
            bool toReturn = false;
            try
            {
                if (IsValidString(username))
                {
                    this.memberName = username;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditName(string name)
        {
            bool toReturn = false;
            try
            {
                if (IsValidString(name))
                {
                    this.memberName = name;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditAvatar(string path)
        {
            bool toReturn = false;
            try
            {
                if (IsValidString(path))
                {
                    this.avatar = path;
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                toReturn = false;
                return toReturn;
            }
        }

        public bool EditUserPoints(int point)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (IsValidInputNotSmallerZero(point))
                    {
                        this.points = point;
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool ReduceMoneyIfPossible(int amount)
        {
            lock (padlock)
            {
                if (money - amount >= 0)
                {
                    money -= amount;
                    return true;
                }
                return false;
            }
        }

        public void AddMoney(int amount)
        {
            lock (padlock)
            {
                money += amount;
            }
        }

        public bool EditUserMoney(int money)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (IsValidInputNotSmallerZero(money))
                    {
                        this.money = money;
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool EditUserRank(int Rank)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (IsValidInputNotSmallerZero(Rank))
                    {
                        this.rank = Rank;
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool RemoveRoomFromActiveGameList(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && activeGameList.Contains(game))
                    {
                        this.activeGameList.Remove(game);
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool RemoveRoomFromSpectetorGameList(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && spectateGameList.Contains(game))
                    {
                        this.spectateGameList.Remove(game);
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool HasThisActiveGame(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && activeGameList.Contains(game))
                    {
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool HasThisSpectetorGame(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && spectateGameList.Contains(game))
                    {
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool AddRoomToActiveGameList(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && !activeGameList.Contains(game))
                    {
                        this.activeGameList.Add(game);
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool AddRoomToSpectetorGameList(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (game != null && !spectateGameList.Contains(game))
                    {
                        this.spectateGameList.Add(game);
                        toReturn = true;
                        return toReturn;
                    }
                    return toReturn;
                }
                catch
                {
                    toReturn = false;
                    return toReturn;
                }
            }
        }

        public bool IsLogin()
        {
            return isActive;
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

        public bool IsValidPassword(string password)
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
            catch
            {
                toReturn = false;
                return toReturn;
            }

            return toReturn;
        }

        private bool IsValidString(string s)
        {
            bool toReturn = false;
            try
            {
                bool valid = !s.Equals("") && !s.Equals(" ");
                if (valid)
                {
                    toReturn = true;
                    return toReturn;
                }
                return toReturn;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidInputNotSmallerEqualZero(int toCheck)
        {
            return toCheck >= 0;
        }

        private bool IsValidInputNotSmallerZero(int toCheck)
        {
            return toCheck >= 0;
        }
    }
}
