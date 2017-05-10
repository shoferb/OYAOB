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
        public void ToStringTest()
        {

            Notification toTest1 = new Notification(1, "new notificarion to test");
            string toCheck1 = "this is a notification with  massage: " + toTest1.Msg + "to room Id: " + toTest1.RoomId;
            Assert.AreEqual(toTest1.ToString(), toCheck1);
        }
    }
}