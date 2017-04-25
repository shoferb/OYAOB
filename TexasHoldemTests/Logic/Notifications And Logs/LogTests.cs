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
    public class LogTests
    {
        [TestMethod()]
        public void LogTest()
        {
            int logId1 = Log.getNextId();
            Log errorLog = new Log();
            int logId2 = Log.getNextId();
            Log errorLog2 = new Log();
            int logId3 = Log.getNextId();
            Log errorLog3 = new Log();
            Assert.AreNotEqual(errorLog.LogId, errorLog2.LogId);
            Assert.AreNotEqual(errorLog.LogId, errorLog3.LogId);
            Assert.AreNotEqual(errorLog3.LogId, errorLog2.LogId);
            Assert.AreEqual(errorLog.LogId, logId1);
            Assert.AreEqual(errorLog2.LogId,logId2);
            Assert.AreEqual(errorLog3.LogId, logId3);
            Assert.AreEqual(errorLog2.LogId, errorLog.LogId + 1);
            Assert.AreEqual(errorLog3.LogId, errorLog.LogId + 2);
            Assert.AreEqual(errorLog3.LogId, errorLog2.LogId + 1);
           
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Log errorLog = new Log();
            Log errorLog2 = new Log();
            string toCheck1 = "Log id is: " + errorLog.LogId ;
            string toCheck2 = "Log id is: " + errorLog2.LogId ;
            Assert.AreEqual(errorLog.ToString(), toCheck1);
            Assert.AreEqual(errorLog2.ToString(), toCheck2);
        }
    }
}