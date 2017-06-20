using System;
using System.Collections.Generic;
using System.Net.Mail;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;

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
     
        private string email;
        private bool isActive;
        public List<Tuple<int, int>> _gamesAvailableToReplay { get; set; }
        private readonly List<IGame> activeGameList;
        private List<IGame> spectateGameList;
        public int unknowGamesPlay; //counter for "unknow use case if played less than 10 than his an "unknow"
        private LeagueName league;
        public int WinNum { get; set; }
        public int LoseNum { get; set; }
        public int HighestCashGainInGame { get; set; }
        public int TotalProfit { get; set; }
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
            WinNum = 0;
            LoseNum = 0;
            HighestCashGainInGame = 0;
            TotalProfit = 0;
          
            isActive = true;
            avatar = "/GuiScreen/Photos/Avatar/devil.png";
            _gamesAvailableToReplay = new List<Tuple<int, int>>();
            activeGameList = new List<IGame>();
            spectateGameList = new List<IGame>();
            unknowGamesPlay = 0;
            league = LeagueName.Unknow; //TODO change to default one or something
        }

        public User(int id, string name, string memberName, string password, int points, int money, 
            string email, int winNum, int loseNum, int highestCashGainInGame, int totalProfit,string _avatar, int gamesPlayed, bool _isActive,LeagueName _league)
        {
            this.id = id;
            this.name = name;
            this.memberName = memberName;
            this.password = password;
            this.points = points;
            this.money = money;
            this.email = email;
            WinNum = winNum;
            LoseNum = loseNum;
            HighestCashGainInGame = highestCashGainInGame;
            TotalProfit = totalProfit;
           
            isActive = _isActive;
            avatar = _avatar;
            _gamesAvailableToReplay = new List<Tuple<int,int>>();
            activeGameList = new List<IGame>();
            spectateGameList = new List<IGame>();
            unknowGamesPlay = gamesPlayed;
            league = _league;
            
        }

      
        private UserDataProxy userDataProxy = new UserDataProxy();
        public int GetNumberOfGamesUserPlay()
        {
            return this.unknowGamesPlay;
        }
        //return true if play in ess than 11 games.
        public bool IsUnKnow()
        {
            lock (padlock)
            {
                return userDataProxy.GetUserById(id).GetNumberOfGamesUserPlay() <= 10;
          //      return unknowGamesPlay <= 10;
            }
        }

        public bool SetPoints(int amount)
        {
            if (amount > 0)
            {
                points += amount;
                userDataProxy.EditUserPoints(id,points);
                return true;
            }

            return false;
        }

        //inc num of games play
        public bool IncGamesPlay()
        {
            lock (padlock)
            {
                userDataProxy.EditUserNumOfGamesPlayed(id, unknowGamesPlay+1);
                unknowGamesPlay++;
                
                if (unknowGamesPlay > 10 && league == LeagueName.Unknow)
                {
                    league = LeagueName.E;
                    userDataProxy.EditUserLeagueName(id,LeagueName.E);
                }
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
            return password;
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

        public void UpdateHighestCashInGame(int cashToChck)
        {
            if (HighestCashGainInGame < cashToChck)
            {
                HighestCashGainInGame = cashToChck;
                userDataProxy.EditUserHighestCashGainInGame(id,cashToChck);
            }
        }

        public void UpdateTotalProfit(int profit)
        {
            TotalProfit += profit;
            userDataProxy.EditUserTotalProfit(id,TotalProfit);
        }

        //avg profit per win
        public double GetAvgProfit()
        {
            IUser t = userDataProxy.GetUserById(id);
            if (t.WinNum != 0)
            {
                return (double) t.TotalProfit / t.WinNum;
            }
            return 0.0;
        }

        //avg profit in all games
        public double GetAvgCashGainPerGame()
        {
            IUser t = userDataProxy.GetUserById(id);
            if (t.GetNumberOfGamesUserPlay() != 0)
            {
                return (double)t.TotalProfit / (t.GetNumberOfGamesUserPlay());
            }
            return 0.0;
        }

        public bool IncWinNum()
        {
            lock (padlock)
            {
                try
                {

                    WinNum++;
                    userDataProxy.EditUserWinNum(id, WinNum);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool IncLoseNum()
        {
            lock (padlock)
            {
                try
                {
                    LoseNum++;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Login()
        {
            try
            {
                isActive = true;
                return isActive;
            }
            catch
            {
                return false;
            }
          
        }

        public bool Logout()
        {
            bool toReturn = false;
            try
            {
                if (isActive)
                {
                    isActive = false;
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
                    id = Id;
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
                    memberName = username;
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
                    memberName = name;
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
                    avatar = path;
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
                        points = point;
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
                    userDataProxy.EditUserMoney(id,money);
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
                userDataProxy.EditUserMoney(id,money);
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

        public bool RemoveRoomFromActiveGameList(IGame game)
        {
            lock (padlock)
            {
                bool toReturn = false;
                try
                {

                    if (game != null && activeGameList.Contains(game))
                    {
                        activeGameList.Remove(game);
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
                        spectateGameList.Remove(game);
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
                    if (game != null)
                    {
                        bool alreadyIn = HasThisActiveGame(game);
                        if (!alreadyIn)
                        {
                            userDataProxy.AddGameToUserActiveGames(this.Id(), game.Id, game.GameNumber);
                            return true;
                        }

                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool AddRoomToSpectetorGameList(IGame game)
        {
            lock (padlock)
            {
                try
                {
                    if (game != null)
                    {
                        bool alreadyIn = HasThisSpectetorGame(game);
                        if (!alreadyIn)
                        {
                            userDataProxy.AddGameToUserSpectetorGames(this.id, game.Id, game.GameNumber);
                            return true;
                        }

                    }
                    return false;

                }
                catch
                {
                    return false;
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
                var addr = new MailAddress(email);
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

        public LeagueName GetLeague()
        {
            return league;
        }

        public void SetLeague(LeagueName league)
        {
            this.league = league;
        }

        public bool HasEnoughMoney(int startingChip, int fee)
        {
            IUser t = userDataProxy.GetUserById(id);
            return (t.Money() - startingChip - fee >= 0);
        }

    }
}
