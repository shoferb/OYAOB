using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Replay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Actions;

namespace TexasHoldem.Logic.Replay.Tests
{
    [TestClass()]
    public class GameReplayTests
    {
        private GameReplay testGR = new GameReplay(1, 1);
        private Actions.Action testAction = new CallAction(new Card(1), new Card(2), 1, Role.None, 10,
            new User.Player(), 1, 1);

        [TestMethod()]
        public void GameReplayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GameReplayTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RightGameTest()
        {
            Assert.IsTrue(testGR.RightGame(1, 1));
            Assert.IsFalse(testGR.RightGame(2, 1));
            Assert.IsFalse(testGR.RightGame(1, 2));
            Assert.IsFalse(testGR.RightGame(3, 3));
        }

        [TestMethod()]
        public void ReplayGameTest()
        {
            Assert.Fail();
        }
    }
}