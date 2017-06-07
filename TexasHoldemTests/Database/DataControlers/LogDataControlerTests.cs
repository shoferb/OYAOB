using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database.DataControlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;

namespace TexasHoldem.Database.DataControlers.Tests
{
    [TestClass()]
    public class LogDataControlerTests
    {
        [TestMethod()]
        public void AddErrorLogTest()
        {
           ErrorLog toAdd = new ErrorLog();
            toAdd.Log.LogId = 1;
            toAdd.msg = "insert error log try test ";

        }

        [TestMethod()]
        public void AddSystemLogTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNextLogIdTest()
        {
            Assert.Fail();
        }
    }
}