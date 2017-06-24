using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldemTests.communication
{
    class WebCommHandlerTests
    {
        private IWebCommHandler _commHandler;
        private WebEventHandler _eventHandler;
        private Mock<IEventHandler> _serverEventHandlerMock;
        private string _url = "http://127.0.0.1:8080/";
        private HttpWebRequest _request;
        private ICommMsgXmlParser _parser = new ParserImplementation();

        private const string LoginJsonMessage = "c{\"?xml\":{\"@version\":\"1.0\",\"@encoding\"" +
                                                ":\"utf-16\"},\"LoginCommMessage\":{\"@xmlns:xsd\"" +
                                                ":\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\"" +
                                                ":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\"" +
                                                ":\"1\",\"SessionId\":\"-1\",\"IsLogin\":\"true\",\"UserName\"" +
                                                ":\"Oded\",\"Password\":\"12345689\"}}";
        private const string LoginRespXml = "i<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<LoginResponeCommMessage xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +"<UserId>1</UserId>" +
                  "<SessionId>123</SessionId>" +
                  "<Success>false</Success>" +
                  "<OriginalMsg xsi:type=\"LoginCommMessage\">" +
                    "<UserId>1</UserId>" +
                    "<SessionId>-1</SessionId>" +
                    "<IsLogin>true</IsLogin>" +
                    "<UserName>Oded</UserName>" +
                    "<Password>12345689</Password>" +
                  "</OriginalMsg>" +
                  "<Name>Oded</Name>" +
                  "<Username>Oded</Username>" +
                  "<Password>123456789</Password>" +
                  "<Avatar>Avatar</Avatar>" +
                  "<Money>123</Money>" +
                  "<Email>bla@bla.com</Email>" +
                  "<Leauge>League</Leauge>" +
                "</LoginResponeCommMessage>";

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
            _request.Timeout = Int32.MaxValue;
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
            var respToRet = _parser.ParseString(LoginRespXml, false);
            _serverEventHandlerMock.Setup(m => 
                m.HandleEvent(It.IsAny<LoginCommMessage>())).Returns((ResponeCommMessage) respToRet[0]);
            var response = SendWebMsg(LoginJsonMessage);
            Assert.IsNotNull(response);
            string data = "";
            using (var respStreamReader = new StreamReader(response.GetResponseStream()))
            {
                data = respStreamReader.ReadToEnd();
            }
            Assert.False(String.IsNullOrEmpty(data));
            var respXml = _parser.JsonToXml('i' + data);
            Assert.True(respXml.Equals(LoginRespXml));
        }

    }
}
