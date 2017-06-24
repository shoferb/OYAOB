using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.communication.Impl;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemTests.Database.DataControlers;
using ErrorLog = TexasHoldem.Logic.Notifications_And_Logs.ErrorLog;

namespace TexasHoldemTests.DatabaseProxy
{
    [TestClass()]
    public class LogDataProxyTests
    {
        private static LogControl logControl = new LogControl();

        private static SystemControl sysControl = new SystemControl(logControl);
        private static ReplayManager replayManager = new ReplayManager();
        private static SessionIdHandler ses = new SessionIdHandler();
        private static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager, ses);
        private static UserDataProxy _userDataProxy = new UserDataProxy();
        private bool useCommunication;
        private GameDataProxy proxy = new GameDataProxy(gameCenter);
        private readonly LogDataProxy _logDataProxy = new LogDataProxy();
        private readonly LogsOnlyForTest _logsOnlyForTest = new LogsOnlyForTest();

        

        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0, 15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private GameRoom CreateRoomWithId(int gameNum, int roomId, int userId1)
        {

            RegisterUser(userId1);
            useCommunication = false;

            List<Player> toAddPlayers = new List<Player>();
            IUser user = _userDataProxy.GetUserById(userId1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            Player player1 = new Player(user, 1000, roomId);
            toAddPlayers.Add(player1);
            GameRoom gm = new GameRoom(toAddPlayers, roomId, deco, gameCenter, logControl, replayManager, ses);
            gm.GameNumber = gameNum;
            return gm;
        }
        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            return before;
        }


        public void Cleanup(int gameNum, int roomId, int userId1)
        {
            _userDataProxy.DeleteUserById(userId1);

            replayManager.DeleteGameReplay(roomId, 0);
            replayManager.DeleteGameReplay(roomId, 1);
            proxy.DeleteGameRoomPref(roomId);
            proxy.DeleteGameRoom(roomId, gameNum);
        }


        [TestMethod()]
        public void AddErrorLogTest_good_message_match()
        {
            var error = new ErrorLog("AddErrorLogTest_good_message_match");
            _logDataProxy.AddErrorLog(error);
            var logId = error.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(logId).msg, "AddErrorLogTest_good_message_match");
            _logsOnlyForTest.DeleteErrorLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
        }

        

        [TestMethod()]
        public void AddSysLogTest_good_id_match()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAdd = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAdd);
            var systemLog = new SystemLog(roomid, "AddSysLogTest_good_id_match", gameNum);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).logId, logId);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum,roomid,userId);

        }

        [TestMethod()]
        public void AddSysLogTest_good_message_match()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAdd = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAdd);
           
            var systemLog = new SystemLog(roomid, "AddSysLogTest_good_message_match", gameNum);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).msg, "AddSysLogTest_good_message_match");
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum,roomid,userId);
        }

        [TestMethod()]
        public void AddSysLogTest_good_roomId_match()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            GameRoom toAdd = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAdd);
            var systemLog = new SystemLog(roomid, "AddSysLogTest_good_message_match", gameNum);
            _logDataProxy.AddSysLog(systemLog);
            var logId = systemLog.LogId;
            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).roomId, roomid);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum,roomid,userId);
        }
        
        
       //todo - run after all test
       
        [TestMethod()]
        public void GetNextLogIdTest_good()
        {
            var toAdd1 = new ErrorLog("GetNextLogIdTest_good first");
            var toAdd1Id = toAdd1.LogId;
            _logDataProxy.AddErrorLog(toAdd1);
            var next = _logDataProxy.GetNextLogId();
            Assert.AreEqual(next, toAdd1Id + 1);
            _logsOnlyForTest.DeleteErrorLog(toAdd1Id);
            _logsOnlyForTest.DeleteLog(toAdd1Id);

        }
    }
}