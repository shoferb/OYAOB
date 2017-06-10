using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldem.Logic.GameControl;
using static TexasHoldemShared.CommMessages.CommunicationMessage;
using TexasHoldem.Logic.Replay;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.communication.Impl;

namespace TexasHoldem.Logic.Game.Tests
{
    [TestClass()]
    public class GameRoomTests
    {
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
            user1 = new User(1, "test1", "mo", "1234", 0, 5000, "test1@gmail.com");
            user2 = new User(2, "test2", "no", "1234", 0, 5000, "test2@gmail.com");
            user3 = new User(3, "test3", "3test", "1234", 0, 5000, "test3@mailnator.com");
            useCommunication = false;
            roomID = 9999;
            players = new List<Player>();
            player1 = new Player(user1, 1000, roomID);
            players.Add(player1);
            Decorator deco = SetDecoratoresNoLimitWithSpectatores();
            gameRoom = new GameRoom(players, roomID, deco, gameCenter, logControl, replayManager, ses);

        }

        private Decorator SetDecoratoresNoLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.NoLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            return before;
        }

        private void SetDecoratoresLimitNoSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.Limit, 20, 10);
            Decorator before = new BeforeGameDecorator(20, 1500, false, 2, 5, 25, LeagueName.B);
            before.SetNextDecorator(mid);
            gameRoom.SetDecorator(before);
        }

        private void SetDecoratoresPotLimitWithSpectatores()
        {
            Decorator mid = new MiddleGameDecorator(GameMode.PotLimit, 10, 5);
            Decorator before = new BeforeGameDecorator(10, 1000, true, 2, 4, 20, LeagueName.A);
            before.SetNextDecorator(mid);
            gameRoom.SetDecorator(before);
        }


        [TestCleanup()]
        public void Cleanup()
        {
            user1 = null;
            user2 = null;
            players = null;
            player1 = null;
            gameRoom = null;
            replayManager.DeleteGameReplay(roomID, 0);
            replayManager.DeleteGameReplay(roomID, 1);
        }
        [TestMethod()]
        public void DoActionLeaveTest()
        {
            //irrelevant user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Leave, 0, useCommunication)));

            //relevant user
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Leave, 0, useCommunication)));
        }

        [TestMethod()]
        public void DoActionLeaveTest2()
        {
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Leave, 0, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Leave, 0, useCommunication)));
        }

        [TestMethod()]
        public void DoActionJoinTest()
        {
            //already player user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Join, 1000, useCommunication)));
            //new user to the game
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication)));
        }

        [TestMethod()]
        public void DoActionJoinTest2()
        {
            SetDecoratoresLimitNoSpectatores();
            //already player user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Join, 1000, useCommunication)));
            //new user not enough money in amount
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication)));
            //new user not enough money in total
            user2.EditUserMoney(10);
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 2000, useCommunication)));
            //user with enough money
            user2.EditUserMoney(10000);
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1500, useCommunication)));
        }

        [TestMethod()]
        public void DoActionStartGameTest()
        {
            //not enough players
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.StartGame, 0, useCommunication)));
            //irelevant player
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication)));

            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1500, useCommunication)));

            //enough players irrelevant user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.StartGame, 0, useCommunication)));

            //enough players relevant user
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication)));

            //already started game
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.StartGame, 0, useCommunication)));
        }

        [TestMethod()]
        public void DoActionFoldTest()
        {
            StartGameWith3Users();
            //its user1 turn
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Fold, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Fold, 0, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Fold, 0, useCommunication)));
            //now its user2 turn
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Fold, 0, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Fold, 0, useCommunication)));
            //game should be over and not active
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Fold, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Fold, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Fold, 0, useCommunication)));

        }

        [TestMethod()]
        public void DoActionCallTest()
        {
            StartGameWith3Users();
            //its user1 turn 
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 0, useCommunication)));
            // cant bet with less then bb
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 1, useCommunication)));
            //valid call = bb
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 10, useCommunication)));
            //now its user2 turn who is sb (need to add 5 for valid call)
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 3, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 5, useCommunication)));

            //now its user3 turn who is bb can call with 0 (check)
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 0, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 0, useCommunication)));

        }

        [TestMethod()]
        public void DoActioNoLimitRaiseTest()
        {
            SetDecoratoresNoLimitWithSpectatores(); // NoLimit
            StartGameWith3Users();
            //valid raise now is atless bb*2 = 20
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 15, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 20, useCommunication)));

            //now its user2 turn who is sb (need to add 15 for valid call and add (15 + 10) for min raise
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 20, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 25, useCommunication)));

            //now its user3 turn for min raise he need to add 30 (total of 40 - add 10 to the max of 30 last bet)
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 25, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 30, useCommunication)));
        }

        [TestMethod()]
        public void DoActionLimitRaiseTest()
        {
            // Limit
            SetDecoratoresLimitNoSpectatores();
            StartGameWith3Users();
            //valid raise now is bb = 20
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 15, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 20 + 20, useCommunication))); //20 for call and 20 for raise

            //now its user2 turn who is sb (need to add 30 for valid call and add (30 + 20) for min raise
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 40, useCommunication)));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 60, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 50, useCommunication)));

            //now its user3 turn for who is bb (need to add 40  for valid call and (40+20) for min raise
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 80, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 60, useCommunication)));
        }

        [TestMethod()]
        public void DoActioPotLimitRaiseTest()
        {
            SetDecoratoresPotLimitWithSpectatores(); //PotLimit
            StartGameWith3Users();
            //max raise now is current size of pot + call value = 25. so max bet is 10 + 25 =35 
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 40, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 35, useCommunication)));

            //now its user2 turn who is sb (need to add 30 for valid call) 
            // max raise is pot size + valid call = 5+10+35 + 30  = 80 => max bet is 80 +30 = 110
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 120, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 110, useCommunication)));

            //now its user3 turn who is bb (need to add 105 for valid call) 
            // max raise is pot size + valid call = 10+35+115 + 105  = 265 => max bet is 265 +105 = 370
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 375, useCommunication)));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 370, useCommunication)));
        }

        [TestMethod()]
        public void AddSpectetorToRoomTest()
        {
            //user that is a player in the room
            Assert.IsFalse(gameRoom.AddSpectetorToRoom(user1));
            //relevant user
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest()
        {
            //user that is a player in the room but not a spectator
            Assert.IsFalse(gameRoom.RemoveSpectetorFromRoom(user1));
            //irrelevant user
            Assert.IsFalse(gameRoom.RemoveSpectetorFromRoom(user2));
        }

        [TestMethod()]
        public void RemoveSpectetorFromRoomTest2()
        {
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            //relevant user
            Assert.IsTrue(gameRoom.RemoveSpectetorFromRoom(user2));
        }


        [TestMethod()]
        public void CanJoinTest()
        {
            //new user
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication)));
            Assert.IsTrue(user2.Money() == 5000 - 1000 - 20);
            //an already player user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Join, 1000, useCommunication)));
        }

        [TestMethod()]
        public void CanJoinTest2()
        {
            //not enough money enter
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1, useCommunication)));
            Assert.IsTrue(user2.Money() == 5000);
            //an already player user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Join, 1000, useCommunication)));
        }


        [TestMethod()]
        public void CanJoinTestWithSpectator()
        {
            //relevant user
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            //an already spectator user
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication)));
        }

        [TestMethod()]
        public void IsGameActiveTest()
        {
            //non started game
            Assert.IsFalse(gameRoom.IsGameActive());
            //join another player and start game
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication));
            Assert.IsTrue(gameRoom.IsGameActive());
        }

        [TestMethod()]
        public void IsSpectatableTest()
        {
            Assert.IsTrue(gameRoom.IsSpectatable());
            SetDecoratoresLimitNoSpectatores();
            Assert.IsFalse(gameRoom.IsSpectatable());
        }

        [TestMethod()]
        public void IsPotSizEqualTest()
        {
            Assert.IsTrue(gameRoom.IsPotSizeEqual(0));

            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication));
            Assert.IsTrue(gameRoom.IsPotSizeEqual(10 + 5)); //small+big
        }

        [TestMethod()]
        public void IsGameModeEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameModeEqual(GameMode.NoLimit));
            Assert.IsFalse(gameRoom.IsGameModeEqual(GameMode.Limit));

            SetDecoratoresLimitNoSpectatores();
            Assert.IsTrue(gameRoom.IsGameModeEqual(GameMode.Limit));
            Assert.IsFalse(gameRoom.IsGameModeEqual(GameMode.NoLimit));
            Assert.IsFalse(gameRoom.IsGameModeEqual(GameMode.PotLimit));
        }

        [TestMethod()]
        public void IsGameBuyInPolicyEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameBuyInPolicyEqual(20));
            Assert.IsFalse(gameRoom.IsGameBuyInPolicyEqual(25));
            SetDecoratoresLimitNoSpectatores(); // change to 25 fee
            Assert.IsTrue(gameRoom.IsGameBuyInPolicyEqual(25));
            Assert.IsFalse(gameRoom.IsGameBuyInPolicyEqual(20));
        }

        [TestMethod()]
        public void IsGameMinPlayerEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMinPlayerEqual(2));
            Assert.IsFalse(gameRoom.IsGameMinPlayerEqual(3));

            SetDecoratoresLimitNoSpectatores(); // same min players
            Assert.IsTrue(gameRoom.IsGameMinPlayerEqual(2));
            Assert.IsFalse(gameRoom.IsGameMinPlayerEqual(3));
        }

        [TestMethod()]
        public void IsGameMaxPlayerEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMaxPlayerEqual(4));
            Assert.IsFalse(gameRoom.IsGameMaxPlayerEqual(5));

            SetDecoratoresLimitNoSpectatores(); // max 5 player
            Assert.IsTrue(gameRoom.IsGameMaxPlayerEqual(5));
            Assert.IsFalse(gameRoom.IsGameMaxPlayerEqual(4));
        }

        [TestMethod()]
        public void IsGameMinBetEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameMinBetEqual(10));
            Assert.IsFalse(gameRoom.IsGameMinBetEqual(20));

            SetDecoratoresLimitNoSpectatores(); // BB (equal to min bet) is now 20
            Assert.IsTrue(gameRoom.IsGameMinBetEqual(20));
            Assert.IsFalse(gameRoom.IsGameMinBetEqual(10));
        }

        [TestMethod()]
        public void IsGameStartingChipEqualTest()
        {
            Assert.IsTrue(gameRoom.IsGameStartingChipEqual(1000));
            Assert.IsFalse(gameRoom.IsGameStartingChipEqual(1500));

            SetDecoratoresLimitNoSpectatores(); // starting cheap equal to 1500
            Assert.IsTrue(gameRoom.IsGameStartingChipEqual(1500));
            Assert.IsFalse(gameRoom.IsGameStartingChipEqual(1000));
        }

        [TestMethod()]
        public void GetPlayersInRoomTest()
        {
            Assert.IsTrue(gameRoom.GetPlayersInRoom().Count == 1);
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(0).user.Equals(user1));
            // add 1 more player
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication));
            Assert.IsTrue(gameRoom.GetPlayersInRoom().Count == 2);
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(0).user.Equals(user1));
            Assert.IsTrue(gameRoom.GetPlayersInRoom().ElementAt(1).user.Equals(user2));
        }

        [TestMethod()]
        public void GetSpectetorInRoomTest()
        {
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().Count == 0);
            //add spectator
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user2));
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().Count == 1);
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().ElementAt(0).user.Equals(user2));
            //remove spectator
            Assert.IsTrue(gameRoom.RemoveSpectetorFromRoom(user2));
            Assert.IsTrue(gameRoom.GetSpectetorInRoom().Count == 0);
        }

        [TestMethod()]
        public void GetMinPlayerTest()
        {
            Assert.IsTrue(gameRoom.GetMinPlayer() == 2);
            Assert.IsFalse(gameRoom.GetMinPlayer() == 3);

            SetDecoratoresLimitNoSpectatores(); // same min player
            Assert.IsTrue(gameRoom.GetMinPlayer() == 2);
            Assert.IsFalse(gameRoom.GetMinPlayer() == 3);
        }

        [TestMethod()]
        public void GetMinBetTest()
        {
            Assert.IsTrue(gameRoom.GetMinBet() == 10);
            Assert.IsFalse(gameRoom.GetMinBet() == 20);

            SetDecoratoresLimitNoSpectatores(); // min bet is now 20
            Assert.IsTrue(gameRoom.GetMinBet() == 20);
            Assert.IsFalse(gameRoom.GetMinBet() == 10);
        }

        [TestMethod()]
        public void GetMaxPlayerTest()
        {
            Assert.IsTrue(gameRoom.GetMaxPlayer() == 4);
            Assert.IsFalse(gameRoom.GetMaxPlayer() == 5);

            SetDecoratoresLimitNoSpectatores(); // max player is now 5
            Assert.IsTrue(gameRoom.GetMaxPlayer() == 5);
            Assert.IsFalse(gameRoom.GetMaxPlayer() == 4);
        }

        [TestMethod()]
        public void GetPotSizeTest()
        {
            Assert.IsTrue(gameRoom.GetPotSize() == 0);

            //add player and start game taking sb and bb
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Join, 1000, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.StartGame, 0, useCommunication));
            Assert.IsTrue(gameRoom.GetPotSize() == 10 + 5); //small+big
        }

        [TestMethod()]
        public void GetBuyInPolicyTest()
        {
            Assert.IsTrue(gameRoom.GetBuyInPolicy() == 20);
            Assert.IsFalse(gameRoom.GetBuyInPolicy() == 25);

            SetDecoratoresLimitNoSpectatores(); // change to 25 fee
            Assert.IsTrue(gameRoom.GetBuyInPolicy() == 25);
            Assert.IsFalse(gameRoom.GetBuyInPolicy() == 20);
        }

        [TestMethod()]
        public void GetStartingChipTest()
        {
            Assert.IsTrue(gameRoom.GetStartingChip() == 1000);
            Assert.IsFalse(gameRoom.GetStartingChip() == 1500);

            SetDecoratoresLimitNoSpectatores(); // starting cheap equal to 1500
            Assert.IsTrue(gameRoom.GetStartingChip() == 1500);
            Assert.IsFalse(gameRoom.GetStartingChip() == 1000);
        }

        [TestMethod()]
        public void GetGameGameModeTest()
        {
            Assert.IsTrue(gameRoom.GetGameMode() == GameMode.NoLimit);
            Assert.IsFalse(gameRoom.GetGameMode() == GameMode.Limit);

            SetDecoratoresLimitNoSpectatores(); // Game mode is now Limit
            Assert.IsTrue(gameRoom.GetGameMode() == GameMode.Limit);
            Assert.IsFalse(gameRoom.GetGameMode() == GameMode.NoLimit);
        }

        [TestMethod()]
        public void GetLeagueNameTest()
        {
            Assert.IsTrue(gameRoom.GetLeagueName() == LeagueName.A);
            Assert.IsFalse(gameRoom.GetLeagueName() == LeagueName.B);

            SetDecoratoresLimitNoSpectatores(); // League name is now B
            Assert.IsTrue(gameRoom.GetLeagueName() == LeagueName.B);
            Assert.IsFalse(gameRoom.GetLeagueName() == LeagueName.A);
        }

        [TestMethod()]
        public void FullGameTest1()
        {
            SetDecoratoresNoLimitWithSpectatores();
            StartGameWith3Users();
            //user1 turn have to bet 10 or more
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 10, useCommunication)); //call
            Assert.IsTrue(gameRoom.GetPotSize() == 25); // 5 + 10 + 10

            //user2 turn have to bet 5 or more
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 3, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 5, useCommunication));
            Assert.IsTrue(gameRoom.GetPotSize() == 30); // 5 + 10 + 10 +5

            //add spectator
            IUser user4 = new User(4, "test4", "4test", "1234", 0, 5000, "test4@mailnator.com");
            Assert.IsTrue(gameRoom.AddSpectetorToRoom(user4));

            //user3 can check
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 0, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user4, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 0, useCommunication)); // check
            Assert.IsTrue(gameRoom.GetPotSize() == 30); // 5 + 10 + 10 +5

            //new round 
            Assert.IsTrue(gameRoom.GetStep() == GameRoom.HandStep.Flop);

            //user1 can check
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, -10, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication)); //check
            Assert.IsTrue(gameRoom.GetPotSize() == 30); //from last round

            //user2 turn can check but choose to bet 10
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 10, useCommunication)); //raise 10
            Assert.IsTrue(gameRoom.GetPotSize() == 40);

            //user3 can call 10 or raise or fold -> choose to call
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication)); // call
            Assert.IsTrue(gameRoom.GetPotSize() == 50);

            //same round 
            Assert.IsTrue(gameRoom.GetStep() == GameRoom.HandStep.Flop);

            //user1 can call 10 or fold or raise
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 10, useCommunication)); //call
            Assert.IsTrue(gameRoom.GetPotSize() == 60); //from last round

            //new round 
            Assert.IsTrue(gameRoom.GetStep() == GameRoom.HandStep.Turn);

            //user1 can check or fold or raise
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 10, useCommunication));
            Assert.IsFalse(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication)); //check
            Assert.IsTrue(gameRoom.GetPotSize() == 60); //from last round

            //user2 raise 15
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 15, useCommunication)); //raise 15
            Assert.IsTrue(gameRoom.GetPotSize() == 75);

            //user3 re raise another 15
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Bet, 30, useCommunication)); //raise 15
            Assert.IsTrue(gameRoom.GetPotSize() == 105);

            //user1 re raise another 15
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 45, useCommunication)); //raise 15
            Assert.IsTrue(gameRoom.GetPotSize() == 150);

            //user2 call
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 30, useCommunication)); //call 
            Assert.IsTrue(gameRoom.GetPotSize() == 180);

            //same round 
            Assert.IsTrue(gameRoom.GetStep() == GameRoom.HandStep.Turn);

            //user3 fold
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Fold, 0, useCommunication)); //fold
            Assert.IsTrue(gameRoom.GetPotSize() == 180);

            //next round 
            Assert.IsTrue(gameRoom.GetStep() == GameRoom.HandStep.River);

            //user1 check
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Bet, 0, useCommunication)); //check
            Assert.IsTrue(gameRoom.GetPotSize() == 180);

            //user2 check
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Bet, 0, useCommunication)); //check
            Assert.IsTrue(gameRoom.GetPotSize() == 180);

            //game Over
            Assert.IsFalse(gameRoom.IsGameActive());

            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user1, ActionType.Leave, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user2, ActionType.Leave, 0, useCommunication));
            Assert.IsTrue(ActionSuccedded(gameRoom.DoAction(user3, ActionType.Leave, 0, useCommunication));
            Assert.IsTrue(user3.Money() < 5000);
            //user start money = 5000, fee in room is 20
            Assert.IsTrue(user1.Money() + user2.Money() + user3.Money() == 5000 * 3 - 20 * 3);
        }

        private bool ActionSuccedded(IEnumerator<ActionResultInfo> results)
        {
            results.MoveNext();
            ActionResultInfo result = results.Current;
            return result.GameData.IsSucceed;
        }

        private void StartGameWith3Users()
        {
            gameRoom.DoAction(user2, ActionType.Join, 1500, useCommunication);
            gameRoom.DoAction(user3, ActionType.Join, 1500, useCommunication);
            gameRoom.DoAction(user1, ActionType.StartGame, 0, useCommunication);
        }

    }
}