using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game.Tests
{
    [TestClass()]
    public class ConcreteGameRoomTests
    {
        private ConcreteGameRoom _gameRoom;
        private static List<Player> _players;
        private Player _A;
        private Player _B;
        private Player _C;
        private Player _D;
        [TestInitialize()]
        public void Initialize()
        {
            _A = new Player(1000, 100, 1, "Yarden", "Chen", "", 0, 0, "", 0);
            _B = new Player(500, 100, 2,"Aviv","G","",0,0,"",0);
            _C = new Player(500, 100, 2, "Yarden2", "G", "", 0, 0, "", 0);
            _D = new Player(5000, 100, 2, "Aviv2", "G", "", 0, 0, "", 0);
            _players = new List<Player>();
            _players.Add(_A);
            _players.Add(_B);
            _players.Add(_C);
            _players.Add(_D);
            _gameRoom = new ConcreteGameRoom(_players, 50);
           
        }
      
       [TestMethod()]
        public void UpdateGameState4PlayersTest()
        {
            _gameRoom._gm.SetRoles();
            Player curr = _gameRoom._gm._currentPlayer;
            Player sb = _gameRoom._gm._sbPlayer;
            Player bb = _gameRoom._gm._bbPlayer;
            Player dealer = _gameRoom._gm._dealerPlayer;
            _gameRoom.UpdateGameState();
            moveRoles();
            Assert.IsTrue(curr != _gameRoom._gm._currentPlayer);
            Assert.IsTrue(sb != _gameRoom._gm._sbPlayer);
            Assert.IsTrue(bb != _gameRoom._gm._bbPlayer);
            Assert.IsTrue(dealer != _gameRoom._gm._dealerPlayer);

        }

        [TestMethod()]
        public void UpdateGameState3PlayersTest()
        {// the _bb is the curr player
            _gameRoom._players.Remove(_D);
            _gameRoom._gm.SetRoles();
            Player curr = _gameRoom._gm._currentPlayer;
            Player sb = _gameRoom._gm._sbPlayer;
            Player bb = _gameRoom._gm._bbPlayer;
            Player dealer = _gameRoom._gm._dealerPlayer;
            Assert.IsTrue(curr == bb);
            _gameRoom.UpdateGameState();
            //the dealer is the curr
            moveRoles();
            Assert.IsTrue(curr != _gameRoom._gm._currentPlayer);
            Assert.IsTrue(sb != _gameRoom._gm._sbPlayer);
            Assert.IsTrue(bb != _gameRoom._gm._bbPlayer);
            Assert.IsTrue(dealer != _gameRoom._gm._dealerPlayer);

        }

        [TestMethod()]
        public void ClearPublicCardsTest()
        {
            _gameRoom.ClearPublicCards();
            Assert.IsTrue(_gameRoom._publicCards.Count == 0);
        }

        [TestMethod()]
        public void AddNewPublicCardTest()
        {
            _gameRoom.AddNewPublicCard();
           Assert.IsTrue(_gameRoom._publicCards.Count > 0);
        }

       [TestMethod()]
        public void EndTurnTest()
        {
           /* _gameRoom.EndTurn();
            foreach (Player p in _gameRoom._players)
            {
                if (p._isActive)
                Assert.IsTrue(p._lastAction.Equals(""));
            }
            */
        }

        [TestMethod()]
        public void AllDoneWithTurnTest()
        {
            _A._isActive = false;
            _B._isActive = false;
            Assert.IsTrue(_gameRoom.AllDoneWithTurn());
        }

        [TestMethod()]
        public void newSplitPotTest()
        {
            Assert.IsTrue(_gameRoom.newSplitPot(_A));
        }

        private void moveRoles()
        {
            _gameRoom._gm._currentPlayer = _gameRoom.NextToPlay();
            _gameRoom._gm._dealerPlayer =
                           this._gameRoom._players[
                               (this._gameRoom._players.IndexOf(_gameRoom._gm._dealerPlayer) + 1) % this._gameRoom._players.Count];
            _gameRoom._gm._sbPlayer =
                this._gameRoom._players[
                    (this._gameRoom._players.IndexOf(_gameRoom._gm._sbPlayer) + 1) % this._gameRoom._players.Count];
            _gameRoom._gm._bbPlayer =
                this._gameRoom._players[
                    (this._gameRoom._players.IndexOf(_gameRoom._gm._bbPlayer) + 1) % this._gameRoom._players.Count];
        }
    }
}