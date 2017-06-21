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

        private void Init()
        {
            //LogControl lc = new LogControl();
            //SystemControl sc = new SystemControl(lc);
            //ReplayManager rm = new ReplayManager();
            //GameCenter gc = new GameCenter(sc, lc, rm, _webEventHandler);
            //_userService = new UserServiceHandler(gc, sc);
            //_serverEventHandler = new ServerEventHandler(null, null, gc, sc, lc, rm, CommunicationHandler.GetInstance());
            //_webEventHandler = new WebEventHandler(_serverEventHandler);
            //_serverEventHandler.SetSessionIdHandler(_webEventHandler);
        }

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
            _serverEventHandler = null;
            DeleteTheTwoUsers();
        }


        //print jsons to show Yarden
        [TestCase]
        public void TestJson()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var lineDatas = new List<LeaderboardLineData>
            {
                new LeaderboardLineData(1, "bla", 1000, 10, 20, 12),
                new LeaderboardLineData(1, "blabla", 100, 120, 2, 132),
                new LeaderboardLineData(1, "blablablabla", 10032, 12120, 23, 1432)
            };
            var res = new LeaderboardResponseCommMessage(lbcm.UserId, lbcm.SessionId, true, lbcm, lineDatas);
            string xml = _parser.SerializeMsg(res, false);
            Console.WriteLine(xml);
        }

        //private void RegisterTwoUsers()
        //{
        //    Init();
        //    Random rand = new Random();
        //    _userId1 = rand.Next();
        //    _userId2 = rand.Next();
        //    _userService.RegisterToSystem(_userId1, "Oded", "Oded", "123456789", 1000, "bla@bla.com");
        //    _userService.RegisterToSystem(_userId2, "Jordy", "Jordy", "123456789", 1000, "blabla@bla.com");
        //    _userService.EditUserPoints(1, 100);
        //    _userService.EditUserPoints(2, 1000);
        //    _userService.GetUserById(_userId1).WinNum = 11;
        //    _userService.GetUserById(_userId1).LoseNum = 1;
        //    _userService.GetUserById(_userId1).TotalProfit = 1000;
        //    _userService.GetUserById(_userId1).HighestCashGainInGame = 13;
        //    _userService.GetUserById(_userId2).WinNum = 10;
        //    _userService.GetUserById(_userId2).LoseNum = 1;
        //    _userService.GetUserById(_userId2).TotalProfit = 10;
        //    _userService.GetUserById(_userId2).HighestCashGainInGame = 130;
        //}

        private void DeleteTheTwoUsers()
        {
            if (_userId1 != -1)
            {
                _userService.DeleteUserById(_userId1);
            }
            if (_userId2 != -1)
            {
                _userService.DeleteUserById(_userId2);
            }
        }

        [TestCase]
        public void HandleRawMsgsLeaderBoardGood()
        {
            //RegisterTwoUsers();
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

            string expectedResponse =
                "r<?xmlversion=\"1.0\"encoding=\"utf-16\"?>" +
                "<LeaderboardResponseCommMessagexmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"" +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                "<UserId>1</UserId>" +
                "<SessionId>-1</SessionId>" +
                "<Success>true</Success>" +
                "<OriginalMsg xsi:type=\"LeaderboardCommMessage\">" +
                "<UserId>1</UserId>" +
                "<SessionId>-1</SessionId>" +
                "<SortedBy>HighestCashGain</SortedBy>" +
                "</OriginalMsg>" +
                "<Results>" +
                "<LeaderboardLineData>" +
                "<Id>1</Id>" +
                "<Name>bla</Name>" +
                "<Points>1000</Points>" +
                "<TotalGrossProfit>10</TotalGrossProfit>" +
                "<HighestCashGain>20</HighestCashGain>" +
                "<NumOfGamesPlayed>12</NumOfGamesPlayed>" +
                "</LeaderboardLineData>" +
                "<LeaderboardLineData>" +
                "<Id>1</Id>" +
                "<Name>blabla</Name>" +
                "<Points>100</Points>" +
                "<TotalGrossProfit>120</TotalGrossProfit>" +
                "<HighestCashGain>2</HighestCashGain>" +
                "<NumOfGamesPlayed>132</NumOfGamesPlayed>" +
                "</LeaderboardLineData>" +
                "<LeaderboardLineData>" +
                "<Id>1</Id>" +
                "<Name>blablablabla</Name>" +
                "<Points>10032</Points>" +
                "<TotalGrossProfit>12120</TotalGrossProfit>" +
                "<HighestCashGain>23</HighestCashGain>" +
                "<NumOfGamesPlayed>1432</NumOfGamesPlayed>" +
                "</LeaderboardLineData>" +
                "</Results>" +
                "</LeaderboardResponseCommMessage>";


            var xml = _parser.SerializeMsg(lbcm, false);
            var json = _parser.XmlToJson(xml);
            var result = _webEventHandler.HandleRawMsg(json);
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedResponse.Equals(result[0]));
        }

        [TestCase]
        public void ParseStatisticsGood()
        {
            //RegisterTwoUsers();

            var expectedResponse =
                "u{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
                "\"UserStatisticsResponseCommMessage\":{\"@xmlns:xsd\":" +
                "\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
                "\"UserId\":\"1\",\"SessionId\":\"-1\",\"Success\":\"true\",\"OriginalMsg\":{\"@xsi:type\":\"UserStatisticsCommMessage\"," +
                "\"UserId\":\"1\",\"SessionId\":\"-1\"},\"AvgCashGain\":\"83.333333333333329\",\"AvgGrossProfit\":\"90.9090909090909\"}}";

            UserStatisticsCommMessage commMessage = new UserStatisticsCommMessage(1, -1);
            var xml = _parser.SerializeMsg(commMessage, false);
            var json = _parser.XmlToJson(xml);
            var result = _webEventHandler.HandleRawMsg(json);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedResponse, result[0]);
        }



    }
}
