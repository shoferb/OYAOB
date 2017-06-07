using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.GameControl
{
    public class LogControl
    {
        private LogDataProxy logDataProxy;
        private static readonly object padlock = new object();

        public LogControl()
        {
           logDataProxy = new LogDataProxy();
        }

        
      

        public void AddSystemLog(SystemLog log)
        {
            lock (padlock)
            {
                try
                {
                    int nextLogId = logDataProxy.GetNextLogId();
                    if (nextLogId == -2)
                    {
                        log.LogId = 1;
                    }
                    else if (nextLogId == -1)
                    {
                        return;
                    }
                    log.LogId = nextLogId;
                    logDataProxy.AddSysLog(log);
                }
                catch (Exception e)
                {
                    Console.WriteLine("in log control : error in insert systen log control");
                    return;
                }
            }
        }

        public void AddErrorLog(ErrorLog log)
        {
            lock (padlock)
            {
                try
                {
                    logDataProxy.AddErrorLog(log);
                }
                catch (Exception e)
                {
                    Console.WriteLine("in log control : error in insert error log control");
                    return;
                }
            }
        }

       
      

    }
}
