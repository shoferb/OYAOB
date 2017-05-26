//using NUnit.Core;

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Client.Handler;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
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


        //print jsons o show Yarden
        [TestCase]
        public void TestJson()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var jsonStr = JsonConvert.SerializeObject(lbcm);
            var xml = _parser.SerializeMsg(lbcm, false);
            var json = _parser.XmlToJson(xml);
            //Console.WriteLine(json);
            var lb = _parser.ParseString(_parser.JsonToXml(json), false);

            List<LeaderboardLineData> data = new List<LeaderboardLineData>
            {
                new LeaderboardLineData(1, "Oded", 100, 1000, 13, 12),
                new LeaderboardLineData(1, "Jordy", 1000, 10, 130, 11)
            };
            LeaderboardResponseCommMessage response = new LeaderboardResponseCommMessage(1,
                true, lbcm, data);
            xml = _parser.SerializeMsg(response, false);
            json = _parser.XmlToJson(xml);
            Console.WriteLine(json);
        }
    }
}
