using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;

namespace TexasHoldem.Service.Tests
{
    [TestClass()]
    public class UserServiceHandlerTests
    {
        private SystemControl sc;
      
        private void Init()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<Logic.Users.IUser>();
        }

        [TestMethod()]
        public void RegisterToSystemTest_good()
        {
            Init();
            Assert.IsTrue(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_id_taken()
        {
            Init();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_userName_taken()
        {
            Init();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RegisterToSystem(305077902, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_email()
        {
            Init();
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "oreliepost.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_passWord()
        {
            Init();
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Name()
        {
            Init();
            Assert.IsFalse(sc.RegisterToSystem(305077901, " ", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void LoginUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogoutUserTest()
        {
            Assert.Fail();
        }

       

        [TestMethod()]
        public void DeleteUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUserByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserEmailTest()
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
        public void EditIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditMoneyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserNotificationsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserAvatarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetActiveGamesByUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSpectetorGamesByUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetIUserByUserNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserLeagueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DevideLeagueTest()
        {
            Assert.Fail();
        }
    }
}