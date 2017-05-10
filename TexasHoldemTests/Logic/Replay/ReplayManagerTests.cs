using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Replay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Users;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic.Replay.Tests
{
    [TestClass()]
    public class ReplayManagerTests
    {
        private GameReplay _testGR;
        private Actions.Action _testAction;
        private ReplayManager _testRM;

        [TestInitialize()]
        public void Initialize()
        {
            User user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            List<int> ids = new List<int>();
            ids.Add(1);
            _testRM = ReplayManager.ReplayManagerInstance;
            _testGR = new GameReplay(1, 1);
            _testAction = new CallAction(new Player(user1, 1000, 1 ),
                new Card(Suits.Hearts, 1), new Card(Suits.Hearts, 2), 10);
            _testGR.AddAction(_testAction);
            _testRM.AddGameReplay(_testGR, ids);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _testRM.DeleteGameReplay(1, 1);
            _testRM.DeleteGameReplay(2, 1);
            _testRM.DeleteGameReplay(1, 2);

        }

        [TestMethod()]
        public void AddGameReplayTest()
        {
            List<int> ids = new List<int>();
            ids.Add(1);
            List<int> ids2 = new List<int>();
            ids.Add(2);
            GameReplay gr1 = new GameReplay(1, 1);
            Assert.IsFalse(_testRM.AddGameReplay(gr1, ids)); //same room&game
            Assert.IsFalse(_testRM.AddGameReplay(gr1, ids2)); //same room&game
            GameReplay gr2 = new GameReplay(1, 2);
            Assert.IsTrue(_testRM.AddGameReplay(gr2, ids)); //diffrent game same room
            GameReplay gr3 = new GameReplay(2, 1);
            Assert.IsTrue(_testRM.AddGameReplay(gr3, ids)); //diffrent room same game number
        }

        [TestMethod()]
        public void IsExistTest()
        {
            GameReplay gr1 = new GameReplay(1, 1);
            Assert.IsTrue(_testRM.IsExist(gr1._gameRoomID, gr1._gameNumber));
            gr1._gameNumber = 2;
            Assert.IsFalse(_testRM.IsExist(gr1._gameRoomID, gr1._gameNumber));
            gr1._gameNumber = 1;
            gr1._gameRoomID = 2;
            Assert.IsFalse(_testRM.IsExist(gr1._gameRoomID, gr1._gameNumber));
        }

        [TestMethod()]
        public void GetGameReplayForUserTest()
        {
            Assert.IsNotNull(_testRM.GetGameReplayForUser(1, 1, 1));
            Assert.IsNull(_testRM.GetGameReplayForUser(3, 3, 1));
            Assert.IsNull(_testRM.GetGameReplayForUser(1, 3, 1));
            Assert.IsNull(_testRM.GetGameReplayForUser(3, 1, 1));
        }

        [TestMethod()]
        public void ShowGameReplayTest()
        {
            Assert.IsTrue(_testRM.ShowGameReplay(1, 1, 1) != "");
            Assert.IsTrue(_testRM.ShowGameReplay(3, 3, 1) == "");
            Assert.IsTrue(_testRM.ShowGameReplay(1, 3, 1) == "");
            Assert.IsTrue(_testRM.ShowGameReplay(3, 1, 1) == "");
        }

        [TestMethod()]
        public void DeleteGameReplayTest()
        {
            Assert.IsNotNull(_testRM.GetGameReplayForUser(1, 1, 1));
            _testRM.DeleteGameReplay(1, 1);
            Assert.IsNull(_testRM.GetGameReplayForUser(1, 1, 1));
            _testGR = new GameReplay(1, 1);
            _testGR.AddAction(_testAction);
            List<int> ids = new List<int>();
            ids.Add(1);
            Assert.IsTrue(_testRM.AddGameReplay(_testGR, ids));
        }

    }
}