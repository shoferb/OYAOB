using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class Notification
    {
        private int gameId;
        private String msg;

        public Notification(int gameId, string msg)
        {
            this.gameId = gameId;
            this.msg = msg;
        }
        
       
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

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }
    }
}
