using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Notifications_And_Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs.Tests
{
    [TestClass()]
    public class ErrorLogTests
    {
        [TestMethod()]
        public void ErrorLogTest()
        {
            int logId1 = Log.getNextId();
            ErrorLog errorLog = new ErrorLog("error log test");
            int logId2 = Log.getNextId();
            ErrorLog errorLog2 = new ErrorLog("error log test2");
            Assert.AreEqual(errorLog.Msg, "error log test");
            Assert.AreNotEqual(errorLog.LogId,errorLog2.LogId);
            Assert.AreEqual(errorLog.LogId, logId1);
            Assert.AreEqual(errorLog2.LogId, logId2);
            Assert.AreEqual(errorLog2.LogId, errorLog.LogId+1);
            Assert.IsInstanceOfType(errorLog2,typeof(ErrorLog));
            Assert.IsInstanceOfType(errorLog2, typeof(Log));
            Assert.IsNotInstanceOfType(errorLog2, typeof(SystemLog));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            ErrorLog errorLog = new ErrorLog("error log test");
            ErrorLog errorLog2 = new ErrorLog("error log test2");
            string toCheck1 = "Log id is: " + errorLog.LogId + " msg is: " + errorLog.Msg;
            string toCheck2 = "Log id is: " + errorLog2.LogId + " msg is: " + errorLog2.Msg;
            Assert.AreEqual(errorLog.ToString(),toCheck1);
            Assert.AreEqual(errorLog2.ToString(), toCheck2);
        }
    }
}