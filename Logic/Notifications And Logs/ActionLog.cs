using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class ActionLog : Log
    {
        private int gameId;

        public ActionLog(int logId,int gameId) : base(logId)
        {
            this.gameId = gameId;
        }

        //getter setter
        public int GameId
        {
            get
            {
                return gameId;
            }

            set
            {
                gameId = value;
            }
        }
    }
}
