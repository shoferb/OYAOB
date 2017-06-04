using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.DataControlers;

namespace TexasHoldem.DatabaseProxy.Tests
{
    [TestClass()]
    public class GameDataProxyTests
    {
        private UserDataControler userDataControler = new UserDataControler();


        public void SetUp()
        {
            AddNewUsers();
        }

        public void TearDown()
        {
            userDataControler.DeleteUserById(8);
            userDataControler.DeleteUserById(9);
            userDataControler.DeleteUserById(10);

        }
        public void AddNewUsers()
        {
            UserTable toAdd1 = CreateUser(8, "8");
            UserTable toAdd2 = CreateUser(9, "9");
            UserTable toAdd3 = CreateUser(10, "10");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            userDataControler.AddNewUser(toAdd3);


           
        }

        private UserTable CreateUser(int userId, string username)
        {
            UserTable ut = new UserTable();
            ut.userId = userId;
            ut.HighestCashGainInGame = 0;
            ut.TotalProfit = 0;
            ut.avatar = "/GuiScreen/Photos/Avatar/devil.png";
            ut.email = "orelie@post.bgu.ac.il";
            ut.gamesPlayed = 0;
            ut.inActive = true;
            ut.leagueName = 1;
            ut.money = 0;
            ut.name = "orelie";
            ut.username = username;
            ut.password = "password";
            ut.HighestCashGainInGame = 0;
            return ut;
        }
        
    [TestMethod()]
        public void GameDataProxyTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void AddNewGameToDBTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllGamesTest()
        {
            Assert.Fail();
        }
    }
}