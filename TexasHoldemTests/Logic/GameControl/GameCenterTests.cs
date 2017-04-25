using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control.Tests
{
    [TestClass()]
    public class GameCenterTests
    {
        private GameCenter _gameCenter = GameCenter.Instance;
        private SystemControl _systemControl = SystemControl.SystemControlInstance;
        private List<League> _leagueTable;
        private ConcreteGameRoom _gameRoom;
        private static List<Player> _players;
        
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
            _gameCenter.EditLeagueGap(20);
            Assert.IsTrue(_gameCenter.leagueGap==20);
        }

        [TestMethod()]
        public void CreateNewRoomTestUserNoExist()
        {
            Assert.IsTrue(!_gameCenter.CreateNewRoom(3, 50, true, GameMode.Limit, 2, 8, 10, 10));
        }

        [TestMethod()]
        public void CreateNewRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Assert.IsTrue(_gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10));
            Assert.IsTrue(_gameCenter.GetRoomById(_gameCenter.GetNextIdRoom() - 1)._players.Count == 1);
        }

        [TestMethod()]
        public void GetNextIdRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetNextIdRoom() == _gameCenter.GetNextIdRoom() - 1);

        }

        [TestMethod()]
        public void GetLastGameRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetLastGameRoom() == _gameCenter.GetNextIdRoom() - 1);

        }

        [TestMethod()]
        public void GetRoomByIdTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom gm = _gameCenter.GetRoomById(id);
            Assert.IsTrue(gm != null);
        }

        [TestMethod()]
        public void IsRoomExistTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.IsRoomExist(id));
        }

        [TestMethod()]
        public void RemoveRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.RemoveRoom(id));
            Assert.IsTrue(_gameCenter.GetAllActiveGame().Count==0);
        }

        [TestMethod()]
        public void AddPlayerToRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _gameCenter.AddPlayerToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id)._players.Count==2);

        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom()-1;
            Assert.IsTrue(_gameCenter.AddSpectetorToRoom(id,2));
        }

        [TestMethod()]
        public void AddSpectetorToRoomNotForSpectTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, false, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(!_gameCenter.AddSpectetorToRoom(1, 1));
        }

        [TestMethod()]
        public void RemovePlayerFromRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, false, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _gameCenter.RemovePlayerFromRoom( id, 1);
            Assert.IsTrue(_gameCenter.GetRoomById(id)._players.Count == 1);


        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _systemControl.GetUserWithId(1).IsActive = false;
            _gameCenter.AddSpectetorToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id)._spectatores.Count == 1);
            _gameCenter.RemoveSpectetorFromRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id)._spectatores.Count == 0);
        }

        [TestMethod()]
        public void LeagueChangeAfterGapChangeTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Assert.IsTrue(_gameCenter.LeagueChangeAfterGapChange(150));
        }

        [TestMethod()]
        public void CreateFirstLeagueTest()
        {
            _gameCenter.CreateFirstLeague(10);
            Assert.IsTrue(_gameCenter.LeagueTable.Count==1);
        }

        [TestMethod()]
        public void UserLeageInfoTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateFirstLeague(100);
            Assert.IsTrue(_gameCenter.UserLeageInfo(_systemControl.GetUserWithId(1))!=null);
        }

        [TestMethod()]
        public void UserLeageGapPointTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            Tuple<int, int> res = _gameCenter.UserLeageGapPoint(1);
            Assert.IsTrue(res!= null);
            Assert.IsTrue(res.Item1==0 && res.Item2 > 0);
        }

        [TestMethod()]
        public void GetAllActiveGameTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom()-1;
            GameRoom room = _gameCenter.GetRoomById(id);
            room._isActiveGame = true;
            Assert.IsTrue(_gameCenter.GetAllActiveGame().Count==1);
        }

        [TestMethod()]
        public void GetAllSpectetorGameTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            _systemControl.GetUserWithId(1).IsActive = false;
            _gameCenter.AddSpectetorToRoom(id, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(id)._spectatores.Count == 1);
            Assert.IsTrue(_gameCenter.GetAllSpectetorGame().Count==1);
        }

        [TestMethod()]
        public void GetAllGamesTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetAllGames().Count==2);
        }

        [TestMethod()]
        public void GetAllGamesByPotSizeTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            room._potCount = 12345;
            Assert.IsTrue(_gameCenter.GetAllGamesByPotSize(1).Count==0);
        }

        [TestMethod()]
        public void GetGamesByGameModeTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByGameMode(GameMode.Limit).Count==1);
        }

        [TestMethod()]
        public void GetGamesByBuyInPolicyTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByBuyInPolicy(10).Count==2);
        }

        [TestMethod()]
        public void GetGamesByMinPlayerTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMinPlayer(2).Count == 2);

        }

        [TestMethod()]
        public void GetGamesByMaxPlayerTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMaxPlayer(9).Count == 0);

        }

        [TestMethod()]
        public void GetGamesByMinBetTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByMinBet(1).Count == 0);

        }

        [TestMethod()]
        public void GetGamesByStartingChipTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 100, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 200, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.CreateNewRoom(2, 1, true, GameMode.PotLimit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetGamesByStartingChip(2).Count == 0);

        }

        [TestMethod()]
        public void IsGameCanSpecteteTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(_gameCenter.IsGameCanSpectete(id)==true);
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            Assert.IsTrue(!_gameCenter.IsGameActive(id) == true);

        }

       [TestMethod()]
        public void FindLogTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            int id = _gameCenter.GetNextIdRoom() - 1;
            GameRoom room = _gameCenter.GetRoomById(id);
            SystemLog log = new SystemLog(id, room._gameReplay.ToString());
            room._gameCenter.AddSystemLog(log);
            Assert.IsTrue(_gameCenter.FindLog(log.LogId)!= null);

        }

        [TestMethod()]
        public void AddSystemLogTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddErrorLogTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendUserAvailableMovesAndGetChoosenTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisplaymovesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsValidMoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRandomMoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendMoveBackToPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateNewRoomWithRoomIdTest()
        {
            Assert.Fail();
        }
    }
}