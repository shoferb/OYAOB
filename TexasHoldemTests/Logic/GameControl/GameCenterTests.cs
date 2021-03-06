﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game_Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemTests.Database.DataControlers;

namespace TexasHoldem.Logic.Game_Control.Tests
{

    //todo test this class
    [TestClass()]
    public class GameCenterTests
    {
        


        //private List<League> _leagueTable;
        private GameRoom _gameRoom;
        private static List<Player> _players;
        private static LogsOnlyForTest logsOnlyForTest = new LogsOnlyForTest();

        private static LogControl logControl = new LogControl();

        private static SystemControl _systemControl = new SystemControl(logControl);
        private static ReplayManager replayManager = new ReplayManager();
        private static SessionIdHandler ses = new SessionIdHandler();
        private static GameCenter _gameCenter = new GameCenter(_systemControl, logControl, replayManager, ses);
        private static UserDataProxy _userDataProxy = new UserDataProxy();
        private bool useCommunication;
        private GameDataProxy proxy = new GameDataProxy(_gameCenter);


        private void DeleteSysLog(int roomid)
        {
            List<int> toDelete = logsOnlyForTest.GetSysLogIdsByRoomId(roomid);

            foreach (var id in toDelete)
            {
                logsOnlyForTest.DeleteSystemLog(id);
                logsOnlyForTest.DeleteSystemLog(id);
            }
        }
        private bool ActionSuccedded(IEnumerator<ActionResultInfo> results)
        {
            results.MoveNext();
            ActionResultInfo result = results.Current;
            return result.GameData.IsSucceed;
        }

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
            Logic.Game.GameRoom gm = new Logic.Game.GameRoom(toAddPlayers, roomId, deco, _gameCenter, logControl, replayManager, ses);
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

        public void Cleanup(int gameNum, int roomId, int userId1, int userId2, int userId3)
        {
            _userDataProxy.DeleteUserById(userId1);
            _userDataProxy.DeleteUserById(userId2);
            _userDataProxy.DeleteUserById(userId3);
            replayManager.DeleteGameReplay(roomId, gameNum);
            replayManager.DeleteGameReplay(roomId, gameNum);
            proxy.DeleteGameRoomPref(roomId);
            proxy.DeleteGameRoom(roomId, gameNum);
        }
        [TestMethod()]
        public void CreateNewRoomTest_bad_User_null()
        {
            int roomid = new Random().Next();
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid,null, 50, true, GameMode.Limit, 2, 8, 10, 10));
        }

        [TestMethod()]
        public void CreateNewRoomTest_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
           
            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            
            Assert.IsTrue(_gameCenter.CreateNewRoomWithRoomId(roomid,user, 50, true, GameMode.Limit, 2, 8, 10, 10));
            IGame game = _gameCenter.GetRoomById(roomid);

            int gameNum = game.GameNumber;
            Cleanup(gameNum,roomid,userId);
        }

        [TestMethod()]
        public void CreateNewRoomTest_bad_already_exist()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            int gameNum = game.GameNumber;
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10));
            Cleanup(gameNum, roomid, userId);
        }
        [TestMethod()]
        public void CreateNewRoomTest_bad_startingchip_less_than_zero()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, -50, true, GameMode.Limit, 2, 8, 10, 10));
            _userDataProxy.DeleteUserById(userId);
        }

        [TestMethod()]
        public void CreateNewRoomTest_bad_minPlayer_less_than_zero()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, -2, 8, 10, 10));
            _userDataProxy.DeleteUserById(userId);
        }

        [TestMethod()]
        public void CreateNewRoomTest_bad_maxPlayer_less_than_zero()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, -8, 10, 10));
            _userDataProxy.DeleteUserById(userId);
        }


        [TestMethod()]
        public void CreateNewRoomTest_bad_minBet_less_than_zero()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, -10));
            _userDataProxy.DeleteUserById(userId);
        }

        [TestMethod()]
        public void CreateNewRoomTest_bad_buyInPolicy_less_than_zero()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            Assert.IsFalse(_gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, -10, 10));
            _userDataProxy.DeleteUserById(userId);
        }


        [TestMethod()]
        public void GetRoomByIdTest_good_id_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.Id,roomid);
            Cleanup(game.GameNumber,roomid,userId);
        }

        [TestMethod()]
        public void GetRoomByIdTest_good_starting_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.GetStartingChip(), 50);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void GetRoomByIdTest_good_isSpectete_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.IsSpectatable(),true);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void GetRoomByIdTest_good_minPlayer_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.GetMinPlayer(), 2);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void GetRoomByIdTest_good_maxPlayer_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.GetMaxPlayer(), 8);
            Cleanup(game.GameNumber, roomid, userId);
        }


        [TestMethod()]
        public void GetRoomByIdTest_good_buyIn_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.GetBuyInPolicy(), 10);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void GetRoomByIdTest_good_minBet_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.AreEqual(game.GetMinBet(), 10);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void IsRoomExistTest_good_contains()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            Assert.IsTrue(_gameCenter.IsRoomExist(roomid));
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void IsRoomExistTest_bad_dont_contains()
        {
            int roomid = new Random().Next();
            Assert.IsFalse(_gameCenter.IsRoomExist(roomid));

        }

      

        [TestMethod()]
        public void RemoveRoomTest()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _userDataProxy.DeleteUserById(userId);
            Assert.IsTrue(_gameCenter.RemoveRoom(roomid));
        }

   
        [TestMethod()]
        public void DoAction_Join_good_contains()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
           
             _gameCenter.DoAction(user2, CommunicationMessage.ActionType.Join, 50, roomid);
            IGame game = _gameCenter.GetRoomById(roomid);
            List<Player> allPlayers = game.GetPlayersInRoom();
            bool contains = false;
            foreach (Player player in allPlayers)
            {
                if (player.user.Id() == userId2)
                {
                    contains = true;
                }
            }
            Assert.AreEqual(contains,true);
            _userDataProxy.DeleteUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }


     
       

        [TestMethod()]
        public void DoAction_spectete_bad_game_not_spectete()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, false, GameMode.Limit, 2, 8, 10, 10);
            Assert.IsFalse(ActionSuccedded(
                _gameCenter.DoAction(user2, CommunicationMessage.ActionType.Spectate, 0, roomid)));
            IGame game = _gameCenter.GetRoomById(roomid);
            _userDataProxy.GetUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }


        [TestMethod()]
        public void DoAction_spectete_bad_notSpectete_game_dont_ontains()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, false, GameMode.Limit, 2, 8, 10, 10);

            _gameCenter.DoAction(user2, CommunicationMessage.ActionType.Spectate, 200, roomid);
            IGame game = _gameCenter.GetRoomById(roomid);
            List<Spectetor> allspectetor = game.GetSpectetorInRoom();
            bool contains = false;
            foreach (Spectetor spectetor in allspectetor)
            {
                if (spectetor.user.Id() == userId2)
                {
                    contains = true;
                }
            }
            Assert.AreNotEqual(contains, true);
            _userDataProxy.DeleteUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }

      
        [TestMethod()]
        public void DoAction_Spectetor_leave_good_contains()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            _gameCenter.DoAction(user2, CommunicationMessage.ActionType.Spectate, 200, roomid);
            _gameCenter.DoAction(user2, CommunicationMessage.ActionType.SpectatorLeave, -1, roomid);
            IGame game = _gameCenter.GetRoomById(roomid);
            List<Spectetor> allspectetor = game.GetSpectetorInRoom();
            bool contains = false;
            foreach (Spectetor spectetor in allspectetor)
            {
                if (spectetor.user.Id() == userId2)
                {
                    contains = true;
                }
            }
            Assert.AreEqual(contains, false);
            _userDataProxy.DeleteSpectetorGameOfUSer(userId2, roomid, game.GameNumber);

            _userDataProxy.DeleteUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }

        [TestMethod()]
        public void DoAction_SpectetorLeave_bad_user_not_in_game()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);

            Assert.IsFalse(ActionSuccedded(
                _gameCenter.DoAction(user2, CommunicationMessage.ActionType.SpectatorLeave, -1, roomid)));
            IGame game = _gameCenter.GetRoomById(roomid);
            _userDataProxy.DeleteUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }
        [TestMethod()]
        public void DoAction_SpectetorLeave_bad_user_not_in_game_dont_contains()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();
            int userId2 = new Random().Next();

            RegisterUser(userId);
            RegisterUser(userId2);
            IUser user = _systemControl.GetUserWithId(userId);
            IUser user2 = _systemControl.GetUserWithId(userId2);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);

            _gameCenter.DoAction(user2, CommunicationMessage.ActionType.SpectatorLeave, -1, roomid);
            IGame game = _gameCenter.GetRoomById(roomid);
            List<Spectetor> allspectetor = game.GetSpectetorInRoom();
            bool contains = false;
            foreach (Spectetor spectetor in allspectetor)
            {
                if (spectetor.user.Id() == userId2)
                {
                    contains = true;
                }
            }
            Assert.AreEqual(contains, false);
            _userDataProxy.DeleteUserById(userId2);
            DeleteSysLog(roomid);
            Cleanup(game.GameNumber, roomid, userId);
        }

      

        [TestMethod()]
        public void GetAllActiveGameRoomsTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();
     
            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();
       
            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetIsActive(true);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = proxy.GetAllActiveGameRooms();
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
               else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetAllSpectetorGameTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(0, 0, true, 1, 2, 1, GameMode.Limit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 1, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetAllSpectetorGame();
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

     

        [TestMethod()]
        public void GetAllGamesByPotSizeTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();
         
            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetPotSize(151515);
            toAdd.SetDeco(123, 0, true, 111, 999, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetAllGamesByPotSize(151515);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByGameModeTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(0, 0, true, 1, 2, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByGameMode(GameMode.PotLimit);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByBuyInPolicyTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(0, 0, true, 1, 2, 222, GameMode.Limit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByBuyInPolicy(222);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByMinPlayerTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(0, 0, true, 111, 999, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByMinPlayer(111);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
               else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByMaxPlayerTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(0, 0, true, 1, 999, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByMaxPlayer(999);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByMinBetTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetDeco(123, 0, true, 111, 999, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(0, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByMinPlayer(111);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void GetGamesByStartingChipTest()
        {
            int roomId = new Random().Next(); 
            int gameId = new Random().Next();
            int userId1 = new Random().Next();

            int room2Id = new Random().Next(); 
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();

            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1);
            toAdd.SetPotSize(151515);
            toAdd.SetDeco(123, 4848, true, 111, 999, 222, GameMode.PotLimit, LeagueName.B);
            toAdd2.SetDeco(3, 0, false, 1, 2, 2222, GameMode.Limit, LeagueName.B);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = _gameCenter.GetGamesByStartingChip(4848);
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                else if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, false);
            Cleanup(gameId, roomId, userId1);
            Cleanup(game2Id, room2Id, user2Id1);
        }

        [TestMethod()]
        public void IsGameCanSpecteteTest_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            int gameNum = game.GameNumber;
            Assert.IsTrue(_gameCenter.IsGameCanSpectete(roomid));
            Cleanup(gameNum, roomid, userId);
        }

        [TestMethod()]
        public void IsGameActiveTest_good()
        {
            int roomid = new Random().Next();
            int userId = new Random().Next();

            RegisterUser(userId);
            IUser user = _systemControl.GetUserWithId(userId);
            _gameCenter.CreateNewRoomWithRoomId(roomid, user, 50, true, GameMode.Limit, 2, 8, 10, 10);
            IGame game = _gameCenter.GetRoomById(roomid);
            int gameNum = game.GameNumber;
            Assert.IsFalse(_gameCenter.IsGameActive(roomid));
            Cleanup(gameNum, roomid, userId);
        }
      
      
    }
}