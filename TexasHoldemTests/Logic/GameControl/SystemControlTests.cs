using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;

namespace TexasHoldem.Logic.Game_Control.Tests
{
    [TestClass()]
    public class SystemControlTests
    {
        private static readonly LogControl logControl = new LogControl();
        private readonly SystemControl _sc = new SystemControl(logControl);
        private static readonly LogControl LogControl = new LogControl();
        private static readonly SystemControl SysControl = new SystemControl(LogControl);
        private static readonly ReplayManager ReplayManager = new ReplayManager();
        private static readonly SessionIdHandler Sender = new SessionIdHandler();
        private static readonly GameCenter GameCenter = new GameCenter(SysControl, LogControl, ReplayManager, Sender);
        private readonly UserServiceHandler _userService = new UserServiceHandler(GameCenter,SysControl);



        [TestMethod()]
        public void RemoveUserByIdTest_good()
        {
            
            _sc.RegisterToSystem(465002500, "orelie", "orelie465002500", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(_sc.RemoveUserById(465002500));
            _userService.DeleteUserById(465002500);
        }

        [TestMethod()]
        public void RemoveUserByIdTest_Bad_no_user()
        {
            
            Assert.IsFalse(_sc.RemoveUserById(0005522));
        }

        public void RemoveUserByIdTest_Bad_invalidId()
        {
            
            _sc.RegisterToSystem(99665858, "orelie", "orelie99665858", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.RemoveUserById(-1));
            _userService.DeleteUserById(99665858);
        }

        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest_good()
        {
          
            _sc.RegisterToSystem(44500252, "orelie", "orelie44500252", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(_sc.RemoveUserByUserNameAndPassword("orelie44500252", "123456789"));
            _userService.DeleteUserById(44500252);

        }


        [TestMethod()]
        public void RemoveUserByUserNameAndPasswordTest_Bad_password()
        {
           
            _sc.RegisterToSystem(88585000, "orelie", "orelie88585000", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.RemoveUserByUserNameAndPassword("orelie88585000", "83456789"));
            _userService.DeleteUserById(88585000);

        }

        [TestMethod()]
        public void RemoveUserTest_good()
        {
           
            _sc.RegisterToSystem(775800045, "orelie", "orelie775800045", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(_sc.RemoveUser(_sc.GetUserWithId(775800045)));
            _userService.DeleteUserById(775800045);

        }

        [TestMethod()]
        public void RemoveUserTest_bad_user_null()
        {
            
            Assert.IsFalse(_sc.RemoveUser(null));
        }

        [TestMethod()]
        public void RemoveUserTest_bad_not_in_users()
        {
            
            Assert.IsFalse(_sc.RemoveUser(_sc.GetUserWithId(99850000)));
        }

        [TestMethod()]
        public void GetIUSerByUsernameTest_good()
        {
            
            _sc.RegisterToSystem(98985854, "orelie", "orelie98985854", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = _sc.GetUserWithId(98985854);
            Assert.AreEqual(u.MemberName(), "orelie98985854");
            _userService.DeleteUserById(98985854);


        }


        [TestMethod()]
        public void GetIUSerByUsernameTest_bad_no_user()
        {
           
            IUser u = _sc.GetUserWithId(000044585);
            Assert.AreEqual(u,null);
        }

        [TestMethod()]
        public void RegisterToSystemTest_good()
        {
       
            Assert.IsTrue(_sc.RegisterToSystem(565658880, "orelie", "orelie565658880", "123456789", 15000, "orelie@post.bgu.ac.il"));
            _userService.DeleteUserById(565658880);

        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_id_taken()
        {
          
            _sc.RegisterToSystem(90520650, "orelie", "orelie90520650", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.RegisterToSystem(90520650, "orelie", "orelie_taken-RegisterToSystemTest_bad_id_taken()", "123456789", 15000, "orelie@post.bgu.ac.il"));
            _userService.DeleteUserById(90520650);

        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_userName_taken()
        {
           
            _sc.RegisterToSystem(880088052, "orelie", "orelie880088052", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.RegisterToSystem(880088053, "orelie", "orelie880088052", "123456789", 15000, "orelie@post.bgu.ac.il"));
            _userService.DeleteUserById(880088052);

        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_email()
        {
           
            Assert.IsFalse(_sc.RegisterToSystem(005522525, "orelie", "RegisterToSystemTest_bad_Not_Valid_email()", "123456789", 15000, "oreliepost.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_passWord()
        {
          
            Assert.IsFalse(_sc.RegisterToSystem(0055225265, "orelie", "RegisterToSystemTest_bad_Not_Valid_passWord()", "123", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Name()
        {
           
            Assert.IsFalse(_sc.RegisterToSystem(0005522558, " ", "RegisterToSystemTest_bad_Not_Valid_Name()", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Id()
        {
           
            Assert.IsFalse(_sc.RegisterToSystem(-1, "orelie", "RegisterToSystemTest_bad_Not_Valid_Id()", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_money()
        {
            
            Assert.IsFalse(_sc.RegisterToSystem(006656568, "orelie", "RegisterToSystemTest_bad_Not_Valid_money()", "123456789", -10, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void CanCreateNewUserTest_good()
        {
            
            Assert.IsTrue(_sc.CanCreateNewUser(88585900, "orelie88585900", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_Bad_id()
        {
         
            Assert.IsFalse(_sc.CanCreateNewUser(-1, "orelie26", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_Bad_userName()
        {
            
            Assert.IsFalse(_sc.CanCreateNewUser(305077901, " ", "123456789", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_bad_email()
        {
            
            Assert.IsFalse(_sc.CanCreateNewUser(305077901, "orelie26", "123456789", "oreliepost.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_bad_email_empty()
        {
          
            Assert.IsFalse(_sc.CanCreateNewUser(305077901, "orelie26", "123456789", "oreliepost.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_password()
        {
            
            Assert.IsFalse(_sc.CanCreateNewUser(305077901, "orelie26", "123", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void CanCreateNewUserTest_password_empty()
        {
            
            Assert.IsFalse(_sc.CanCreateNewUser(305077901, "orelie26", " ", "orelie@post.bgu.ac.il"));
        }
        [TestMethod()]
        public void IsUsernameFreeTest_good()
        {
            
            Assert.IsTrue(_sc.IsUsernameFree("IsUsernameFreeTest_good()"));

        }

        [TestMethod()]
        public void IsUsernameFreeTest_bad_taken()
        {
           
            _sc.RegisterToSystem(305091, "orelie", "orelie305091", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.IsUsernameFree("orelie305091"));
            _userService.DeleteUserById(305091);
        }

        [TestMethod()]
        public void IsUsernameFreeTest_bad_empty()
        {
           
            _sc.RegisterToSystem(8830521, "orelie", "orelie8830521", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(_sc.IsUsernameFree(" "));
            _userService.DeleteUserById(8830521);
        }

        [TestMethod()]
        public void IsIdFreeTest_good()
        {
          
            Assert.IsTrue(_sc.IsIdFree(0000005851));
        }

        [TestMethod()]
        public void IsIdFreeTest_bad_taken()
        {
            
            _sc.RegisterToSystem(5551549, "orelie", "orelie5551549", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(_sc.IsIdFree(5551549));
            _userService.DeleteUserById(5551549);

        }

        [TestMethod()]
        public void IsIdFreeTest_bad_Invalid_id()
        {
           
            Assert.IsFalse(_sc.IsIdFree(-1));
        }

        [TestMethod()]
        public void IsUserExistTest_good()
        {
           
            _sc.RegisterToSystem(55000845, "orelie", "orelie55000845", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(_sc.IsUserExist(55000845));
            _userService.DeleteUserById(55000845);

        }

        [TestMethod()]
        public void IsUserExistTest_bad_no_user()
        {
            Assert.IsFalse(_sc.IsUserExist(54000875));
        }

        [TestMethod()]
        public void IsUserExistTest_bad_invalid_id()
        {
            Assert.IsFalse(_sc.IsUserExist(-3));
        }

        [TestMethod()]
        public void GetUserWithIdTest_good()
        {
           
            _sc.RegisterToSystem(88880252, "orelie", "orelie88880252", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = _sc.GetUserWithId(88880252);
            Assert.IsTrue(u!=null);
            _userService.DeleteUserById(88880252);

        }

        [TestMethod()]
        public void GetUserWithIdTest_bad_no_user()
        {

            IUser u = _sc.GetUserWithId(006560001);
            Assert.AreEqual(u,null);
        }



     

      


        [TestMethod()]
        public void DivideLeagueTest_good()
        {
          
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i+8852;
                _sc.RegisterToSystem(i + 8852, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");

                _sc.GetUserWithId(i + 8852).EditUserPoints(i + 8852);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i + 8852).IncGamesPlay();
                }
            }
            Assert.IsTrue(_sc.DivideLeague());
            for (int i = 1; i < 11; i++)
            {
                int t = i + 8852;
                _userService.DeleteUserById(t);
            }
        }
        /*
        [TestMethod()]
        public void DivideLeagueTest_good_A()
        {
            _sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*58501;
                _sc.RegisterToSystem(i * 58501, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                _userService.EditUserPoints(i * 58501, i * 58501);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i * 58501).IncGamesPlay();
                }
            }
            _sc.DivideLeague();
            Assert.AreEqual(_sc.GetUserWithId(10  * 58501).GetLeague(), LeagueName.A);
            for (int i = 1; i < 11; i++)
            {
                _userService.DeleteUserById(i * 58501);
            }
        }

        [TestMethod()]
        public void DivideLeagueTest_good_B()
        {
            _sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i * 450504;
                _sc.RegisterToSystem(i * 450504, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                _userService.EditUserPoints(i * 450504, i * 450504);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i * 450504).IncGamesPlay();
                }
            }
            _sc.DivideLeague();
            Assert.AreEqual(_sc.GetUserWithId(8 * 450504).GetLeague(), LeagueName.B);
            for (int i = 1; i < 11; i++)
            {
                _userService.DeleteUserById(i * 450504);
            }

        }

        [TestMethod()]
        public void DivideLeagueTest_good_C()
        {
            _sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i * 774852;
                _sc.RegisterToSystem(i * 774852, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                _userService.EditUserPoints(i * 774852, i * 774852);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i * 774852).IncGamesPlay();
                }
            }
            _sc.DivideLeague();
            Assert.AreEqual(_sc.GetUserWithId(6 * 774852).GetLeague(), LeagueName.C);
            for (int i = 1; i < 11; i++)
            {
                _userService.DeleteUserById(i * 774852);
            }
            
        }

        [TestMethod()]
        public void DivideLeagueTest_good_D()
        {
            _sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i * 9500052;
                _sc.RegisterToSystem(i * 9500052, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                _userService.EditUserPoints(i * 9500052, i * 9500052);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i * 9500052).IncGamesPlay();
                }
            }
            _sc.DivideLeague();
            Assert.AreEqual(_sc.GetUserWithId(4 * 9500052).GetLeague(), LeagueName.D);
            for (int i = 1; i < 11; i++)
            {
                _userService.DeleteUserById(i * 9500052);
            }
        }
        [TestMethod()]
        public void DivideLeagueTest_good_E()
        {
             _sc.Users = new List<IUser>();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i * 39552;
                _sc.RegisterToSystem(i * 39552, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                _userService.EditUserPoints(i * 39552, i * 39552);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _sc.GetUserWithId(i * 39552).IncGamesPlay();
                }
            }
            _sc.DivideLeague();
            Assert.AreEqual(_sc.GetUserWithId(2 * 39552).GetLeague(), LeagueName.E);
            for (int i = 1; i < 11; i++)
            {
                _userService.DeleteUserById(i * 39552);
            }
            
        }
        */
        [TestMethod()]
        public void DivideLeagueTest_good_Unknow()
        {
            
            _sc.RegisterToSystem(58515120, "orelie", "orelie58515120", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.AreEqual(_sc.GetUserWithId(58515120).GetLeague(), LeagueName.Unknow);
            _userService.DeleteUserById(58515120);
        }
    }
}


