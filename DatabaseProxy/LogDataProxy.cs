using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.DatabaseProxy
{
    public class LodDataProxy
    {
        public void AddErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            try
            {
                LogDataControler udp = new LogDataControler();
                Database.LinqToSql.ErrorLog errorlog = convertErrorLog(error);
                udp.AddErrorLog(errorlog);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in lod data control : error log insert fail");
                return;
            }

        }

        private Database.LinqToSql.ErrorLog convertErrorLog(Logic.Notifications_And_Logs.ErrorLog error)
        {
            Database.LinqToSql.ErrorLog toReturn = new Database.LinqToSql.ErrorLog();
            toReturn.Log.LogId = error.LogId;
            toReturn.Log.PriorityLogEnum = error.
            return toReturn;
        }

        public void AddSystemLog(Logic.Notifications_And_Logs.SystemLog sysLog)
        {

            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine("error in lod data control : system log insert fail");
                return;
            }

        }
    }
}
