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
        [TestInitialize()]
        public void Initialize()
        {
            _A = new Player(1000, 100, 1, "Yarden", "Chen", "", 0, 0, "", 0);
            _B = new Player(500, 100, 2,"Aviv","G","",0,0,"",0);
            _players = new List<Player>();
            _players.Add(_A);
            _players.Add(_B);
            _gameRoom = new ConcreteGameRoom(_players, 50);
           
        }
      
       [TestMethod()]
        public void UpdateGameStateTest()
        {
            _gameRoom._gm.SetRoles();

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
    }
}