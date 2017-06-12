using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;
using Log = TexasHoldem.Logic.Notifications_And_Logs.Log;

namespace TexasHoldemTests.Database.DataControlers
{
    public class LogsOnlyForTest
    {
        private TexasHoldem.Database.LinqToSql.ErrorLog ConvertErrorLog(TexasHoldem.Logic.Notifications_And_Logs.ErrorLog error)
        {
            TexasHoldem.Database.LinqToSql.ErrorLog toReturn = new TexasHoldem.Database.LinqToSql.ErrorLog();

            toReturn.Log.LogPriority = GetPriorityNum(error.Priority).PriorityValue;
            toReturn.msg = error.Msg;
            toReturn.logId = error.LogId;
            return toReturn;
        }

        private TexasHoldem.Database.LinqToSql.SystemLog ConvertSysLog(TexasHoldem.Logic.Notifications_And_Logs.SystemLog sysLog)
        {
            TexasHoldem.Database.LinqToSql.SystemLog toReturn = new TexasHoldem.Database.LinqToSql.SystemLog();
            toReturn.Log.LogId = sysLog.LogId;
            toReturn.Log.PriorityLogEnum = GetPriorityNum(sysLog.Priority);
            toReturn.msg = sysLog.Msg;
            toReturn.logId = sysLog.LogId;
            toReturn.roomId = sysLog.RoomId;
            return toReturn;
        }
        private PriorityLogEnum GetPriorityNum(TexasHoldem.Logic.Notifications_And_Logs.Log.LogPriority priority)
        {
            PriorityLogEnum toReturn = new PriorityLogEnum();
            switch (priority)
            {
                case (TexasHoldem.Logic.Notifications_And_Logs.Log.LogPriority.Info):
                    toReturn.PriorityValue = 1;
                    toReturn.ProprityName = "Info";
                    break;
                case (TexasHoldem.Logic.Notifications_And_Logs.Log.LogPriority.Warn):
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
        public void DeleteLog(int logId)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.DeleteLogById(logId);
                }
            }
            catch (Exception e)
            {
                return;
            }

        }

        public void DeleteErrorLog(int logId)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.DeleteErrorLogById(logId);
                }
            }
            catch (Exception e)
            {
                return;
            }

        }

        public void DeleteSystemLog(int logId)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {

                    db.DeleteSystemLogById(logId);
                }
            }
            catch (Exception e)
            {
                return;
            }

        }
        public GetErrorLogByIdResult GetErrorLogById(int logId)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    GetErrorLogByIdResult res = db.GetErrorLogById(logId).ToList().First();
                    return res;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public GetSystemLogByIdResult GetSystemLogById(int logId)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    GetSystemLogByIdResult res = db.GetErrorLogById(logId).ToList().First();
                    return res;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<TexasHoldem.Database.LinqToSql.Log> GetAllLogs()
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    return db.Logs.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<TexasHoldem.Database.LinqToSql.ErrorLog> GetAllErrorLogs()
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    return db.ErrorLogs.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<TexasHoldem.Database.LinqToSql.SystemLog> GetAllSystemLogs()
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    return db.SystemLogs.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
