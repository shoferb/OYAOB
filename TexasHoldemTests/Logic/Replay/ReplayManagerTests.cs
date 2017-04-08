using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Replay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Replay.Tests
{
    [TestClass()]
    public class ReplayManagerTests
    {
        private GameReplay _testGR = new GameReplay(1, 1);
        private Actions.Action _testAction = new CallAction(new Card(1), new Card(2), 1, Role.None, 10,
            new Player(1, "test", "mem", 123, 10, 100, "email@gmail.com", 1, true), 1, 1);
        private ReplayManager _testRM = new ReplayManager();

        [TestInitialize()]
        public void Initialize()
        {
            _testGR.AddAction(_testAction);
            _testRM.AddGameReplay(_testGR);
        }

        [TestMethod()]
        public void AddGameReplayTest()
        {
            GameReplay gr1 = new GameReplay(1, 1);
            Assert.IsFalse(_testRM.AddGameReplay(gr1)); //same room&game
            gr1._gameNumber = 2;
            Assert.IsTrue(_testRM.AddGameReplay(gr1)); //diffrent game same room
            GameReplay gr2 = new GameReplay(2, 1);
            Assert.IsTrue(_testRM.AddGameReplay(gr2)); //diffrent room same game number
        }

        [TestMethod()]
        public void IsExistTest()
        {
            GameReplay gr1 = new GameReplay(1, 1);
            Assert.IsTrue(_testRM.IsExist(gr1));
            gr1._gameNumber = 2;
            Assert.IsFalse(_testRM.IsExist(gr1));
            gr1._gameNumber = 1;
            gr1._gameRoomID = 2;
            Assert.IsFalse(_testRM.IsExist(gr1));
        }

        [TestMethod()]
        public void ReplayGameTest()
        {
            Assert.IsNotNull(_testRM.GetGameReplay(1,1));
            Assert.IsNull(_testRM.GetGameReplay(3, 3));
            Assert.IsNull(_testRM.GetGameReplay(1, 3));
            Assert.IsNull(_testRM.GetGameReplay(3, 1));
        }
    }
}