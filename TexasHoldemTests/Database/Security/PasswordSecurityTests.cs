using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;

namespace TexasHoldem.Database.Security.Tests
{
    [TestClass()]
    public class PasswordSecurityTests
    {
        private readonly UserDataControler _userDataControler = new UserDataControler();

        [TestMethod()]
        public void DecryptTest_user_From_DB_getBYId()
        {
            UserTable toAdd1 = CreateUser(5565852, "5565852");
            toAdd1.password = "DecryptTest";
            _userDataControler.AddNewUser(toAdd1);
            Assert.AreEqual(_userDataControler.GetUserById(5565852).password, "DecryptTest");
            _userDataControler.DeleteUserById(5565852);
        }

      

        [TestMethod()]
        public void DecryptTest_user_From_DB_getBYUserName()
        {
            UserTable toAdd1 = CreateUser(5565854, "5565854");
            toAdd1.password = "DecryptTest";
            _userDataControler.AddNewUser(toAdd1);
            Assert.AreEqual(_userDataControler.GetUserByUserName("5565854").password, "DecryptTest");
            _userDataControler.DeleteUserById(5565854);
        }


        [TestMethod()]
        public void DecryptTest_good()
        {
          
            
            var newPassword = "DecryptTest";
            var encryptedpassword = PasswordSecurity.Encrypt("securityPassword", newPassword, false);
            var decryptedPassword = PasswordSecurity.Decrypt("securityPassword", encryptedpassword, false);
           
            Assert.AreEqual(newPassword, decryptedPassword);
        }

        [TestMethod()]
        public void DecryptTest_good_enc_and_dec_dont_match()
        {


            var newPassword = "DecryptTest";
            var encryptedpassword = PasswordSecurity.Encrypt("securityPassword", newPassword, false);
            var decryptedPassword = PasswordSecurity.Decrypt("securityPassword", encryptedpassword, false);

            Assert.AreNotEqual(encryptedpassword, decryptedPassword);
        }
        [TestMethod()]

        public void EncryptTest()
        {
            var newPassword = "EncryptTest";
            var encryptedpassword = PasswordSecurity.Encrypt("securityPassword", newPassword, false);

            Assert.AreNotEqual(newPassword, encryptedpassword);
        }

        private UserTable CreateUser(int userId, string username)
        {
            UserTable ut = new UserTable
            {
                userId = userId,
                HighestCashGainInGame = 0,
                TotalProfit = 0,
                avatar = "/GuiScreen/Photos/Avatar/devil.png",
                email = "orelie@post.bgu.ac.il",
                gamesPlayed = 0,
                inActive = true,
                leagueName = 1,
                money = 0,
                name = "orelie",
                username = username,
                password = "123456789"
            };
            ut.HighestCashGainInGame = 0;
            return ut;
        }

        [TestMethod()]
        public void DecryptTest_user_From_DB_getByname()
        {
            UserTable toAdd1 = CreateUser(5565853, "5565853");
            toAdd1.password = "DecryptTest";
            _userDataControler.AddNewUser(toAdd1);
            Assert.AreEqual(_userDataControler.GetUserByUserName("5565853").password, "DecryptTest");
            _userDataControler.DeleteUserByUsername("5565853");
        }
    }
}