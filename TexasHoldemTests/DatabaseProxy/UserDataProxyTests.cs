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
        private readonly UserDataProxy _userDataProxy = new UserDataProxy();

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
        public void LoginTest()
        {
            UserTable ut = CreateUser(88, "oo5o");
            ut.inActive = false;

            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            _userDataProxy.Login(user);
            Console.WriteLine(user.Id() +  user.Name() +  user.MemberName() + user.Password()+  user.Points() +
                user.Money() + user.Email()+ user.WinNum +  0 + user.HighestCashGainInGame + user.TotalProfit + user.Avatar() +
                 user.GetNumberOfGamesUserPlay() + user.IsLogin() + user.GetLeague());
            IUser t = _userDataProxy.GetUserById(88);
            Console.WriteLine("!!!!Iuserrr  in test password  " + t.Password());
            Assert.IsTrue(t.IsLogin());
            _userDataProxy.DeleteUserById(88);
        }

        [TestMethod()]
        public void LogoutTest()
        {
            UserTable ut = CreateUser(89, "eeeo");
            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            _userDataProxy.Logout(user);
            Console.WriteLine(user.Id() + user.Name() + user.MemberName() + user.Password() + user.Points() +
                              user.Money() + user.Email() + user.WinNum + 0 + user.HighestCashGainInGame + user.TotalProfit + user.Avatar() +
                              user.GetNumberOfGamesUserPlay() + user.IsLogin() + user.GetLeague());
            IUser t = _userDataProxy.GetUserById(89);

            Assert.IsFalse(t.IsLogin());
            _userDataProxy.DeleteUserById(89);
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            UserTable ut = CreateUser(49, "eeo");
            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            IUser t = _userDataProxy.GetUserById(49);

            Assert.AreEqual(t.Id(),49);
            _userDataProxy.DeleteUserById(49);
        }

        [TestMethod()]
        public void GetUserByUserNameTest()
        {
            UserTable ut = CreateUser(78, "toFind");
            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            IUser t = _userDataProxy.GetUserById(78);

            Assert.AreEqual(t.Id(), 78);
            _userDataProxy.DeleteUserById(78);
        }

   


        [TestMethod()]
        public void GetAllUserTest_good_firstId()
        {
            UserTable ut = CreateUser(487, "toFind");
            UserTable ut2 = CreateUser(488, "toFind2");
            IUser user = ConvertToIUser(ut);
            IUser user2 = ConvertToIUser(ut2);
            _userDataProxy.AddNewUser(user);
            _userDataProxy.AddNewUser(user2);

            Assert.IsTrue(_userDataProxy.GetUserById(487)!=null);
            _userDataProxy.DeleteUserById(487);
            _userDataProxy.DeleteUserById(488);
        }

        [TestMethod()]
        public void GetAllUserTest_good_secId()
        {
            UserTable ut = CreateUser(382, "toFind");
            UserTable ut2 = CreateUser(383, "toFind2");
            IUser user = ConvertToIUser(ut);
            IUser user2 = ConvertToIUser(ut2);
            _userDataProxy.AddNewUser(user);
            _userDataProxy.AddNewUser(user2);

            Assert.IsTrue(_userDataProxy.GetUserById(383) != null);
            _userDataProxy.DeleteUserById(382);
            _userDataProxy.DeleteUserById(383);
        }
        [TestMethod()]
        public void DeleteUserByUserNameTest()
        {
            UserTable ut = CreateUser(8268, "toRemove");
            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            _userDataProxy.DeleteUserByUserName("toRemove");
            Assert.AreEqual(_userDataProxy.GetUserById(8268), null);

        }

        [TestMethod()]
        public void DeleteUserByIdTest()
        {
            UserTable ut = CreateUser(83668, "toRemove2");
            IUser user = ConvertToIUser(ut);

            _userDataProxy.AddNewUser(user);
            _userDataProxy.DeleteUserById(83668);
            Assert.AreEqual(_userDataProxy.GetUserById(83668), null);
        }
        private IUser ConvertToIUser(UserTable user)
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
            UserTable ut = CreateUser(3502333, "AddNewUserTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            IUser user = _userDataProxy.GetUserById(3502333);
            Assert.IsTrue(user!=null);
            _userDataProxy.DeleteUserById(3502333);
        }

        [TestMethod()]
        public void EditUserIdTest_good()
        {
            UserTable ut = CreateUser(35667, "orelieS");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserId(35667,951);
            IUser user = _userDataProxy.GetUserById(951);
            Assert.AreEqual(user.Id(), 951);
            _userDataProxy.DeleteUserById(951);
        }

        [TestMethod()]
        public void EditUserIdTest_bad_id_taken()
        {
            UserTable ut = CreateUser(35998, "orelieS2");
            UserTable ut2 = CreateUser(35999, "orelieS3");
            _userDataProxy.AddNewUser(ConvertToIUser(ut2));
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserId(35998, 35999);
            IUser user = _userDataProxy.GetUserById(35998);
            Assert.AreEqual(user.Id(), 35998);
            _userDataProxy.DeleteUserById(35998);
            _userDataProxy.DeleteUserById(35999);
        }

        [TestMethod()]
        public void EditUserNameTest()
        {
            UserTable ut = CreateUser(6850, "orelieS4");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserName(6850, "changed-editName_proxy");
            IUser user = _userDataProxy.GetUserById(6850);

            Assert.AreEqual(user.MemberName(), "changed-editName_proxy");
            _userDataProxy.DeleteUserById(6850);
        }
        
        [TestMethod()]
        public void EditNameTest_good()
        {
            UserTable ut = CreateUser(845, "orelie845");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditName(845, "EditNameTest_good()_proxy");
            IUser user = _userDataProxy.GetUserById(845);

            Assert.AreEqual(user.Name(), "EditNameTest_good()_proxy");
            _userDataProxy.DeleteUserById(845);
        }

     

        [TestMethod()]
        public void EditEmailTest_good()
        {
            UserTable ut = CreateUser(985950000, "EditEmailTest_good()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditEmail(985950000, "NewEmail@post.bgu.ac.il");
            IUser user = _userDataProxy.GetUserById(985950000);

            Assert.AreEqual(user.Email(), "NewEmail@post.bgu.ac.il");
            _userDataProxy.DeleteUserById(985950000);
        }

       

        [TestMethod()]
        public void EditPasswordTest()
        {
            UserTable ut = CreateUser(858580000, "EditPasswordTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditPassword(858580000, "EditPasswordTest");
            IUser user = _userDataProxy.GetUserById(858580000);

            Assert.AreEqual(user.Password(), "EditPasswordTest");
            _userDataProxy.DeleteUserById(858580000);
        }

        [TestMethod()]
        public void EditUserHighestCashGainInGameTest()
        {
            UserTable ut = CreateUser(1414552, "EditUserHighestCashGainInGameTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserHighestCashGainInGame(1414552, 50);
            IUser user = _userDataProxy.GetUserById(1414552);

            Assert.AreEqual(user.HighestCashGainInGame, 50);
            _userDataProxy.DeleteUserById(1414552);
        }

        [TestMethod()]
        public void EditUserLeagueNameTest()
        {
            UserTable ut = CreateUser(858566222, "EditUserLeagueNameTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserLeagueName(858566222, LeagueName.B);
            IUser user = _userDataProxy.GetUserById(858566222);

            Assert.AreEqual(user.GetLeague(), LeagueName.B);
            _userDataProxy.DeleteUserById(858566222);
        }

        [TestMethod()]
        public void EditUserMoneyTest()
        {
            UserTable ut = CreateUser(886022005, "EditUserHighestCashGainInGameTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserMoney(886022005, 5000);
            IUser user = _userDataProxy.GetUserById(886022005);

            Assert.AreEqual(user.Money(), 5000);
            _userDataProxy.DeleteUserById(886022005);
        }

        [TestMethod()]
        public void EditUserNumOfGamesPlayedTest()
        {
            UserTable ut = CreateUser(959511110, "EditUserNumOfGamesPlayedTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserNumOfGamesPlayed(959511110, 300);
            IUser user = _userDataProxy.GetUserById(959511110);

            Assert.AreEqual(user.GetNumberOfGamesUserPlay(), 300);
            _userDataProxy.DeleteUserById(959511110);
        }

        [TestMethod()]
        public void EditUserTotalProfitTest()
        {
            UserTable ut = CreateUser(950010252, "EditUserTotalProfitTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserTotalProfit(950010252, 5000000);
            IUser user = _userDataProxy.GetUserById(950010252);

            Assert.AreEqual(user.TotalProfit, 5000000);
            _userDataProxy.DeleteUserById(950010252);
        }

        [TestMethod()]
        public void EditUserWinNumTest()
        {
            UserTable ut = CreateUser(33355550, "EditUserWinNumTest()-proxy");
            _userDataProxy.AddNewUser(ConvertToIUser(ut));
            _userDataProxy.EditUserWinNum(33355550, 544);
            IUser user = _userDataProxy.GetUserById(33355550);

            Assert.AreEqual(user.WinNum, 544);
            _userDataProxy.DeleteUserById(33355550);
        }
    
    }
}