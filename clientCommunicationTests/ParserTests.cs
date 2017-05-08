using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldemShared.Parser;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages;

namespace clientCommunicationTests
{
    [TestClass]
    public class ParserTests
    {
        public ParserImplementation parser;
        private void SetUp()
        {
            parser = new ParserImplementation();
        }

        [TestMethod]
        public void ParserActionCommMessage()
        {
            SetUp();
            ActionCommMessage toParse = new ActionCommMessage(1, CommunicationMessage.ActionType.Fold, -1, 1);
            string afterParse = parser.SerializeMsg(toParse);
            Assert.AreEqual(parser.ParseString(afterParse), toParse);


        }
    }
}
