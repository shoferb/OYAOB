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
            string decryptedstring = PasswordSecurity.Decrypt(toAddUT.password, "securityPassword");
            toAddUT.password = decryptedstring;
            Console.WriteLine("password after deription:" + toAddUT.password);
            Assert.AreEqual(toAddUT.password, "password");

           
        }

        [TestMethod()]
        public void LoginTest()
        {
            UserTable ut = CreateUser(88, "oo5o");
            ut.inActive = false;

            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            userDataProxy.Login(user);
            Console.WriteLine(user.Id() +  user.Name() +  user.MemberName() + user.Password()+  user.Points() +
                user.Money() + user.Email()+ user.WinNum +  0 + user.HighestCashGainInGame + user.TotalProfit + user.Avatar() +
                 user.GetNumberOfGamesUserPlay() + user.IsLogin() + user.GetLeague());
            IUser t = userDataProxy.GetUserById(88);
            Console.WriteLine("!!!!Iuserrr  in test password  " + t.Password());
            Assert.IsTrue(t.IsLogin());
            userDataProxy.DeleteUserById(88);
        }

        [TestMethod()]
        public void LogoutTest()
        {
            UserTable ut = CreateUser(89, "eeeo");
            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            userDataProxy.Logout(user);
            Console.WriteLine(user.Id() + user.Name() + user.MemberName() + user.Password() + user.Points() +
                              user.Money() + user.Email() + user.WinNum + 0 + user.HighestCashGainInGame + user.TotalProfit + user.Avatar() +
                              user.GetNumberOfGamesUserPlay() + user.IsLogin() + user.GetLeague());
            IUser t = userDataProxy.GetUserById(89);

            Assert.IsFalse(t.IsLogin());
            userDataProxy.DeleteUserById(89);
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            UserTable ut = CreateUser(49, "eeo");
            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            IUser t = userDataProxy.GetUserById(49);

            Assert.AreEqual(t.Id(),49);
            userDataProxy.DeleteUserById(49);
        }

        [TestMethod()]
        public void GetUserByUserNameTest()
        {
            UserTable ut = CreateUser(78, "toFind");
            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            IUser t = userDataProxy.GetUserById(78);

            Assert.AreEqual(t.Id(), 78);
            userDataProxy.DeleteUserById(78);
        }

        [TestMethod()]
        public void GetAllUserTest_good_count()
        {
            UserTable ut = CreateUser(45, "toFind");
            UserTable ut2 = CreateUser(46, "toFind2");
            IUser user = convertToIUser(ut);
            IUser user2 = convertToIUser(ut2);
            userDataProxy.AddNewUser(user);
            userDataProxy.AddNewUser(user2);
            List<IUser> temp = userDataProxy.GetAllUser();

            Assert.AreEqual(temp.Count, 2);
            userDataProxy.DeleteUserById(45);
            userDataProxy.DeleteUserById(46);
        }


        [TestMethod()]
        public void GetAllUserTest_good_firstId()
        {
            UserTable ut = CreateUser(47, "toFind");
            UserTable ut2 = CreateUser(48, "toFind2");
            IUser user = convertToIUser(ut);
            IUser user2 = convertToIUser(ut2);
            userDataProxy.AddNewUser(user);
            userDataProxy.AddNewUser(user2);
            List<IUser> temp = userDataProxy.GetAllUser();

            Assert.AreEqual(temp[0].Id(), 47);
            userDataProxy.DeleteUserById(47);
            userDataProxy.DeleteUserById(48);
        }

        [TestMethod()]
        public void GetAllUserTest_good_secId()
        {
            UserTable ut = CreateUser(32, "toFind");
            UserTable ut2 = CreateUser(33, "toFind2");
            IUser user = convertToIUser(ut);
            IUser user2 = convertToIUser(ut2);
            userDataProxy.AddNewUser(user);
            userDataProxy.AddNewUser(user2);
            List<IUser> temp = userDataProxy.GetAllUser();

            Assert.AreEqual(temp[1].Id(), 33);
            userDataProxy.DeleteUserById(32);
            userDataProxy.DeleteUserById(33);
        }
        [TestMethod()]
        public void DeleteUserByUserNameTest()
        {
            UserTable ut = CreateUser(828, "toRemove");
            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            userDataProxy.DeleteUserByUserName("toRemove");
            Assert.AreEqual(userDataProxy.GetAllUser().Count, 0);
        }

        [TestMethod()]
        public void DeleteUserByIdTest()
        {
            UserTable ut = CreateUser(838, "toRemove2");
            IUser user = convertToIUser(ut);

            userDataProxy.AddNewUser(user);
            userDataProxy.DeleteUserById(838);
            Assert.AreEqual(userDataProxy.GetAllUser().Count, 0);
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
        public void EditUserIdTest_good()
        {
            UserTable ut = CreateUser(357, "orelieS");
            userDataProxy.AddNewUser(convertToIUser(ut));
            userDataProxy.EditUserId(357,951);
            Assert.AreEqual(userDataProxy.GetAllUser()[0].Id(), 951);
            userDataProxy.DeleteUserById(951);
        }

        [TestMethod()]
        public void EditUserIdTest_bad_id_taken()
        {
            UserTable ut = CreateUser(358, "orelieS2");
            UserTable ut2 = CreateUser(359, "orelieS3");
            userDataProxy.AddNewUser(convertToIUser(ut2));
            userDataProxy.AddNewUser(convertToIUser(ut));
            userDataProxy.EditUserId(358, 359);
            Assert.AreEqual(userDataProxy.GetAllUser()[0].Id(), 358);
            userDataProxy.DeleteUserById(358);
            userDataProxy.DeleteUserById(359);
        }

        [TestMethod()]
        public void EditUserNameTest()
        {
            UserTable ut = CreateUser(60, "orelieS4");
            userDataProxy.AddNewUser(convertToIUser(ut));
            userDataProxy.EditUserName(60, "changed");
            Assert.AreEqual(userDataProxy.GetAllUser()[0].MemberName(), "changed");
            userDataProxy.DeleteUserById(60);
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