using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class Log
    {
        private static int counter = 0;
        private int logId;

        public Log()
        {
            this.logId = System.Threading.Interlocked.Increment(ref counter);
        }


        //getter Setter
        public int LogId
        {
            get
            {
                return logId;
            }

            set
            {
                logId = value;
            }
        }

        public string ToString()
        {
            string toReturn = "Log id is: " + logId;
            return toReturn;
        }

    }
}
