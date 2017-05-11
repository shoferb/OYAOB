using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldemShared.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldemShared.Parser.ClientTests
{
    [TestClass()]
    public class parserImplementationTests
    {
        parserImplementation parser = new parserImplementation();
        [TestMethod()]
        public void SerializeAndDeserializeMsgTest()
        {
            LoginCommMessage msg = new LoginCommMessage(1,true,"bar","12345");
            string parsed = parser.SerializeMsg(msg);
            LoginCommMessage afterDe = (LoginCommMessage)parser.ParseString(parsed);
            Assert.AreEqual(msg.IsLogin,afterDe.IsLogin);
            Assert.AreEqual(msg.Password, afterDe.Password);
            Assert.AreEqual(msg.UserId, afterDe.UserId);
            Assert.AreEqual(msg.UserName, afterDe.UserName);
        }
    }
}