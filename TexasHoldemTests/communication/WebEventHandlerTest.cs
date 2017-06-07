////using NUnit.Core;

//using System;
//using System.Collections.Generic;
//using System.Web.Script.Serialization;
//using Client.Handler;
//using Moq;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using TexasHoldem.communication.Impl;
//using TexasHoldem.communication.Interfaces;
//using TexasHoldem.Logic.GameControl;
//using TexasHoldem.Logic.Game_Control;
//using TexasHoldem.Logic.Replay;
//using TexasHoldem.Service;
//using TexasHoldemShared.CommMessages;
//using TexasHoldemShared.CommMessages.ClientToServer;
//using TexasHoldemShared.CommMessages.ServerToClient;
//using TexasHoldemShared.Parser;

//namespace TexasHoldemTests.communication
//{
//    [TestFixture]
//    public class WebEventHandlerTest
//    {
//        private ServerEventHandler _serverEventHandler;
//        private UserServiceHandler _userService;
//        private WebEventHandler _webEventHandler;
//        private readonly ParserImplementation _parser = new ParserImplementation();

//        private void Init()
//        {
//            LogControl lc = new LogControl();
//            SystemControl sc = new SystemControl(lc);
//            ReplayManager rm = new ReplayManager();
//            GameCenter gc = new GameCenter(sc, lc, rm);
//            _userService = new UserServiceHandler(gc, sc);
//            _serverEventHandler = new ServerEventHandler(null, null, gc, sc, lc, rm, CommunicationHandler.GetInstance());
//            _webEventHandler = new WebEventHandler(_serverEventHandler);
//            _serverEventHandler.SetSessionIdHandler(_webEventHandler);
//        }

//        [SetUp]
//        public void Setup()
//        {
//        }

//        [TearDown]
//        public void Teardown()
//        {
//            _serverEventHandler = null;
//        }


//        //print jsons to show Yarden
//        [TestCase]
//        public void TestJson()
//        {
//            //LoginCommMessage login = new LoginCommMessage(1, true, "Oded", "12345689");
//            //LoginResponeCommMessage response = new LoginResponeCommMessage(1, 123, "Oded", "Oded",
//            //    "123456789", "Avatar", 123, "bla@bla.com", "League", false, login);

//            //UserStatisticsCommMessage msg = new UserStatisticsCommMessage(1, 1);
//            //UserStatisticsResponseCommMessage resp = new UserStatisticsResponseCommMessage(1, 1, true, msg, 2.2, 23.1);

//            LeaderboardCommMessage msg = new LeaderboardCommMessage(1, 1, LeaderboardCommMessage.SortingOption.HighestCashGain);

//            var xml = _parser.SerializeMsg(msg, false);
//            var json = _parser.XmlToJson(xml);
//            Console.WriteLine(json);
//            //xml = _parser.SerializeMsg(resp, false);
//            //json = _parser.XmlToJson(xml);
//            //Console.WriteLine(json);
//            //xml = _parser.JsonToXml(json);
//            //var parsed = _parser.ParseString(xml, false);
//            //Console.WriteLine(xml);
//            //var respXml = _parser.SerializeMsg(response, false);
//            //var parsed = _parser.ParseString("c{\"?xml\":{\"@version\":\"1.0\",\"" +
//            //                                 "@encoding\":\"utf-16\"},\"LoginCommMessage\"" +
//            //                                 ":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
//            //                                 "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
//            //                                 "\"UserId\":\"1\",\"SessionId\":\"-1\",\"IsLogin\":\"true\"," +
//            //                                 "\"UserName\":\"Oded\",\"Password\":\"12345689\"}}", false);

//            //Console.WriteLine(respXml);
//        }

//        [TestCase]
//        public void ParseLeaderboarrdRespTestGood()
//        {
//            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
//            List<LeaderboardLineData> data = new List<LeaderboardLineData>
//            {
//                new LeaderboardLineData(1, "Oded", 100, 1000, 13, 12),
//                new LeaderboardLineData(1, "Jordy", 1000, 10, 130, 11)
//            };
//            LeaderboardResponseCommMessage response = new LeaderboardResponseCommMessage(1, -1,
//                true, lbcm, data);
//            string msgStr =
//                "r{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
//                "\"LeaderboardResponseCommMessage\":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
//                "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\":\"1\",\"Success\":\"true\"," +
//                "\"OriginalMsg\":{\"@xsi:type\":\"LeaderboardCommMessage\",\"UserId\":\"1\",\"SortedBy\":\"HighestCashGain\"}," +
//                "\"Results\":{\"LeaderboardLineData\":[{\"Id\":\"1\",\"Name\":\"Oded\",\"Points\":\"100\",\"TotalGrossProfit\":" +
//                "\"1000\",\"HighestCashGain\":\"13\",\"NumOfGamesPlayed\":\"12\"},{\"Id\":\"1\",\"Name\":\"Jordy\",\"Points\":" +
//                "\"1000\",\"TotalGrossProfit\":\"10\",\"HighestCashGain\":\"130\",\"NumOfGamesPlayed\":\"11\"}]}}}";
//            var parsed = _parser.ParseString(_parser.JsonToXml(msgStr), false);
//            Assert.AreEqual(1, parsed.Count);
//            Assert.True(response.Equals(parsed[0]));
//        }

//        private void RegisterTwoUsers()
//        {
//            Init();
//            _userService.RegisterToSystem(1, "Oded", "Oded", "123456789", 1000, "bla@bla.com");
//            _userService.RegisterToSystem(2, "Jordy", "Jordy", "123456789", 1000, "blabla@bla.com");
//            _userService.EditUserPoints(1, 100);
//            _userService.EditUserPoints(2, 1000);
//            _userService.GetUserById(1).WinNum = 11;
//            _userService.GetUserById(1).LoseNum = 1;
//            _userService.GetUserById(1).TotalProfit = 1000;
//            _userService.GetUserById(1).HighestCashGainInGame = 13;
//            _userService.GetUserById(2).WinNum = 10;
//            _userService.GetUserById(2).LoseNum = 1;
//            _userService.GetUserById(2).TotalProfit = 10;
//            _userService.GetUserById(2).HighestCashGainInGame = 130;
//        }

//        [TestCase]
//        public void HandleEventLeaderBoardGood()
//        {
//            RegisterTwoUsers();

//            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
//            var result = _serverEventHandler.HandleEvent(lbcm);
//            Console.WriteLine(result);
//            Assert.NotNull(result);
//            Assert.AreNotEqual("", result);
//        }

//        [TestCase]
//        public void HandleRawMsgsLeaderBoardGood()
//        {
//            RegisterTwoUsers();

//            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
//            var xml = _parser.SerializeMsg(lbcm, false);
//            var json = _parser.XmlToJson(xml);
//            var result = _webEventHandler.HandleRawMsg(json);
//            Console.WriteLine(result);

//            Assert.NotNull(result);
//            Assert.AreEqual(1, result.Count);
//        }

//        [TestCase]
//        public void ParseStatisticsGood()
//        {
//            RegisterTwoUsers();

//            var expectedMessage =
//                "t{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
//                "\"UserStatisticsCommMessage\":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
//                "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\":\"1\",\"SessionId\":\"-1\"}}";

//            var expectedResponse =
//                "u{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
//                "\"UserStatisticsResponseCommMessage\":{\"@xmlns:xsd\":" +
//                "\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
//                "\"UserId\":\"1\",\"SessionId\":\"-1\",\"Success\":\"true\",\"OriginalMsg\":{\"@xsi:type\":\"UserStatisticsCommMessage\"," +
//                "\"UserId\":\"1\",\"SessionId\":\"-1\"},\"AvgCashGain\":\"83.333333333333329\",\"AvgGrossProfit\":\"90.9090909090909\"}}";

//            UserStatisticsCommMessage commMessage = new UserStatisticsCommMessage(1, -1);
//            var xml = _parser.SerializeMsg(commMessage, false);
//            var json = _parser.XmlToJson(xml);
//            Assert.AreEqual(expectedMessage, json);
//            var result = _webEventHandler.HandleRawMsg(json);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual(expectedResponse, result[0]);
//        }

//        [TestCase]
//        public void RegisterSidGood()
//        {
//            Init();

//            RegisterCommMessage registerMsg = new RegisterCommMessage(1, "Oded", "Oded", "123456789", 1000, "some@mail.com");
//            var responseStr = _serverEventHandler.HandleEvent(registerMsg);
//            var respLst = _parser.ParseString(responseStr, false);
//            Assert.AreEqual(1, respLst.Count);
//            Assert.AreNotEqual(-1, respLst[0].SessionId);
//        }




//    }
//}
