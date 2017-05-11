using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldemShared.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages;

namespace TexasHoldemShared.Parser.ClientTests
{
    [TestClass()]
    public class parserImplementationTests
    {
        parserImplementation parser = new parserImplementation();
      
        [TestMethod()]
        public void SerializeAndDeserializeMsgTest()
        {
            LoginCommMessage msg = new LoginCommMessage(1, false, "bar", "12345");
            string parsed = parser.SerializeMsg(msg);
            CommunicationMessage backToOrigin = parser.ParseString(parsed);
            LoginCommMessage p = (LoginCommMessage)backToOrigin;
            Assert.AreEqual(msg.IsLogin, p.IsLogin);
            Assert.AreEqual(msg.Password, p.Password);
            Assert.AreEqual(msg.UserId, p.UserId);
            Assert.AreEqual(msg.UserName, p.UserName);
        }
    }
}