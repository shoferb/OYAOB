using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Database.DatabaseObject
{
    public class userDatabaseOb
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; } //- image path
        public int points { get; set; }
        public int money { get; set; }
        public int gamesPlayed { get; set; } //counter for "unknow use case if played less than 10 than his an "unknow"
        public int leagueName { get; set; }
        public int WinNum { get; set; }
        public int HighestCashGainInGame { get; set; }
        public int TotalProfit { get; set; }
        public bool inActive { get; set; }

        public userDatabaseOb(int _userId, string _username, string _name, string _email, string _password, String _avatar,
            int _points, int _money, int _gamesPlayed, int _leagueName, int _WinNum, int _HighestCashGainInGame,
            int _TotalProfit, bool _inActive)
        {
            this.userId = _userId;
            this.username = _username;
            this.name = _name;
            this.email = _email;
            this.password = _password;
            this.avatar = _avatar;
            this.points = _points;
            this.money = _money;
            this.gamesPlayed = _gamesPlayed;
            this.leagueName = _leagueName;
            this.WinNum = _WinNum;
            this.HighestCashGainInGame = _HighestCashGainInGame;
            this.TotalProfit = _TotalProfit;
            this.inActive = _inActive;
        }
    }
}
