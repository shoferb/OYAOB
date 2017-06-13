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
                LogId = 1,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 1;
            toAdd.msg = "test AddErrorLogTest_good_id()";
            _logDataControler.AddErrorLog(toAdd);
            
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(1).logId, 1);
            _logsOnlyForTest.DeleteErrorLog(1);
            _logsOnlyForTest.DeleteLog(1);
        }

        [TestMethod()]
        public void AddErrorLogTest_good_message()
        {

            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = 2,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 2;
            toAdd.msg = "test AddErrorLogTest_good_message()";
            _logDataControler.AddErrorLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(2).msg, "test AddErrorLogTest_good_message()");
            _logsOnlyForTest.DeleteErrorLog(2);
            _logsOnlyForTest.DeleteLog(2);
        }
        
        [TestMethod()]
        public void AddSystemLogTest_good_id()
        {
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = 3,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 3;
            toAdd.msg = "test AddSystemLogTest_good_id()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(3).logId, 3);
            _logsOnlyForTest.DeleteSystemLog(3);
            _logsOnlyForTest.DeleteLog(3);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_message()
        {
            var toAdd = new SystemLog();
            var logs = new Log
            {
                LogId = 4,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 4;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(4).msg, "test AddSystemLogTest_good_message()");
            _logsOnlyForTest.DeleteSystemLog(4);
            _logsOnlyForTest.DeleteLog(4);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_roomId()
        {
            var toAdd = new SystemLog();
            var logs = new Log
            {
                LogId = 5,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 5;
            toAdd.msg = "test AddSystemLogTest_good_message()";
            toAdd.roomId = 1;
            toAdd.game_Id = 1;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(5).roomId,1);
            _logsOnlyForTest.DeleteSystemLog(5);
            _logsOnlyForTest.DeleteLog(5);
        }
      
    }
}