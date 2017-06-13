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
        private readonly LogDataControler _logDataControler = new LogDataControler();
        private readonly LogsOnlyForTest _logsOnlyForTest = new LogsOnlyForTest();

        [TestMethod()]
        public void GetNextLogIdTest_good()
        {
            var toAdd = new ErrorLog();
            var logs = new Log
            {
                LogId = 10,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 10;
            toAdd.msg = "test GetNextLogIdTest_good()";
            _logDataControler.AddErrorLog(toAdd);

            var toAdd2 = new ErrorLog();
            var logs2 = new Log
            {
                LogId = 10000,
                LogPriority = 1
            };
            toAdd2.Log = logs2;
            toAdd2.logId = 10000;
            toAdd2.msg = "test GetNextLogIdTest_good()";
            _logDataControler.AddErrorLog(toAdd2);
            var next = _logDataControler.GetNextLogId();
            Assert.AreEqual(next, 10001);
            _logsOnlyForTest.DeleteErrorLog(10);
            _logsOnlyForTest.DeleteLog(10);
            _logsOnlyForTest.DeleteErrorLog(10000);
            _logsOnlyForTest.DeleteLog(10000);
        }


        [TestMethod()]
        public void GetNextLogIdTest_bad_no_logs()
        {
            var next = _logDataControler.GetNextLogId();
            Assert.AreEqual(next, -2);
          
        }

        [TestMethod()]
        public void AddErrorLogTest_good_id()
        {
           
            var toAdd = new ErrorLog();
            var logs = new Log
            {
                LogId = 10005,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 10005;
            toAdd.msg = "test AddErrorLogTest_good_id()";
            _logDataControler.AddErrorLog(toAdd);
            
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(10005).logId, 10005);
            _logsOnlyForTest.DeleteErrorLog(10005);
            _logsOnlyForTest.DeleteLog(10005);
        }

        [TestMethod()]
        public void AddErrorLogTest_good_message()
        {

            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = 2000,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 2000;
            toAdd.msg = "test AddErrorLogTest_good_message()";
            _logDataControler.AddErrorLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(2000).msg, "test AddErrorLogTest_good_message()");
            _logsOnlyForTest.DeleteErrorLog(2000);
            _logsOnlyForTest.DeleteLog(2000);
        }
        
        [TestMethod()]
        public void AddSystemLogTest_good_id()
        {
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = 3555,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 3555;
            toAdd.msg = "test AddSystemLogTest_good_id()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(3555).logId, 3555);
            _logsOnlyForTest.DeleteSystemLog(3555);
            _logsOnlyForTest.DeleteLog(3555);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_message()
        {
            var toAdd = new SystemLog();
            var logs = new Log
            {
                LogId = 45555,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 45555;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(45555).msg, "test AddSystemLogTest_good_message()");
            _logsOnlyForTest.DeleteSystemLog(45555);
            _logsOnlyForTest.DeleteLog(45555);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_roomId()
        {
            var toAdd = new SystemLog();
            var logs = new Log
            {
                LogId = 5555555,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 5555555;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId =55555555;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(5555555).roomId,1);
            _logsOnlyForTest.DeleteSystemLog(5555555);
            _logsOnlyForTest.DeleteLog(5555555);
        }
      
    }
}