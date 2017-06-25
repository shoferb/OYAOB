using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Database.DataControlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.DatabaseProxy;
using TexasHoldemTests.Database.DataControlers;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic;
using GameMode = TexasHoldemShared.CommMessages.GameMode;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;
using TexasHoldem.Logic.Users;
using Player = TexasHoldem.Logic.Users.Player;

namespace TexasHoldem.Database.DataControlers.Tests
{
    [TestClass()]
    public class LogDataControlerTests
    {

        private static LogControl logControl = new LogControl();

        private static SystemControl sysControl = new SystemControl(logControl);
        private static ReplayManager replayManager = new ReplayManager();
        private static SessionIdHandler ses = new SessionIdHandler();
        private static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager, ses);
        private static UserDataProxy _userDataProxy = new UserDataProxy();
        private bool useCommunication;
        private GameDataProxy proxy = new GameDataProxy(gameCenter);

        private readonly LogDataControler _logDataControler = new LogDataControler();
        private readonly LogsOnlyForTest _logsOnlyForTest = new LogsOnlyForTest();



        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0, 15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private Logic.Game.GameRoom CreateRoomWithId(int gameNum, int roomId, int userId1)
        {

            RegisterUser(userId1);
            useCommunication = false;

            List<Player> toAddPlayers = new List<Player>();
            IUser user = _userDataProxy.GetUserById(userId1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            Player player1 = new Player(user, 1000, roomId);
            toAddPlayers.Add(player1);
            Logic.Game.GameRoom gm = new Logic.Game.GameRoom(toAddPlayers, roomId, deco, gameCenter, logControl, replayManager, ses);
            gm.GameNumber = gameNum;
            return gm;
        }
        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit,  10, 5);
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
        public void GetNextLogIdTest_good()
        {
            var toAdd = new ErrorLog();
            var logs = new Log
            {
                LogId = 10,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 10;
            toAdd.msg = "test GetNextLogIdTest_good()";
            _logDataControler.AddErrorLog(toAdd);

            var toAdd2 = new ErrorLog();
            var logs2 = new Log
            {
                LogId = 10000,
                LogPriority = 1
            };
            toAdd2.Log = logs2;
            toAdd2.logId = 10000;
            toAdd2.msg = "test GetNextLogIdTest_good()";
            _logDataControler.AddErrorLog(toAdd2);
            var next = _logDataControler.GetNextLogId();
            Assert.AreEqual(next, 10001);
            _logsOnlyForTest.DeleteErrorLog(10);
            _logsOnlyForTest.DeleteLog(10);
            _logsOnlyForTest.DeleteErrorLog(10000);
            _logsOnlyForTest.DeleteLog(10000);
        }


       

        [TestMethod()]
        public void AddErrorLogTest_good_id()
        {
           
            var toAdd = new ErrorLog();
            var logs = new Log
            {
                LogId = 10005,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 10005;
            toAdd.msg = "test AddErrorLogTest_good_id()";
            _logDataControler.AddErrorLog(toAdd);
            
            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(10005).logId, 10005);
            _logsOnlyForTest.DeleteErrorLog(10005);
            _logsOnlyForTest.DeleteLog(10005);
        }

        [TestMethod()]
        public void AddErrorLogTest_good_message()
        {

            Database.LinqToSql.ErrorLog toAdd = new Database.LinqToSql.ErrorLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = 20000000,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = 20000000;
            toAdd.msg = "test AddErrorLogTest_good_message()";
            _logDataControler.AddErrorLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetErrorLogById(20000000).msg, "test AddErrorLogTest_good_message()");
            _logsOnlyForTest.DeleteErrorLog(20000000);
            _logsOnlyForTest.DeleteLog(20000000);
        }
        

        [TestMethod()]
        public void AddSystemLogTest_good_id()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int logId = new Random().Next();
            Logic.Game.GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAddg);
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = logId,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = logId;
            toAdd.msg = "test AddSystemLogTest_good_id()";
            toAdd.roomId = roomid;
            toAdd.game_Id = gameNum;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).logId, logId);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum,roomid,userId);
        }



        [TestMethod()]
        public void AddSystemLogTest_good_id2()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int logId = new Random().Next();
            Logic.Game.GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAddg);
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = logId,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = logId;
            toAdd.msg = "test AddSystemLogTest_good_id()";
            toAdd.roomId = roomid;
            toAdd.game_Id = gameNum;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).logId, logId);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum, roomid, userId);
        }


        [TestMethod()]
        public void AddSystemLogTest_good_message()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int logId = new Random().Next();
            Logic.Game.GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAddg);
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = logId,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = logId;
            toAdd.msg = "AddSystemLogTest_good_message()";
            toAdd.roomId = roomid;
            toAdd.game_Id = gameNum;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).msg, "AddSystemLogTest_good_message()");
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_roomId()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int logId = new Random().Next();
            Logic.Game.GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAddg);
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = logId,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = logId;
            toAdd.msg = "AddSystemLogTest_good_roomId()";
            toAdd.roomId = roomid;
            toAdd.game_Id = gameNum;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).roomId,roomid);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void AddSystemLogTest_good_gameNum()
        {
            int roomid = new Random().Next();
            int gameNum = new Random().Next();
            int userId = new Random().Next();
            int logId = new Random().Next();
            Logic.Game.GameRoom toAddg = CreateRoomWithId(gameNum, roomid, userId);
            proxy.InsertNewGameRoom(toAddg);
            Database.LinqToSql.SystemLog toAdd = new Database.LinqToSql.SystemLog();
            Database.LinqToSql.Log logs = new Database.LinqToSql.Log
            {
                LogId = logId,
                LogPriority = 1
            };
            toAdd.Log = logs;
            toAdd.logId = logId;
            toAdd.msg = "AddSystemLogTest_good_gameNum()";
            toAdd.roomId = roomid;
            toAdd.game_Id = gameNum;
            _logDataControler.AddSystemLog(toAdd);

            Assert.AreEqual(_logsOnlyForTest.GetSystemLogById(logId).game_Id, gameNum);
            _logsOnlyForTest.DeleteSystemLog(logId);
            _logsOnlyForTest.DeleteLog(logId);
            Cleanup(gameNum, roomid, userId);
        }

    }
}