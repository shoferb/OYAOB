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
    public class NotificationTests
    {
        [TestMethod()]
        public void NotificationTest()
        {
            Notification toTest1 = new Notification(1,"new notificarion to test");
            Notification toTest2 = new Notification(2, "new notificarion to test2");
            Assert.AreEqual(toTest1.Msg, "new notificarion to test");
            Assert.AreEqual(toTest2.Msg, "new notificarion to test2");
            Assert.AreEqual(toTest1.RoomId, 1);
            Assert.AreEqual(toTest2.RoomId, 2);
            
        }

        [TestMethod()]
        public void ToStringTest()
        {

            Notification toTest1 = new Notification(1, "new notificarion to test");
            Notification toTest2 = new Notification(2, "new notificarion to test2");
            string toCheck1 = "this is a notification with  massage: " + toTest1.Msg + "to room Id: " + toTest1.RoomId;
            string toCheck2 = "this is a notification with  massage: " + toTest2.Msg + "to room Id: " + toTest2.RoomId;
            Assert.AreEqual(toTest1.ToString(), toCheck1);
            Assert.AreEqual(toTest2.ToString(), toCheck2);
        }
    }
}