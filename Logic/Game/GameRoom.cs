using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
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
        public bool _isActiveGame { get; set; }
        public List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public int _gameRoles { get; set; }
        public GameMode _gameMode { get; set; }
        public bool _isSpectetor { get; set; }
        public int _minPlayersInRoom { get; set; }
        public int _maxPlayersInRoom { get; set; }
        public int _enterPayingMoney { get; set; }
        public int _startingChip { get; set; }
        public GameReplay _gameReplay { get; set; }
        public GameManager _gm;
        public ReplayManager _replayManager;
        public GameCenter _gameCenter;
        public int _minBetInRoom { get; set; }
        public int _minRank { get; set; }

        public int _maxRank { get; set; }
        public GameRoom(List<Player> players, int startingChip, int ID, bool isSpectetor, 
            GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom, 
            int enterPayingMoney, int _minBetInRoom)
        {
            _spectatores = new List<Spectetor>();
            this._players = players;
            this._bb = _minBetInRoom;
            this._id = ID;
            this._isSpectetor = isSpectetor;
            this._gameMode = gameModeChosen;
            this._minPlayersInRoom = minPlayersInRoom;
            this._maxPlayersInRoom = maxPlayersInRoom;
            this._enterPayingMoney = enterPayingMoney;
            _gameCenter = GameCenter.Instance;
            _replayManager = _gameCenter.GetReplayManager();
            _gameReplay = new GameReplay(ID, 0);
            _startingChip = startingChip;

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
        public void CheckIfPlayerWantToLeave() { }
    }
}
