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
        private HandOfPoker _pokerHand;
        private bool _isGameOver = false;
        private static List<Player> _players;
        private List<Card> _publicCards;
        private int _sb = 0;
        private List<Tuple<int, List<Player>>> _sidePots;
        private ConcreteGameRoom.HandStep _handStep;
        private Deck _deck;
        private Player _A;
        private Player _B;
        [TestInitialize()]
        public void Initialize()
        {
            _A = new Player(1000, 100, 1, "Yarden", "Chen", "", 0, 0, "", 0, false);
            _B = new Player(500, 100, 2,"Aviv","G","",0,0,"",0,false);
            _players = new List<Player>();
            _players.Add(_A);
            _players.Add(_B);
            _gameRoom = new ConcreteGameRoom(_players, 2);
            _pokerHand = new HandOfPoker(_gameRoom);
            _publicCards = new List<Card>();
            _sidePots = new List<Tuple<int, List<Player>>>();
            _deck = new Deck();
        }
      
        [TestMethod()]
        public void ToCallTest()
        {
             Assert.IsTrue(_gameRoom.ToCall()==1);
        }

        [TestMethod()]
        public void UpdateGameStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearPublicCardsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddNewPublicCardTest()
        {
            _gameRoom.AddNewPublicCard();
            Assert.IsTrue(_gameRoom._publicCards.Count > 0);
        }

        [TestMethod()]
        public void UpdateMaxCommittedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndTurnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetActionPosTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MoveChipsToPotTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayersInHandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PlayersAllInTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AllDoneWithTurnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void newSplitPotTest()
        {
            Assert.Fail();
        }
    }
}