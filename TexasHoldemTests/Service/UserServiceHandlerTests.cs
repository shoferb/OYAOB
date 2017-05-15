using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Users;
using Moq;
using TexasHoldem.Logic.GameControl;


namespace TexasHoldem.Service.Tests
{
    [TestClass()]
    public class UserServiceHandlerTests
    {
        private SystemControl sc;
        private UserServiceHandler userService;
        private void Init()
        {
            sc = SystemControl.SystemControlInstance;
            userService = new UserServiceHandler();
            sc.Users = new List<Logic.Users.IUser>();
        }

        [TestMethod()]
        public void RegisterToSystemTest_good()
        {
            Init();
            Assert.IsTrue(userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_id_taken()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.RegisterToSystem(305077901, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_userName_taken()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.RegisterToSystem(305077902, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_email()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "oreliepost.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_passWord()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(305077901, "orelie", "orelie26", "123", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Name()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(305077901, " ", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Id()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(-1, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_money()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", -10, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void LoginUserTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.LoginUser("orelie26", "123456789"));

        }

        [TestMethod()]
        public void LoginUserTest_bad_username()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.LoginUser("orelie2", "123456789"));

        }

        [TestMethod()]
        public void LoginUserTest_bad_password()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.LoginUser("orelie26", "123s56789"));
        }

        [TestMethod()]
        public void LogoutUserTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.LoginUser("orelie26", "123456789");
            Assert.IsTrue(userService.LogoutUser(305077901));
        }

        [TestMethod()]
        public void LogoutUserTest_Bad_no_User()
        {
            Init();
            Assert.IsFalse(userService.LogoutUser(305077901));
        }

        [TestMethod()]
        public void LogoutUserTest_Bad_Id()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.LoginUser("orelie26", "123456789");
            Assert.IsFalse(userService.LogoutUser(305077902));
        }

        [TestMethod()]
        public void DeleteUserTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.DeleteUser("orelie26", "123456789"));
        }


        [TestMethod()]
        public void DeleteUserTest_Bad_user_name()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.DeleteUser("orelie24", "123456789"));
        }


        [TestMethod()]
        public void DeleteUserTest_Bad_password()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.DeleteUser("orelie26", "1234567s9"));
        }

        [TestMethod()]
        public void DeleteUserByIdTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.DeleteUserById(305077901));
        }


        [TestMethod()]
        public void DeleteUserByIdTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.DeleteUserById(305077902));
        }

        [TestMethod()]
        public void DeleteUserByIdTest_bad_inValid_id()
        {
            Init();
            Assert.IsFalse(userService.DeleteUserById(-2));
        }

        [TestMethod()]
        public void EditUserPointsTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserPoints(305077901,10));
        }

        [TestMethod()]
        public void EditUserPointsTest_Bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserPoints(305077901, 10));
        }

        [TestMethod()]
        public void EditUserPointsTest_Bad_Ivalid_points()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPoints(305077901, -100));
        }

        [TestMethod()]
        public void EditUserPasswordTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserPassword(305077901, "or123456")); 
        }

        [TestMethod()]
        public void EditUserPasswordTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserPassword(305077901, "or123456"));
        }

        [TestMethod()]
        public void EditUserPasswordTest_Bad_invalid_Password_small()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPassword(305077901, "or"));
        }


        [TestMethod()]
        public void EditUserPasswordTest_Bad_invalid_Password_empty()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPassword(305077901, " "));
        }

        [TestMethod()]
        public void EditUserEmailTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserEmail(305077901, "or123456@wall.co.il"));
        }

        [TestMethod()]
        public void EditUserEmailTest_Bad_invlid_email()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserEmail(305077901, "or123456wall.co.il"));
        }


        [TestMethod()]
        public void EditUserEmailTest_Bad_invlid_empty()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserEmail(305077901, " "));
        }

        [TestMethod()]
        public void EditUserNameTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserName(305077901, "NewUserName"));
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_userName_taken()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.RegisterToSystem(305077902, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(userService.EditUserName(305077902, "orelie26"));
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_userName_empty()
        {
            Init();
            userService.RegisterToSystem(305077902, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserName(305077902, " "));
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserName(305077902, "orelie"));
        }

        [TestMethod()]
        public void EditNameTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditName(305077901, "NewName"));
        }

        [TestMethod()]
        public void EditNameTest_Bad_name_empty()
        {
            Init();
            userService.RegisterToSystem(305077902, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(userService.EditName(305077902, " "));
        }

        [TestMethod()]
        public void EditNameTest_Bad_No_user()
        {
            Init();
            Assert.IsFalse(userService.EditName(305077902, "orelie"));
        }

        [TestMethod()]
        public void EditIdTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditId(305077901, 305077902));
        }

        [TestMethod()]
        public void EditIdTest_bad_id_taken()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.RegisterToSystem(305077902, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditId(305077901, 305077902));
        }

        [TestMethod()]
        public void EditIdTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditId(305077901, 305077902));
        }

        [TestMethod()]
        public void EditIdTest_bad_invalid_id()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditId(305077901, -1));
        }

        [TestMethod()]
        public void EditMoneyTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditMoney(305077901,800));
        }

        [TestMethod()]
        public void EditMoneyTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditMoney(305077901, 800));
        }

        [TestMethod()]
        public void EditMoneyTest_bad_invalidMoney()
        {
            Init();
            Assert.IsFalse(userService.EditMoney(305077901, -800));
        }

       

        [TestMethod()]
        public void EditUserAvatarTest()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserAvatar(305077901, "newPic"));
        }


        [TestMethod()]
        public void GetIUserByUserNameTest()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetIUserByUserName("orelie26");
            Assert.IsTrue(sc.Users.Contains(u));
        }

        [TestMethod()]
        public void GetAllUserTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.GetAllUser().Count==1);
        }

        [TestMethod()]
        public void GetUserByIdTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetUserById(305077901);
            Assert.IsTrue(sc.Users.Contains(u));
        }

        [TestMethod()]
        public void GetUserLeagueTest_good()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetUserById(305077901);
            Assert.AreEqual(u.GetLeague(),LeagueName.Unknow);
        }

        [TestMethod()]
        public void GetUserLeagueTest_bad()
        {
            Init();
            userService.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetUserById(305077901);
            Assert.AreNotEqual(u.GetLeague(), LeagueName.A);
        }

        [TestMethod()]
        public void DevideLeagueTest_good()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
            Assert.IsTrue(userService.DevideLeague());
         
        }

        [TestMethod()]
        public void DevideLeagueTest_good_check_league_A()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
           
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(10).GetLeague(),LeagueName.A);

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_C()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(6).GetLeague(), LeagueName.C);

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_B()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(8).Points(), 8);
            Assert.AreEqual(userService.GetUserById(8).GetLeague(), LeagueName.B);

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_D()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(3).GetLeague(), LeagueName.D);

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_E()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                userService.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i, i);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(1).GetLeague(), LeagueName.E);

        }
    }
}