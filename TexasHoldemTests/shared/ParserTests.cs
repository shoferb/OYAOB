using System.Collections.Generic;
using NUnit.Framework;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldemTests.shared
{
    [TestFixture()]
    class ParserTests
    {
        private readonly ParserImplementation _parser = new ParserImplementation();

        [TestCase]
        public void ParseLeaderboarrdRespTestGood()
        {
            LeaderboardCommMessage lbcm = new LeaderboardCommMessage(1, -1, LeaderboardCommMessage.SortingOption.HighestCashGain);
            List<LeaderboardLineData> data = new List<LeaderboardLineData>
            {
                new LeaderboardLineData(1, "Oded", 100, 1000, 13, 12),
                new LeaderboardLineData(1, "Jordy", 1000, 10, 130, 11)
            };
            LeaderboardResponseCommMessage response = new LeaderboardResponseCommMessage(1, -1,
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
        public void ParseStatisticsGood()
        {
            var expectedMessage =
                "t{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
                "\"UserStatisticsCommMessage\":{\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\"," +
                "\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\",\"UserId\":\"1\",\"SessionId\":\"-1\"}}";

            //var expectedResponse =
            //    "u{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"}," +
            //    "\"UserStatisticsResponseCommMessage\":{\"@xmlns:xsd\":" +
            //    "\"http://www.w3.org/2001/XMLSchema\",\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\"," +
            //    "\"UserId\":\"1\",\"SessionId\":\"-1\",\"Success\":\"true\",\"OriginalMsg\":{\"@xsi:type\":\"UserStatisticsCommMessage\"," +
            //    "\"UserId\":\"1\",\"SessionId\":\"-1\"},\"AvgCashGain\":\"83.333333333333329\",\"AvgGrossProfit\":\"90.9090909090909\"}}";

            UserStatisticsCommMessage commMessage = new UserStatisticsCommMessage(1, -1);
            var xml = _parser.SerializeMsg(commMessage, false);
            var json = _parser.XmlToJson(xml);
            Assert.AreEqual(expectedMessage, json);
        }
    }
}
