using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.DatabaseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Logic.Users;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic;

namespace TexasHoldem.DatabaseProxy.Tests
{
    [TestClass()]
    public class GameDataProxyTests
    {
        private UserDataControler userDataControler = new UserDataControler();
        private GameDataProxy gProx = new GameDataProxy();
        private IUser user1, user2, user3, user0;
        private List<Logic.Users.Player> players;
        private Logic.Users.Player player1;
        private int roomID;
        private Logic.Game.GameRoom gameRoom;
        private bool useCommunication;
        private static LogControl logControl = new LogControl();
        private static SystemControl sysControl = new SystemControl(logControl);
        private static Logic.Replay.ReplayManager replayManager = new Logic.Replay.ReplayManager();
        private static GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager);

        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(TexasHoldemShared.CommMessages.GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, TexasHoldem.Logic.GameControl.LeagueName.A);
            before.SetNextDecorator(mid);
            return before;
        }

        public void SetUp()
        {
            AddNewUsers();
            user1 = new User(8, "orelie", "8", "1234", 0, 5000, "orelie@post.bgu.ac.il");
            user0 = new User(0, "orelie", "0", "1234", 0, 5000, "orelie@post.bgu.ac.il");
            user2 = new User(9, "orelie", "9", "1234", 0, 5000, "orelie@post.bgu.ac.il");
            user3 = new User(10, "orelie", "10", "1234", 0, 5000, "orelie@post.bgu.ac.il");
            useCommunication = false;
            roomID = 9999;
            players = new List<Logic.Users.Player>();
            player1 = new Logic.Users.Player(user1, 1000, roomID);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            ServerToClientSender sender = new ServerToClientSender(gameCenter, sysControl, logControl, replayManager);
            gameRoom = new Logic.Game.GameRoom(players,12, deco,gameCenter,logControl,replayManager,sender,1,false,0,5,null,null,
                player1,Logic.GameControl.LeagueName.Unknow,0,player1,player1,player1,player1,
                1,1,1,1,1,null,Logic.Game.GameRoom.HandStep.Flop,null);

        }

        public void TearDown()
        {
            userDataControler.DeleteUserById(8);
            userDataControler.DeleteUserById(9);
            userDataControler.DeleteUserById(10);
            userDataControler.DeleteUserById(0);
     

        }
        public void AddNewUsers()
        {
            UserTable toAdd0 = CreateUser(0, "0");
            UserTable toAdd1 = CreateUser(8, "8");
            UserTable toAdd2 = CreateUser(9, "9");
            UserTable toAdd3 = CreateUser(10, "10");
            userDataControler.AddNewUser(toAdd1);
            userDataControler.AddNewUser(toAdd2);
            userDataControler.AddNewUser(toAdd3);
            userDataControler.AddNewUser(toAdd0);



        }

        private UserTable CreateUser(int userId, string username)
        {
            UserTable ut = new UserTable();
            ut.userId = userId;
            ut.HighestCashGainInGame = 0;
            ut.TotalProfit = 0;
            ut.avatar = "/GuiScreen/Photos/Avatar/devil.png";
            ut.email = "orelie@post.bgu.ac.il";
            ut.gamesPlayed = 0;
            ut.inActive = true;
            ut.leagueName = 1;
            ut.money = 5000;
            ut.name = "orelie";
            ut.username = username;
            ut.password = "1234";
            ut.HighestCashGainInGame = 0;
            return ut;
        }
        
    [TestMethod()]
        public void GameDataProxyTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void AddNewGameToDBTest()
        {
            SetUp();
            bool ans = gProx.AddNewGameToDB(gameRoom);
            Assert.IsTrue(ans);
        }

        [TestMethod()]
        public void GetAllGamesTest()
        {
            Assert.Fail();
        }
    }
}