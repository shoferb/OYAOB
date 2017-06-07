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
        public void LogTestNextIdSucces()
        {
            int logId1 = Log.getNextId();
            Log errorLog = new Log();
            Assert.AreEqual(errorLog.LogId, logId1);
        }

        [TestMethod()]
        public void LogTesNotSameIdSucces()
        {
            int logId1 = Log.getNextId();
            Log errorLog = new Log();
            int logId2 = Log.getNextId();
            Log errorLog2 = new Log();
            Assert.AreNotEqual(errorLog.LogId, errorLog2.LogId);
           
        }

        [TestMethod()]
        public void LogTesIncGood()
        {
            int logId1 = Log.getNextId();
            Log errorLog = new Log();
            int logId2 = Log.getNextId();
            Log errorLog2 = new Log();
            Assert.AreEqual(errorLog2.LogId, errorLog.LogId + 1);

        }
        

        [TestMethod()]
        public void ToStringTest()
        {
            Log errorLog = new Log();

            string toCheck1 = "Log Id is: " + errorLog.LogId ;

            Assert.AreEqual(errorLog.ToString(), toCheck1);

        }
    }
}