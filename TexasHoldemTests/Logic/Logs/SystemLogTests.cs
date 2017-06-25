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
        public void SystemLogTest_typeOf()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1,"system log to test", 1);
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            
            
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
        public void SystemLogTest_log_id_enter_good()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);

            Assert.AreEqual(sysLog.LogId, logId1);
        }


        [TestMethod()]
        public void SystemLogTest_log_id_counter_good()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            Assert.AreEqual(sysLog2.LogId, sysLog.LogId + 1);
        }


        [TestMethod()]
        public void SystemLogTest_Log_id_not_the_same()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            Assert.AreNotEqual(sysLog.LogId, sysLog2.LogId);
        }

        [TestMethod()]
        public void SystemLogTest_Msg_good()
        {
 
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);

            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);


            Assert.AreEqual(sysLog.Msg, "system log to test");

            Assert.AreEqual(sysLog2.Msg, "system log to test2");

         
        }

        [TestMethod()]
        public void SystemLogTest_MsgSet__good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.Msg = "system log to test - edit";
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            sysLog2.Msg = "system log to test2 - edit";

            Assert.AreNotEqual(sysLog.Msg, "system log to test");

            Assert.AreNotEqual(sysLog2.Msg, "system log to test2");


        }

        [TestMethod()]
        public void SystemLogTest_MsgSet_good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.Msg = "system log to test - edit";
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            sysLog2.Msg = "system log to test2 - edit";

            Assert.AreEqual(sysLog.Msg, "system log to test - edit");

            Assert.AreEqual(sysLog2.Msg, "system log to test2 - edit");


        }

        [TestMethod()]
        public void SystemLogTest_roomId_enter_good()
        {
            
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);

            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            
            Assert.AreEqual(sysLog.RoomId, 1);
            Assert.AreEqual(sysLog2.RoomId, 2);
            

        }


        [TestMethod()]
        public void SystemLogTest_logId_noThesame_good()
        {
            
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);


            Assert.AreNotEqual(sysLog.LogId, sysLog2.LogId);

        }


        [TestMethod()]
        public void SystemLogTest_roomId_set_good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.RoomId = 55;
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            sysLog2.RoomId = 88;

            Assert.AreNotEqual(sysLog.RoomId, 1);
            Assert.AreNotEqual(sysLog2.RoomId, 2);
        }

        [TestMethod()]
        public void SystemLogTest_roomId_set2_good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.RoomId = 55;
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            sysLog2.RoomId = 88;

            Assert.AreEqual(sysLog.RoomId, 55);
            Assert.AreEqual(sysLog2.RoomId, 88);
        }

        [TestMethod()]
        public void SystemLogTest_gameId_set_good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.GameId = 55;
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 2);
            sysLog2.GameId = 88;

            Assert.AreNotEqual(sysLog.GameId, 1);
            Assert.AreNotEqual(sysLog2.GameId, 2);
        }

        [TestMethod()]
        public void SystemLogTest_gameId_set2_good()
        {

            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            sysLog.GameId = 55;
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            sysLog2.GameId = 88;

            Assert.AreEqual(sysLog.GameId, 55);
            Assert.AreEqual(sysLog2.GameId, 88);
        }

        [TestMethod()]
        public void SystemLogTestNextIdSucces()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            Assert.AreEqual(sysLog.LogId, logId1);
        }



        [TestMethod()]
        public void SystemLogTesNotSameIdSucces()
        {
            int logId1 = Log.getNextId();
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
            int logId2 = Log.getNextId();
            SystemLog sysLog2 = new SystemLog(2, "system log to test2", 1);
            Assert.AreNotEqual(sysLog.LogId, sysLog2.LogId);

        }

       


        [TestMethod()]
        public void ToStringTest()
        {
            SystemLog sysLog = new SystemLog(1, "system log to test", 1);
           
            string toCheck1 = "Log Id is: " + sysLog.LogId + " msg is: " + sysLog.Msg +"to room Id: "+sysLog.RoomId;
            Assert.AreEqual(sysLog.ToString(), toCheck1);
            
        }
    }
}