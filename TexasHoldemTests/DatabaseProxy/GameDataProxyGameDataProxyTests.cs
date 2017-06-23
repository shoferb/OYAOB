using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldem.Logic.GameControl;
using static TexasHoldemShared.CommMessages.CommunicationMessage;
using TexasHoldem.Logic.Replay;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace TexasHoldem.DatabaseProxy.Tests

{
    [TestClass()]
    public class GameDataProxyGameDataProxyTests
    {

        private GameDataProxy proxy;
        private UserDataProxy _userDataProxy;
        private IUser user1, user2, user3;
        private List<Player> players;
        private Player player1;
        private int roomID;
        private GameRoom gameRoom;
        private bool useCommunication;
        private static LogControl logControl = new LogControl();
        private static SystemControl sysControl = new SystemControl(logControl);
        private static ReplayManager replayManager = new ReplayManager();
        private static SessionIdHandler ses = new SessionIdHandler();
        private static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager, ses);


        [TestInitialize()]
        public void Initialize()
        {
            proxy = new GameDataProxy(gameCenter);
            _userDataProxy = new UserDataProxy();
            user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            user2 = new User(2, "test2", "no", "1234", 0, 5000, "test2@gmail.com");
            user3 = new User(3, "test3", "3test", "1234", 0, 5000, "test3@mailnator.com");
            _userDataProxy.AddNewUser(user1);
            _userDataProxy.AddNewUser(user2);
            _userDataProxy.AddNewUser(user3);
            useCommunication = false;
            roomID = 9999;
            players = new List<Player>();
            player1 = new Player(user1, 1000, roomID);
            player1.RoundChipBet = 22;
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            gameRoom = new GameRoom(players, roomID, deco, gameCenter, logControl, replayManager, ses);
        }

        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0,15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private GameRoom CreateRoomWithId(int gameNum,int roomId, int userId1, int userId2, int userId3)
        {

            RegisterUser(userId1);
            RegisterUser(userId2);
            RegisterUser(userId3);
            useCommunication = false;

            List<Player> toAddPlayers = new List<Player>();
            IUser user = _userDataProxy.GetUserById(userId1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            player1.RoundChipBet = 22;
            players.Add(player1);
            player1 = new Player(user, 1000, roomId);
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

      
        public void Cleanup(int gameNum,int roomId, int userId1,int userId2,int userId3)
        {
            _userDataProxy.DeleteUserById(userId1);
            _userDataProxy.DeleteUserById(userId2);
            _userDataProxy.DeleteUserById(userId3);
            replayManager.DeleteGameReplay(roomID, 0);
            replayManager.DeleteGameReplay(roomID, 1);
            proxy.DeleteGameRoomPref(roomId);
            proxy.DeleteGameRoom(roomId, gameNum);
        }

       

        [TestMethod()]
        public void UpdateGameRoomPotSizeTest_good_boolReturn()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = 1;
            int userId2 = 2;
            int userId3 = 3;
            GameRoom toAdd = CreateRoomWithId(gameId,roomId,userId1,userId2,userId3);
            proxy.InsertNewGameRoom(toAdd);
            bool ans = proxy.UpdateGameRoomPotSize(777, roomId);
            Assert.IsTrue(ans);
            Cleanup(gameId, roomId, userId1, userId2, userId3);
        }


        [TestMethod()]
        public void UpdateGameRoomPotSizeTest_good()
        {      
            proxy.InsertNewGameRoom(gameRoom);
            gameRoom.SetPotSize(777);
            bool ans = proxy.UpdateGameRoom(gameRoom);
            IGame g = proxy.GetGameRoombyId(gameRoom.Id);        
            Assert.IsTrue(ans);
            Assert.IsTrue(g.IsPotSizeEqual(777));
            proxy.DeleteGameRoomPref(gameRoom.Id);
            proxy.DeleteGameRoom(gameRoom.Id, gameRoom.GetGameNum());
        }

        [TestMethod()]
        public void GameRoomTest_Update_Bb_good()
        {
            proxy.InsertNewGameRoom(gameRoom);
            gameRoom.SetBB(89);
            proxy.UpdateGameRoom(gameRoom);
            GameRoom g = (GameRoom)proxy.GetGameRoombyId(gameRoom.Id);
            Assert.AreEqual(g.getBBnum(), 89);
            proxy.DeleteGameRoomPref(gameRoom.Id);
            proxy.DeleteGameRoom(gameRoom.Id, gameRoom.GetGameNum());
        }



        [TestMethod()]
        public void GameRoomTest_Update_isActive_good()
        {
            proxy.InsertNewGameRoom(gameRoom);
            gameRoom.SetIsActive(true);
            proxy.UpdateGameRoom(gameRoom);
            IGame g = proxy.GetGameRoombyId(gameRoom.Id);
            Assert.AreEqual(g.IsGameActive(), true);
            proxy.DeleteGameRoomPref(gameRoom.Id);
            proxy.DeleteGameRoom(gameRoom.Id, gameRoom.GetGameNum());
        }


        [TestMethod()]
        public void GetAllGamesTest()
        {
            int roomId = new Random().Next();
            int gameId = new Random().Next();
            int userId1 = new Random().Next();
            int userId2 = new Random().Next();
            int userId3 = new Random().Next();
            int room2Id = new Random().Next();
            int game2Id = new Random().Next();
            int user2Id1 = new Random().Next();
            int user2Id2 = new Random().Next();
            int user2Id3 = new Random().Next();
            GameRoom toAdd = CreateRoomWithId(gameId, roomId, userId1, userId2, userId3);
            GameRoom toAdd2 = CreateRoomWithId(game2Id, room2Id, user2Id1, user2Id2, user2Id3);
            proxy.InsertNewGameRoom(toAdd);
            proxy.InsertNewGameRoom(toAdd2);
            proxy.UpdateGameRoom(toAdd);
            List<IGame> g = proxy.GetAllGames();
            bool g1 = false;
            bool g2 = false;
            foreach (var game in g)
            {
                if (game.Id == roomId)
                {
                    g1 = true;
                }
                if (game.Id == room2Id)
                {
                    g2 = true;
                }
            }
            Assert.AreEqual(g1, true);
            Assert.AreEqual(g2, true);
            Cleanup(gameId, roomId, userId1, userId2, userId3);
            Cleanup(game2Id, room2Id, user2Id1, user2Id2, userId3);
        }

        [TestMethod()]
        public void GetAllActiveGameRoomsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllSpectatebleGameRoomsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByBuyInPolicyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByGameModeTest()
        {
           Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMaxPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinPlayersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByMinBetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByPotSizeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGameRoomsByStartingChipTest()
        {
           Assert.Fail();
        }

        [TestMethod()]
        public void InsertNewGameRoomTest()
        {
           Assert.Fail();

        }

        [TestMethod()]
        public void GetGameRoombyIdTest()
        {
            bool ans = proxy.InsertNewGameRoom(gameRoom);
            IGame gac = proxy.GetGameRoombyId(gameRoom.Id);
            Assert.IsTrue(gac.Id == gameRoom.Id);
            Assert.IsFalse(gac.IsGameActive()== gameRoom.IsGameActive());
            Assert.IsTrue(gac.GetBuyInPolicy() == gameRoom.GetBuyInPolicy());
            Assert.IsTrue(gac.GetCurrPosition() == gameRoom.GetCurrPosition());
            proxy.DeleteGameRoomPref(gameRoom.Id);
            proxy.DeleteGameRoom(gameRoom.Id, gameRoom.GetGameNum());

        }

        [TestMethod()]
        public void GetGameRoomReplyByIdTest()
        {
           Assert.Fail();
        }
    }
}