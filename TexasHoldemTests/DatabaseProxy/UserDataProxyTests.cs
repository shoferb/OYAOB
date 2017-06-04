using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.Security;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.Security;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;

namespace TexasHoldem.DatabaseProxy.Tests
{
    [TestClass()]
    public class UserDataProxyTests
    {
        private UserDataProxy userDataProxy = new UserDataProxy();
        private readonly ISecurity _security = new SecurityHandler();
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
            ut.password = "password";
            ut.HighestCashGainInGame = 0;
            return ut;
        }

        [TestMethod()]
        public void enpcrptpasswordTest_good()
        {

            UserTable toAddUT = CreateUser(305077937, "orelie");
            Console.WriteLine("password before encription:" + toAddUT.password);
            string encryptedstring = PasswordSecurity.Encrypt(toAddUT.password, "securityPassword");
            toAddUT.password = encryptedstring;
            Console.WriteLine("password encription:" + toAddUT.password);
            Assert.AreNotEqual(toAddUT.password, "123456789");
        }

        [TestMethod()]
        public void decpcrptpasswordTest_good()
        {

            UserTable toAddUT = CreateUser(305077938, "orelie");
            Console.WriteLine("password before encription:" + toAddUT.password);
            string encryptedstring = PasswordSecurity.Encrypt(toAddUT.password, "securityPassword");
            toAddUT.password = encryptedstring;
            Console.WriteLine("password encription:" + toAddUT.password);
            string decryptedstring = PasswordSecurity.Decrypt(encryptedstring, "securityPassword");
            toAddUT.password = decryptedstring;
            Console.WriteLine("password after deription:" + toAddUT.password);
            Assert.AreEqual(toAddUT.password, "123456789");

           
        }

        [TestMethod()]
        public void LoginTest()
        {
            UserTable ut = CreateUser(88, "oo5o");
            ut.inActive = false;

            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            userDataProxy.Login(user);
            IUser t = userDataProxy.GetUserById(88);
            Console.WriteLine("!!!!Iuserrr    " +t==null);
            Assert.AreEqual(t.Id(),88);
            userDataProxy.DeleteUserById(88);
        }

        [TestMethod()]
        public void LogoutTest()
        {
            Assert.Fail();
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
        public void GetAllUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUserByUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUserByIdTest()
        {
            Assert.Fail();
        }
        private IUser convertToIUser(UserTable user)
        {
  
            IUser toResturn = new User(user.userId, user.name, user.username, user.password, user.points,
                user.money, user.email, user.winNum, 0, user.HighestCashGainInGame, user.TotalProfit, user.avatar
                , user.gamesPlayed, user.inActive, GetLeagueName(user.leagueName));
            return toResturn;
        }
        private Logic.GameControl.LeagueName GetLeagueName(int league)
        {
            Logic.GameControl.LeagueName toReturn = 0;
            switch (league)
            {
                case 1:
                    toReturn = Logic.GameControl.LeagueName.A;
                    break;
                case 2:
                    toReturn = Logic.GameControl.LeagueName.B;
                    break;
                case 3:
                    toReturn = Logic.GameControl.LeagueName.C;
                    break;
                case 4:
                    toReturn = Logic.GameControl.LeagueName.D;
                    break;
                case 5:
                    toReturn = Logic.GameControl.LeagueName.E;
                    break;
                case 6:
                    toReturn = LeagueName.Unknow;
                    break;

            }
            return toReturn;
        }

        [TestMethod()]
        public void AddNewUserTest()
        {
            UserTable ut = CreateUser(333333, "ooo");
            userDataProxy.AddNewUser(convertToIUser(ut));
            Assert.AreEqual(userDataProxy.GetAllUser().Count,1);
            userDataProxy.DeleteUserById(333333);
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

        [TestMethod()]
        public void LoginTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogoutTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByIdTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByUserNameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUserTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUserByUserNameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUserByIdTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddNewUserTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserIdTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserAvatarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserNameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditNameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditEmailTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditPasswordTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserLeagueNameTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserMoneyTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserTotalProfitTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserWinNumTest1()
        {
            Assert.Fail();
        }
    }
}