using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;

namespace TexasHoldem.Database.DataControlers
{
    public class LogDataControler
    {
        public void AddErrorLog(ErrorLog error)
        {
            
            try
            {

                using (var db = new connectionsLinqDataContext())
                {
                    //Log log = new Log();
                    //log.LogId = error.logId;
                    //log.PriorityLogEnum = error.Log.PriorityLogEnum;
                    //db.Logs.InsertOnSubmit(log);
                    db.Logs.InsertOnSubmit(error.Log);
                    db.ErrorLogs.InsertOnSubmit(error);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                return ;
            }

        }

        public void AddSystemLog(SystemLog sysLog)
        {

            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    //Log log = new Log();
                    //log.LogId = error.logId;
                    //log.PriorityLogEnum = error.Log.PriorityLogEnum;
                    //db.Logs.InsertOnSubmit(log);
                    db.Logs.InsertOnSubmit(sysLog.Log);
                    db.SystemLogs.InsertOnSubmit(sysLog);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        public int GetNextLogId()
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    var allLogs = db.Logs.ToList();
                    if (allLogs.Count == 0)
                    {
                        return -2;
                    }
                    var logsOrderByDescending = allLogs.OrderByDescending(log => log.LogId);
                    int currMax = logsOrderByDescending.First().LogId;
                    return currMax + 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }

        }

        
    }
}
