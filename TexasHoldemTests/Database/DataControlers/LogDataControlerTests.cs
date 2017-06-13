using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database.DataControlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;
using TexasHoldemTests.Database.DataControlers;

namespace TexasHoldem.Database.DataControlers.Tests
{
    [TestClass()]
    public class LogDataControlerTests
    {
        private LogDataControler logDataControler = new LogDataControler();
        private LogsOnlyForTest logsOnlyForTest = new LogsOnlyForTest();

        [TestMethod()]
        public void GetNextLogIdTest_good()
        {
            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 10;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 10;
            toAdd.msg = "test GetNextLogIdTest_good()";
            logDataControler.AddErrorLog(toAdd);

            Database.LinqToSql.ErrorLog toAdd2 = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs2 = new Database.LinqToSql.Log();
            logs2.LogId = 10000;
            logs2.LogPriority = 1;
            toAdd2.Log = logs2;
            toAdd2.logId = 10000;
            toAdd2.msg = "test GetNextLogIdTest_good()";
            logDataControler.AddErrorLog(toAdd2);
            int next = logDataControler.GetNextLogId();
            Assert.AreEqual(next, 10001);
            logsOnlyForTest.DeleteErrorLog(10);
            logsOnlyForTest.DeleteLog(10);
            logsOnlyForTest.DeleteErrorLog(10000);
            logsOnlyForTest.DeleteLog(10000);
        }


        [TestMethod()]
        public void GetNextLogIdTest_bad_no_logs()
        {
            
            int next = logDataControler.GetNextLogId();
            Assert.AreEqual(next, -2);
            logsOnlyForTest.DeleteErrorLog(10);
            logsOnlyForTest.DeleteLog(10);
            logsOnlyForTest.DeleteErrorLog(10000);
            logsOnlyForTest.DeleteLog(10000);
        }
        [TestMethod()]
        public void AddErrorLogTest_good_id()
        {
           
            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 1;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 1;
            toAdd.msg = "test AddErrorLogTest_good_id()";
            logDataControler.AddErrorLog(toAdd);
            
            Assert.AreEqual(logsOnlyForTest.GetErrorLogById(1).logId, 1);
            logsOnlyForTest.DeleteErrorLog(1);
            logsOnlyForTest.DeleteLog(1);
        }

        [TestMethod()]
        public void AddErrorLogTest_good_message()
        {

            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 2;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 2;
            toAdd.msg = "test AddErrorLogTest_good_message()";
            logDataControler.AddErrorLog(toAdd);

            Assert.AreEqual(logsOnlyForTest.GetErrorLogById(2).msg, "test AddErrorLogTest_good_message()");
            logsOnlyForTest.DeleteErrorLog(2);
            logsOnlyForTest.DeleteLog(2);
        }
        
        [TestMethod()]
        public void AddSystemLogTest_good_id()
        {
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 3;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 3;
            toAdd.msg = "test AddSystemLogTest_good_id()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(logsOnlyForTest.GetSystemLogById(3).logId, 3);
            logsOnlyForTest.DeleteSystemLog(3);
            logsOnlyForTest.DeleteLog(3);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_message()
        {
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 4;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 4;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(logsOnlyForTest.GetSystemLogById(4).msg, "test AddSystemLogTest_good_message()");
            logsOnlyForTest.DeleteSystemLog(4);
            logsOnlyForTest.DeleteLog(4);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_roomId()
        {
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 5;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 5;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(logsOnlyForTest.GetSystemLogById(5).roomId,1);
            logsOnlyForTest.DeleteSystemLog(5);
            logsOnlyForTest.DeleteLog(5);
        }
      
    }
}