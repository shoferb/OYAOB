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
        private List<GameRoom> activeGameList;
        private List<GameRoom> spectateGameList;
        private List<Actions.Action> _favActions { get; set; }
        public bool IsHigherRank { get; set; }
        public int rank { get; set; }

        public int winNum { get; set; }
       
     

        public User(int id, string name, string memberName, string password, int points, int money, string email)
        {
            this.id = id;
            this.name = name;
            this.memberName = memberName;
            this.password = password;
            this.points = points;
            this.money = money;
            this.email = email;
            this.IsHigherRank = false;
            this.WaitListNotification = new List<Notification>();
            this.isActive = false;
            this.IsHigherRank = false;
            this.avatar = "path?";
            _gamesAvailableToReplay = new List<Tuple<int,int>>();
            activeGameList = new List<GameRoom>();
            spectateGameList = new List<GameRoom>();
            _favActions = new List<Actions.Action>();
            this.winNum = 0;
            
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

        public bool AddActionToFavorite(Actions.Action action)
        {
            _favActions.Add(action);
            return true;
        }

       

        //getters setters
        public int Id
        {
            get
            {
                return id;
            }

        }
        public string Name
        {
            get
            {
                return name;
            }

        }
        public string MemberName
        {
            get
            {
                return memberName;
            }

        }
        public string Password
        {
            get
            {
                return password;
            }

            
        }
        public int Points
        {
            get
            {
                return points;
            }
            
        }
        public int Money
        {
            get
            {
                return money;
            }
            
        }
        public string Email
        {
            get
            {
                return email;
            }
            
        }
        public bool IsActive
        {
            get
            {
                return isActive;
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
            
        }

        public List<GameRoom> SpectateGameList
        {
            get
            {
                return spectateGameList;
            }

        }

        public List<GameRoom> ActiveGameList
        {
            get
            {
                return activeGameList;
            }

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
            catch (Exception e)
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
            catch (Exception e)
            {
                toReturn = false;
                return toReturn;
            }
            
        }

        public bool EditEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool EditPassword(string password)
        {
            throw new NotImplementedException();
        }

        public bool EditUserName(string username)
        {
            throw new NotImplementedException();
        }

        public bool EditAvatar(string path)
        {
            throw new NotImplementedException();
        }

        public bool EditUserPoint(int point)
        {
            throw new NotImplementedException();
        }

        public bool EditUserMoney(int money)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoomFromActiveGameList(IGame game)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoomFromSpectetorGameList(IGame game)
        {
            throw new NotImplementedException();
        }

        public bool HasThisActiveGame(IGame game)
        {
            throw new NotImplementedException();
        }

        public bool HasThisSpectetorGame(IGame game)
        {
            throw new NotImplementedException();
        }

        public bool AddRoomFromActiveGameList(IGame game)
        {
            throw new NotImplementedException();
        }

        public bool AddRoomFromSpectetorGameList(IGame game)
        {
            throw new NotImplementedException();
        }
    }
}
