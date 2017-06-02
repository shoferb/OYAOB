using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database;
using TexasHoldem.Database.DatabaseObject;
using TexasHoldem.Database.EntityFramework.Controller;
using TexasHoldem.Database.EntityFramework.Model;

namespace TexasHoldemTests.Database
{
    [TestClass()]
    public class UserDBTests
    {
       
      
        [TestMethod()]
        public void GetAllUserTest_good_id()
        {
            UserController uco = new UserController();
            UserTable ut = new UserTable();
            ut.userId = 305077901;
            ut.HighestCashGainInGame = 0;
            ut.TotalProfit = 0;
            ut.avatar = "/GuiScreen/Photos/Avatar/devil.png";
            ut.email = "orelie@post.bgu.ac.il";
            ut.gamesPlayed = 0;
            ut.inActive = true;
            ut.leagueName = 1;
            ut.money = 0;
            ut.name = "orelie";
            ut.HighestCashGainInGame = 0;
           
            uco.AddNewUser(ut);
           
           
           
           Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddNewUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserIsActiveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserLeagueNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserMoneyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserTotalProfitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserWinNumTest()
        {
            Assert.Fail();
        }
    }
}