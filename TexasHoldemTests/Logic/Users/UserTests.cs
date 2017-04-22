using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.Users.Tests
{
    [TestClass()]
    public class UserTests
    {

        private User orelie = new User(305077901,"orelie","orelie26","123456",0,500,"orelie@post.bgu.ac.il");
        private Notification toSend1 = new Notification(11,"joind");
        private Notification toSend2 = new Notification(11, "Exit");
       
        [TestMethod()]
        public void UserIdTest()
        {
            Assert.AreEqual(orelie.Id,305077901);
            Assert.AreNotEqual(orelie.Id,305077902);
        }

        [TestMethod()]
        public void UserNameTest()
        {
            Assert.AreEqual(orelie.Name,"orelie");
            Assert.AreNotEqual(orelie.Name, "odsdf");
        }
        [TestMethod()]
        public void UserUserNameTest()
        {
            Assert.AreEqual(orelie.MemberName,"orelie26");
            Assert.AreNotEqual(orelie.MemberName, "odsdf");
        }
        [TestMethod()]
        public void UserPasswordTest()
        {
            Assert.AreEqual(orelie.Password,"123456");
            Assert.AreNotEqual(orelie.Password, "slafkjasp");
            Assert.AreNotEqual(orelie.Password, "654321");
        }
        [TestMethod()]
        public void UserPointTest()
        {
            Assert.AreEqual(orelie.Points,0);
            Assert.AreNotEqual(orelie.Points,5);
        }
        [TestMethod()]
        public void UserMoneyTest()
        {
            Assert.AreEqual(orelie.Money,500);
            Assert.AreNotEqual(orelie.Money,-500);
        }
        [TestMethod()]
        public void UserEmailTest()
        {
            Assert.AreEqual(orelie.Email,"orelie@post.bgu.ac.il");
            Assert.AreNotEqual(orelie.Email, "slafkjasp");
            Assert.AreNotEqual(orelie.Email, "orelie@gmail.com");
        }
        /*
        [TestMethod()]
        public void IsValidEmailTest()
        {
            Assert.IsTrue(orelie.IsValidEmail("orelie@post.bgu.ac.il"));
            Assert.IsTrue(orelie.IsValidEmail("orelie.shahar@gmail.com"));
            Assert.IsTrue(orelie.IsValidEmail("orelie@walla.co.il"));
            Assert.IsFalse(orelie.IsValidEmail("orelie.post.bgu.ac.il"));
            Assert.IsFalse(orelie.IsValidEmail("wromgEmail"));
            Assert.IsFalse(orelie.IsValidEmail("oreli2198#@%*_)(*&^%#!?@bgu.ac.il"));
        }*/

        [TestMethod()]
        public void SendNotificationTest()
        {
            Assert.IsTrue(orelie.SendNotification(toSend1));
            Assert.IsTrue(orelie.SendNotification(toSend2));

        }

        [TestMethod()]
        public void AddNotificationToListTest()
        {
            Assert.IsTrue(orelie.AddNotificationToList(toSend1));
            Assert.IsTrue(orelie.AddNotificationToList(toSend2));
        }
        
    }
}