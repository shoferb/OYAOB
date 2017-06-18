//using NUnit.Core;

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Castle.Components.DictionaryAdapter;
using Client.Handler;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Service;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldemTests.communication
{
    [TestFixture]
    public class WebEventHandlerTest
    {
        private ServerEventHandler _serverEventHandler;
        private UserServiceHandler _userService;
        private WebEventHandler _webEventHandler;
        private readonly ParserImplementation _parser = new ParserImplementation();
        private int _userId1 = -1;
        private int _userId2 = -1;

        private void Init()
        {
            LogControl lc = new LogControl();
            SystemControl sc = new SystemControl(lc);
            ReplayManager rm = new ReplayManager();
            GameCenter gc = new GameCenter(sc, lc, rm, _webEventHandler);
            _userService = new UserServiceHandler(gc, sc);
            _serverEventHandler = new ServerEventHandler(null, null, gc, sc, lc, rm, CommunicationHandler.GetInstance());
            _webEventHandler = new WebEventHandler(_serverEventHandler);
            _serverEventHandler.SetSessionIdHandler(_webEventHandler);
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
        //[TestCase]
        //public void TestJson()
        //{
        //    //LoginCommMessage login = new LoginCommMessage(1, true, "Oded", "12345689");
        //    //LoginResponeCommMessage response = new LoginResponeCommMessage(1, 123, "Oded", "Oded",
        //    //    "123456789", "Avatar", 123, "bla@bla.com", "League", false, login);

        //    //UserStatisticsCommMessage msg = new UserStatisticsCommMessage(1, 1);
        //    //UserStatisticsResponseCommMessage resp = new UserStatisticsResponseCommMessage(1, 1, true, msg, 2.2, 23.1);

        //    var msg = new SearchCommMessage(1, 794565, SearchCommMessage.SearchType.ByPotSize, "", 10, GameMode.Limit);
        //    var response = new SearchResponseCommMessage(new List<ClientGame>()
        //    {
        //        new ClientGame(true, false, GameMode.Limit, 1, 2, 13, 2, 1000, 2, "A", 200),
        //        new ClientGame(true, false, GameMode.NoLimit, 3, 2, 133, 2, 100, 22, "A", 200),
        //    }, 794565, 1, true, msg);

        //    var xml = _parser.SerializeMsg(response, false);
        //    //var json = _parser.XmlToJson(xml);
        //    Console.WriteLine(xml);
        //    //xml = _parser.SerializeMsg(resp, false);
        //    //json = _parser.XmlToJson(xml);
        //    //Console.WriteLine(json);
        //    //xml = _parser.JsonToXml(json);
        //    //var parsed = _parser.ParseString(xml, false);
        //    //Console.WriteLine(xml);
        //    //var respXml = _parser.SerializeMsg(response, false);
        //    //var parsed = _parser.ParseString("c{\"?xml\":{\"@version\":\"1.0\",\"" +
        //    //                                 "@encoding\":\"utf-16\"},\"LoginCommMessage\"" +
        //    //                                 ":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
        //    //                                 "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
        //    //                                 "\"UserId\":\"1\",\"SessionId\":\"-1\",\"IsLogin\":\"true\"," +
        //    //                                 "\"UserName\":\"Oded\",\"Password\":\"12345689\"}}", false);

        //    //Console.WriteLine(respXml);
        //}

        private void RegisterTwoUsers()
        {
            Init();
            Random rand = new Random();
            _userId1 = rand.Next();
            _userId2 = rand.Next();
            _userService.RegisterToSystem(_userId1, "Oded", "Oded", "123456789", 1000, "bla@bla.com");
            _userService.RegisterToSystem(_userId2, "Jordy", "Jordy", "123456789", 1000, "blabla@bla.com");
            _userService.EditUserPoints(1, 100);
            _userService.EditUserPoints(2, 1000);
            _userService.GetUserById(_userId1).WinNum = 11;
            _userService.GetUserById(_userId1).LoseNum = 1;
            _userService.GetUserById(_userId1).TotalProfit = 1000;
            _userService.GetUserById(_userId1).HighestCashGainInGame = 13;
            _userService.GetUserById(_userId2).WinNum = 10;
            _userService.GetUserById(_userId2).LoseNum = 1;
            _userService.GetUserById(_userId2).TotalProfit = 10;
            _userService.GetUserById(_userId2).HighestCashGainInGame = 130;
        }

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
        public void HandleEventLeaderBoardGood()
        {
            RegisterTwoUsers();

            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var result = _serverEventHandler.HandleEvent(lbcm);
            Console.WriteLine(result);
            Assert.NotNull(result);
            Assert.AreNotEqual("", result);
        }

        [TestCase]
        public void HandleRawMsgsLeaderBoardGood()
        {
            RegisterTwoUsers();

            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var xml = _parser.SerializeMsg(lbcm, false);
            var json = _parser.XmlToJson(xml);
            var result = _webEventHandler.HandleRawMsg(json);
            Console.WriteLine(result);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestCase]
        public void ParseStatisticsGood()
        {
            RegisterTwoUsers();

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
