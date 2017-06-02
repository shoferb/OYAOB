using System.IO;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared;
using TexasHoldemShared.Security;

namespace TexasHoldemTests.communication
{
    class WebCommHandlerTests
    {
        private IWebCommHandler _commHandler;
        private ISecurity _security = new SecurityHandler();
        private WebEventHandler _eventHandler;
        private Mock<IEventHandler> _serverEventHandlerMock;
        private string _url = "http://127.0.0.1:8080/";
        private HttpWebRequest _request;
        private const string Message = "ShortMsg";

        [SetUp]
        public void Setup()
        {
            _serverEventHandlerMock = new Mock<IEventHandler>();
            _eventHandler = new WebEventHandler(_serverEventHandlerMock.Object);
            _commHandler = new WebCommHandler(_eventHandler);
            Task.Factory.StartNew(() => _commHandler.Start());
            _request = null;
        }

        [TearDown]
        public void Teardown()
        {

            _commHandler.Close();

            _serverEventHandlerMock = null;
            _eventHandler = null;
            _commHandler = null;
        }

        private WebResponse SendWebMsg(string msg)
        {
            _request = WebRequest.CreateHttp(_url);
            _request.ContentType = "application/json";
            _request.Method = "POST";
            using (var streamWriter = new StreamWriter(_request.GetRequestStream()))
            {
                streamWriter.Write(msg);
                streamWriter.Flush();
                streamWriter.Close();
            }

            return _request.GetResponse();
        }

        [TestCase]
        public void AcceptTest()
        {
            var response = SendWebMsg(Message);
            Assert.IsNotNull(response);
        }

    }
}
