using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
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
            Assert.IsFalse(sc.RemoveUser(sc.GetUserWithId(305077901)));
        }

        [TestMethod()]
        public void GetIUSerByUsernameTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = sc.GetUserWithId(305077901);
            Assert.IsTrue(sc.Users.Contains(u));
        }


        [TestMethod()]
        public void GetIUSerByUsernameTest_bad_no_user()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            IUser u = sc.GetUserWithId(305077901);
            Assert.IsFalse(sc.Users.Contains(u));
        }

        [TestMethod()]
        public void RegisterToSystemTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsTrue(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_id_taken()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_userName_taken()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.RegisterToSystem(305077902, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_email()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "oreliepost.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_passWord()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Name()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RegisterToSystem(305077901, " ", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Id()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RegisterToSystem(-1, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_money()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", -10, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void CanCreateNewUserTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsTrue(sc.CanCreateNewUser(305077901, "orelie26", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_Bad_id()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(-1, "orelie26", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_Bad_userName()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(305077901, " ", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_bad_email()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(305077901, "orelie26", "123456789", "oreliepost.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_bad_email_empty()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(305077901, "orelie26", "123456789", "oreliepost.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_password()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(305077901, "orelie26", "123", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_password_empty()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.CanCreateNewUser(305077901, "orelie26", " ", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void IsUsernameFreeTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsTrue(sc.IsUsernameFree("orelie"));

        }

        [TestMethod()]
        public void IsUsernameFreeTest_bad_taken()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.IsUsernameFree("orelie26"));
        }

        [TestMethod()]
        public void IsUsernameFreeTest_bad_empty()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(sc.IsUsernameFree(" "));
        }

        [TestMethod()]
        public void IsIdFreeTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();

            Assert.IsTrue(sc.IsIdFree(305077901));
        }

        [TestMethod()]
        public void IsIdFreeTest_bad_taken()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(sc.IsIdFree(305077901));
        }

        [TestMethod()]
        public void IsIdFreeTest_bad_Invalid_id()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.IsIdFree(-1));
        }

        [TestMethod()]
        public void IsUserExistTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(sc.IsUserExist(305077901));

        }

        [TestMethod()]
        public void IsUserExistTest_bad_no_user()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.IsUserExist(3));
        }

        [TestMethod()]
        public void IsUserExistTest_bad_invalid_id()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            Assert.IsFalse(sc.IsUserExist(-3));
        }

        [TestMethod()]
        public void GetUserWithIdTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = sc.GetUserWithId(305077901);
            Assert.IsTrue(sc.Users.Contains(u));
        }

        [TestMethod()]
        public void GetUserWithIdTest_bad_no_user()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            IUser u = sc.GetUserWithId(305077901);
            Assert.IsFalse(sc.Users.Contains(u));
        }

      

        [TestMethod()]
        public void GetAllUserTes_good1()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            sc.RegisterToSystem(305077902, "orelie", "orelie25", "123456789", 15000, "orelie@post.bgu.ac.il");
            Users.IUser u1 = sc.GetUserWithId(305077901);
            List<IUser> u = sc.GetAllUser();
            Assert.IsTrue(u.Contains(u1));
        }

        [TestMethod()]
        public void GetAllUserTes_good2()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(305077901, "orelie", "orelie26", "123456789", 15000, "orelie@post.bgu.ac.il");
            sc.RegisterToSystem(305077902, "orelie", "orelie25", "123456789", 15000, "orelie@post.bgu.ac.il");
            Users.IUser u1 = sc.GetUserWithId(305077902);
            List<IUser> u = sc.GetAllUser();
            Assert.IsTrue(u.Contains(u1));
        }

      

        [TestMethod()]
        public void GetAllUnKnowUsersTest_good_1()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 5; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                
                sc.GetUserWithId(i).EditUserPoints( i);
            }

            for (int i = 3; i < 5; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            List<IUser> un = sc.GetAllUnKnowUsers();
            Assert.IsTrue(un.Count==2);
        }

        [TestMethod()]
        public void GetAllUnKnowUsersTest_good_2()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 5; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 3; i < 5; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            List<IUser> un = sc.GetAllUnKnowUsers();
            IUser u = sc.GetUserWithId(1);
            Assert.IsTrue(un.Contains(u));
        }

      

        [TestMethod()]
        public void DivideLeagueTest_good()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
           Assert.IsTrue(sc.DivideLeague());
        }

        [TestMethod()]
        public void DivideLeagueTest_good_A()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            sc.DivideLeague();
            Assert.AreEqual(sc.GetUserWithId(10).GetLeague(),LeagueName.A);
        }

        [TestMethod()]
        public void DivideLeagueTest_good_B()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            sc.DivideLeague();
            Assert.AreEqual(sc.GetUserWithId(8).GetLeague(), LeagueName.B);
        }

        [TestMethod()]
        public void DivideLeagueTest_good_C()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            sc.DivideLeague();
            Assert.AreEqual(sc.GetUserWithId(6).GetLeague(), LeagueName.C);
        }

        [TestMethod()]
        public void DivideLeagueTest_good_D()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            sc.DivideLeague();
            Assert.AreEqual(sc.GetUserWithId(4).GetLeague(), LeagueName.D);
        }
        [TestMethod()]
        public void DivideLeagueTest_good_E()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i;
                sc.RegisterToSystem(i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                sc.GetUserWithId(i).EditUserPoints(i);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sc.GetUserWithId(i).IncGamesPlay();
                }
            }
            sc.DivideLeague();
            Assert.AreEqual(sc.GetUserWithId(2).GetLeague(), LeagueName.E);
        }

        [TestMethod()]
        public void DivideLeagueTest_good_Unknow()
        {
            sc = SystemControl.SystemControlInstance;
            sc.Users = new List<IUser>();
            sc.RegisterToSystem(1, "orelie", "orelie", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.AreEqual(sc.GetUserWithId(1).GetLeague(), LeagueName.Unknow);
        }
    }
}


