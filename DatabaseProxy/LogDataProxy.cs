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
        private readonly LogDataControler _logDataControler = new LogDataControler();
        public void AddErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            try
            {
                
                var errorlog = new Database.LinqToSql.ErrorLog();
                 errorlog =  ConvertErrorLog(error);

                _logDataControler.AddErrorLog(errorlog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

        }

        public void AddSysLog(Logic.Notifications_And_Logs.SystemLog sysLog)
        {
            try
            {
               var systemLog = new Database.LinqToSql.SystemLog();
                systemLog = ConvertSysLog(sysLog);
                _logDataControler.AddSystemLog(systemLog);
            }
            catch (Exception)
            {
                return;
            }

        }
        private Database.LinqToSql.ErrorLog ConvertErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            var toReturn = new Database.LinqToSql.ErrorLog();
            var log = new Database.LinqToSql.Log();
            log.LogId = error.LogId;
            log.LogPriority = GetPriorityNum(error.Priority);
            toReturn.Log = log;
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
            var log = new Database.LinqToSql.Log();
            log.LogId = sysLog.LogId;
            log.LogPriority = GetPriorityNum(sysLog.Priority);
            toReturn.Log = log;
            toReturn.msg = sysLog.Msg;
            toReturn.logId = sysLog.LogId;
            toReturn.game_Id = sysLog.GameId;
            toReturn.roomId = sysLog.RoomId;

            return toReturn;
        }
        private int GetPriorityNum(Logic.Notifications_And_Logs.Log.LogPriority priority)
        {
            var toReturn = -1;
            switch (priority)
            {
                case (Log.LogPriority.Info):
                    toReturn = 1;
                    break;
                case (Log.LogPriority.Warn):
                    toReturn = 2;
                    break;
                case (Log.LogPriority.Error):
                    toReturn = 3;
                    break;
                default:
                    toReturn = -1;
                    break;
            }
            return toReturn;
        }

     
    }
}
