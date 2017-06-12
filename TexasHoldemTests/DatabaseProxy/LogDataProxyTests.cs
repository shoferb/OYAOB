using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using ErrorLog = TexasHoldem.Logic.Notifications_And_Logs.ErrorLog;
using Log = TexasHoldem.Logic.Notifications_And_Logs.Log;

namespace TexasHoldem.DatabaseProxy.Tests
{
    [TestClass()]
    public class LogDataProxyTests
    {
       
        [TestMethod()]
        public void AddErrorLogTest()
        {
           
        }

        [TestMethod()]
        public void AddSysLogTest()
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