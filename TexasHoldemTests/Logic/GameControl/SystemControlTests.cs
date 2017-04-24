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
            Assert.AreEqual(s1,s2);
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
            Assert.IsTrue(sc.CanCreateNewUser(id,UserName,password,email));
            Assert.IsTrue(sc.RegisterToSystem(id,name,UserName,password,money,email));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.Users.Remove(user));
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
            //bad id
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
        }


        [TestMethod()]
        public void AddNewUserBadTest()
        {
            //empty username
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            //create user with same id
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.IsUsernameFree(userName2));
            Assert.IsTrue((sc.IsUsernameFree(userName2)));
            Assert.IsFalse(sc.CanCreateNewUser(id, userName2, password2, email2));
            Assert.IsFalse(sc.RegisterToSystem(id, name2, userName2, password2, money, email2));
            User user2 = sc.FindUser(userName2);
            Assert.IsFalse(sc.GetAllUser().Contains(user2));
            //create user with same user name
            
            Assert.IsTrue(sc.IsIdFree(id2));
            Assert.IsFalse(sc.IsUsernameFree(UserName));
            Assert.IsFalse(sc.CanCreateNewUser(id2, UserName, password2, email2));
            Assert.IsFalse(sc.RegisterToSystem(id2, name2, UserName, password2, money, email2));
            User user3 = sc.FindUser(userName2);
            Assert.IsFalse(sc.GetAllUser().Contains(user3));
            
            Assert.IsTrue(sc.Users.Remove(user));

        }

        [TestMethod()]
        public void RemoveUserByIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUserByUserNameAndPassword(UserName,password));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

        [TestMethod()]
        public void RemoveUserTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.IsTrue(sc.RemoveUser(user));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

       

        [TestMethod()]
        public void LoginTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.AreEqual(user.IsActive,false);
            Assert.IsTrue(sc.Login(UserName,password));
            Assert.AreEqual(user.IsActive, true);

            Assert.IsTrue(sc.RemoveUser(user));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

        [TestMethod()]
        public void LogoutTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            Assert.AreEqual(user.IsActive, false);
            Assert.IsTrue(sc.Login(UserName, password));
            Assert.AreEqual(user.IsActive, true);
            Assert.IsTrue(sc.Logout(id));
            Assert.AreEqual(user.IsActive,false);
            Assert.IsTrue(sc.RemoveUser(user));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

        [TestMethod()]
        public void RegisterToSystemTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            User user = sc.GetUserWithId(id);
            Assert.IsTrue(sc.GetAllUser().Contains(user));
            
            Assert.IsTrue(sc.RemoveUser(user));
            Assert.IsFalse(sc.GetAllUser().Contains(user));
        }

        [TestMethod()]
        public void IsUsernameFreeTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
        }

        [TestMethod()]
        public void IsIdFreeTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
        }

        [TestMethod()]
        public void IsUserWithIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            Assert.IsTrue(sc.IsUserWithId(id));
            Assert.IsTrue(sc.IsUserWithId(id2));
            Assert.IsFalse(sc.IsUserWithId(-500));
            Assert.IsFalse(sc.IsUserWithId(0));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            Assert.IsFalse(sc.IsUserWithId(id));
            Assert.IsFalse(sc.IsUserWithId(id2));
        }

        [TestMethod()]
        public void GetUserWithIdTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsTrue(sc.IsUserWithId(id));
            User user = sc.GetUserWithId(id);
            Assert.AreEqual(user.Id,305077901);
            Assert.AreEqual(user.Name,"orelie");
            Assert.AreEqual(user.MemberName,"orelie123456");
            Assert.AreEqual(user.Points,0);
            Assert.AreEqual(user.Password,"123456789");
            Assert.AreEqual(user.Email,"orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Money,1000);
            Assert.IsTrue(sc.RemoveUserById(id));

        }

        [TestMethod()]
        public void IsValidEmailTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            Assert.IsTrue(sc.IsValidEmail("orelie@post.bgu.ac.il"));
            Assert.IsTrue(sc.IsValidEmail("orelie.shahar@gmail.com"));
            Assert.IsTrue(sc.IsValidEmail("orelie@walla.co.il"));
            Assert.IsFalse(sc.IsValidEmail("orelie.post.bgu.ac.il"));
            Assert.IsFalse(sc.IsValidEmail("wromgEmail"));
            Assert.IsFalse(sc.IsValidEmail("oreli2198#@%*_)(*&^%#!?@bgu.ac.il"));
        }

        [TestMethod()]
        public void EditEmailTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsTrue(sc.EditEmail(id,"orelie@post.bgu.ac.il"));
            Assert.IsTrue(sc.EditEmail(id, "orelie.shahar@gmail.com"));
            Assert.IsTrue(sc.EditEmail(id, "orelie@walla.co.il"));
            Assert.IsFalse(sc.EditEmail(id, "orelie.post.bgu.ac.il"));
            Assert.IsFalse(sc.EditEmail(id, "wromgEmail"));
            Assert.IsFalse(sc.EditEmail(id, "oreli2198#@%*_)(*&^%#!?@bgu.ac.il"));
            Assert.IsTrue(sc.RemoveUserById(id));

        }

        [TestMethod()]
        public void IsValidPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            Assert.IsFalse(sc.IsValidPassword(""));
            Assert.IsFalse(sc.IsValidPassword("  "));
            Assert.IsFalse(sc.IsValidPassword("1234!@"));
            Assert.IsFalse(sc.IsValidPassword("12345678912346"));
            Assert.IsTrue(sc.IsValidPassword("12345678"));
            Assert.IsTrue(sc.IsValidPassword("orelie123"));
            Assert.IsTrue(sc.IsValidPassword("1234567891"));
            Assert.IsTrue(sc.IsValidPassword("12345678912"));
            Assert.IsTrue(sc.IsValidPassword("123456789123"));
           
        }

        [TestMethod()]
        public void EditPasswordTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.EditPassword(id,""));
            Assert.IsFalse(sc.EditPassword(id, "  "));
            Assert.IsFalse(sc.EditPassword(id, "1234!@"));
            Assert.IsFalse(sc.EditPassword(id, "12345678912346"));
            Assert.IsTrue(sc.EditPassword(id, "12345678"));
            Assert.IsTrue(sc.EditPassword(id, "orelie123"));
            Assert.IsTrue(sc.EditPassword(id, "1234567891"));
            Assert.IsTrue(sc.EditPassword(id, "12345678912"));
            Assert.IsTrue(sc.EditPassword(id, "123456789123"));
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        [TestMethod()]
        public void EditUserNameTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            string newUserName1 = "try1";
            string newUserName2 = "try2";
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsFalse(sc.EditUserName(id,UserName2));
            Assert.IsFalse(sc.EditUserName(id2, UserName));
            Assert.IsTrue(sc.EditUserName(id,newUserName1));
            Assert.IsTrue(sc.EditUserName(id2, UserName));
            Assert.IsTrue(sc.EditUserName(id2, UserName2));
            Assert.IsFalse(sc.EditUserName(id2, newUserName1));
            Assert.IsTrue(sc.EditUserName(id2, newUserName2));
            Assert.IsTrue(sc.EditUserName(id, UserName2));
            Assert.IsFalse(sc.EditUserName(id, newUserName2));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

        [TestMethod()]
        public void EditAvatarTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsTrue(sc.EditAvatar(id,"newPath"));
            Assert.AreEqual(sc.GetUserWithId(id).Avatar,"newPath");
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        [TestMethod()]
        public void EditUserIDTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            int newId1 = 11111;
            int newId2 = 2222;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            Assert.IsFalse(sc.EditUserID(id, id2));
            Assert.IsFalse(sc.EditUserID(id2, id));
            Assert.IsTrue(sc.EditUserID(id, newId1));//id=1111
            Assert.IsTrue(sc.EditUserID(id2, id));//id2=305077901
            Assert.IsFalse(sc.EditUserID(id2, newId1));//fail
            Assert.IsTrue(sc.EditUserID(id, id2));//id=305077902
            Assert.IsFalse(sc.EditUserID(id2, 0));//fail
            Assert.IsFalse(sc.EditUserID(id2, -1));//fail
            Assert.IsFalse(sc.EditUserID(id2, -111111));//fail
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

        [TestMethod()]
        public void EditUserPointsTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(id).Points,0);
            Assert.IsTrue(sc.EditUserPoints(id,500));
            Assert.AreEqual(sc.GetUserWithId(id).Points, 500);
            Assert.IsTrue(sc.EditUserPoints(id, 0));
            Assert.AreEqual(sc.GetUserWithId(id).Points, 0);
            Assert.IsFalse(sc.EditUserPoints(id, -100));
            Assert.AreEqual(sc.GetUserWithId(id).Points, 0);;
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        [TestMethod()]
        public void EditUserMoneyTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(id).Money, 1000);
            Assert.IsTrue(sc.EditUserMoney(id, 500));
            Assert.AreEqual(sc.GetUserWithId(id).Money, 500);
            Assert.IsTrue(sc.EditUserMoney(id, 0));
            Assert.AreEqual(sc.GetUserWithId(id).Money, 0);
            Assert.IsFalse(sc.EditUserMoney(id, -100));
            Assert.AreEqual(sc.GetUserWithId(id).Money, 0); ;
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        [TestMethod()]
        public void EditActiveGameTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(id).IsActive, false);
            Assert.IsTrue(sc.EditActiveGame(id, true));
            Assert.AreEqual(sc.GetUserWithId(id).IsActive, true);
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        //Todo - always return -1 -10 why?
        [TestMethod()]
        public void RemoveRoomFromActiveRoomTest()
        {
            
            SystemControl sc = SystemControl.SystemControlInstance;
            int id = 305077901;
            string name1 = "orelie";
            String UserName = "orelie123456";
            string password = "123456789";
            string email1 = "orelie@post.bgu.ac.il";
            int money = 1000;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.AreEqual(sc.GetUserWithId(id).IsActive, false);
            Assert.IsTrue(sc.EditActiveGame(id, true));
            Assert.IsTrue(sc.EditUserPoints(id,50));
            bool CreateRoom = GameCenter.Instance.CreateNewRoom(id, 100, true, GameMode.Limit, 2, 8, 20, 5);
            Assert.IsTrue(CreateRoom);
            
            int size = GameCenter.Instance.GetAllGames().Count;
            Assert.AreEqual(size,1);
           List<GameRoom> rooms= GameCenter.Instance.GetAllGames();
            GameRoom room = rooms.First();
            Assert.AreNotEqual(room,null);
            Assert.AreEqual(GameCenter.Instance.LeagueGap,100);
            Assert.AreEqual(room._minRank,0);
            Assert.AreEqual(room._minRank, 100);
            Assert.AreEqual(GameCenter.Instance.GetLastGameRoom(),2);
//            Assert.AreEqual(GameCenter.Instance.GetLastGameRoom(), 1);
  //          Assert.AreEqual(GameCenter.Instance.GetLastGameRoom(), 0);
            Assert.AreEqual(room._id,2);
            Assert.IsTrue(GameCenter.Instance.RemoveRoom(2));
            Assert.IsTrue(sc.RemoveUserById(id));
        }

        [TestMethod()]
        public void RemoveRoomFromSpectetRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HasThisActiveGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HasThisSpectetorGameTest()
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
        public void IsHigestRankUserTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            int newId1 = 11111;
            int newId2 = 2222;
            Assert.IsTrue(sc.CanCreateNewUser(id, UserName, password, email1));
            Assert.IsTrue(sc.RegisterToSystem(id, name1, UserName, password, money, email1));
            Assert.IsFalse(sc.IsIdFree(id));
            Assert.IsTrue(sc.CanCreateNewUser(id2, UserName2, password2, email2));
            Assert.IsTrue(sc.RegisterToSystem(id2, name2, UserName2, password2, money2, email2));
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            List<User> all = sc.GetAllUser();
            int size = all.Count;
            Assert.IsTrue(size>1);
            User higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, null);
            Assert.IsTrue(sc.EditUserPoints(id,100));
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
            Assert.IsFalse(sc.IsHigestRankUser(id));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

        [TestMethod()]
        public void ChangeGapByHighestUserAndCreateNewLeagueTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            List<User> all = sc.GetAllUser();
            int size = all.Count;
            Assert.IsTrue(size > 1);
            User higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, null);
            Assert.IsTrue(sc.EditUserPoints(id, 100));
            Assert.AreEqual(orelie.Points, 100);
            Assert.AreEqual(orelie.IsHigherRank, true);
            Assert.AreEqual(michele.IsHigherRank, false);
            higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, orelie);
            Assert.IsTrue(sc.EditUserPoints(id2, 1000));
            Assert.AreEqual(michele.Points, 1000);
            Assert.AreEqual(GameCenter.Instance.leagueGap, 100);
            Assert.IsFalse(sc.ChangeGapByHighestUserAndCreateNewLeague(id,50));
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreNotEqual(GameCenter.Instance.leagueGap ,50);
            Assert.IsFalse(sc.ChangeGapByHighestUserAndCreateNewLeague(id2, 50));
            Assert.AreEqual(michele.IsHigherRank, true);
            Assert.AreEqual(GameCenter.Instance.leagueGap, 50);
            higher = GameCenter.Instance.HigherRank;
            Assert.AreEqual(higher, michele);


            Assert.IsTrue(sc.IsHigestRankUser(id2));
            Assert.IsFalse(sc.IsHigestRankUser(id));
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

        [TestMethod()]
        public void SortByRankTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            User Amir = sc.GetUserWithId(id3);
            Assert.IsTrue(sc.EditUserPoints(id2, 1000));
            Assert.AreEqual(michele.Points, 1000);
            Assert.IsTrue(sc.EditUserPoints(id, 10));
            Assert.AreEqual(orelie.Points, 10);
            Assert.IsTrue(sc.EditUserPoints(id3, 5000));
            Assert.AreEqual(Amir.Points, 5000);
            List<User> byRank = sc.SortByRank();
            int orelieIndex = byRank.IndexOf(orelie);
            int micheleIndex = byRank.IndexOf(michele);
            int AmirIndex = byRank.IndexOf(Amir);
            Assert.AreEqual(AmirIndex, 0);
            Assert.AreEqual(micheleIndex, 1);
            Assert.AreEqual(orelieIndex, 2);
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            Assert.IsTrue(sc.RemoveUserById(id3));
        }

        [TestMethod()]
        public void GetUserRankTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            List<User> all = sc.GetAllUser();
            int size = all.Count;
            Assert.IsTrue(size > 1);
            Assert.IsTrue(sc.EditUserPoints(id, 50));
            Assert.AreEqual(orelie.Points, 50);
            Assert.IsTrue(sc.EditUserPoints(id2, 150));
            Assert.AreEqual(michele.Points, 150);
            Assert.IsTrue(sc.IsHigestRankUser(id2));
            Assert.IsFalse(sc.IsHigestRankUser(id));
            Assert.AreEqual(sc.GetUserRank(id),2);
            Assert.AreEqual(sc.GetUserRank(id2), 1);

           
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

       

        [TestMethod()]
        public void MovePlayerBetweenLeagueTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            Assert.IsTrue(sc.EditUserPoints(id2, 1000));
            Assert.AreEqual(michele.Points, 1000);
            Assert.AreEqual(michele.IsHigherRank, true);
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreEqual(orelie.Points, 0);
            Assert.IsTrue(sc.MovePlayerBetweenLeague(id2,id,450));
            Assert.AreEqual(michele.IsHigherRank, true);
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreEqual(orelie.Points, 450);
            Assert.IsTrue(sc.MovePlayerBetweenLeague(id2, id, 1500));
            Assert.AreEqual(michele.Points, 1000);
            Assert.AreEqual(michele.IsHigherRank, false);
            Assert.AreEqual(orelie.IsHigherRank, true);
            Assert.AreEqual(orelie.Points, 1500);
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
        }

        [TestMethod()]
        public void SetDefultLeauseToNewUsersTest()
        {
            SystemControl sc = SystemControl.SystemControlInstance;
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
            User orelie = sc.GetUserWithId(id);
            User michele = sc.GetUserWithId(id2);
            User Amir = sc.GetUserWithId(id3);
            Assert.IsTrue(sc.EditUserPoints(id2, 1000));
            Assert.AreEqual(michele.Points, 1000);
            Assert.AreEqual(michele.IsHigherRank, true);
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreEqual(Amir.IsHigherRank, false);
            Assert.AreEqual(orelie.Points, 0);
            Assert.AreEqual(Amir.Points, 0);
            Assert.IsTrue(sc.SetDefultLeauseToNewUsers(id2,500));
            
            Assert.AreEqual(orelie.Points, 500);
            Assert.AreEqual(Amir.Points, 500);
            
            Assert.IsTrue(sc.EditUserPoints(id3, 0));
            Assert.IsTrue(sc.SetDefultLeauseToNewUsers(id2, 2000));
            Assert.AreEqual(Amir.Points, 2000);
            Assert.AreEqual(michele.IsHigherRank, false);
            Assert.AreEqual(orelie.IsHigherRank, false);
            Assert.AreEqual(Amir.IsHigherRank, true);
            Assert.IsTrue(sc.RemoveUserById(id));
            Assert.IsTrue(sc.RemoveUserById(id2));
            Assert.IsTrue(sc.RemoveUserById(id3));
        }
    }
}