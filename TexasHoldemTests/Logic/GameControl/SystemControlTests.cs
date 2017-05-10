using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod()]
        public void SingltonTest()
        {
            SystemControl s1 = SystemControl.SystemControlInstance;
            SystemControl s2 = SystemControl.SystemControlInstance;
            Assert.AreEqual(s1, s2);
        }

        [TestMethod()]
        public void AddNewUserGoodTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name = "orelie";
            string UserName = "orelie123456";
            string password = "12345678";
            string email = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email));
            Assert.IsTrue(sc.RegisterToSystem(id, name, UserName, password, money, email));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUserById(id));
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
        }

        [TestMethod()]
        public void AddNewUserBadfieldTest()
        {
            //empty username
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            int badId = -3216;
            string name = "orelie";
            string badName = " ";
            string badUserName = "";
            String UserName = "orelie123456";
            string password = "123456789";
            string badPassword = "123";
            string email = "orelie@post.bgu.ac.il";
            string badEmail = "orelie.bgu.ac.il";
            int money = 1000;
            int badMoney = -100;
            //bad Id
            Assert.IsFalse(sc.CanCreateNewUser(badId, UserName, password, email));
            Assert.IsFalse(sc.RegisterToSystem(badId, name, UserName, password, money, email));
            //bad username
            Assert.IsFalse(sc.CanCreateNewUser(id, badUserName, password, email));
            Assert.IsFalse(sc.RegisterToSystem(id, name, badUserName, password, money, email));
            //empty name
            Assert.IsFalse(sc.RegisterToSystem(id, badUserName, UserName, password, money, email));
            //bad email
            Assert.IsFalse(sc.CanCreateNewUser(id, UserName, password, badEmail));
            Assert.IsFalse(sc.RegisterToSystem(id, name, UserName, password, money, badEmail));
            //bad password
            Assert.IsFalse(sc.CanCreateNewUser(id, UserName, badPassword, email));
            Assert.IsFalse(sc.RegisterToSystem(id, name, UserName, badPassword, money, email));
            //bad money
            Assert.IsFalse(sc.RegisterToSystem(id, name, UserName, password, badMoney, email));
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
        }


        [TestMethod()]
        public void AddNewUserBadTest()
        {
            //empty username
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            int id2 = 30507902;
            string name1 = "orelie";
            string name2 = "michele";
            string userName2 = "michele12";
            String UserName = "orelie123456";
            string password = "123456789";
            string password2 = "65432156";
            string email1 = "orelie@post.bgu.ac.il";
            string email2 = "michele@post.bgu.ac.il";
            string badEmail = "orelie.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            //create user with same Id
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.IsUsernameFree(userName2));
            Assert.IsTrue((sc.IsUsernameFree(userName2)));
            Assert.IsFalse(sc.CanCreateNewUser(id, userName2, password2, email2));
            Assert.IsFalse(sc.RegisterToSystem(id, name2, userName2, password2, money, email2));
            IUser user2 = sc.GetIUSerByUsername(userName2);
            Assert.IsFalse(sc.GetAllUser().Contains(user2));
            //create user with same user name

            Assert.IsTrue(sc.IsIdFree(id2));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            Assert.IsFalse(sc.CanCreateNewUser(id2, UserName, password2, email2));
            Assert.IsFalse(sc.RegisterToSystem(id2, name2, UserName, password2, money, email2));
            IUser user3 = sc.GetIUSerByUsername(userName2);
            Assert.IsFalse(sc.GetAllUser().Contains(user3));

            Assert.IsTrue(sc.RemoveUserById(id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void RemoveUserByIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUserByUserNameAndPassword(UserName, password));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void RemoveUserTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUser(user));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }



        [TestMethod()]
        public void LoginTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.AreEqual(user.IsLogin(), false);
            Assert.IsTrue(user.Login());
            Assert.AreEqual(user.IsLogin(), true);

            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void LogoutTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.AreEqual(user.IsLogin(), false);
            Assert.IsTrue(user.Login());
            Assert.AreEqual(user.IsLogin(), true);
            Assert.IsTrue(user.Logout());
            Assert.AreEqual(user.IsLogin(), false);
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void RegisterToSystemTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            IUser user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));

            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

        [TestMethod()]
        public void IsUsernameFreeTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            Assert.IsTrue(sc.RemoveUserById(id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void IsIdFreeTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.RemoveUserById(id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }



        [TestMethod()]
        public void IsUserWithIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsFalse(sc.IsIdFree(id2));
            Assert.IsTrue(sc.IsUserExist(id));
            Assert.IsTrue(sc.IsUserExist(id2));
            Assert.IsFalse(sc.IsUserExist(-500));
            Assert.IsFalse(sc.IsUserExist(0));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            Assert.IsFalse(sc.IsUserExist(id));
            Assert.IsFalse(sc.IsUserExist(id2));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

        [TestMethod()]
        public void GetUserWithIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsTrue(sc.IsUserExist(id));
            IUser user = sc.GetUserWithId(id);
            Assert.AreEqual(user.Id(), 305077901);
            Assert.AreEqual(user.Name(), "orelie");
            Assert.AreEqual(user.MemberName(), "orelie123456");
            Assert.AreEqual(user.Points(), 0);
            Assert.AreEqual(user.Password(), "123456789");
            Assert.AreEqual(user.Email(), "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Money(), 1000);
            Assert.IsTrue(sc.RemoveUserById(id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }
/*
        [TestMethod()]
        public void IsValidEmailTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            Assert.IsTrue(sc.IsValidEmail("orelie@post.bgu.ac.il"));
            Assert.IsTrue(sc.IsValidEmail("orelie.shahar@gmail.com"));
            Assert.IsTrue(sc.IsValidEmail("orelie@walla.co.il"));
            Assert.IsFalse(sc.IsValidEmail("orelie.post.bgu.ac.il"));
            Assert.IsFalse(sc.IsValidEmail("wromgEmail"));
            Assert.IsFalse(sc.IsValidEmail("oreli2198#@%*_)(*&^%#!?@bgu.ac.il"));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }*/
/*
        [TestMethod()]
        public void EditEmailTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsTrue(user.EditEmail(Id, "orelie@post.bgu.ac.il"));
            Assert.IsTrue(sc.EditEmail(Id, "orelie.shahar@gmail.com"));
            Assert.IsTrue(sc.EditEmail(Id, "orelie@walla.co.il"));
            Assert.IsFalse(sc.EditEmail(Id, "orelie.post.bgu.ac.il"));
            Assert.IsFalse(sc.EditEmail(Id, "wromgEmail"));
            Assert.IsFalse(sc.EditEmail(Id, "oreli2198#@%*_)(*&^%#!?@bgu.ac.il"));
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);

        }

        [TestMethod()]
        public void IsValidPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            Assert.IsFalse(sc.IsValidPassword(""));
            Assert.IsFalse(sc.IsValidPassword("  "));
            Assert.IsFalse(sc.IsValidPassword("1234!@"));
            Assert.IsFalse(sc.IsValidPassword("12345678912346"));
            Assert.IsTrue(sc.IsValidPassword("12345678"));
            Assert.IsTrue(sc.IsValidPassword("orelie123"));
            Assert.IsTrue(sc.IsValidPassword("1234567891"));
            Assert.IsTrue(sc.IsValidPassword("12345678912"));
            Assert.IsTrue(sc.IsValidPassword("123456789123"));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void EditPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.EditPassword(Id, ""));
            Assert.IsFalse(sc.EditPassword(Id, "  "));
            Assert.IsFalse(sc.EditPassword(Id, "1234!@"));
            Assert.IsFalse(sc.EditPassword(Id, "12345678912346"));
            Assert.IsTrue(sc.EditPassword(Id, "12345678"));
            Assert.IsTrue(sc.EditPassword(Id, "orelie123"));
            Assert.IsTrue(sc.EditPassword(Id, "1234567891"));
            Assert.IsTrue(sc.EditPassword(Id, "12345678912"));
            Assert.IsTrue(sc.EditPassword(Id, "123456789123"));
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void EditUserNameTest()
        {

            SystemControl sc = SystemControl.SystemControlInstance;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;
            string newUserName1 = "try1";
            string newUserName2 = "try2";
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(Id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsFalse(sc.EditUserName(Id, UserName2));
            Assert.IsFalse(sc.EditUserName(id2, UserName));
            Assert.IsTrue(sc.EditUserName(Id, newUserName1));
            Assert.IsTrue(sc.EditUserName(id2, UserName));
            Assert.IsTrue(sc.EditUserName(id2, UserName2));
            Assert.IsFalse(sc.EditUserName(id2, newUserName1));
            Assert.IsTrue(sc.EditUserName(id2, newUserName2));
            Assert.IsTrue(sc.EditUserName(Id, UserName2));
            Assert.IsFalse(sc.EditUserName(Id, newUserName2));
            Assert.IsTrue(sc.RemoveUserById(Id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

        [TestMethod()]
        public void EditAvatarTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsTrue(sc.EditAvatar(Id, "newPath"));
            Assert.AreEqual(sc.GetUserWithId(Id).Avatar, "newPath");
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


        [TestMethod()]
        public void EditUserIDTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;
            int newId1 = 11111;
            int newId2 = 2222;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(Id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsFalse(sc.EditUserID(Id, id2));
            Assert.IsFalse(sc.EditUserID(id2, Id));
            Assert.IsTrue(sc.EditUserID(Id, newId1));//Id=1111
            Assert.IsTrue(sc.EditUserID(id2, Id));//id2=305077901
            Assert.IsFalse(sc.EditUserID(id2, newId1));//fail
            Assert.IsTrue(sc.EditUserID(Id, id2));//Id=305077902

            Assert.IsFalse(sc.EditUserID(id2, -1));//fail
            Assert.IsFalse(sc.EditUserID(id2, -111111));//fail
            Assert.IsTrue(sc.RemoveUserById(newId1));
            Assert.IsTrue(sc.RemoveUserById(id2));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

        [TestMethod()]
        public void EditUserPointsTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int size = sc.Users.Count;
            Assert.AreEqual(size, 0);
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(Id).Points, 0);
            Assert.IsTrue(sc.EditUserPoints(Id, 500));
            Assert.AreEqual(sc.GetUserWithId(Id).Points, 500);
            Assert.IsTrue(sc.EditUserPoints(Id, 0));
            Assert.AreEqual(sc.GetUserWithId(Id).Points, 0);
            Assert.IsFalse(sc.EditUserPoints(Id, -100));
            Assert.AreEqual(sc.GetUserWithId(Id).Points, 0); ;
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

        [TestMethod()]
        public void EditUserMoneyTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(Id).Money, 1000);
            Assert.IsTrue(sc.EditUserMoney(Id, 500));
            Assert.AreEqual(sc.GetUserWithId(Id).Money, 500);
            Assert.IsTrue(sc.EditUserMoney(Id, 0));
            Assert.AreEqual(sc.GetUserWithId(Id).Money, 0);
            Assert.IsFalse(sc.EditUserMoney(Id, -100));
            Assert.AreEqual(sc.GetUserWithId(Id).Money, 0); ;
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
        }

        [TestMethod()]
        public void EditActiveGameTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(Id).IsActive, false);
            Assert.IsTrue(sc.EditActiveGame(Id, true));
            Assert.AreEqual(sc.GetUserWithId(Id).IsActive, true);
            Assert.IsTrue(sc.RemoveUserById(Id));
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
        }

        [TestMethod()]
        public void IsHigestRankUserTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
            int Id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;
            int newId1 = 11111;
            int newId2 = 2222;
            Assert.IsTrue(sc.CanCreateNewUser(Id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(Id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(Id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            User orelie = sc.GetUserWithId(Id);
            User michele = sc.GetUserWithId(id2);
            List<User> all = sc.GetAllUser();
            int size = all.Count;
            Assert.IsTrue(size > 1);
            User higher = GameCenter.Instance.HigherRank;
            Assert.IsTrue(sc.EditUserPoints(Id, 100));
            Assert.AreEqual(orelie.Points, 100);
            Assert.AreEqual(orelie.IsHigherRank, true);
            Assert.AreEqual(michele.IsHigherRank, false);
            higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, orelie);
            Assert.IsTrue(sc.EditUserPoints(id2, 1000));
            Assert.AreEqual(michele.Points, 1000);
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreEqual(michele.IsHigherRank, true);
            higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, michele);


            Assert.IsTrue(sc.IsHigestRankUser(id2));
            Assert.IsFalse(sc.IsHigestRankUser(Id));
            Assert.IsTrue(sc.RemoveUserById(Id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }

    */
    


        [TestMethod()]
        public void SortByRankTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
           // GameCenter.Instance.HigherRank = null;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;
            int id3 = 305077903;
            string name3 = "Amir";
            String UserName3 = "Amir29";
            string password3 = "123456789";
            string email3 = "Amirf@post.bgu.ac.il";
            int money3 = 5000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsTrue(sc.CanCreateNewUser(id3, UserName3, password3, email3));
            Assert.IsTrue(sc.RegisterToSystem(id3, name3, UserName3, password3, money3, email3));
            IUser orelie = sc.GetUserWithId(id);
            IUser michele = sc.GetUserWithId(id2);
            IUser Amir = sc.GetUserWithId(id3);
            Assert.IsTrue(michele.EditUserPoints(1000));
            Assert.AreEqual(michele.Points(), 1000);
            Assert.IsTrue(orelie.EditUserPoints( 10));
            Assert.AreEqual(orelie.Points(), 10);
            Assert.IsTrue(Amir.EditUserPoints(5000));
            Assert.AreEqual(Amir.Points(), 5000);
            List<IUser> byRank = sc.SortByRank();
            int orelieIndex = byRank.IndexOf(orelie);
            int micheleIndex = byRank.IndexOf(michele);
            int AmirIndex = byRank.IndexOf(Amir);
            Assert.AreEqual(AmirIndex, 0);
            Assert.AreEqual(micheleIndex, 1);
            Assert.AreEqual(orelieIndex, 2);
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            Assert.IsTrue(sc.RemoveUserById(id3));
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
        }


        [TestMethod()]
        public void GetUserRankTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int size1 = sc.Users.Count;
            Assert.AreEqual(size1, 0);
           // GameCenter.Instance.HigherRank = null;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            int id2 = 305077902;
            string name2 = "michele";
            String UserName2 = "michele";
            string password2 = "123456789";
            string email2 = "michele@post.bgu.ac.il";
            int money2 = 5000;

            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            IUser orelie = sc.GetUserWithId(id);
            IUser michele = sc.GetUserWithId(id2);
            List<IUser> all = sc.GetAllUser();
            int size = all.Count;
            Assert.IsTrue(size > 1);
            Assert.IsTrue(orelie.EditUserPoints( 50));
            Assert.AreEqual(orelie.Points(), 50);
            Assert.IsTrue(michele.EditUserPoints( 150));
            Assert.AreEqual(michele.Points(), 150);
        //    Assert.IsTrue(sc.IsHigestRankUser(id2));
          //  Assert.IsFalse(sc.IsHigestRankUser(Id));
            Assert.AreEqual(sc.GetUserRank(id), 2);
            Assert.AreEqual(sc.GetUserRank(id2), 1);


            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            int size2 = sc.Users.Count;
            Assert.AreEqual(size2, 0);
        }


       
      










    }


}