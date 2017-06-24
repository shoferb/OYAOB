using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Database.LinqToSql;
using Log = TexasHoldem.Logic.Notifications_And_Logs.Log;

namespace TexasHoldemTests.Database.DataControlers
{
    public class LogsOnlyForTest
    {
        private ErrorLog ConvertErrorLog(TexasHoldem.Logic.Notifications_And_Logs.ErrorLog error)
        {
            ErrorLog toReturn = new ErrorLog();

            toReturn.Log.LogPriority = GetPriorityNum(error.Priority).PriorityValue;
            toReturn.msg = error.Msg;
            toReturn.logId = error.LogId;
            return toReturn;
        }

        private SystemLog ConvertSysLog(TexasHoldem.Logic.Notifications_And_Logs.SystemLog sysLog)
        {
            SystemLog toReturn = new SystemLog();
            toReturn.Log.LogId = sysLog.LogId;
            toReturn.Log.PriorityLogEnum = GetPriorityNum(sysLog.Priority);
            toReturn.msg = sysLog.Msg;
            toReturn.logId = sysLog.LogId;
            toReturn.roomId = sysLog.RoomId;
            return toReturn;
        }
        private PriorityLogEnum GetPriorityNum(Log.LogPriority priority)
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
            }

        }
        public GetErrorLogByIdResult GetErrorLogById(int logId)
        {

            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    var res = db.GetErrorLogById(logId).ToList().First();
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
                    GetSystemLogByIdResult res = db.GetSystemLogById(logId).ToList().First();
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

        public List<ErrorLog> GetAllErrorLogs()
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

        public List<SystemLog> GetAllSystemLogs()
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

        public List<int> GetSysLogIdsByRoomId(int roomId)
        {
            var allSysLogs = GetAllSystemLogs();
            List<int> ids = allSysLogs.FindAll(log => log.roomId == roomId)
                .ConvertAll(log => log.logId);
            return ids;
        }

        
    }
}
