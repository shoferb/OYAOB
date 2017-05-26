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
        private ServerEventHandler _eventHandler;
        private UserServiceHandler _userService;
        private readonly ParserImplementation _parser = new ParserImplementation();

        private void createUserService()
        {
            LogControl lc = new LogControl();
            SystemControl sc = new SystemControl(lc);
            ReplayManager rm = new ReplayManager();
            GameCenter gc = new GameCenter(sc, lc, rm);
            _userService = new UserServiceHandler(gc, sc);
            _eventHandler = new ServerEventHandler(null, gc, sc, lc, rm, CommunicationHandler.GetInstance());
        }

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
            _eventHandler = null;
        }


        //print jsons to show Yarden
        [TestCase]
        public void TestJson()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var xml = _parser.SerializeMsg(lbcm, false);
            var json = _parser.XmlToJson(xml);
            Console.WriteLine(json);
            //var lb = _parser.ParseString(_parser.JsonToXml(json), false);

            //List<LeaderboardLineData> data = new List<LeaderboardLineData>
            //{
            //    new LeaderboardLineData(1, "Oded", 100, 1000, 13, 12),
            //    new LeaderboardLineData(1, "Jordy", 1000, 10, 130, 11)
            //};
            //LeaderboardResponseCommMessage response = new LeaderboardResponseCommMessage(1,
            //    true, lbcm, data);
            //xml = _parser.SerializeMsg(response, false);
            //json = _parser.XmlToJson(xml);
            //Console.WriteLine(json);
        }

        [TestCase]
        public void ParseLeaderboarrdRespTestGood()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            List<LeaderboardLineData> data = new List<LeaderboardLineData>
            {
                new LeaderboardLineData(1, "Oded", 100, 1000, 13, 12),
                new LeaderboardLineData(1, "Jordy", 1000, 10, 130, 11)
            };
            LeaderboardResponseCommMessage response = new LeaderboardResponseCommMessage(1,
                true, lbcm, data);
            string msgStr =
                "r{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
                "\"LeaderboardResponseCommMessage\":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
                "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\":\"1\",\"Success\":\"true\"," +
                "\"OriginalMsg\":{\"@xsi:type\":\"LeaderboardCommMessage\",\"UserId\":\"1\",\"SortedBy\":\"HighestCashGain\"}," +
                "\"Results\":{\"LeaderboardLineData\":[{\"Id\":\"1\",\"Name\":\"Oded\",\"Points\":\"100\",\"TotalGrossProfit\":" +
                "\"1000\",\"HighestCashGain\":\"13\",\"NumOfGamesPlayed\":\"12\"},{\"Id\":\"1\",\"Name\":\"Jordy\",\"Points\":" +
                "\"1000\",\"TotalGrossProfit\":\"10\",\"HighestCashGain\":\"130\",\"NumOfGamesPlayed\":\"11\"}]}}}";
            var parsed = _parser.ParseString(_parser.JsonToXml(msgStr), false);
            Assert.AreEqual(1, parsed.Count);
            Assert.True(response.Equals(parsed[0]));
        }

        [TestCase]
        public void HandleRawMsgLeaderBoardGood()
        {
            _userService.RegisterToSystem(1, "Oded", "Oded", "123456789", 1000, "bla@bla.com");
            _userService.RegisterToSystem(2, "Jordy", "Jordy", "123456789", 1000, "blabla@bla.com");
            _userService.EditUserPoints(1, 100);
            _userService.EditUserPoints(2, 1000);
            _userService.GetUserById(1).WinNum = 11;
            _userService.GetUserById(1).LoseNum = 1;
            _userService.GetUserById(1).TotalProfit = 1000;
            _userService.GetUserById(1).HighestCashGainInGame = 13;
            _userService.GetUserById(2).WinNum = 10;
            _userService.GetUserById(2).LoseNum = 1;
            _userService.GetUserById(2).TotalProfit = 10;
            _userService.GetUserById(2).HighestCashGainInGame = 130;

            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            var result = _eventHandler.HandleEvent(lbcm);
            Console.WriteLine(result);
        }


    }
}
