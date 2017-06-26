using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.DatabaseProxy;
using TexasHoldem.Logic.Users;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic;
using TexasHoldemShared.CommMessages;
using TexasHoldem.Logic.Replay;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldem;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace Client.Logic.Tests
{
    [TestClass()]
    public class ResponseNotifierTests
    {
        private GameDataCommMessage msg;
        private static ClientLogic cl = new ClientLogic();
        private GameDataProxy proxy;
        private UserDataProxy _userDataProxy;
        private IUser user1, user2, user3;
        private List<Player> players;
        private Player player1;
        private int roomID;
        private GameRoom gameRoom;
        private ResponeCommMessage res;
        private bool useCommunication;
        private static ReplayManager replayManager = new ReplayManager();
        private static LogControl logControl = new LogControl();
        private static SystemControl sysControl = new SystemControl(logControl);
        private ResponseNotifier rn;
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
            Card c1 = new Card(Suits.Clubs, 1);
            Card c2 = new Card(Suits.Clubs, 2);
            List<Card> pCards = new List<Card>();
            pCards.Add(c1);
            pCards.Add(c2);
            long sid = 1;
            msg = new GameDataCommMessage(gameRoom.GetPlayersInRoom().First().user.Id(), gameRoom.Id, sid, new Card(Suits.Diamonds, 1), new Card(Suits.Diamonds, 2),
                pCards, 3, 4, new List<string>(), new List<string>(), "", "", "", true, "", "", 1,
                new CommunicationMessage.ActionType(), "Flop", "");
            var ob = new List<Tuple<CommunicationMessage, bool, bool, TexasHoldemShared.CommMessages.ServerToClient.ResponeCommMessage>>();
             res = new ResponeCommMessage(1, 1, true, msg);
            var t = new Tuple<CommunicationMessage, bool, bool, TexasHoldemShared.CommMessages.ServerToClient.ResponeCommMessage>(msg, false, false,res);
            ob.Add(t);
            rn = new ResponseNotifier(ob, cl);
        }

        private void RegisterUser(int userId)
        {
            IUser toAdd = new User(userId, "orelie", "orelie" + userId, "123456789", 0, 15000, "orelie@post.bgu.ac.il");
            _userDataProxy.AddNewUser(toAdd);
        }
        private GameRoom CreateRoomWithId(int gameNum, int roomId, int userId1, int userId2, int userId3)
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


        public void Cleanup(int gameNum, int roomId, int userId1, int userId2, int userId3)
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
        public void GeneralCaseTest()
        {
            bool ans = false;
            ans = rn.GeneralCase(msg);
            Assert.IsTrue(ans);
        }

        [TestMethod()]
        public void ObserverNotifyTest()
        {
            bool ans = false;
            ans = rn.ObserverNotify(res);
            Assert.IsTrue(ans);
        }

        [TestMethod()]
        public void NotifyChatTest_PlayerWhisper()
        {
            ChatCommMessage chatMsg = new ChatCommMessage(1, 1, 1, "Bar", "Hi", ChatCommMessage.ActionType.PlayerWhisper);
            ResponeCommMessage resMsg = new ResponeCommMessage(1, 1, true, chatMsg);
            bool ans = rn.NotifyChat(resMsg);
            Assert.IsTrue(ans);
        }

        [TestMethod()]
        public void NotifyChatTest_SpectetorWhisper()
        {
            ChatCommMessage chatMsg = new ChatCommMessage(1, 1, 1, "Bar", "Hi", ChatCommMessage.ActionType.SpectetorWhisper);
            ResponeCommMessage resMsg = new ResponeCommMessage(1, 1, true, chatMsg);
            bool ans = rn.NotifyChat(resMsg);
            Assert.IsTrue(ans);
        }

    }
}