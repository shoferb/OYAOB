using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Users.Tests
{
    [TestClass()]
    public class SpectetorTests
    {
        [TestMethod()]
        public void SpectetorTest()
        {

            User user = new User(305077901, "orelie", "orelie12",
                "12345678", 0, 100, "orelie@post.bgu.ac.il");
            Spectetor toTest = new Spectetor(user, 1);
            Assert.AreEqual(toTest.user.Id(),305077901);
            toTest.user.EditId(305077902);
            Assert.AreNotEqual(toTest.user.Id(), 305077901);
            Assert.AreEqual(toTest.user.Id(), 305077902);
            Assert.AreEqual(toTest.roomId,1);
           
            Assert.AreEqual(toTest.roomId, 1);
            Assert.AreEqual(toTest.user.Name(),"orelie");
            toTest.user.EditName("Amir");
            Assert.AreNotEqual(toTest.user.Name(), "orelie");
            Assert.AreEqual(toTest.user.Name(), "Amir");
            Assert.AreEqual(toTest.user.Points(),0);
            toTest.user.EditUserPoints(100);
            Assert.AreNotEqual(toTest.user.Points(), 0);
            Assert.AreEqual(toTest.user.Points(), 100);
            Assert.AreEqual(toTest.user.MemberName(),"orelie12");
            toTest.user.EditUserName( "orelie123");
            Assert.AreNotEqual(toTest.user.MemberName(), "orelie12");
            Assert.AreEqual(toTest.user.MemberName(), "orelie123");
            Assert.AreEqual(toTest.user.Password(),"12345678");
            Assert.AreEqual(toTest.user.Email(),"orelie@post.bgu.ac.il");
            Assert.AreEqual(toTest.user.Money(), 100);
            toTest.user.EditUserMoney(10);
            Assert.AreNotEqual(toTest.user.Money(), 100);
            Assert.AreEqual(toTest.user.Money(), 10);
        }
    }
}