//using NUnit.Core;

using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Service;
using TexasHoldem.Service.interfaces;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldemTests.communication
{
    [TestFixture]
    public class WebEventHandlerTest
    {
        private Mock<IEventHandler> _serverEventHandler;
        private IUserService _userService;
        private WebEventHandler _webEventHandler;
        private readonly ParserImplementation _parser = new ParserImplementation();
        private int _userId1 = -1;
        private int _userId2 = -1;

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
            _serverEventHandler = null;
            _webEventHandler = null;
        }


        //print jsons to show Yarden
        //[TestCase]
        //public void TestJson()
        //{
        //    LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
        //    var lineDatas = new List<LeaderboardLineData>
        //    {
        //        new LeaderboardLineData(1, "bla", 1000, 10, 20, 12),
        //        new LeaderboardLineData(1, "blabla", 100, 120, 2, 132),
        //        new LeaderboardLineData(1, "blablablabla", 10032, 12120, 23, 1432)
        //    };

        //    var res = new LeaderboardResponseCommMessage(lbcm.UserId, lbcm.SessionId, true, lbcm, lineDatas);
        //    string xml = _parser.SerializeMsg(res, false);
        //    string json = _parser.XmlToJson(xml);
        //    Console.WriteLine(json);
        //}

        [TestCase]
        public void HandleRawMsgsLeaderBoardGood()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var lineDatas = new List<LeaderboardLineData>
            {
                new LeaderboardLineData(1, "bla", 1000, 10, 20, 12),
                new LeaderboardLineData(1, "blabla", 100, 120, 2, 132),
                new LeaderboardLineData(1, "blablablabla", 10032, 12120, 23, 1432)
            };
            _serverEventHandler = new Mock<IEventHandler>();
            _serverEventHandler.Setup(e => e.HandleEvent(It.IsAny<LeaderboardCommMessage>()))
                .Returns(new LeaderboardResponseCommMessage(lbcm.UserId, lbcm.SessionId, true, lbcm, lineDatas));

            _webEventHandler = new WebEventHandler(_serverEventHandler.Object);

            string expectedResponse = "r{\"?xml\":{\"@version\":\"1.0\""+
                                ",\"@encoding\":\"utf-16\"},\""+
                                "LeaderboardResponseCommMessage\":{\"@xmlns:xsd\""+
                                ":\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\""+
                                ":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\""+
                                ":\"1\",\"SessionId\":\"-1\",\"Success\":\"true\","+
                                "\"OriginalMsg\":{\"@xsi:type\":\"LeaderboardCommMessage\""+
                                ",\"UserId\":\"1\",\"SessionId\":\"-1\",\"SortedBy\":"+
                                "\"HighestCashGain\"},\"Results\":{\"LeaderboardLineData\""+
                                ":[{\"Id\":\"1\",\"Name\":\"bla\",\"Points\":\"1000\",\""+
                                "TotalGrossProfit\":\"10\",\"HighestCashGain\":\"20\","+
                                "\"NumOfGamesPlayed\":\"12\"},{\"Id\":\"1\",\"Name\":"+
                                "\"blabla\",\"Points\":\"100\",\"TotalGrossProfit\":\"120\""+
                                ",\"HighestCashGain\":\"2\",\"NumOfGamesPlayed\":\"132\"},{"+
                                "\"Id\":\"1\",\"Name\":\"blablablabla\",\"Points\":\"10032\""+
                                ",\"TotalGrossProfit\":\"12120\",\"HighestCashGain\":\"23\""+
                                ",\"NumOfGamesPlayed\":\"1432\"}]}}}";

            var xml = _parser.SerializeMsg(lbcm, false);
            var json = _parser.XmlToJson(xml);
            var result = _webEventHandler.HandleRawMsg(json);
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedResponse.Equals(result[0]));
        }

        [TestCase]
        public void HandleRawMsgsStatisticsGood()
        {
            UserStatisticsCommMessage commMessage = new UserStatisticsCommMessage(1, -1);
            _serverEventHandler = new Mock<IEventHandler>();
            _serverEventHandler.Setup(e => e.HandleEvent(It.IsAny<UserStatisticsCommMessage>()))
                .Returns(new UserStatisticsResponseCommMessage(commMessage.UserId, commMessage.SessionId,
                true, commMessage, 83.333333333333329, 90.9090909090909));

            _webEventHandler = new WebEventHandler(_serverEventHandler.Object);

            var expectedResponse =
                "u{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
                "\"UserStatisticsResponseCommMessage\":{\"@xmlns:xsd\":" +
                "\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
                "\"UserId\":\"1\",\"SessionId\":\"-1\",\"Success\":\"true\",\"OriginalMsg\":{\"@xsi:type\":\"UserStatisticsCommMessage\"," +
                "\"UserId\":\"1\",\"SessionId\":\"-1\"},\"AvgCashGain\":\"83.333333333333329\",\"AvgGrossProfit\":\"90.9090909090909\"}}";

            var xml = _parser.SerializeMsg(commMessage, false);
            var json = _parser.XmlToJson(xml);
            var result = _webEventHandler.HandleRawMsg(json);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedResponse, result[0]);
        }



    }
}
