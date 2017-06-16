using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldemTests.Database.DataControlers;
using ErrorLog = TexasHoldem.Logic.Notifications_And_Logs.ErrorLog;

namespace TexasHoldemTests.DatabaseProxy
{
    [TestClass()]
    public class LogDataProxyTests
    {
        private readonly LogDataProxy _logDataProxy = new LogDataProxy();
        private readonly LogsOnlyForTest _logsOnlyForTest = new LogsOnlyForTest();

        [TestMethod()]
        public void AddErrorLogTest_good_id_match()
        {
           var error = new ErrorLog("AddErrorLogTest_good_id_match");
           _logDataProxy.AddErrorLog(error);
            var logId = error.LogId;
            var errorlogdb = _logsOnlyForTest.GetErrorLogById(logId);
            Assert.AreEqual(errorlogdb.logId,logId);
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
/*
        [TestMethod()]
        public void AddSysLogTest_good_id_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_id_match", 1);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).logId, logId);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);


        }

        [TestMethod()]
        public void AddSysLogTest_good_message_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_message_match", 1);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).msg, "AddSysLogTest_good_message_match");
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }

        [TestMethod()]
        public void AddSysLogTest_good_roomId_match()
        {
            var systemLog = new SystemLog(1, "AddSysLogTest_good_message_match", 1);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).roomId, 1);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }
        
        
       //todo - run after all test
       
        [TestMethod()]
        public void GetNextLogIdTest_good()
        {
            var toAdd1 = new ErrorLog("GetNextLogIdTest_good first");
            var toAdd1Id = toAdd1.LogId;
            var toAdd2 = new ErrorLog("GetNextLogIdTest_good secound");
            var toAdd2Id = toAdd2.LogId;
            _logDataProxy.AddErrorLog(toAdd1);
            _logDataProxy.AddErrorLog(toAdd2);
            var next = _logDataProxy.GetNextLogId();
            Assert.AreEqual(next, toAdd2Id + 1);
            _logsOnlyForTest.DeleteErrorLog(toAdd1Id);
            _logsOnlyForTest.DeleteLog(toAdd1Id);
            _logsOnlyForTest.DeleteErrorLog(toAdd2Id);
            _logsOnlyForTest.DeleteLog(toAdd2Id);
        }*/
    }
}