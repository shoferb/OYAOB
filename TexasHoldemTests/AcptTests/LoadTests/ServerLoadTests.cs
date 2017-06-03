using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemTests.AcptTests.Bridges;
using TexasHoldem.Logic.GameControl;
using System.Threading;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class ServerLoadTests : AcptTest
    {
        private int _userId2 = -1;
        private string _user2Name;
        private string _user2Pw;
        private string _user2EmailGood;
        private int _userId3 = -1;
        private string _user3Name;
        private string _user3Pw;
        private string _user3EmailGood;
        private int _userId4 = -1;
        private string _user4Name;
        private string _user4Pw;
        private string _user4EmailGood;

        //setup: (called from case)
        protected override void SubClassInit()
        {
            _userId2 = new Random().Next() + 9292;
            _user2Name = "yarden";
            _user2EmailGood = "yarden@gmail.com";
            _user2Pw = "123456789";

            _userId3 = new Random().Next() + 91;
            _user3Name = "Aviv";
            _user3EmailGood = "Aviv@gmail.com";
            _user3Pw = "123456789";

            _userId4 = new Random().Next() + 234;
            _user4Name = "Yarden2";
            _user4EmailGood = "YRD@gmail.com";
            _user4Pw = "123456789";

            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            RegisterUser(_userId4, _user4Name, _user4Pw, _user4EmailGood);
        }


        //tear down: (called from case)
        protected override void SubClassDispose()
        {

            if (DeleteUser(_userId2))
                _userId2 = -1;
            if (DeleteUser(_userId3))
                _userId3 = -1;
            if (DeleteUser(_userId4))
                _userId4 = -1;

            Assert.True(_userId2 == -1);
            Assert.True(_userId3 == -1);
            Assert.True(_userId4 == -1);
        }

        private bool DeleteUser(int id)
        {
            if (id != -1)
            {
                List<int> user2Games = UserBridge.GetUsersGameRooms(id);
                foreach (var roomId in user2Games)
                {
                    UserBridge.RemoveUserFromRoom(id, RoomId);
                }

                UserBridge.DeleteUser(id);
                return true;
            }
            return false;

        }

        protected void RegisterUser(int userId, string name, string pass, string mail)
        {
            if (!UserBridge.IsThereUser(userId))
            {
                UserBridge.RegisterUser(userId, name, pass, mail);

                Users.Add(userId);
            }
            else if (!UserBridge.IsUserLoggedIn(userId))
            {
                UserBridge.LoginUser(name, pass);
            }
        }

        [TestCase]
        public void RegisterLoadTest1()
        {
            RestartSystem();

            //bomb the game
            Thread thread1 = new Thread(new ThreadStart(RegisterLoop));
            Thread thread2 = new Thread(new ThreadStart(RegisterLoop));
            Thread thread3 = new Thread(new ThreadStart(RegisterLoop));
            thread1.Start();
            thread2.Start();
            thread3.Start();

            Thread.Sleep(3); //let the threads work
            //wait for threads
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }

        [TestCase]
        public void RegisterAndLoginLoopTest1()
        {
            RestartSystem();

            //bomb the game
            Thread thread1 = new Thread(new ThreadStart(RegisterAndLoginLoop));
            Thread thread2 = new Thread(new ThreadStart(RegisterAndLoginLoop));
            Thread thread3 = new Thread(new ThreadStart(RegisterAndLoginLoop));
            thread1.Start();
            thread2.Start();
            thread3.Start();

            Thread.Sleep(3); //let the threads work
            //wait for threads
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }

        private void RegisterLoop()
        {
            string pass = "goodPw1234";
            string email = "test@test.com";
            string name = Thread.CurrentThread.ManagedThreadId.ToString();
            for (int i = 0; i < 5000; i++)
            {
                Assert.True(UserBridge.RegisterUser(i + name, pass, email) != -1);
                UserBridge.DeleteUser(i + name, User1Pw);
            }
        }

        private void RegisterAndLoginLoop()
        {
            string pass = "goodPw1234";
            string email = "test@test.com";
            string name = Thread.CurrentThread.ManagedThreadId.ToString();
            int id ;
            for (int i = 0; i < 10000; i++)
            {
                id = UserBridge.RegisterUser(i + name, pass, email);
                Assert.True(id != -1);
                Assert.True(UserBridge.LoginUser(i + name, pass));
                Assert.True(UserBridge.LogoutUser(id));
                UserBridge.DeleteUser(i + name, pass);
            }
        }

        [TestCase]
        public void DoActionLoadTest1()
        {
            RestartSystem();
            SetupUser1();
            CreateGameWithUser1();
            RegisterUser(_userId2, _user2Name, _user2Pw, _user2EmailGood);
            IUser user2 = UserBridge.getUserById(_userId2);
            user2.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, user2.Money()));
            RegisterUser(_userId3, _user3Name, _user3Pw, _user3EmailGood);
            IUser user3 = UserBridge.getUserById(_userId3);
            user3.AddMoney(1000);
            Assert.True(UserBridge.AddUserToGameRoomAsPlayer(_userId3, RoomId, user3.Money()));
            GameBridge.StartGame(UserId, RoomId);
            IUser user1 = UserBridge.getUserById(UserId);
            Player player1 = GetInGamePlayerFromUser(user1, RoomId);
            Player player2 = GetInGamePlayerFromUser(user2, RoomId);
            Player player3 = GetInGamePlayerFromUser(user3, RoomId);

            //bomb the game
            Thread thread1 = new Thread(new ThreadStart(doAction1));
            Thread thread2 = new Thread(new ThreadStart(doAction2));
            Thread thread3 = new Thread(new ThreadStart(doAction3));
            thread1.Start();
            thread2.Start();
            thread3.Start();

            Thread.Sleep(3); //let the threads work
                             //wait for threads
            thread1.Join();
            thread2.Join();
            thread3.Join();


            Assert.True(GameBridge.GetPlayersInRoom(RoomId).Contains(player1));
            Assert.True(GameBridge.GetPlayersInRoom(RoomId).Contains(player2));
            Assert.True(GameBridge.GetPlayersInRoom(RoomId).Contains(player3));
            Assert.True(player2.isPlayerActive);
            Assert.True(player3.isPlayerActive);
        }

        [TestCase]
        public void CreateRoomLoadTest1()
        {
            RestartSystem();

            //bomb the game
            Thread thread1 = new Thread(new ThreadStart(CreateRoomLoop1));
            Thread thread2 = new Thread(new ThreadStart(CreateRoomLoop1));
            Thread thread3 = new Thread(new ThreadStart(CreateRoomLoop1));
            thread1.Start();
            thread2.Start();
            thread3.Start();

            Thread.Sleep(3); //let the threads work
            //wait for threads
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }

        private void CreateRoomLoop1()
        {
            string pass = "goodPw1234";
            string email = "test@test.com";
            string name = "99999" + Thread.CurrentThread.ManagedThreadId.ToString();
            int id = UserBridge.RegisterUser(name, pass, email);
            Assert.True(id != -1);
            Assert.True(UserBridge.LoginUser(name, pass));
            int RoomID;
            List<int> roomIDS = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                RoomID = GameBridge.CreateGameRoom(id, 100);
                roomIDS.Add(RoomID);
            }

            for (int i = 0; i < roomIDS.Count; i++)
            {
                GameBridge.DoAction(id, CommunicationMessage.ActionType.Leave, 0, roomIDS[i]);
            }

            Assert.True(UserBridge.LogoutUser(id));
            UserBridge.DeleteUser(name, pass);
        }

        private void doAction2()
        {
            for (int i = 0; i < 10000; i++)
            {
                GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Fold, -1, RoomId);
                GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Bet, 0, RoomId);
                GameBridge.DoAction(_userId2, CommunicationMessage.ActionType.Join, 100, RoomId);
            }
        }

        private void doAction3()
        {
            for (int i = 0; i < 10000; i++)
            {
                GameBridge.DoAction(_userId3, CommunicationMessage.ActionType.Fold, -1, RoomId);
                GameBridge.DoAction(_userId3, CommunicationMessage.ActionType.Bet, 0, RoomId);
                GameBridge.DoAction(_userId3, CommunicationMessage.ActionType.Join, 100, RoomId);
            }
        }

        private void doAction1()
        {
            for (int i = 0; i < 10000; i++)
            {
                GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Join, 100, RoomId);
                GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Bet, 1, RoomId);
                GameBridge.DoAction(UserId, CommunicationMessage.ActionType.Join, 100, RoomId);
            }
        }

        private Player GetInGamePlayerFromUser(IUser user, int roomId)
        {

            foreach (Player player in GameBridge.GetPlayersInRoom(roomId))
            {
                if (player.user.Id() == user.Id())
                {
                    return player;
                }
            }
            return null;
        }
    }
}