using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic.Users
{
    public class User
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
        private List<Tuple<int, int>> _gamesAvailableToReplay { get; set; }
        private List<GameRoom> activeGameList;
        private List<GameRoom> spectateGameList;
        public bool IsHigherRank { get; set; }
        //להוסיף שדות מספר נצחונות והפסדים
        //todo create toString
        public User(int id, string name, string memberName, string password, int points, int money, string email)
        {
            this.id = id;
            this.name = name;
            this.memberName = memberName;
            this.password = password;
            this.points = points;
            this.money = money;
            if (IsValidEmail(email))
            {
                this.email = email;
            }
            else
            {
                //Console.WriteLine("this is not a valid email, please edit it");
            }
            this.WaitListNotification = new List<Notification>();
            this.isActive = false;
            this.IsHigherRank = false;
            this.avatar = "path?";
            _gamesAvailableToReplay = new List<Tuple<int,int>>();
            activeGameList = new List<GameRoom>();
            spectateGameList = new List<GameRoom>();

        }

        public bool IsValidEmail(string email)
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

        //function to recive notificaion - return the notification.
        public bool SendNotification(Notification toSend)
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


        //private method - add the notification to list so can print when not in game
        public bool AddNotificationToList(Notification toAdd)
        {
            this.WaitListNotification.Add(toAdd);
            return true;
        }

        public bool AddGameAvailableToReplay(int roomID, int gameID)
        {
            Tuple<int, int> tup = new Tuple<int,int>(roomID, gameID);
            if (_gamesAvailableToReplay.Contains(tup))
            {
                return false;
            }
            _gamesAvailableToReplay.Add(tup);
            return true;
        }


        //getters setters
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public string MemberName
        {
            get
            {
                return memberName;
            }

            set
            {
                memberName = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        public int Points
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
            }
        }
        public int Money
        {
            get
            {
                return money;
            }

            set
            {
                money = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }

        public List<Notification> WaitListNotification
        {
            get
            {
                return waitListNotification;
            }

            set
            {
                waitListNotification = value;
            }
        }

        public string Avatar
        {
            get
            {
                return avatar;
            }

            set
            {
                avatar = value;
            }
        }

        public List<GameRoom> SpectateGameList
        {
            get
            {
                return spectateGameList;
            }

            set
            {
                spectateGameList = value;
            }
        }

        public List<GameRoom> ActiveGameList
        {
            get
            {
                return activeGameList;
            }

            set
            {
                activeGameList = value;
            }
        }
        
    }
}
