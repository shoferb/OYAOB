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
            Spectetor toTest = new Spectetor(305077901,"orelie","orelie12",
                "12345678",0,100,"orelie@post.bgu.ac.il",1);
            Assert.AreEqual(toTest.Id,305077901);
            toTest.Id = 305077902;
            Assert.AreNotEqual(toTest.Id, 305077901);
            Assert.AreEqual(toTest.Id, 305077902);
            Assert.AreEqual(toTest.RoomId,1);
            toTest.RoomId = 2;
            Assert.AreNotEqual(toTest.RoomId, 1);
            Assert.AreEqual(toTest.RoomId, 2);
            Assert.AreEqual(toTest.Name,"orelie");
            toTest.Name = "Amir";
            Assert.AreNotEqual(toTest.Name, "orelie");
            Assert.AreEqual(toTest.Name, "Amir");
            Assert.AreEqual(toTest.Points,0);
            toTest.Points = 100;
            Assert.AreNotEqual(toTest.Points, 0);
            Assert.AreEqual(toTest.Points, 100);
            Assert.AreEqual(toTest.MemberName,"orelie12");
            toTest.MemberName = "orelie123";
            Assert.AreNotEqual(toTest.MemberName, "orelie12");
            Assert.AreEqual(toTest.MemberName, "orelie123");
            Assert.AreEqual(toTest.Password,"12345678");
            Assert.AreEqual(toTest.Email,"orelie@post.bgu.ac.il");
            Assert.AreEqual(toTest.Money, 100);
            toTest.Money = 10;
            Assert.AreNotEqual(toTest.Money, 100);
            Assert.AreEqual(toTest.Money, 10);
        }
    }
}