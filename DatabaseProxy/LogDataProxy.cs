using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic.Notifications_And_Logs;
using Log = TexasHoldem.Logic.Notifications_And_Logs.Log;

namespace TexasHoldem.DatabaseProxy
{
    public class LogDataProxy
    {
        public void AddErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            try
            {
                LogDataControler udp = new LogDataControler();
                Database.LinqToSql.ErrorLog errorlog = ConvertErrorLog(error);
                udp.AddErrorLog(errorlog);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in log proxy : error log insert fail");
                return;
            }

        }

        public void AddSysLog(Logic.Notifications_And_Logs.SystemLog sysLog)
        {
            try
            {
                LogDataControler udp = new LogDataControler();
                Database.LinqToSql.SystemLog systemLog = ConvertSysLog(sysLog);
                udp.AddSystemLog(systemLog);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in log proxy : systrm log insert fail");
                return;
            }

        }
        private Database.LinqToSql.ErrorLog ConvertErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            Database.LinqToSql.ErrorLog toReturn = new Database.LinqToSql.ErrorLog();
            toReturn.Log.LogId = error.LogId;
            toReturn.Log.PriorityLogEnum = GetPriorityNum(error.Priority);
            toReturn.msg = error.Msg;
            toReturn.logId = error.LogId;
            return toReturn;
        }

        public int GetNextLogId()
        {
            LogDataControler ldc = new LogDataControler();
            return ldc.GetNextLogId();
        }
        private Database.LinqToSql.SystemLog ConvertSysLog(Logic.Notifications_And_Logs.SystemLog sysLog)
        {
            Database.LinqToSql.SystemLog toReturn = new Database.LinqToSql.SystemLog();
            toReturn.Log.LogId = sysLog.LogId;
            toReturn.Log.PriorityLogEnum = GetPriorityNum(sysLog.Priority);
            toReturn.msg = sysLog.Msg;
            toReturn.logId = sysLog.LogId;
            toReturn.roomId = sysLog.RoomId;
            return toReturn;
        }
        private PriorityLogEnum GetPriorityNum(Logic.Notifications_And_Logs.Log.LogPriority priority)
        {
            PriorityLogEnum toReturn = new PriorityLogEnum();
            switch (priority)
            {
                case (Log.LogPriority.Info):
                    toReturn.PriorityValue = 1;
                    toReturn.ProprityName = "Info";
                    break;
                case (Log.LogPriority.Warn):
                    toReturn.PriorityValue = 2;
                    toReturn.ProprityName = "Warnning";
                    break;
                case (Log.LogPriority.Error):
                    toReturn.PriorityValue = 3;
                    toReturn.ProprityName = "Error";
                    break;
            }
            return toReturn;
        }

     
    }
}
