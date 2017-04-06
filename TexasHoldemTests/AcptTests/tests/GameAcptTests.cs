
using System.Linq;
using NUnit.Framework;
using TexasHoldemTests.AcptTests.Bridges;

namespace TexasHoldemTests.AcptTests.tests
{
    [TestFixture]
    public class GameAcptTests
    {
        private IGameBridge _gameBridge;
        private IUserBridge _userBridge;

        private const int UserId1 = ConstVarDefs.UserId1;
        private int _userId2;
        private const int RoomId = ConstVarDefs.RoomId;

        [SetUp]
        public void Init()
        {
            _gameBridge = new GameBridgeProxy();
            _userBridge = new UserBridgeProxy();
        }

        [TearDown]
        public void Dispose()
        {
            if (_userId2 != -1)
            {
                _userBridge.DeleteUser(_userId2);
            }

            //remove user1 from all games
            if (_userBridge.GetUsersGameRooms(UserId1).Count > 0)
            {
                _userBridge.GetUsersGameRooms(UserId1).ForEach(gId =>
                {
                    _userBridge.RemoveUserFromRoom(UserId1, gId);
                });
            }

            //delete room1
            if (_gameBridge.DoesRoomExist(RoomId))
            {
                _gameBridge.RemoveGameRoom(RoomId);
            }

            _userId2 = -1;
        }

        [TestCase]
        public void CreateGameTest()
        {
            Assert.True(_gameBridge.CreateGameRoom(UserId1, RoomId));
            Assert.True(_gameBridge.DoesRoomExist(RoomId));
            Assert.Equals(1, _gameBridge.GetPlayersInRoom(RoomId).Count);
            Assert.Equals(UserId1, _gameBridge.GetPlayersInRoom(RoomId).First());
        }

        [TestCase]
        public void GameBecomesInactiveGood()
        {
            _userId2 = _userBridge.GetNextFreeUserId();

            Assert.True(_gameBridge.CreateGameRoom(UserId1, RoomId));
            Assert.True(_userBridge.AddUserToGameRoomAsPlayer(_userId2, RoomId, 0));
            Assert.True(_gameBridge.StartGame(RoomId));
            Assert.True(_gameBridge.IsRoomActive(RoomId));

            Assert.True(_userBridge.RemoveUserFromRoom(_userId2, RoomId));
            Assert.False(_gameBridge.IsRoomActive(RoomId));
            Assert.False(_gameBridge.StartGame(RoomId));
        }
    }
}
