using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database;
using TexasHoldem.Database.DatabaseObject;
using TexasHoldem.Database.EntityFramework.Controller;
using TexasHoldem.Database.BackupOldFrameworks.EntityFramework.Model;
using TexasHoldem.Database.BackupOldFrameworks.EntityNewTry;
using TexasHoldem.Database.BackupOldFrameworks.EntityNewTry;
using TexasHoldem.Database.LinqToSql;

using UserTable = TexasHoldem.Database.LinqToSql;


namespace TexasHoldemTests.Database
{
    [TestClass()]
    public class UserDBTests
    {

        public void AddNewUser(TexasHoldem.Database.BackupOldFrameworks.EntityFramework.Model.UserTable objUser)
        {
            try
            {
                using (var context = new DataBaseSadnaEntitiesNewest())
                {

                    var ob = new TexasHoldem.Database.BackupOldFrameworks.EntityFramework.Model.UserTable
                    {
                        userId = objUser.userId,
                        username = objUser.username,
                        name = objUser.name,
                        email = objUser.email,
                        password = objUser.password,
                        avatar = objUser.avatar,
                        points = objUser.points,
                        money = objUser.money,
                        gamesPlayed = objUser.gamesPlayed,
                        leagueName = objUser.leagueName,
                        winNum = objUser.winNum,
                        HighestCashGainInGame = objUser.HighestCashGainInGame,
                        TotalProfit = objUser.TotalProfit,
                        inActive = objUser.inActive
                    };
                    context.UserTables.Add(ob);
                    context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query entityFramework");
                Console.WriteLine(e);
                return;
            }


        }

        public void AddNewUserLinq(UserTable.UserTable objUser)
        {
            try
            {

                connectionsLinqDataContext con = new connectionsLinqDataContext();
                
                var ob = new TexasHoldem.Database.LinqToSql.UserTable
                    {
                        userId = objUser.userId,
                        username = objUser.username,
                        name = objUser.name,
                        email = objUser.email,
                        password = objUser.password,
                        avatar = objUser.avatar,
                        points = objUser.points,
                        money = objUser.money,
                        gamesPlayed = objUser.gamesPlayed,
                        leagueName = objUser.leagueName,
                        winNum = objUser.winNum,
                        HighestCashGainInGame = objUser.HighestCashGainInGame,
                        TotalProfit = objUser.TotalProfit,
                        inActive = objUser.inActive
                    };
                con.UserTables.InsertOnSubmit(ob);
                con.SubmitChanges();



            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query entityFramework");
                Console.WriteLine(e);
                return;
            }


        }


       

       

        [TestMethod()]
        public void GetUserByIdTest()
        {

            TexasHoldem.Database.LinqToSql.UserTable ut = new TexasHoldem.Database.LinqToSql.UserTable();
            ut.userId = 30501771;
            ut.HighestCashGainInGame = 0;
            ut.TotalProfit = 0;
            ut.avatar = "/GuiScreen/Photos/Avatar/devil.png";
            ut.email = "orelie@post.bgu.ac.il";
            ut.gamesPlayed = 0;
            ut.inActive = true;
            ut.leagueName = 1;
            ut.money = 0;
            ut.name = "orelie";
            ut.username = "LinqLastTry";
            ut.password = "123456789";
            ut.HighestCashGainInGame = 0;

            AddNewUserLinq(ut);



            Assert.IsTrue(true);

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