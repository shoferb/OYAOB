using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.GuiScreen;
using Client.Handler;

namespace Client.Logic.Tests
{
    [TestClass()]
    public class ClientLogicClientLogicTests
    {
        public ClientLogic cl;
        ClientCommunicationHandler _commHandler;
        ClientEventHandler _eventHandler;

        [TestInitialize()]
        public void Initialize()
        {
            cl = new ClientLogic();
            _commHandler = new ClientCommunicationHandler("localhost");
            _eventHandler = new ClientEventHandler(_commHandler);
            _eventHandler.Init(cl);
            cl.Init(_eventHandler, _commHandler);
        }

        [TestMethod()]
        public void AddNewRoomTest()
        {
           
            int numOfGames =cl._games.Capacity;
            GameScreen gs = new GameScreen(cl);
            cl.AddNewRoom(gs);
            Assert.IsTrue(cl._games.Capacity == (numOfGames+1));
        }

        [TestMethod()]
        public void GameUpdateReceivedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SpectateRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CloseSystemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditDetailsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void JoinTheGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReturnGamePlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReturnGameSpecTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateNewRoomTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeaveTheGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SpectetorLeaveTheGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendChatMsgTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartTheGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoginTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AskForReplaysTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogoutTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GotMsgTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotifyChosenMoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayerReturnsToGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SpecReturnsToGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotifyResponseReceivedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void JoinAsPlayerReceivedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeaveAsPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeaveAsSpectetorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void JoinAsSpectatorReceivedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchResultRecivedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetReturnToGameScreenTest()
        {
            Assert.Fail();
        }
    }
}