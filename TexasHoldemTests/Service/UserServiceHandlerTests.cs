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
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;

namespace TexasHoldem.Service.Tests
{
    [TestClass()]
    public class UserServiceHandlerTests
    {
        private SystemControl sc;
        private LogControl logs;
        private GameCenter games;
        private ReplayManager replays;
        private UserServiceHandler userService;
        private static SessionIdHandler sender;
        private void Init()
        {
            logs = new LogControl();
            sc = new SystemControl(logs);
            replays = new ReplayManager();
            sender = new SessionIdHandler();


        games = new GameCenter(sc, logs, replays,sender);
            userService = new UserServiceHandler(games, sc);
            sc.Users = new List<Logic.Users.IUser>();
        }

        [TestMethod()]
        public void RegisterToSystemTest_good()
        {
            Init();
            Assert.IsTrue(userService.RegisterToSystem(789987, "orelie", "orelie789987", "123456789", 15000, "orelie@post.bgu.ac.il"));
            userService.DeleteUserById(789987);

        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_id_taken()
        {
            Init();
            userService.RegisterToSystem(95959595, "orelie", "orelie95959595", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.RegisterToSystem(95959595, "orelie-notTaken", "orelie2", "123456789", 15000, "orelie@post.bgu.ac.il"));
            userService.DeleteUserById(95959595);
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_userName_taken()
        {
            Init();
            userService.RegisterToSystem(95959596, "orelie", "orelie95959596", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.RegisterToSystem(959595957, "orelie", "orelie95959596", "123456789", 15000, "orelie@post.bgu.ac.il"));
            userService.DeleteUserById(95959596);
     
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_email()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(84848485, "orelie", "orelie84848485", "123456789", 15000, "oreliepost.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_passWord()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(465465487, "orelie", "orelie465465487", "123", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Name()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(59595009, " ", "orelie59595009", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_Id()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(-1, "orelie", "orelie26-1", "123456789", 15000, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void RegisterToSystemTest_bad_Not_Valid_money()
        {
            Init();
            Assert.IsFalse(userService.RegisterToSystem(598432, "orelie", "orelie598432", "123456789", -10, "orelie@post.bgu.ac.il"));
        }

        [TestMethod()]
        public void LoginUserTest_good()
        {
            Init();
            userService.RegisterToSystem(585858524, "orelie", "orelie585858524", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.LoginUser("orelie585858524", "123456789").IsLogin());
            userService.DeleteUserById(585858524);
        }


        [TestMethod()]
        public void LoginUserTest_bad_password()
        {
            Init();
            userService.RegisterToSystem(94949042, "orelie", "orelie94949042", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.LogoutUser(94949042);
            Assert.IsFalse(userService.LoginUser("orelie94949042", "123s56789").IsLogin());
            userService.DeleteUserById(94949042);
        }

        [TestMethod()]
        public void LogoutUserTest_good()
        {
            Init();
            userService.RegisterToSystem(48653256, "orelie", "orelie48653256", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.LoginUser("orelie48653256", "123456789");
            Assert.IsFalse(userService.LogoutUser(48653256).IsLogin());
            userService.DeleteUserById(48653256);
        }

     
     

        [TestMethod()]
        public void DeleteUserTest_good()
        {
            Init();
            userService.RegisterToSystem(87564587, "orelie", "orelie87564587", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.DeleteUser("orelie87564587", "123456789"));
            userService.DeleteUserById(87564587);
        }


        [TestMethod()]
        public void DeleteUserTest_Bad_user_name()
        {
            Init();
            userService.RegisterToSystem(5858412, "orelie", "orelie5858412", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.DeleteUser("orelie58584no", "123456789"));
            userService.DeleteUserById(5858412);
        }


        [TestMethod()]
        public void DeleteUserTest_Bad_password()
        {
            Init();
            userService.RegisterToSystem(777888558, "orelie", "orelie777888558", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.DeleteUser("orelie777888558", "1234567s9"));
            userService.DeleteUserById(777888558);
        }

        [TestMethod()]
        public void DeleteUserByIdTest_good()
        {
            Init();
            userService.RegisterToSystem(99663356, "orelie", "orelie99663356", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.DeleteUserById(99663356));
        }


        [TestMethod()]
        public void DeleteUserByIdTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.DeleteUserById(000000));
        }

        [TestMethod()]
        public void DeleteUserByIdTest_bad_inValid_id()
        {
            Init();
            Assert.IsFalse(userService.DeleteUserById(-2000));
        }

        [TestMethod()]
        public void EditUserPointsTest_good()
        {
            Init();
            userService.RegisterToSystem(778854580, "orelie", "orelie778854580", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserPoints(778854580, 778854850));
            userService.DeleteUserById(778854580);
        }

        [TestMethod()]
        public void EditUserPointsTest_Bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserPoints(0050505, 884858523));
        }

        [TestMethod()]
        public void EditUserPointsTest_Bad_Ivalid_points()
        {
            Init();
            userService.RegisterToSystem(595596522, "orelie", "orelie595596522", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPoints(595596522, -100));
            userService.DeleteUserById(595596522);
        }
      

        [TestMethod()]
        public void EditUserPasswordTest_good()
        {
            Init();
            userService.RegisterToSystem(898989854, "orelie", "orelie898989854", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserPassword(898989854, "or123456"));
            userService.DeleteUserById(898989854);
        }

        [TestMethod()]
        public void EditUserPasswordTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserPassword(789877452, "or123456"));
        }

        [TestMethod()]
        public void EditUserPasswordTest_Bad_invalid_Password_small()
        {
            Init();
            userService.RegisterToSystem(852465000, "orelie", "orelie852465000", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPassword(852465000, "or"));
            userService.DeleteUserById(852465000);
        }


        [TestMethod()]
        public void EditUserPasswordTest_Bad_invalid_Password_empty()
        {
            Init();
            userService.RegisterToSystem(852467300, "orelie", "orelie852467300", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserPassword(852467300, " "));
            userService.DeleteUserById(852467300);
        }

        [TestMethod()]
        public void EditUserEmailTest_good()
        {
            Init();
            userService.RegisterToSystem(956868001, "orelie", "orelie956868001", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserEmail(956868001, "or123456@wall.co.il"));
            userService.DeleteUserById(956868001);
        }

        [TestMethod()]
        public void EditUserEmailTest_Bad_invlid_email()
        {
            Init();
            userService.RegisterToSystem(85111220, "orelie", "orelie85111220", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserEmail(85111220, "or123456wall.co.il"));
            userService.DeleteUserById(85111220);
        }


        [TestMethod()]
        public void EditUserEmailTest_Bad_invlid_empty()
        {
            Init();
            userService.RegisterToSystem(888844201, "orelie", "orelie888844201", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserEmail(888844201, " "));
            userService.DeleteUserById(888844201);
        }

        [TestMethod()]
        public void EditUserNameTest_good()
        {
            Init();
            userService.RegisterToSystem(959588841, "orelie", "orelie959588841", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserName(959588841, "NewUserName"));
            userService.DeleteUserById(959588841);
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_userName_taken()
        {
            Init();
            userService.RegisterToSystem(884466572, "orelie", "orelie884466572", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.RegisterToSystem(884466573, "orelie", "orelie884466573", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(userService.EditUserName(884466573, "orelie884466572"));
            userService.DeleteUserById(884466572);
            userService.DeleteUserById(884466573);
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_userName_empty()
        {
            Init();
            userService.RegisterToSystem(336652125, "orelie", "orelie336652125", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditUserName(336652125, " "));
            userService.DeleteUserById(336652125);
        }

        [TestMethod()]
        public void EditUserNameTest_Bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditUserName(0000000545, "orelie"));
        }

        [TestMethod()]
        public void EditNameTest_good()
        {
            Init();
            userService.RegisterToSystem(592220015, "orelie", "orelie592220015", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditName(592220015, "NewName-test-editName_good"));
            userService.DeleteUserById(592220015);

        }

        [TestMethod()]
        public void EditNameTest_Bad_name_empty()
        {
            Init();
            userService.RegisterToSystem(884650054, "orelie", "orelie884650054", "123456789", 15000, "orelie@post.bgu.ac.il");

            Assert.IsFalse(userService.EditName(884650054, " "));
            userService.DeleteUserById(884650054);
        }

        [TestMethod()]
        public void EditNameTest_Bad_No_user()
        {
            Init();
            Assert.IsFalse(userService.EditName(0000004587, "orelie-noUsertoEdit"));
        }

        [TestMethod()]
        public void EditIdTest_good()
        {
            Init();
            userService.RegisterToSystem(887570054, "orelie", "orelie887570054", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditId(887570054, 887570050));
            userService.DeleteUserById(887570050);

        }

        [TestMethod()]
        public void EditIdTest_bad_id_taken()
        {
            Init();
            userService.RegisterToSystem(005523512, "orelie", "orelie005523512", "123456789", 15000, "orelie@post.bgu.ac.il");
            userService.RegisterToSystem(005523513, "orelie", "orelie005523513", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditId(005523512, 005523513));
            userService.DeleteUserById(005523512);
            userService.DeleteUserById(005523513);
        }

        [TestMethod()]
        public void EditIdTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditId(00556485, 005564866));
        }

        [TestMethod()]
        public void EditIdTest_bad_invalid_id()
        {
            Init();
            userService.RegisterToSystem(525685000, "orelie", "orelie525685000", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditId(525685000, -1));
            userService.DeleteUserById(525685000);
        }

        [TestMethod()]
        public void EditMoneyTest_good()
        {
            Init();
            userService.RegisterToSystem(500264550, "orelie", "orelie500264550", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditMoney(500264550, 800));
            userService.DeleteUserById(500264550);
        }

        [TestMethod()]
        public void EditMoneyTest_bad_no_user()
        {
            Init();
            Assert.IsFalse(userService.EditMoney(003356568, 800));
        }

        [TestMethod()]
        public void EditMoneyTest_bad_invalidMoney()
        {
            Init();
            userService.RegisterToSystem(003356568, "orelie", "orelie003356568", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsFalse(userService.EditMoney(003356568, -800));
            userService.DeleteUserById(003356568);

        }



        [TestMethod()]
        public void EditUserAvatarTest_good()
        {
            Init();
            userService.RegisterToSystem(646558801, "orelie", "orelie646558801", "123456789", 15000, "orelie@post.bgu.ac.il");
            Assert.IsTrue(userService.EditUserAvatar(646558801, "newPic-EditUserAvatarTest_good"));
            userService.DeleteUserById(646558801);

        }


        [TestMethod()]
        public void GetIUserByUserNameTest()
        {
            Init();
            userService.RegisterToSystem(500005243, "orelie", "orelie500005243", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetIUserByUserName("orelie500005243");
            Assert.AreEqual(sc.GetIUSerByUsername("orelie500005243").MemberName(), "orelie500005243");
            userService.DeleteUserById(500005243);

        }

      

        [TestMethod()]
        public void GetUserByIdTest_good()
        {
            Init();
            userService.RegisterToSystem(90036055, "orelie", "orelie90036055", "123456789", 15000, "orelie@post.bgu.ac.il");
            
            Assert.AreEqual(userService.GetUserById(90036055).Id(), 90036055);
            userService.DeleteUserById(90036055);

        }

        [TestMethod()]
        public void GetUserLeagueTest_good()
        {
            Init();
            userService.RegisterToSystem(555000642, "orelie", "orelie555000642", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetUserById(555000642);
            Assert.AreEqual(u.GetLeague(), LeagueName.Unknow);
            userService.DeleteUserById(555000642);

        }

        [TestMethod()]
        public void GetUserLeagueTest_bad()
        {
            Init();
            userService.RegisterToSystem(556600025, "orelie", "orelie556600025", "123456789", 15000, "orelie@post.bgu.ac.il");
            IUser u = userService.GetUserById(556600025);
            Assert.AreNotEqual(u.GetLeague(), LeagueName.A);
            userService.DeleteUserById(556600025);

        }

        [TestMethod()]
        public void DevideLeagueTest_good()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i * 542;
                userService.RegisterToSystem(i * 542, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 542, i * 542);
            }
            Assert.IsTrue(userService.DevideLeague());
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 542);
            }

        }
        /*
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_A()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*536;
                userService.RegisterToSystem(i * 536, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 536, i * 536);
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i * 536).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(10*536).GetLeague(), LeagueName.A);
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 536);
            }

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_C()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*81523;
                userService.RegisterToSystem(i * 81523, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 81523, i * 81523);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i * 81523).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(6* 81523).GetLeague(), LeagueName.C);
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 81523);
            }
        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_B()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*5021;
                userService.RegisterToSystem(i * 5021, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 5021, i * 5021);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i * 5021).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(8*5021).Points(), 8*5021);
            Assert.AreEqual(userService.GetUserById(8*5021).GetLeague(), LeagueName.B);
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 5021);
            }

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_D()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*22514;
                userService.RegisterToSystem(22514*i, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 22514, i * 22514);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i * 22514).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(3*22514).GetLeague(), LeagueName.D);
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 22514);
            }

        }
        [TestMethod()]
        public void DevideLeagueTest_good_check_league_E()
        {
            Init();
            string name = "";
            for (int i = 1; i < 11; i++)
            {
                name = "" + i*99856;
                userService.RegisterToSystem(i * 99856, "orelie", name, "123456789", 15000, "orelie@post.bgu.ac.il");
                userService.EditUserPoints(i * 99856, i * 99856);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    userService.GetUserById(i * 99856).IncGamesPlay();
                }
            }
            userService.DevideLeague();
            Assert.AreEqual(userService.GetUserById(99856).GetLeague(), LeagueName.E);
            for (int i = 1; i < 11; i++)
            {
                userService.DeleteUserById(i * 99856);
            }
        }*/
    }
}