using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.Users.Tests
{
    [TestClass()]
    public class UserTests
    {

        private IUser orelie = new User(305077901, "orelie", "orelie26", "123456", 0, 500, "orelie@post.bgu.ac.il");
        private Notification toSend1 = new Notification(11, "joind");
        private Notification toSend2 = new Notification(11, "Exit");

        [TestMethod()]
        public void UserIdTest()
        {
            Assert.AreEqual(orelie.Id(), 305077901);
            Assert.AreNotEqual(orelie.Id(), 305077902);
        }

        [TestMethod()]
        public void UserNameTest()
        {
            Assert.AreEqual(orelie.Name(), "orelie");
            Assert.AreNotEqual(orelie.Name(), "odsdf");
        }
        [TestMethod()]
        public void UserUserNameTest()
        {
            Assert.AreEqual(orelie.MemberName(), "orelie26");
            Assert.AreNotEqual(orelie.MemberName(), "odsdf");
        }
        [TestMethod()]
        public void UserPasswordTest()
        {
            Assert.AreEqual(orelie.Password(), "123456");
            Assert.AreNotEqual(orelie.Password(), "slafkjasp");
            Assert.AreNotEqual(orelie.Password(), "654321");
        }
        [TestMethod()]
        public void UserPointTest()
        {
            Assert.AreEqual(orelie.Points(), 0);
            Assert.AreNotEqual(orelie.Points(), 5);
        }
        [TestMethod()]
        public void UserMoneyTest()
        {
            Assert.AreEqual(orelie.Money(), 500);
            Assert.AreNotEqual(orelie.Money(), -500);
        }
        [TestMethod()]
        public void UserEmailTest()
        {
            Assert.AreEqual(orelie.Email(), "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(orelie.Email(), "slafkjasp");
            Assert.AreNotEqual(orelie.Email(), "orelie@gmail.com");
        }


        [TestMethod()]
        public void SendNotificationTest()
        {
            Assert.IsTrue(orelie.SendNotification(toSend1));


        }

        [TestMethod()]
        public void AddNotificationToListTest()
        {
            Assert.IsTrue(orelie.AddNotificationToList(toSend1));

        }


      
        [TestMethod()]
        public void UserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsUnKnowTestGood_on_Create()
        {
            IUser user =  new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.IsTrue(user.IsUnKnow());
        }


        [TestMethod()]
        public void IsUnKnowTestGood_on_at_10_Games()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            for (int i = 0; i < 10; i++)
            {
                user.IncGamesPlay();
            }
            Assert.IsTrue(user.IsUnKnow());
        }


        [TestMethod()]
        public void IsUnKnowTestGood_More_Than_10()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            for (int i = 0; i < 11; i++)
            {
                user.IncGamesPlay();
            }
            Assert.IsFalse(user.IsUnKnow());
        }

       /* [TestMethod()]
        public void IncGamesPlayTest()
        {
            Assert.Fail();
        }*/

      

        [TestMethod()]
        public void IdTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Id(), 305077901);
        }

        [TestMethod()]
        public void IdTest_bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Id(), 305077902);
        }

        [TestMethod()]
        public void NameTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Name(), "orelie");
        }


        [TestMethod()]
        public void NameTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Name(), " ");
        }

        [TestMethod()]
        public void MemberNameTest_good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.MemberName(), "orelie26");
        }


        [TestMethod()]
        public void MemberNameTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.MemberName(), "orelie18");
        }


        [TestMethod()]
        public void PasswordTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Password(), "123456789");
        }


        [TestMethod()]
        public void PasswordTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Password(), "12");
        }


        [TestMethod()]
        public void AvatarTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Avatar(), "/GuiScreen/Photos/Avatar/devil.png");
            
        }


        [TestMethod()]
        public void AvatarTest_Bad()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreNotEqual(user.Avatar(), " ");

        }

        [TestMethod()]
        public void PointsTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Points(), 0);
        }

        [TestMethod()]
        public void MoneyTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Money(), 500); 
        }

        [TestMethod()]
        public void WaitListNotificationTest_Good_inc_size_onCreate()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Notification toSend1 = new Notification(11, "joind");
            Assert.AreEqual(user.WaitListNotification().Count, 0);
         
        }

        [TestMethod()]
        public void WaitListNotificationTest_Good_Added_notifiction()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Notification toSend1 = new Notification(11, "joind");
            user.SendNotification(toSend1);
            Assert.IsTrue(user.WaitListNotification().Contains(toSend1));

        }

        [TestMethod()]
        public void EmailTest_Good()
        {
            IUser user = new User(305077901, "orelie", "orelie26", "123456789", 0, 500, "orelie@post.bgu.ac.il");
            Assert.AreEqual(user.Email(), "orelie@post.bgu.ac.il");
        }

        [TestMethod()]
        public void GamesAvailableToReplayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ActiveGameListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SpectateGameListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WinNumTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IncWinNumTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoginTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogoutTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditIdTest()
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
        public void EditAvatarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReduceMoneyIfPossibleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddMoneyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditUserMoneyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveRoomFromActiveGameListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveRoomFromSpectetorGameListTest()
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
        public void AddRoomToActiveGameListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddRoomToSpectetorGameListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsLoginTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsValidPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetLeagueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetLeagueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HasEnoughMoneyTest()
        {
            Assert.Fail();
        }
    }
}