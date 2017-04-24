using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class SystemControl
    {
        private List<User> users;

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
            this.users = new List<User>();
        }

        //getter seeter user list
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

        /*
        public Tuple<int, int> RankGapOfUser(int userID)
        {
            Tuple<int, int> toReturn;
            User user = GetUserWithId(userID);

            return toReturn;
        */


        //add new user  - syncronized
        public bool AddNewUser(User newUser)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (newUser == null)
                {
                    return toReturn;
                }
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
        }

        //return null if one of the field is not valid
        public User CreateNewUser(int id, string name, string memberName,
            string password, string email,int money)
        {
            User toReturn = null;
            if (CanCreateNewUser(id, memberName, password, email) && IsValidInputNotSmallerZero(money))
            {
                toReturn = new User(id, name, memberName, password, 0, money, email);
            }

            
            return toReturn;
        }

        //remove user from user list byID - syncronized
        public bool RemoveUserById(int id)
        {
            lock (padlock)
            {
                bool toReturn = false;
                User original = GetUserWithId(id);
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
            User toRemove = null;
            bool found = false;
            lock (padlock)
            {
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
        }


        //remove user - syncronized
        public bool RemoveUser(User toRemove)
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


        //?to remove - change 2 user in list nade to swap chcnges of user into list - synctonized
       /* public bool ReplaceUser(User oldUser, User newUser)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (oldUser == null || newUser == null)
                {
                    return toReturn;
                }
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i] == oldUser)
                    {
                        Users[i] = null;
                        users[i] = newUser;
                        toReturn = true;
                    }
                }
                return toReturn;
            }
        }*/


        //find user by user name - syncronized (due to foreatch)
        //return null if not found or user name is "" || " "
        public User FindUser(string username)
        {
            lock (padlock)
            {
                User toRerutn = null;
                if (username.Equals("")|| username.Equals(" "))
                {
                    return toRerutn;
                }
                try
                {
                    
                    foreach (User u in users)
                    {
                        if (u.MemberName.Equals(username))
                        {
                            toRerutn = u;
                        }
                    }
                }
                catch (Exception e)
                {
                    return toRerutn;
                }
                
                return toRerutn;
            }
        }



        //login to system- syncronized
        public bool Login(string UserName, string password)
        {
            bool toReturn = false;
            User original = FindUser(UserName);
            if (UserName.Equals("") || UserName.Equals(" "))
            {
                return toReturn;
            }
            if (original == null)
            {
                return toReturn;

            }
            lock (padlock)
            {
                foreach (User u in users)
                {
                    if ((u.Password.Equals(password)) && (u.MemberName.Equals(UserName)))
                    {
                        User toAdd = original;
                        toAdd.IsActive = true;
                        // ReplaceUser(original, toAdd);
                        toReturn = true;
                    }
                }
                return toReturn;
            }
        }


        //user logout from system - syncronized
        public bool Logout(int id)
        {
            bool toReturn = false;
            if (!IsValidInputNotSmallerZero(id))
            {
                return toReturn;
            }
            User original = GetUserWithId(id);
            User changed = original;
            if (original == null)
            {
                return toReturn;

            }
            lock (padlock)
            {
                changed.IsActive = false;
                //toReturn = ReplaceUser(original, changed);
                toReturn = true;
                return toReturn;
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
                toReturn = AddNewUser(newUser);
                return toReturn;
            }
        }

        public Player GetPlayer(int userId, int roomId)
        {
            lock (padlock)
            {
                Player toReturn = null;
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                bool roomExist = GameCenter.Instance.IsRoomExist(roomId);
                if (!roomExist)
                {
                    return toReturn;
                }
                GameRoom room = GameCenter.Instance.GetRoomById(roomId);
                if (room == null)
                {
                    return toReturn;
                }
                if (!HasThisActiveGame(roomId, userId))
                {
                    return toReturn;
                }
                foreach (Player player in room._players)
                {
                    if (player.Id == userId)
                    {
                        toReturn = player;
                    }
                }
                return toReturn;
            }
        }

        public bool CanCreateNewUser(int id, string memberName,
            string password, string email)
        {
            //todo - add loger

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
                foreach (User u in users)
                {
                    if (u.MemberName.Equals(username))
                    {
                        toReturn = false;
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
                foreach (User u in users)
                {
                    if (u.Id ==  ID)
                    {
                        toReturn = false;
                    }
                }
                return toReturn;
            }
        }
        //return true if user with id exist 
        //syncronized - due to foreatch
        public bool IsUserWithId(int id)
        {
            lock (padlock)
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
        }


        //get user by id - null if not exist / invalid id
        //syncronized - due to for
        public User GetUserWithId(int id)
        {
            lock (padlock)
            {
                User toReturn = null;
                if (!IsValidInputNotSmallerZero(id))
                {
                    return toReturn;
                }
                
                foreach (User u in users)
                {
                    if (u.Id == id)
                    {
                        toReturn = u;
                    }
                }
                return toReturn;
            }
        }


        //check if email valid according to .NET convention
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


        //use case - allow edit email - syncronized
        public bool EditEmail(int id, string newEmail)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }
                    User changed = toEdit;
                    
                    bool valid = IsValidEmail(newEmail);
                    if (valid)
                    {
                        changed.Email = newEmail;
                        // toReturn = ReplaceUser(toEdit, changed);
                        toReturn = true;
                    }
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                /*
                else
                {
                    toReturn = false;
                }
                */
                return toReturn;
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
            catch (Exception e)
            {
                toReturn = false;
                return toReturn;
            }

            return toReturn;
        }

        
      
        //use case allow to change password - syncronized
        public bool EditPassword(int id, string newPassword)
        {
            lock (padlock)
            {
                bool toReturn= false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }
                    User changed = toEdit;
                    bool valid = IsValidPassword(newPassword);
                    if (!valid)
                    {
                        return toReturn;
                    }
                    changed.Password = newPassword;
                    //toReturn = ReplaceUser(toEdit, changed);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                    return toReturn;
                }
                
                return toReturn;
            }
        }


       
        //not use leave only namy for future use
        public bool EditUserName(int id, string newUserName)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }
                    User changed = toEdit;

                    bool validname = IsUsernameFree(newUserName);
                    if (validname)
                    {
                        changed.MemberName = newUserName;
                        //toReturn = ReplaceUser(toEdit, changed);
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
        }


        //edit avatar pic path - syncronized
        public bool EditAvatar(int id, string newAvatarPath)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(id))
                {
                    return toReturn;
                }
                User toEdit = GetUserWithId(id);
                if (toEdit == null)
                {
                    return toReturn;
                }
                try
                {
                    toEdit.Avatar = newAvatarPath;
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                    return toReturn;
                }
                return toReturn;
            }
        }


       
        //edit user id - syncronized
        public bool EditUserID(int id, int newId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(id) || !IsValidInputNotSmallerEqualZero(newId))
                {
                    return toReturn;
                }
                
                try
                {
                    bool isFree = IsIdFree(newId);
                    if (isFree)
                    {
                        User toEdit = GetUserWithId(id);
                        if (toEdit == null)
                        {
                            return toReturn;
                        }
                        User changed = toEdit;
                        changed.Id = newId;
                        //toReturn = ReplaceUser(toEdit, changed);
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
        }


        //edit user point - syncronized
        
        public bool EditUserPoints(int id, int newPoints)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    if (!IsValidInputNotSmallerZero(newPoints))
                    {
                        return toReturn;
                    }
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }

                    toEdit.Points = newPoints;
                    IsHigestRankUser(id);
                    
                    //toReturn = ReplaceUser(toEdit, changed);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
               
                return toReturn;
            }
        }


        //syncronized
        
        public bool EditUserMoney(int id, int newMoney)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    if (!IsValidInputNotSmallerZero(newMoney))
                    {
                        return toReturn;
                    }
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }
                    User changed = toEdit;

                    changed.Money = newMoney;
                    //toReturn = ReplaceUser(toEdit, changed);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                
                return toReturn;
            }
        }


        //syncronized - cange when user login / logout
        public bool EditActiveGame(int id, bool activemode)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {
                    if (!IsValidInputNotSmallerZero(id))
                    {
                        return toReturn;
                    }
                    
                    User toEdit = GetUserWithId(id);
                    if (toEdit == null)
                    {
                        return toReturn;
                    }
                    User changed = toEdit;

                    changed.IsActive = activemode;
                    //toReturn = ReplaceUser(toEdit, changed);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //remove room form user active game list - syncronized
        public bool RemoveRoomFromActiveRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn =false;
                try
                {
                    if (!IsValidInputNotSmallerZero(userId))
                    {
                        return toReturn;
                    }
                    if (!IsValidInputNotSmallerZero(roomId))
                    {
                        return toReturn;
                    }
                    bool roomExist = GameCenter.Instance.IsRoomExist(roomId);
                    if (!roomExist)
                    {
                        return toReturn;
                    }
                    
                    
                    User user = GetUserWithId(userId);
                    if (user == null)
                    {
                        return toReturn;
                    }
                    if (!HasThisActiveGame(roomId, userId))//user dont have this game
                    {
                        return toReturn;
                    }
                    GameRoom toRemove = GameCenter.Instance.GetRoomById(roomId);
                    
                    user.ActiveGameList.Remove(toRemove);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //remove room from user specteted game list - sncromized
        public bool RemoveRoomFromSpectetRoom(int roomId, int userId)
        {
            bool toReturn=false;
            lock (padlock)
            {
                try
                {
                    if (!IsValidInputNotSmallerZero(userId))
                    {
                        return toReturn;
                    }
                    if (!IsValidInputNotSmallerZero(roomId))
                    {
                        return toReturn;
                    }
                    bool roomExist = GameCenter.Instance.IsRoomExist(roomId);
                    if (!roomExist)
                    {
                        return toReturn;
                    }


                    User user = GetUserWithId(userId);
                    if (user == null)
                    {
                        return toReturn;
                    }
                    if (!HasThisSpectetorGame(roomId, userId))//user dont have this game
                    {
                        return toReturn;
                    }
                    GameRoom toRemove = GameCenter.Instance.GetRoomById(roomId);
                   
                    user.SpectateGameList.Remove(toRemove);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //return if game is active game on is acrive game list
        public bool HasThisActiveGame(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                if (!GameCenter.Instance.IsRoomExist(roomId))
                {
                    return toReturn;
                }

                GameRoom toCheck = GameCenter.Instance.GetRoomById(roomId);
                if (toCheck == null)
                {
                    return toReturn;
                }
                User user = GetUserWithId(userId);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = user.ActiveGameList.Contains(toCheck);
                return toReturn;
            }
        }


        //return if game is spectetable game on is spectet game ist
        public bool HasThisSpectetorGame(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                if (!GameCenter.Instance.IsRoomExist(roomId))
                {
                    return toReturn;
                }

                GameRoom toCheck = GameCenter.Instance.GetRoomById(roomId);
                if (toCheck == null)
                {
                    return toReturn;
                }
                User user = GetUserWithId(userId);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = user.SpectateGameList.Contains(toCheck);
                return toReturn;
            }
        }


        //get all active games of user 
        //syncronizef due to for
        public List<GameRoom> GetActiveGamesByUserName(string userName)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = null;
                if (userName.Equals("")||userName.Equals(" ")|| IsUsernameFree(userName))
                {
                    return toReturn;
                }
               
                User user = FindUser(userName);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = new List<GameRoom>();
                foreach (GameRoom room in user.ActiveGameList)
                {
                    if (room._isActiveGame)
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }


        //get all the game user spectete
        //syncronized due to for
        public List<GameRoom> GetSpectetorGamesByUserName(string userName)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = null;
                if (userName.Equals("") || userName.Equals(" ") || IsUsernameFree(userName))
                {
                    return toReturn;
                }

                User user = FindUser(userName);
                if (user == null)
                {
                    return toReturn;
                }
                toReturn = new List<GameRoom>();
                foreach (GameRoom room in user.ActiveGameList)
                {
                    if (room._isSpectetor)
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }


        //return true if user is with the highest rank
        public bool IsHigestRankUser(int userId)
        {
            bool toReturn = false;
            try
            { 
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                
                User user = GetUserWithId(userId);
                
                if (user == null)
                {
                    return toReturn;
                }
                //List<User> byPoint = SortUserByPoint();
                //if (byPoint[0] == user)
                //{
                    if (GameCenter.Instance.HigherRank == null)
                    {
                        GameCenter.Instance.HigherRank = user;
                        user.IsHigherRank = true;
                        toReturn = true;
                        return toReturn;
                    }
                    
                    if(GameCenter.Instance.HigherRank != null)
                    {

                        if (user.Points > GameCenter.Instance.HigherRank.Points)
                        {
                            GameCenter.Instance.HigherRank.IsHigherRank = false;
                            GameCenter.Instance.HigherRank = user;
                            user.IsHigherRank = true;
                            toReturn = true;
                            return toReturn;
                        
                        }
                        int userPoint = user.Points;
                        int highPoint = GameCenter.Instance.HigherRank.Points;
                        if ((userPoint == highPoint) && (user.Id == GameCenter.Instance.HigherRank.Id))
                        {
                            return true;
                        }
                }
                //}
  
            }
            catch (Exception e)
            {
                toReturn = false;
                return toReturn;
            }
            
            return toReturn;
        }

        public List<User> GetAllUser()
        {
            lock (padlock)
            {
                return users;
            }
        }

        //change the gap and change league table
        //syncronized
        public bool ChangeGapByHighestUserAndCreateNewLeague(int userId, int newGap)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(userId) || IsIdFree(userId))
                {
                    return toReturn;
                }
                User user = GetUserWithId(userId);
                bool isHighest = user.IsHigherRank;
                if (isHighest)
                {
                    toReturn = GameCenter.Instance.EditLeagueGap(newGap);
                    bool change = GameCenter.Instance.LeagueChangeAfterGapChange(newGap);
                    toReturn = (toReturn && change);
                    return toReturn;
                }
                
                return toReturn;
            }
        }

        public List<User> SortByRank()
        {
            lock (padlock)
            {
                List<User> sort = GetAllUser();
                sort.Sort(delegate(User x, User y)
                {
                    return y.Points.CompareTo(x.Points);
                });
                return sort;
            }
        }

        
        //return -1 if error
        public int GetUserRank(int userId)
        {
            lock (padlock)
            {
                int toReturn;
                try
                {
                    if (!IsValidInputNotSmallerZero(userId))
                    {
                        return -1;
                    }
                    List<User> sort = SortByRank();
                    User user = GetUserWithId(userId);
                    if (user == null)
                    {
                        return -1;
                    }
                    toReturn = sort.IndexOf(user)+1;
                    user.rank = toReturn+1;
                    
                }
                catch (Exception e)
                {
                    return -1;
                }
                return toReturn;
            }
        }

       

        public bool MovePlayerBetweenLeague(int highestId, int userToMove, int newPoint)
        {
            lock (padlock)
            {
                bool toReturn = false;
                //check to see is the higest user
                if (!IsValidInputNotSmallerZero(highestId))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(userToMove))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(newPoint))
                {
                    return toReturn;
                }

                
                try
                {
                    User highest = GetUserWithId(highestId);
                    if (highest == null)
                    {
                        return toReturn;
                    }
                    if (!highest.IsHigherRank)
                    {
                        return toReturn;
                    }
                    User toChange = GetUserWithId(userToMove);
                    if (toChange == null)
                    {
                        return toReturn;
                    }
                    bool changedPoint = EditUserPoints(userToMove, newPoint);
                    
                    toReturn = changedPoint;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }

                return toReturn;
            }
        }

        public bool SetDefultLeauseToNewUsers(int highestId, int newPoint)
        {
            bool toReturn = false;
            //check to see is the higest user
            lock (padlock)
            {
                if (!IsValidInputNotSmallerZero(highestId))
                {
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(newPoint))
                {
                    return toReturn;
                }
                User user = GetUserWithId(highestId);
                if (!user.IsHigherRank)
                {
                    return toReturn;
                }
                try
                {
                    List<User> newUser = new List<User>();
                    foreach (User u in users)
                    {
                        if (u.Points == 0)
                        { 
                            newUser.Add(u);
                        }
                    }
                    int id = 0;
                    for (int i = 0; i < newUser.Count; i++)
                    {
                        
                        EditUserPoints(newUser[i].Id, newPoint);
                        
                    }

                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }

                return toReturn;
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
