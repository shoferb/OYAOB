//using NUnit.Core;

using Client.Handler;
using Moq;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldemTests.communication
{
    [TestFixture]
    public class ServerEventHandlerTest
    {
        private Mock<ICommunicationHandler> _commHandlerMock;
        private ServerEventHandler _eventHandler;
        private ParserImplementation _parser = new ParserImplementation();

        [SetUp]
        public void Setup()
        {
            _commHandlerMock = new Mock<ICommunicationHandler>();
            _eventHandler = new ServerEventHandler();
            _eventHandler.SetCommHandler(_commHandlerMock.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _commHandlerMock = null;
            _eventHandler = null;
        }

    }
}
