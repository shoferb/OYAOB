using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;
using Action = TexasHoldem.Logic.Game.Action;

namespace TexasHoldem.Logic.Game_Control.Tests
{
    [TestClass()]
    public class GameCenterTests
    {
        private GameCenter _gameCenter = GameCenter.Instance;
        private SystemControl _systemControl = SystemControl.SystemControlInstance;
        private List<League> _leagueTable;
        private GameRoom _gameRoom;
        private static List<Player> _players;

        private void initForAllTest()
        {
            _gameCenter.HigherRank = null;
            _gameCenter.LeagueGap = 100;
            _systemControl.Users = new List<User>();
            _gameCenter.Games = new List<GameRoom>();
           
        }
        private Player _A;
        private Player _B; public void Initialize()
        {
            _leagueTable = new List<League>();
            _A = new Player(1000, 100, 1, "Yarden", "Chen", "", 0, 0, "", 0);
            _B = new Player(500, 100, 2, "Aviv", "G", "", 0, 0, "", 0);
            _players.Add(_A);
            _players.Add(_B);
            
                
        }

       
        [TestMethod()]
        public void EditLeagueGapTest()
        {
            initForAllTest();
            _gameCenter.EditLeagueGap(20);
            Assert.IsTrue(_gameCenter.leagueGap==20);
            initForAllTest();
        }

        [TestMethod()]
        public void CreateNewRoomTestUserNoExist()
        {
            initForAllTest();
            Assert.IsTrue(!_gameCenter.CreateNewRoom(3, 50, true, GameMode.Limit, 2, 8, 10, 10));
            initForAllTest();
        }

        [TestMethod()]
        public void CreateNewRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Assert.IsTrue(_gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10));
            Assert.IsTrue(_gameCenter.GetRoomById(_gameCenter.GetNextIdRoom() - 1).Players.Count == 1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetNextIdRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetNextIdRoom() == _gameCenter.GetNextIdRoom() - 1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetLastGameRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetLastGameRoom() == _gameCenter.GetNextIdRoom() - 1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetRoomByIdTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom gm = _gameCenter.GetRoomById(id);
            Assert.IsTrue(gm != null);
            initForAllTest();
        }

        [TestMethod()]
        public void IsRoomExistTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.IsRoomExist(id));
            initForAllTest();
        }

        [TestMethod()]
        public void RemoveRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.RemoveRoom(id));
            Assert.IsTrue(_gameCenter.GetAllActiveGame().Count==0);
            initForAllTest();
        }

        [TestMethod()]
        public void AddPlayerToRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _gameCenter.AddPlayerToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id).Players.Count==2);
            initForAllTest();
        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom()-1;
            Assert.IsTrue(_gameCenter.AddSpectetorToRoom(id,2));
            initForAllTest();
        }

        [TestMethod()]
        public void AddSpectetorToRoomNotForSpectTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, false, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(!_gameCenter.AddSpectetorToRoom(1, 1));
            initForAllTest();
        }

        [TestMethod()]
        public void RemovePlayerFromRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, false, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _gameCenter.RemovePlayerFromRoom( id, 1);
            Assert.IsTrue(_gameCenter.GetRoomById(id).Players.Count == 1);

            initForAllTest();
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _systemControl.GetUserWithId(1).IsActive = false;
            _gameCenter.AddSpectetorToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id).Spectatores.Count == 1);
            _gameCenter.RemoveSpectetorFromRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id).Spectatores.Count == 0);
            initForAllTest();
        }

        [TestMethod()]
        public void LeagueChangeAfterGapChangeTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Assert.IsTrue(_gameCenter.LeagueChangeAfterGapChange(150));
            initForAllTest();
        }

        [TestMethod()]
        public void CreateFirstLeagueTest()
        {
            initForAllTest();
            _gameCenter.CreateFirstLeague(10);
            Assert.IsTrue(_gameCenter.LeagueTable.Count==1);
            initForAllTest();
        }

        [TestMethod()]
        public void UserLeageInfoTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateFirstLeague(100);
            Assert.IsTrue(_gameCenter.UserLeageInfo(_systemControl.GetUserWithId(1))!=null);
            initForAllTest();
        }

        [TestMethod()]
        public void UserLeageGapPointTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Tuple<int, int> res = _gameCenter.UserLeageGapPoint(1);
            Assert.IsTrue(res!= null);
            Assert.IsTrue(res.Item1==0 && res.Item2 > 0);
            initForAllTest();
        }

        [TestMethod()]
        public void GetAllActiveGameTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom()-1;
            GameRoom room = _gameCenter.GetRoomById(id);
            room.IsActiveGame = true;
            Assert.IsTrue(_gameCenter.GetAllActiveGame().Count==1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetAllSpectetorGameTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _systemControl.GetUserWithId(1).IsActive = false;
            _gameCenter.AddSpectetorToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id).Spectatores.Count == 1);
            Assert.IsTrue(_gameCenter.GetAllSpectetorGame().Count==1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetAllGamesTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetAllGames().Count==2);
            initForAllTest();
        }

        [TestMethod()]
        public void GetAllGamesByPotSizeTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            room.PotCount = 12345;
            Assert.IsTrue(_gameCenter.GetAllGamesByPotSize(1).Count==0);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByGameModeTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByGameMode(GameMode.Limit).Count==1);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByBuyInPolicyTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByBuyInPolicy(10).Count==2);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByMinPlayerTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMinPlayer(2).Count == 2);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByMaxPlayerTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMaxPlayer(9).Count == 0);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByMinBetTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMinBet(1).Count == 0);
            initForAllTest();
        }

        [TestMethod()]
        public void GetGamesByStartingChipTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByStartingChip(2).Count == 0);
            initForAllTest();
        }

        [TestMethod()]
        public void IsGameCanSpecteteTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.IsGameCanSpectete(id)==true);
            initForAllTest();
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(!_gameCenter.IsGameActive(id) == true);
            initForAllTest();
        }
        //todo - move to log control log
       [TestMethod()]
        public void FindLogTest()
        {
            initForAllTest();
            LogControl logControl = LogControl.Instance;
            
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            SystemLog log = new SystemLog(id, room.GameReplay.ToString());
            logControl.AddSystemLog(log);
            Assert.IsTrue(logControl.FindLog(log.LogId)!= null);
            logControl.RemoveSystenLog(log);
            initForAllTest();
        }

        [TestMethod()]
        public void AddSystemLogTest()
        {
            initForAllTest();
            LogControl logControl = LogControl.Instance;
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            SystemLog log = new SystemLog(id, room.GameReplay.ToString());
            logControl.AddSystemLog(log);
            Assert.IsTrue(logControl.FindLog(log.LogId) != null);
            logControl.RemoveSystenLog(log);
            initForAllTest();
        }

        [TestMethod()]
        public void AddErrorLogTest()
        {
            initForAllTest();
            LogControl logControl = LogControl.Instance;
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            ErrorLog log = new ErrorLog("hello world");
            logControl.AddErrorLog(log);
            
            Assert.IsTrue(logControl.FindLog(log.LogId) != null);
            logControl.RemoveErrorLog(log);
            initForAllTest();
        }
        //TODO
       [TestMethod()]
        public void IsValidMoveTest()
        {
            initForAllTest();
            List<Tuple<Action, bool, int, int>> moves = new List<Tuple<Action, bool, int, int>>();

            initForAllTest();
        }
        //todo
        /*
        [TestMethod()]
        public void GetRandomMoveTest()
        {
            Assert.Fail();
        }
        */
        [TestMethod()]
        public void GetGamesTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGames().Count==1);
            initForAllTest();
        }

        [TestMethod()]
        public void CreateNewRoomWithRoomIdTest()
        {
            initForAllTest();
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            int id = _gameCenter.GetNextIdRoom();
            _gameCenter.CreateNewRoomWithRoomId(id, 1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGames().Count == 1);
            initForAllTest();
        }
    }
}