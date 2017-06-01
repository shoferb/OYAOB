using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database;
using TexasHoldem.Database.DatabaseObject;

namespace TexasHoldemTests
{
    [TestClass()]
    public class UserDBTests
    {
        [TestMethod()]
        public void GetAllUserTest()
        {
            UserDB udb = new UserDB();
            List<userDatabaseOb> temp = udb.GetAllUser();


        }
    }
}
