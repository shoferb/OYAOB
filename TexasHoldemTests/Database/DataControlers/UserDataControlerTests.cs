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

        private readonly UserDataControler _userDataControler = new UserDataControler();
       


        private UserTable CreateUser(int userId, string username)
        {
            UserTable ut = new UserTable
            {
                userId = userId,
                HighestCashGainInGame = 0,
                TotalProfit = 0,
                avatar = "/GuiScreen/Photos/Avatar/devil.png",
                email = "orelie@post.bgu.ac.il",
                gamesPlayed = 0,
                inActive = true,
                leagueName = 1,
                money = 0,
                name = "orelie",
                username = username,
                password = "123456789"
            };
            ut.HighestCashGainInGame = 0;
            return ut;
        }

        [TestMethod()]
        public void GetAllUserTest_good_count_equal()
        {
            UserTable toAdd1 = CreateUser(1, "1");
            UserTable toAdd2 = CreateUser(2, "2");
            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(_userDataControler.GetAllUser().Count,2);
            _userDataControler.DeleteUserById(1);
            _userDataControler.DeleteUserById(2);
        }

        [TestMethod()]
        public void GetAllUserTest_good_First_Id_equal()
        {
            UserTable toAdd1 = CreateUser(3, "3");

            _userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(_userDataControler.GetAllUser().First().userId, toAdd1.userId);
            _userDataControler.DeleteUserById(3);

        }

        [TestMethod()]
        public void GetAllUserTest_bad_empty()
        {
          
            Assert.AreEqual(_userDataControler.GetAllUser().Count, 0);


        }

        [TestMethod()]
        public void GetUserByIdTest_good()
        {
            UserTable toAdd1 = CreateUser(4, "4");

            _userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(_userDataControler.GetUserById(4).userId, toAdd1.userId);
            _userDataControler.DeleteUserById(4);
        }

        [TestMethod()]
        public void GetUserByIdTest_bad_noUser()
        {
            Assert.AreEqual(_userDataControler.GetUserById(5), null);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(6, "6");
            _userDataControler.AddNewUser(toAdd1);
            Assert.AreEqual(_userDataControler.GetUserByUserName("6").username, toAdd1.username);
            _userDataControler.DeleteUserById(6);
        }

        [TestMethod()]
        public void GetUserByUserNameTest_bad_no_user()
        {
            Assert.AreEqual(_userDataControler.GetUserByUserName("7"), null);
            ;
           
        }
        [TestMethod()]
        public void AddNewUserTest_good_count()
        {
            UserTable toAdd1 = CreateUser(8, "8");

            _userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(_userDataControler.GetAllUser().Count, 1);
            _userDataControler.DeleteUserById(8);
        }

        [TestMethod()]
        public void AddNewUserTest_good_Id()
        {
            UserTable toAdd1 = CreateUser(9, "9");

            _userDataControler.AddNewUser(toAdd1);

            Assert.AreEqual(_userDataControler.GetAllUser().First().userId,toAdd1.userId);
            _userDataControler.DeleteUserById(9);
        }

        [TestMethod()]
        public void AddNewUserTest_bad_IdTaken()
        {
            UserTable toAdd1 = CreateUser(10, "10");

            _userDataControler.AddNewUser(toAdd1);
            UserTable toAdd2 = CreateUser(10, "10");
            _userDataControler.AddNewUser(toAdd2);
            Assert.AreEqual(_userDataControler.GetAllUser().Count,1);
            _userDataControler.DeleteUserById(10);
        }
        [TestMethod()]
        public void EditUserIdTest_good()
        {
            UserTable toAdd1 = CreateUser(11, "11");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserId(11,12);
            Assert.AreEqual(_userDataControler.GetAllUser().First().userId, 12);
            _userDataControler.DeleteUserById(12);
        }


      
        [TestMethod()]
        public void EditUserNameTest_good()
        {
            UserTable toAdd1 = CreateUser(13, "13");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserName(13, "14");
            Assert.AreEqual(_userDataControler.GetAllUser().First().username, "14");
            _userDataControler.DeleteUserById(13);
        }

        [TestMethod()]
        public void EditNameTest_bad_userNameTaken()
        {
            UserTable toAdd1 = CreateUser(15, "15");
            UserTable toAdd2 = CreateUser(16, "16");
            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.AddNewUser(toAdd2);
            _userDataControler.EditUserName(15, "16");
            Assert.AreEqual(_userDataControler.GetAllUser().First().username, "15");
            _userDataControler.DeleteUserById(15);
            _userDataControler.DeleteUserById(16);
        }

        [TestMethod()]
        public void EditEmailTest_good()
        {
            UserTable toAdd1 = CreateUser(17, "17");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditEmail(17, "new@o.com");
            Assert.AreEqual(_userDataControler.GetAllUser().First().email, "new@o.com");
            _userDataControler.DeleteUserById(17);
        }

      
        [TestMethod()]
        public void EditPasswordTest_good()
        {
            UserTable toAdd1 = CreateUser(18, "18");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditPassword(18, "newPassword");
            Assert.AreEqual(_userDataControler.GetAllUser().First().password, "newPassword");
            _userDataControler.DeleteUserById(18);
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest_good()
        {

            UserTable toAdd1 = CreateUser(20, "20");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserHighestCashGainInGame(20, 10);
            Assert.AreEqual(_userDataControler.GetAllUser().First().HighestCashGainInGame,10);
            _userDataControler.DeleteUserById(20);
        }

        [TestMethod()]
        public void EditUserIsActiveTest_good()
        {
            UserTable toAdd1 = CreateUser(21, "21");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserIsActive(21, false);
            Assert.IsFalse(_userDataControler.GetAllUser().First().inActive);
            _userDataControler.DeleteUserById(21);
        }

        [TestMethod()]
        public void EditUserLeagueNameTest_good()
        {
            UserTable toAdd1 = CreateUser(22, "22");

            _userDataControler.AddNewUser(toAdd1);
            LeagueName n = new LeagueName
            {
                League_Value = 3,
                League_Name = "C"
            };
            _userDataControler.EditUserLeagueName(22, n);
            Assert.AreEqual(_userDataControler.GetAllUser().First().leagueName, 3);
            _userDataControler.DeleteUserById(22);
        }

        [TestMethod()]
        public void EditUserMoneyTest_good()
        {
            UserTable toAdd1 = CreateUser(23, "23");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserMoney(23, 10000);
            Assert.AreEqual(_userDataControler.GetAllUser().First().money, 10000);
            _userDataControler.DeleteUserById(23);
        }

     

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest()
        {
            UserTable toAdd1 = CreateUser(24, "24");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserNumOfGamesPlayed(24, 40);
            Assert.AreEqual(_userDataControler.GetAllUser().First().gamesPlayed, 40);
            _userDataControler.DeleteUserById(24);
        }

        [TestMethod()]
        public void EditUserTotalProfitTest_ggod()
        {
            UserTable toAdd1 = CreateUser(25, "25");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserTotalProfit(25, 550);
            Assert.AreEqual(_userDataControler.GetAllUser().First().TotalProfit, 550);
            _userDataControler.DeleteUserById(25);
        }

        [TestMethod()]
        public void EditUserWinNumTest_good()
        {
            UserTable toAdd1 = CreateUser(26, "26");

            _userDataControler.AddNewUser(toAdd1);
            _userDataControler.EditUserWinNum(26, 25);
            Assert.AreEqual(_userDataControler.GetAllUser().First().winNum, 25);
            _userDataControler.DeleteUserById(26);
        }
      
    }
}