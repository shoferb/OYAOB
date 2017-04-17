 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int _gameNumber=0;
        public GameManager _gm;
        public GameReplay _gameReplay { get; set; }
        public ConcreteGameRoom(List<Player> players, int startingChip, int ID) : base(players, startingChip, ID)
        {
            this._isGameOver = false;
            this._potCount = 0;          
            this._players = players;
            this._maxCommitted = 0;
            this._publicCards = new List<Card>();
            this._sb = startingChip;
            this._bb = _sb*2;
            this._sidePots = new List<Tuple<int, List<Player>>>();
            _gameReplay = new GameReplay(_id.ToString() , 0);
            this._gm = new GameManager(this);
         }

        public List<Player> _players { get; set; }
        public Guid _id { get; set; }
        public List<Spectetor> _spectatores { get; set; }
        public int _dealerPos { get; set; }
        public int _maxCommitted { get; set; }
        public int _actionPos { get; set; }
        public int _potCount { get; set; }
        public int _bb { get; set; }
        public int _sb { get; set; }
        public Deck _deck { get; set; }
        public HandStep _handStep { get; set; }
        public List<Card> _publicCards { get; set; }
        public bool _isGameOver { get; set; }
        public List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public int _gameRoles { get; set; }

        
        public Player NextToPlay()
        {
            return _players[_actionPos];
        }

        public int ToCall()
        {
            return _maxCommitted - _players[_actionPos]._gameChip;

        }

        public void AddNewPublicCard()
        {
            Card c = _deck.ShowCard();
            foreach (Player player in _players)
            {
                player.AddCard(c);
            }
            _publicCards.Add(_deck.Draw());
            DrawCard draw = new DrawCard(c, _publicCards, _potCount);
            _gameReplay.AddAction(draw);
        }

        public void UpdateGameState()
        {
            // next player picked

            do { _actionPos = (_actionPos + 1) % _players.Count; }
            while (!_players[_actionPos]._isActive);

            UpdateMaxCommitted();
        }

        public void ClearPublicCards()
        {
            _publicCards.Clear();
        }

        public void UpdateMaxCommitted()
        {
            foreach (Player player in _players)
                if (player._gameChip > _maxCommitted)
                    _maxCommitted = player._gameChip;
        }
        public void EndTurn()
        {
            MoveChipsToPot();

           foreach (Player player in _players)
                if (player._isActive)
                    player._lastAction = "";


        }
        
        public void MoveChipsToPot()
        {
            foreach (Player player in _players)
            {
                _potCount += player._gameChip;
            }
        }
        public int PlayersInGame()
        {
            int playersInGame = 0;
            foreach (Player player in _players)
                if (player._isActive)
                    playersInGame++;
            return playersInGame;

        }

        public int PlayersAllIn()
        {
            int playersAllIn = 0;
            foreach (Player player in _players)
                if (player.IsAllIn())
                    playersAllIn++;
            return playersAllIn;
        }

        public bool AllDoneWithTurn()
        {
            bool allDone = true;
            foreach (Player player in _players)
                if (!(player._isActive == false || (player._lastAction == "call" || player._lastAction == "check" || player._lastAction == "bet" || player._lastAction == "raise") && player._gameChip == _maxCommitted))
                    allDone = false;
            return allDone;



        }

        public bool newSplitPot(Player allInPlayer)
        {
            List<Player> eligiblePlayers = new List<Player>();
            int sidePotCount = 0;
            int chipsToMatch = allInPlayer._gameChip;
            foreach (Player player in _players)
            {
                if (player._isActive && player._gameChip > 0)
                {
                    player._gameChip -= chipsToMatch;
                    sidePotCount += chipsToMatch;
                    eligiblePlayers.Add(player);
                }
            }
            sidePotCount += _potCount;
            _potCount = 0;

            if (sidePotCount > 0)
            {
                _sidePots.Add(new Tuple<int, List<Player>>(sidePotCount, eligiblePlayers));
            }
            return true;

        }



    }
}
