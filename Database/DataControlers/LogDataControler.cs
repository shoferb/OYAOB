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

                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
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
            catch (Exception e)
            {
                return ;
            }

        }

        public void AddSystemLog(SystemLog sysLog)
        {

            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
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
            catch (Exception e)
            {
                return;
            }

        }

        public int GetNextLogId()
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    List<Log> allLogs = db.Logs.ToList();
                    if (allLogs.Count == 0)
                    {
                        return -2;
                    }
                    allLogs.OrderByDescending(log => log.LogId);
                    int currMax =  allLogs.Last().LogId;
                    return currMax + 1;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        
    }
}
