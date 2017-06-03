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
    public class UserDataControlerTests
    {

        private UserDataControler userDataControler = new UserDataControler();


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
            ut.password = "123456789";
            ut.HighestCashGainInGame = 0;
            return ut;
        }

        [TestMethod()]
        public void GetAllUserTest_good_count_equal()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");
            UserTable toAdd2 = CreateUser(305077902, "orelie2");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(userDataControler.GetAllUser().Count,2);
            userDataControler.DeleteUserById(305077901);
            userDataControler.DeleteUserById(305077902);
        }

        [TestMethod()]
        public void GetAllUserTest_good_First_Id_equal()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().First().userId, toAdd1.userId);
            userDataControler.DeleteUserById(305077901);

        }

        [TestMethod()]
        public void GetAllUserTest_bad_empty()
        {
          
            Assert.AreEqual(userDataControler.GetAllUser().Count, 0);


        }

        [TestMethod()]
        public void GetUserByIdTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetUserById(305077901).userId, toAdd1.userId);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void GetUserByIdTest_bad_noUser()
        {
            Assert.AreEqual(userDataControler.GetUserById(305077901), null);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetUserByUserName("orelie1").username, toAdd1.username);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_bad_no_user()
        {
            Assert.AreEqual(userDataControler.GetUserByUserName("orelie1"), null);
            ;
           
        }
        [TestMethod()]
        public void AddNewUserTest_good_count()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().Count, 1);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void AddNewUserTest_good_Id()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().First().userId,toAdd1.userId);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void AddNewUserTest_bad_IdTaken()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            UserTable toAdd2 = CreateUser(305077901, "orelie1");
            userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(userDataControler.GetAllUser().Count,1);
            userDataControler.DeleteUserById(305077901);
        }
        [TestMethod()]
        public void EditUserIdTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserId(305077901,305077902);
            Assert.AreEqual(userDataControler.GetAllUser().First().userId, 305077902);
            userDataControler.DeleteUserById(305077902);
        }


      
        [TestMethod()]
        public void EditUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserName(305077901, "orelie2");
            Assert.AreEqual(userDataControler.GetAllUser().First().username, "orelie2");
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditNameTest_bad_userNameTaken()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");
            UserTable toAdd2 = CreateUser(305077902, "orelie2");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            userDataControler.EditUserName(305077901, "orelie2");
            Assert.AreEqual(userDataControler.GetAllUser().First().username, "orelie1");
            userDataControler.DeleteUserById(305077901);
            userDataControler.DeleteUserById(305077902);
        }

        [TestMethod()]
        public void EditEmailTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditEmail(305077901, "new@o.com");
            Assert.AreEqual(userDataControler.GetAllUser().First().email, "new@o.com");
            userDataControler.DeleteUserById(305077901);
        }

      
        [TestMethod()]
        public void EditPasswordTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditPassword(305077901, "newPassword");
            Assert.AreEqual(userDataControler.GetAllUser().First().password, "newPassword");
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest_good()
        {

            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserHighestCashGainInGame(305077901, 10);
            Assert.AreEqual(userDataControler.GetAllUser().First().HighestCashGainInGame,10);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserIsActiveTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserIsActive(305077901, false);
            Assert.AreEqual(userDataControler.GetAllUser().First().inActive, false);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserLeagueNameTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            LeagueName n = new LeagueName();
            n.League_Value = 3;
            n.League_Name = "C";
            userDataControler.EditUserLeagueName(305077901, n);
            Assert.AreEqual(userDataControler.GetAllUser().First().leagueName, 3);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserMoneyTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserMoney(305077901, 10000);
            Assert.AreEqual(userDataControler.GetAllUser().First().money, 10000);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserNumOfGamesPlayed(305077901, 40);
            Assert.AreEqual(userDataControler.GetAllUser().First().gamesPlayed, 40);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserTotalProfitTest_ggod()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserTotalProfit(305077901, 550);
            Assert.AreEqual(userDataControler.GetAllUser().First().TotalProfit, 550);
            userDataControler.DeleteUserById(305077901);
        }

        [TestMethod()]
        public void EditUserWinNumTest_good()
        {
            UserTable toAdd1 = CreateUser(305077901, "orelie1");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserWinNum(305077901, 25);
            Assert.AreEqual(userDataControler.GetAllUser().First().winNum, 25);
            userDataControler.DeleteUserById(305077901);
        }
    }
}