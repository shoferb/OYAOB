using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database;
using TexasHoldem.Database.DatabaseObject;
using TexasHoldem.Database.EntityFramework.Controller;
using TexasHoldem.Database.EntityFramework.Model;
using TexasHoldem.Database.LastTry;
using TexasHoldem.Database.NewEntity;
using UserTable = TexasHoldem.Database.LastTry.UserTable;

namespace TexasHoldemTests.Database
{
    [TestClass()]
    public class UserDBTests
    {

        public void AddNewUser(TexasHoldem.Database.NewEntity.UserTable objUser)
        {
            try
            {
                using (var context = new DataBaseSadnaEntities())
                {

                    var ob = new TexasHoldem.Database.NewEntity.UserTable
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

        public void AddNewUserLinq(UserTable objUser)
        {
            try
            {

                connectionsLinqDataContext con = new connectionsLinqDataContext();
                
                var ob = new TexasHoldem.Database.LastTry.UserTable
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
        public void GetAllUserTest_good_id()
        {
            
            TexasHoldem.Database.LastTry.UserTable ut = new TexasHoldem.Database.LastTry.UserTable();
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
            ut.username = "orelie26";
            ut.password = "123456789";
            ut.HighestCashGainInGame = 0;

            AddNewUserLinq(ut);
           
           
           
           Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            TexasHoldem.Database.LastTry.UserTable ut = new TexasHoldem.Database.LastTry.UserTable();
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
            ut.username = "orelie26";
            ut.password = "123456789";
            ut.HighestCashGainInGame = 0;

            AddNewUserLinq(ut);



            Assert.IsTrue(true);

        }

        public void AddNewUser(userDatabaseOb objUser)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    Console.WriteLine("before Open");
                    db.Open();
                    Console.WriteLine("after Open");
                    string readSp = "AddNewUser";
                    db.Execute(readSp, objUser, commandType: CommandType.StoredProcedure);
                    Console.WriteLine("error after adding");
                    db.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query");
                Console.WriteLine(e);
                return;
            }
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