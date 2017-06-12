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
        public void AddErrorLogTest_good_count()
        {
           
            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log();
            logs.LogId = 1;
            logs.LogPriority = 1;
            toAdd.Log = logs;
            toAdd.logId = 1;
            toAdd.msg = "test AddErrorLogTest_good_count()";
            logDataControler.AddErrorLog(toAdd);
            List<Database.LinqToSql.ErrorLog> allLogs = logsOnlyForTest.GetAllErrorLogs();
            Assert.AreEqual(allLogs.Count, 1);
            logsOnlyForTest.DeleteErrorLog(toAdd);
            logsOnlyForTest.DeleteLog(logs);
        }

        [TestMethod()]
        public void AddSystemLogTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNextLogIdTest()
        {
            Assert.Fail();
        }
    }
}