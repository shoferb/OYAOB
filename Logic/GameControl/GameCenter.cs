using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class GameCenter
    {
        private List<League> leagueTable;
        private List<Log> logs;
        private User higherRank;
        private int leagueGap;
        private ReplayManager _replayManager;

        public bool LeagueChange(int leagugap)
        {
            bool toReturn = false;
            int higherRank = HigherRank.Points;
            leagueTable = null;
            int currpoint = 0;
            int i = 1;
            int to = 0;
            String leaugeName;
            while (currpoint < higherRank)
            {
                leaugeName = "" + i;
                to = currpoint + leagueGap;
                League toAdd = new League(leaugeName, currpoint, to);
                i++;
                currpoint = to;
            }
            return toReturn;
        }

        public string UserLeageInfo(User user)
        {
            string toReturn = "";
            int userRank = user.Points;
            int i = 1;
            int min = 0;
            int max = leagueGap;
            bool flag = ((userRank > min) && (userRank < max));
            while (!flag)
            {
                if ((userRank > min) && (userRank < max))
                {
                    flag = true;
                    toReturn = "" + i;
                }
                else
                {
                    min = max + 1;
                    max = min + leagueGap;
                }
            }
            return toReturn;
        }
        public GameCenter()
        {
            this.leagueTable = new List<League>();
            //add first league function
            this.logs = new List<Log>();
            _replayManager = new ReplayManager();
        }

        public bool SendNotification(User reciver, Notification toSend)
        {
            bool toReturn = false;
            reciver.SendNotification(toSend);
            return toReturn;
        }

        //getters setters
        public List<League> LeagueTable
        {
            get
            {
                return leagueTable;
            }

            set
            {
                leagueTable = value;
            }
        }
        public List<Log> Logs
        {
            get
            {
                return logs;
            }

            set
            {
                logs = value;
            }
        }

        public int LeagueGap
        {
            get
            {
                return LeagueGap;
            }

            set
            {
                LeagueGap = value;
            }
        }



        public User HigherRank
        {
            get
            {
                return higherRank;
            }

            set
            {
                higherRank = value;
            }
        }
    }
}
