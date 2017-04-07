using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class GameCenter
    {
        private List<League> leagueTable;
        private List<Log> logs;
        

        public GameCenter()
        {
            this.leagueTable = new List<League>();
            //add first league function
            this.logs = new List<Log>();
        }

        public bool SendNotification(User reciver,Notification toSend)
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
    }
}
