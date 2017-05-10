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
    public class SystemLogTests
    {
        [TestMethod()]
        public void SystemLogTest()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1,"system log to test");
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2");
            
            
            Assert.AreEqual(sysLog.Msg, "system log to test");
            Assert.AreEqual(sysLog.LogId, logId1);
            Assert.AreEqual(sysLog2.Msg, "system log to test2");
            Assert.AreEqual(sysLog.RoomId, 1);
            Assert.AreEqual(sysLog2.RoomId, 2);
            Assert.AreNotEqual(sysLog.LogId, sysLog2.LogId);
            
            Assert.AreEqual(sysLog2.LogId, logId2);
            Assert.AreEqual(sysLog2.LogId, sysLog.LogId + 1);
            Assert.IsNotInstanceOfType(sysLog2, typeof(ErrorLog));
            Assert.IsInstanceOfType(sysLog, typeof(Log));
            Assert.IsInstanceOfType(sysLog2, typeof(SystemLog));
        }


        [TestMethod()]
        public void SystemLogTestNextIdSucces()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test");
            Assert.AreEqual(sysLog.LogId, logId1);
        }



        [TestMethod()]
        public void SystemLogTesNotSameIdSucces()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test");
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2");
            Assert.AreNotEqual(sysLog.LogId, sysLog2.LogId);

        }

       


        [TestMethod()]
        public void ToStringTest()
        {
            SystemLog sysLog = new SystemLog(1, "system log to test");
           
            string toCheck1 = "Log Id is: " + sysLog.LogId + " msg is: " + sysLog.Msg +"to room Id: "+sysLog.RoomId;
            Assert.AreEqual(sysLog.ToString(), toCheck1);
            
        }
    }
}