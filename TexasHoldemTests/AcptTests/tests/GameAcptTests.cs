
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class GameAcptTests : AcptTest
    {
        private int _userId2;

        //setup: (called from base)
        protected override void SubClassInit()
        {
            //nothing at the moment
        }

        //tear down: (called from case)
        protected override void SubClassDispose()
        {
            if (_userId2 != -1)
            {
                UserBridge.DeleteUser(_userId2);
            }

            _userId2 = -1;
        }

        //general tests:
        [TestCase]
        public void CreateGameTestGood()
        {
            RegisterUser1();

            GameBridge.RemoveGameRoom(RoomId);

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.True(GameBridge.DoesRoomExist(RoomId));
            Assert.AreEqual(1, GameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.AreEqual(UserId, GameBridge.GetPlayersInRoom(RoomId).First());
        }

        [TestCase]
        public void CreateGameTestBad()
        {
            //user1 is not logged in
            Assert.False(GameBridge.CreateGameRoom(-1, RoomId));
            Assert.False(GameBridge.DoesRoomExist(-1));
        }

        [TestCase]
        public void GameBecomesInactiveGood()
        {
            _userId2 = GetNextUser();

            RegisterUser1();

            GameBridge.RemoveGameRoom(RoomId);

            Assert.True(GameBridge.CreateGameRoom(UserId, RoomId));
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 0));
           
            Assert.True(UserBridge.RemoveUserFromRoom(_userId2, RoomId));
            Assert.False(GameBridge.IsRoomActive(RoomId));
        }

        [TestCase]
        public void ListGamesByRankTestGood()
        {
            //delete all users and games
            RestartSystem();

            RegisterUser1();

            int rank = UserBridge.GetUserPoints(UserId);
            int someUser = GetNextUser();
            UserBridge.SetUserPoints(someUser, rank);

            Assert.True(GameBridge.CreateGameRoom(someUser, RoomId));
            Assert.Contains(RoomId, GameBridge.ListAvailableGamesByUserRank(rank));
        }

        [TestCase]
        public void ListGamesByRankTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();

            int rank1 = UserBridge.GetUserPoints(UserId);
            int someUser = GetNextUser();
            UserBridge.SetUserPoints(someUser, rank1 + 10);

            Assert.True(GameBridge.CreateGameRoom(someUser, RoomId));
            Assert.IsEmpty(GameBridge.ListAvailableGamesByUserRank(rank1));
        }

        [TestCase]
        public void ListSpectatableGamesTestGood()
        {
            //delete all users and games, register user1
            RestartSystem();

            int someUser1 = GetNextUser();
            int someUser2 = GetNextUser();

            Assert.True(GameBridge.CreateGameRoom(someUser1, RoomId));
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(someUser2, RoomId, 0));

            Assert.Contains(RoomId, GameBridge.ListSpectateableRooms());
        }

        [TestCase]
        public void ListSpectatableGamesTestSad()
        {
            //delete all users and games, register user1
            RestartSystem();

            Assert.IsEmpty(GameBridge.ListSpectateableRooms());
        }

        //game related tests:

        //tests a whole game including all actions, card deals, pot size changes, etc.
        //[TestCase]
        //public void GameTestGood()
        //{
        //    //create users:
        //    RegisterUser1();
        //    List<int> userList = new List<int>(4)
        //    {
        //        CreateGameWithUser(),UserId, GetNextUser(), GetNextUser()
        //    };

        //    List<int> moneyList = new List<int>(4)
        //    {
        //        UserBridge.GetUserMoney(userList[0]),
        //        UserBridge.GetUserMoney(userList[1]),
        //        UserBridge.GetUserMoney(userList[2]),
        //        UserBridge.GetUserMoney(userList[3])
        //    };

        //    List<int> chipsList = new List<int>
        //    {
        //        UserBridge.GetUserChips(userList[0]),
        //        UserBridge.GetUserChips(userList[1]),
        //        UserBridge.GetUserChips(userList[2]),
        //        UserBridge.GetUserChips(userList[3]),
        //    };

        //    int smallBlind = GameBridge.GetSbSize(RoomId);
        //    int bb = 2 * smallBlind;
        //    int currMinBet = bb;
        //    int potSize = 0;

        //    //add users to game:
        //    //user0 alleady in game
        //    for (int i = 0; i < userList.Count; i++)
        //    {
        //        if (i > 0) //user #0 allready in game
        //        {
        //            UserBridge.AddUserToGameRoomAsPlayer(userList[i], RoomId, 10);
        //        }
        //        Assert.AreEqual(moneyList[i] - 10, UserBridge.GetUserMoney(userList[i]));
        //        moneyList[i] -= 10;
        //        Assert.AreEqual(chipsList[i] + 10, UserBridge.GetUserChips(userList[i], RoomId));
        //        chipsList[i] += 10;
        //    }

        //    //pot should be empty
        //    Assert.AreEqual(0, GameBridge.GetPotSize(RoomId));

        //    GameBridge.StartGame(RoomId);

        //    potSize += smallBlind + bb;

        //    Assert.True(GameBridge.IsRoomActive(RoomId));
        //    Assert.AreEqual(userList[0], GameBridge.GetDealerId(RoomId));
        //    Assert.AreEqual(UserId, GameBridge.GetSbId(RoomId));
        //    Assert.AreEqual(userList[2], GameBridge.GetBbId(RoomId));
        //    Assert.AreEqual(52 - 6, GameBridge.GetDeckSize(RoomId));
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //sb and _bb paied:
        //    Assert.AreEqual(chipsList[1] - smallBlind, UserBridge.GetUserChips(userList[1], RoomId));
        //    chipsList[1] -= smallBlind;
        //    potSize += smallBlind;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(chipsList[2] - currMinBet, UserBridge.GetUserChips(userList[2], RoomId));
        //    chipsList[2] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //game start:
        //    Assert.AreEqual(userList[3], GameBridge.GetCurrPlayer(RoomId)); //user0 is dealr, user1 is sb, user2 is _bb => user3 starts
        //    Assert.True(GameBridge.Call(userList[3], RoomId, currMinBet)); //user3 calls equal to _bb
        //    Assert.AreEqual(chipsList[3] - currMinBet, UserBridge.GetUserChips(userList[3], RoomId));
        //    chipsList[3] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[0], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Raise(userList[0], RoomId, currMinBet * 2)); //user0 raises
        //    currMinBet *= 2;
        //    Assert.AreEqual(chipsList[0] - currMinBet, UserBridge.GetUserChips(userList[1], RoomId));
        //    chipsList[1] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[1], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Call(userList[1], RoomId, currMinBet)); //user1 calls
        //    Assert.AreEqual(chipsList[1] - currMinBet, UserBridge.GetUserChips(userList[1], RoomId));
        //    chipsList[1] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[2], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Fold(userList[2], RoomId)); //user2 folds
        //    Assert.AreEqual(chipsList[2], UserBridge.GetUserChips(userList[2], RoomId));
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[3], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Call(userList[3], RoomId, currMinBet)); //user3 calls
        //    Assert.AreEqual(chipsList[3] - currMinBet, UserBridge.GetUserChips(userList[3], RoomId));
        //    chipsList[3] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //deal flop:
        //    Assert.True(GameBridge.DealFlop(RoomId));
        //    Assert.AreEqual(43, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[1], GameBridge.GetCurrPlayer(RoomId)); //user1 is left of dealer
        //    Assert.True(GameBridge.Check(userList[1], RoomId));
        //    Assert.AreEqual(chipsList[1], UserBridge.GetUserChips(userList[1], RoomId));
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[3], GameBridge.GetCurrPlayer(RoomId)); //user2 folded so user3 is next
        //    Assert.True(GameBridge.Call(userList[3], RoomId, currMinBet)); //user3 calls
        //    Assert.AreEqual(chipsList[3] - currMinBet, UserBridge.GetUserChips(userList[3], RoomId));
        //    chipsList[3] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[0], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Call(userList[0], RoomId, currMinBet)); //user0 calls
        //    Assert.AreEqual(chipsList[0] - currMinBet, UserBridge.GetUserChips(userList[0], RoomId));
        //    chipsList[0] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //deal turn:
        //    Assert.True(GameBridge.DealSingleCardToTable(RoomId));
        //    Assert.AreEqual(42, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[1], GameBridge.GetCurrPlayer(RoomId)); //user1 is left of dealer
        //    Assert.True(GameBridge.Check(userList[1], RoomId));
        //    Assert.AreEqual(chipsList[1], UserBridge.GetUserChips(userList[1], RoomId));
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[3], GameBridge.GetCurrPlayer(RoomId)); //user2 folded so user3 is next
        //    Assert.True(GameBridge.Call(userList[3], RoomId, currMinBet)); //user3 calls
        //    Assert.AreEqual(chipsList[3] - currMinBet, UserBridge.GetUserChips(userList[3], RoomId));
        //    chipsList[3] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[0], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Call(userList[0], RoomId, currMinBet)); //user0 calls
        //    Assert.AreEqual(chipsList[0] - currMinBet, UserBridge.GetUserChips(userList[0], RoomId));
        //    chipsList[0] -= currMinBet;
        //    potSize += currMinBet;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //deal River:
        //    Assert.True(GameBridge.DealSingleCardToTable(RoomId));
        //    Assert.AreEqual(41, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[1], GameBridge.GetCurrPlayer(RoomId)); //user1 is left of dealer
        //    Assert.True(GameBridge.Raise(userList[1], RoomId, bb));
        //    Assert.AreEqual(chipsList[1] - bb, UserBridge.GetUserChips(userList[1], RoomId));
        //    chipsList[0] -= bb;
        //    potSize += bb;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    Assert.AreEqual(userList[3], GameBridge.GetCurrPlayer(RoomId)); //user2 folded so user3 is next
        //    Assert.True(GameBridge.Fold(userList[3], RoomId)); //user3 folds
        //    Assert.AreEqual(chipsList[3], UserBridge.GetUserChips(userList[3], RoomId));

        //    Assert.AreEqual(userList[0], GameBridge.GetCurrPlayer(RoomId));
        //    Assert.True(GameBridge.Call(userList[0], RoomId, currMinBet)); //user0 calls
        //    Assert.AreEqual(chipsList[0] - bb, UserBridge.GetUserChips(userList[0], RoomId));
        //    chipsList[0] -= bb;
        //    potSize += bb;
        //    Assert.AreEqual(potSize, GameBridge.GetPotSize(RoomId));

        //    //winner:
        //    var winners = GameBridge.GetWinner(RoomId);
        //    Assert.True(winners.TrueForAll(winner => userList.Contains(winner)));
        //    Assert.False(winners.Contains(userList[2])); //user2 folded
        //    Assert.False(winners.Contains(userList[3])); //user3 folded

        //    //game is saved:

        //    //each player has this game saved:
        //    userList.ForEach(uid =>
        //    {
        //        Assert.Contains(RoomId, ReplayBridge.GetReplayableGames(uid));
        //    });

        //    //all moves are saved:
        //    CheckGameIsSaved();
        //}

        //private void CheckGameIsSaved()
        //{
        //    var moves = ReplayBridge.ViewReplay(RoomId, 1);
        //    Assert.NotNull(moves);
        //    Assert.GreaterOrEqual(22, moves.Count);
        //}
    }
}
