using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.Users
{
    public class User
    {
        private int id;
        private String name;
        private String memberName;
        private string password;
        //private ?String avatr - image path
        private int points;
        private int money;
        private List<Notification> waitListNotification;
        private String email;

        public User(int id, string name, string memberName, string password, int points, int money, String email)
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
                Console.WriteLine("this is not a valid email, please edit it");
            }
            this.waitListNotification = new List<Notification>();
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

        //function to recive notificaion - return the notification.
        internal bool SendNotification(Notification toSend)
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
        private bool AddNotificationToList(Notification toAdd)
        {
            this.waitListNotification.Add(toAdd);
            return true;
        }

        //use case - allow edit email 
        public bool EditEmail(String newEmail)
        {
            bool toReturn;
            bool valid = IsValidEmail(newEmail);
            if (valid)
            {
                this.email = newEmail;
                toReturn = true;
            }
            else
            {
                toReturn = false;
            }
            return toReturn;
        }
        
        //use case allow to change password
        public bool EditPassword(string newPassword)
        {
            bool toReturn;
            int len = newPassword.Length;
            if (len > 7 && len < 13)
            {
                toReturn = true;
                this.password = newPassword;
            }
            else
            {
                toReturn = false;
            }
            return toReturn;
        }

        private int IntLength(int i)
        {
            if (i < 0)
                throw new ArgumentOutOfRangeException();
            if (i == 0)
                return 1;
            return (int)Math.Floor(Math.Log10(i)) + 1;
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
    }
}
