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
            UserTable toAdd1 = CreateUser(1, "1");
            UserTable toAdd2 = CreateUser(2, "2");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(userDataControler.GetAllUser().Count,2);
            userDataControler.DeleteUserById(1);
            userDataControler.DeleteUserById(2);
        }

        [TestMethod()]
        public void GetAllUserTest_good_First_Id_equal()
        {
            UserTable toAdd1 = CreateUser(3, "3");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().First().userId, toAdd1.userId);
            userDataControler.DeleteUserById(3);

        }

        [TestMethod()]
        public void GetAllUserTest_bad_empty()
        {
          
            Assert.AreEqual(userDataControler.GetAllUser().Count, 0);


        }

        [TestMethod()]
        public void GetUserByIdTest_good()
        {
            UserTable toAdd1 = CreateUser(4, "4");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetUserById(4).userId, toAdd1.userId);
            userDataControler.DeleteUserById(4);
        }

        [TestMethod()]
        public void GetUserByIdTest_bad_noUser()
        {
            Assert.AreEqual(userDataControler.GetUserById(5), null);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(6, "6");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetUserByUserName("6").username, toAdd1.username);
            userDataControler.DeleteUserById(6);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_bad_no_user()
        {
            Assert.AreEqual(userDataControler.GetUserByUserName("7"), null);
            ;
           
        }
        [TestMethod()]
        public void AddNewUserTest_good_count()
        {
            UserTable toAdd1 = CreateUser(8, "8");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().Count, 1);
            userDataControler.DeleteUserById(8);
        }

        [TestMethod()]
        public void AddNewUserTest_good_Id()
        {
            UserTable toAdd1 = CreateUser(9, "9");

            userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(userDataControler.GetAllUser().First().userId,toAdd1.userId);
            userDataControler.DeleteUserById(9);
        }

        [TestMethod()]
        public void AddNewUserTest_bad_IdTaken()
        {
            UserTable toAdd1 = CreateUser(10, "10");

            userDataControler.AddNewUser(toAdd1);
            UserTable toAdd2 = CreateUser(10, "10");
            userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(userDataControler.GetAllUser().Count,1);
            userDataControler.DeleteUserById(10);
        }
        [TestMethod()]
        public void EditUserIdTest_good()
        {
            UserTable toAdd1 = CreateUser(11, "11");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserId(11,12);
            Assert.AreEqual(userDataControler.GetAllUser().First().userId, 12);
            userDataControler.DeleteUserById(12);
        }


      
        [TestMethod()]
        public void EditUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(13, "13");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserName(13, "14");
            Assert.AreEqual(userDataControler.GetAllUser().First().username, "14");
            userDataControler.DeleteUserById(13);
        }

        [TestMethod()]
        public void EditNameTest_bad_userNameTaken()
        {
            UserTable toAdd1 = CreateUser(15, "15");
            UserTable toAdd2 = CreateUser(16, "16");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            userDataControler.EditUserName(15, "16");
            Assert.AreEqual(userDataControler.GetAllUser().First().username, "15");
            userDataControler.DeleteUserById(15);
            userDataControler.DeleteUserById(16);
        }

        [TestMethod()]
        public void EditEmailTest_good()
        {
            UserTable toAdd1 = CreateUser(17, "17");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditEmail(17, "new@o.com");
            Assert.AreEqual(userDataControler.GetAllUser().First().email, "new@o.com");
            userDataControler.DeleteUserById(17);
        }

      
        [TestMethod()]
        public void EditPasswordTest_good()
        {
            UserTable toAdd1 = CreateUser(18, "18");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditPassword(18, "newPassword");
            Assert.AreEqual(userDataControler.GetAllUser().First().password, "newPassword");
            userDataControler.DeleteUserById(18);
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest_good()
        {

            UserTable toAdd1 = CreateUser(20, "20");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserHighestCashGainInGame(20, 10);
            Assert.AreEqual(userDataControler.GetAllUser().First().HighestCashGainInGame,10);
            userDataControler.DeleteUserById(20);
        }

        [TestMethod()]
        public void EditUserIsActiveTest_good()
        {
            UserTable toAdd1 = CreateUser(21, "21");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserIsActive(21, false);
            Assert.IsFalse(userDataControler.GetAllUser().First().inActive);
            userDataControler.DeleteUserById(21);
        }

        [TestMethod()]
        public void EditUserLeagueNameTest_good()
        {
            UserTable toAdd1 = CreateUser(22, "22");

            userDataControler.AddNewUser(toAdd1);
            LeagueName n = new LeagueName();
            n.League_Value = 3;
            n.League_Name = "C";
            userDataControler.EditUserLeagueName(22, n);
            Assert.AreEqual(userDataControler.GetAllUser().First().leagueName, 3);
            userDataControler.DeleteUserById(22);
        }

        [TestMethod()]
        public void EditUserMoneyTest_good()
        {
            UserTable toAdd1 = CreateUser(23, "23");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserMoney(23, 10000);
            Assert.AreEqual(userDataControler.GetAllUser().First().money, 10000);
            userDataControler.DeleteUserById(23);
        }

     

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest()
        {
            UserTable toAdd1 = CreateUser(24, "24");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserNumOfGamesPlayed(24, 40);
            Assert.AreEqual(userDataControler.GetAllUser().First().gamesPlayed, 40);
            userDataControler.DeleteUserById(24);
        }

        [TestMethod()]
        public void EditUserTotalProfitTest_ggod()
        {
            UserTable toAdd1 = CreateUser(25, "25");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserTotalProfit(25, 550);
            Assert.AreEqual(userDataControler.GetAllUser().First().TotalProfit, 550);
            userDataControler.DeleteUserById(25);
        }

        [TestMethod()]
        public void EditUserWinNumTest_good()
        {
            UserTable toAdd1 = CreateUser(26, "26");

            userDataControler.AddNewUser(toAdd1);
            userDataControler.EditUserWinNum(26, 25);
            Assert.AreEqual(userDataControler.GetAllUser().First().winNum, 25);
            userDataControler.DeleteUserById(26);
        }
      
    }
}