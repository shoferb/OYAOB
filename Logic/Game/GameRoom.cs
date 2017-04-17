using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class GameRoom
    {
        public List<Player> _players { get; set; }
        public int _id { get;  set; }
        public List<Spectetor> _spectatores { get; set; }
        public int _dealerPos { get; set; }
        public int _maxCommitted { get; set; }
        public int _actionPos { get; set; }
        public int _potCount { get; set; }
        public int _bb { get; set; }
        public int _sb { get; set; }
        public Deck _deck { get; set; }
        public ConcreteGameRoom.HandStep _handStep { get; set; }
        public List<Card> _publicCards { get; set; }
        public bool _isGameOver { get; set; }
        public List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public int _gameRoles { get; set; }
        public GameReplay _gameReplay { get; set; }
        public GameManager _gameManager;
        public ReplayManager _replayManager;
        public GameRoom(List<Player> players, int startingChip, int ID, ReplayManager rm)
        {
            this._players = players;
            this._sb = startingChip;
            this._id = ID;
            _replayManager = rm;
        }

        public void AddNewPublicCard()
        {
        }

        public Player NextToPlay()
        {
            return null;
        }
        public int ToCall()
        {
            return 0;
        }
        public void UpdateGameState() { }
        public void ClearPublicCards() { }
        public void UpdateMaxCommitted() { }
        public void EndTurn() { }
        public void MoveChipsToPot() { }
        public int PlayersInGame()
        {
            return 0;
        }
        public int PlayersAllIn()
        {
            return 0;
        }
        public bool AllDoneWithTurn()
        {
            return true;
        }
        public bool newSplitPot(Player allInPlayer) { return true;}
    }
}
