using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
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
            Assert.IsTrue(_gameCenter.roomIdCounter==1);
            Assert.IsTrue(_gameCenter.GetRoomById(1)._players.Count == 1);
        }

        [TestMethod()]
        public void GetNextIdRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetNextIdRoom()==2);
        }

        [TestMethod()]
        public void GetLastGameRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.GetLastGameRoom() == 1);
        }

        [TestMethod()]
        public void GetRoomByIdTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            GameRoom gm = _gameCenter.GetRoomById(1);
            Assert.IsTrue(gm != null);
        }

        [TestMethod()]
        public void IsRoomExistTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.IsRoomExist(1));
        }

        [TestMethod()]
        public void RemoveRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.RemoveRoom(1));
            Assert.IsTrue(_gameCenter.GetAllActiveGame().Count==0);
        }

        [TestMethod()]
        public void AddPlayerToRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.AddPlayerToRoom(1, 2);
            Assert.IsTrue(_gameCenter.GetRoomById(1)._players.Count==2);

        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsTrue(_gameCenter.AddSpectetorToRoom(1,1));
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
            _gameCenter.RemovePlayerFromRoom(1, 1);
            Assert.IsTrue(_gameCenter.GetRoomById(1)._players.Count == 1);


        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            _systemControl.RegisterToSystem(1, "yarden", "chen", "12345678", 1000, "hh@gmail.com");
            _systemControl.RegisterToSystem(2, "yardnnnnnn", "chennnnn", "12345678", 1000, "h@gmail.com");
            _gameCenter.CreateNewRoom(1, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.AddSpectetorToRoom(1, 1);
            Assert.IsTrue(_gameCenter.GetRoomById(1)._spectatores.Count == 0);
        }

        [TestMethod()]
        public void LeagueChangeAfterGapChangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateFirstLeagueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UserLeageInfoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UserLeageGapPointTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllActiveGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllSpectetorGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllGamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllGamesByPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByGameModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByBuyInPolicyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByMinPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByMaxPlayerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByMinBetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGamesByStartingChipTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameCanSpecteteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendNotificationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindLogTest()
        {
            Assert.Fail();
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