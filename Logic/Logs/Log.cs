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
        public enum LogPriority { Info, Warn, Error }
        public LogPriority Priority { get; set; }
        public Log()
        {
            this.logId = System.Threading.Interlocked.Increment(ref counter);
            this.Priority = LogPriority.Info;
        }

        public static int getNextId()
        {
            return counter + 1;
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
            string toReturn = "Log Id is: " + logId;
            return toReturn;
        }

    }
}
