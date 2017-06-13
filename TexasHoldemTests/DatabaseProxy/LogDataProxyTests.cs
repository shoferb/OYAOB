using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldemTests.Database.DataControlers;
using ErrorLog = TexasHoldem.Logic.Notifications_And_Logs.ErrorLog;
using Log = TexasHoldem.Logic.Notifications_And_Logs.Log;
using SystemLog = TexasHoldem.Logic.Notifications_And_Logs.SystemLog;
namespace TexasHoldem.DatabaseProxy.Tests
{
    [TestClass()]
    public class LogDataProxyTests
    {
        readonly LogDataProxy _logDataProxy = new LogDataProxy();
        private readonly LogsOnlyForTest _logsOnlyForTest = new LogsOnlyForTest();

        [TestMethod()]
        public void AddErrorLogTest_good_id_match()
        {
           var error = new ErrorLog("AddErrorLogTest_good_id_match");
           _logDataProxy.AddErrorLog(error);
            var logId = error.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(logId).logId,logId);
            _logsOnlyForTest.DeleteErrorLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }


        [TestMethod()]
        public void AddErrorLogTest_good_message_match()
        {
            var error = new ErrorLog("AddErrorLogTest_good_message_match");
            _logDataProxy.AddErrorLog(error);
            var logId = error.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(logId).msg, "AddErrorLogTest_good_message_match");
            _logsOnlyForTest.DeleteErrorLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }

        [TestMethod()]
        public void AddSysLogTest_good_id_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_id_match", TODO);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).logId, logId);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);


        }

        [TestMethod()]
        public void AddSysLogTest_good_message_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_message_match", TODO);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).msg, "AddSysLogTest_good_message_match");
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }

        [TestMethod()]
        public void AddSysLogTest_good_roomId_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_message_match", TODO);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).roomId, 1);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }

        
        [TestMethod()]
        public void GetNextLogIdTest()
        {
            Assert.Fail();
        }

    }
}