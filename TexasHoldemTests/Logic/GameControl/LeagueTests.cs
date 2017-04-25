using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game_Control.Tests
{
    [TestClass()]
    public class LeagueTests
    {
        

        [TestMethod()]
        public void getNameTest()
        {
            string toComp1 = "1";
            string toComp2 = "2";
            string toComp3 = "3";
            string toComp4 = "4";
            string toComp5 = "5";
            League toTest1 = new League("1",0,50);
            League toTest2 = new League("2", 0, 50);
            League toTest3 = new League("3", 0, 50);
            League toTest4 = new League("4", 0, 50);
            League toTest5 = new League("5", 0, 50);
            Assert.AreEqual(toTest1.getName(),toComp1);
            Assert.AreEqual(toTest2.getName(), toComp2);
            Assert.AreEqual(toTest3.getName(), toComp3);
            Assert.AreEqual(toTest4.getName(), toComp4);
            Assert.AreEqual(toTest5.getName(), toComp5);
            Assert.AreNotEqual(toTest1.getName(), toComp5);
            Assert.AreNotEqual(toTest2.getName(), toComp1);
            Assert.AreNotEqual(toTest3.getName(), toComp2);
            Assert.AreNotEqual(toTest4.getName(), toComp3);
            Assert.AreNotEqual(toTest5.getName(), toComp4);
        }

        [TestMethod()]
        public void setNameTest()
        {
            string toComp1 = "1";
            string toComp2 = "2";
            string toComp3 = "3";
            string toComp4 = "4";
            string toComp5 = "5";
            League toTest1 = new League("1", 0, 50);
            League toTest2 = new League("2", 0, 50);
            League toTest3 = new League("3", 0, 50);
            League toTest4 = new League("4", 0, 50);
            League toTest5 = new League("5", 0, 50);

            Assert.AreEqual(toTest1.getName(), toComp1);
            Assert.AreEqual(toTest2.getName(), toComp2);
            Assert.AreEqual(toTest3.getName(), toComp3);
            Assert.AreEqual(toTest4.getName(), toComp4);
            Assert.AreEqual(toTest5.getName(), toComp5);
            //after change
            toTest1.setName(toComp5);
            toTest2.setName(toComp1);
            toTest3.setName(toComp2);
            toTest4.setName(toComp3);
            toTest5.setName(toComp4);

            Assert.AreNotEqual(toTest1.getName(), toComp1);
            Assert.AreNotEqual(toTest2.getName(), toComp2);
            Assert.AreNotEqual(toTest3.getName(), toComp3);
            Assert.AreNotEqual(toTest4.getName(), toComp4);
            Assert.AreNotEqual(toTest5.getName(), toComp5);

            Assert.AreEqual(toTest1.getName(), toComp5);
            Assert.AreEqual(toTest2.getName(), toComp1);
            Assert.AreEqual(toTest3.getName(), toComp2);
            Assert.AreEqual(toTest4.getName(), toComp3);
            Assert.AreEqual(toTest5.getName(), toComp4);
        }

        [TestMethod()]
        public void getMinRankTest()
        {
            int toComp1 = 0;
            int toComp2 = 50;
            int toComp3 = 100;
            int toComp4 = 150;
            int toComp5 = 200;
            League toTest1 = new League("1", 0, 50);
            League toTest2 = new League("2", 50, 100);
            League toTest3 = new League("3", 100, 150);
            League toTest4 = new League("4", 150, 200);
            League toTest5 = new League("5", 200, 250);
            Assert.AreEqual(toTest1.getMinRank(), toComp1);
            Assert.AreEqual(toTest2.getMinRank(), toComp2);
            Assert.AreEqual(toTest3.getMinRank(), toComp3);
            Assert.AreEqual(toTest4.getMinRank(), toComp4);
            Assert.AreEqual(toTest5.getMinRank(), toComp5);
            Assert.AreNotEqual(toTest1.getMinRank(), toComp5);
            Assert.AreNotEqual(toTest2.getMinRank(), toComp1);
            Assert.AreNotEqual(toTest3.getMinRank(), toComp2);
            Assert.AreNotEqual(toTest4.getMinRank(), toComp3);
            Assert.AreNotEqual(toTest5.getMinRank(), toComp4);
        }

        [TestMethod()]
        public void setMinRankTest()
        {
            int toComp1 = 0;
            int toComp2 = 50;
            int toComp3 = 100;
            int toComp4 = 150;
            int toComp5 = 200;
            League toTest1 = new League("1", 0, 50);
            League toTest2 = new League("2", 50, 100);
            League toTest3 = new League("3", 100, 150);
            League toTest4 = new League("4", 150, 200);
            League toTest5 = new League("5", 200, 250);
            Assert.AreEqual(toTest1.getMinRank(), toComp1);
            Assert.AreEqual(toTest2.getMinRank(), toComp2);
            Assert.AreEqual(toTest3.getMinRank(), toComp3);
            Assert.AreEqual(toTest4.getMinRank(), toComp4);
            Assert.AreEqual(toTest5.getMinRank(), toComp5);
            //change
            toTest1.setMinRank(toComp5);
            toTest2.setMinRank(toComp1);
            toTest3.setMinRank(toComp2);
            toTest4.setMinRank(toComp3);
            toTest5.setMinRank(toComp4);
            Assert.AreNotEqual(toTest1.getMinRank(), toComp1);
            Assert.AreNotEqual(toTest2.getMinRank(), toComp2);
            Assert.AreNotEqual(toTest3.getMinRank(), toComp3);
            Assert.AreNotEqual(toTest4.getMinRank(), toComp4);
            Assert.AreNotEqual(toTest5.getMinRank(), toComp5);
            Assert.AreEqual(toTest1.getMinRank(), toComp5);
            Assert.AreEqual(toTest2.getMinRank(), toComp1);
            Assert.AreEqual(toTest3.getMinRank(), toComp2);
            Assert.AreEqual(toTest4.getMinRank(), toComp3);
            Assert.AreEqual(toTest5.getMinRank(), toComp4);
        }

        [TestMethod()]
        public void getMaxRankTest()
        {
            int toComp1 = 50;
            int toComp2 = 100;
            int toComp3 = 150;
            int toComp4 = 200;
            int toComp5 = 250;
            League toTest1 = new League("1", 0, 50);
            League toTest2 = new League("2", 50, 100);
            League toTest3 = new League("3", 100, 150);
            League toTest4 = new League("4", 150, 200);
            League toTest5 = new League("5", 200, 250);
            Assert.AreEqual(toTest1.getMaxRank(), toComp1);
            Assert.AreEqual(toTest2.getMaxRank(), toComp2);
            Assert.AreEqual(toTest3.getMaxRank(), toComp3);
            Assert.AreEqual(toTest4.getMaxRank(), toComp4);
            Assert.AreEqual(toTest5.getMaxRank(), toComp5);
            Assert.AreNotEqual(toTest1.getMaxRank(), toComp5);
            Assert.AreNotEqual(toTest2.getMaxRank(), toComp1);
            Assert.AreNotEqual(toTest3.getMaxRank(), toComp2);
            Assert.AreNotEqual(toTest4.getMaxRank(), toComp3);
            Assert.AreNotEqual(toTest5.getMaxRank(), toComp4);
        }

        [TestMethod()]
        public void setMaxRankTest()
        {
            int toComp1 = 50;
            int toComp2 = 100;
            int toComp3 = 150;
            int toComp4 = 200;
            int toComp5 = 250;
            League toTest1 = new League("1", 0, 50);
            League toTest2 = new League("2", 50, 100);
            League toTest3 = new League("3", 100, 150);
            League toTest4 = new League("4", 150, 200);
            League toTest5 = new League("5", 200, 250);
            Assert.AreEqual(toTest1.getMaxRank(), toComp1);
            Assert.AreEqual(toTest2.getMaxRank(), toComp2);
            Assert.AreEqual(toTest3.getMaxRank(), toComp3);
            Assert.AreEqual(toTest4.getMaxRank(), toComp4);
            Assert.AreEqual(toTest5.getMaxRank(), toComp5);
            //change
            toTest1.setMaxRank(toComp5);
            toTest2.setMaxRank(toComp1);
            toTest3.setMaxRank(toComp2);
            toTest4.setMaxRank(toComp3);
            toTest5.setMaxRank(toComp4);
            Assert.AreNotEqual(toTest1.getMaxRank(), toComp1);
            Assert.AreNotEqual(toTest2.getMaxRank(), toComp2);
            Assert.AreNotEqual(toTest3.getMaxRank(), toComp3);
            Assert.AreNotEqual(toTest4.getMaxRank(), toComp4);
            Assert.AreNotEqual(toTest5.getMaxRank(), toComp5);
            Assert.AreEqual(toTest1.getMaxRank(), toComp5);
            Assert.AreEqual(toTest2.getMaxRank(), toComp1);
            Assert.AreEqual(toTest3.getMaxRank(), toComp2);
            Assert.AreEqual(toTest4.getMaxRank(), toComp3);
            Assert.AreEqual(toTest5.getMaxRank(), toComp4);
        }
    }
}