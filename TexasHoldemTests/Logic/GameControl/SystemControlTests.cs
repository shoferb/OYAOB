﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control.Tests
{
    [TestClass()]
    public class SystemControlTests
    {
        private SystemControl sc;

        

        [TestMethod()]
        public void SingltonTest()
        {
            SystemControl s1 = SystemControl.SystemControlInstance;
            SystemControl s2 = SystemControl.SystemControlInstance;
            Assert.AreEqual(s1, s2);
        }

        [TestMethod()]
        public void RemoveUserByIdTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(sc.RemoveUserById(305077901));
        }

        [TestMethod()]
        public void RemoveUserByIdTest_Bad_no_user()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RemoveUserById(305077901));
        }

        public void RemoveUserByIdTest_Bad_invalidId()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RemoveUserById(-1));
        }

        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(sc.RemoveUserByUserNameAndPassword("orelie26", "123456789"));
        }

        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest_Bad_no_user_name()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RemoveUserByUserNameAndPassword("orelie6", "123456789"));
        }


        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest_Bad_password()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RemoveUserByUserNameAndPassword("orelie26", "83456789"));
        }

         [TestMethod()]
        public void RemoveUserTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(sc.RemoveUser(sc.GetUserWithId(305077901)));
        }

        [TestMethod()]
        public void RemoveUserTest_bad_user_null()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RemoveUser(null));
        }

        [TestMethod()]
        public void RemoveUserTest_bad_not_in_users()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RemoveUser(sc.GetUserWithId(305077901)));
        }

        [TestMethod()]
        public void GetIUSerByUsernameTest()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = sc.GetUserWithId(305077901);
            Assert.IsTrue(sc.Users.Contains(u));
        }

        [TestMethod()]
        public void RegisterToSystemTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanCreateNewUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsUsernameFreeTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsIdFreeTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsUserExistTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserWithIdTest1()
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
        public void GetAllUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SortByRankTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUnKnowUsersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SortByPointTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DivideStartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DivideLeagueTest()
        {
            Assert.Fail();
        }


    }
}


