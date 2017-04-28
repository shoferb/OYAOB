using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.GameControl
{
    public class LogControl
    {

        private List<ErrorLog> errorLog;
        private List<SystemLog> systemLog;

        private static LogControl instance = null;


        private static readonly object padlock = new object();

        private LogControl()
        {
            this.errorLog = new List<ErrorLog>();
            this.systemLog = new List<SystemLog>();
        }

        public static LogControl Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LogControl();
                    }
                    return instance;
                }
            }
        }

        
        public Log FindLog(int logId)
        {
            lock (padlock)
            {
                Log toReturn = null;
                bool found = false;
                foreach (SystemLog sl in systemLog)
                {
                    if (sl.LogId == logId)
                    {
                        toReturn = sl;
                        found = true;
                    }
                }
                if (!found)
                {
                    foreach (ErrorLog el in errorLog)
                    {
                        if (el.LogId == logId)
                        {
                            toReturn = el;
                            found = true;
                        }
                    }
                }
                return toReturn;
            }
        }

        public void AddSystemLog(SystemLog log)
        {
            lock (padlock)
            {
                systemLog.Add(log);
            }
        }

        public void AddErrorLog(ErrorLog log)
        {
            lock (padlock)
            {
                errorLog.Add(log);
            }
        }

        public void RemoveErrorLog(ErrorLog log)
        {
            lock (padlock)
            {
                errorLog.Remove(log);
            }
        }

        public void RemoveSystenLog(SystemLog log)
        {
            lock (padlock)
            {
                systemLog.Remove(log);
            }
        }

    }
}
