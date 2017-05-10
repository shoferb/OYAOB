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
        public void ErrorLogTestNextIdSucces()
        {
            int logId1 = Log.getNextId();
            
            ErrorLog errorLog = new ErrorLog("error log test");
            Assert.AreEqual(errorLog.LogId, logId1);
        }



        [TestMethod()]
        public void LogTesNotSameIdSucces()
        {
            int logId1 = Log.getNextId();
            ErrorLog errorLog = new ErrorLog("error log test");
            int logId2 = Log.getNextId();
            ErrorLog errorLog2 = new ErrorLog("error log test2");
            Assert.AreNotEqual(errorLog.LogId, errorLog2.LogId);

        }

        [TestMethod()]
        public void LogTesIncGood()
        {
            int logId1 = Log.getNextId();
            ErrorLog errorLog = new ErrorLog("error log test");
            int logId2 = Log.getNextId();
            ErrorLog errorLog2 = new ErrorLog("error log test2");
            Assert.AreEqual(errorLog2.LogId, errorLog.LogId + 1);

        }

        [TestMethod()]
        public void ToStringTest()
        {
            ErrorLog errorLog = new ErrorLog("error log test");
            string toCheck1 = "Log Id is: " + errorLog.LogId + " msg is: " + errorLog.Msg;
            Assert.AreEqual(errorLog.ToString(),toCheck1);
        }
    }
}