 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public static int _gameNumber=0;
        public GameManager _gm;
        public GameReplay _gameReplay { get; set; }
        public ConcreteGameRoom(List<Player> players, int startingChip) : base(players, startingChip)
        {
            this._isGameOver = false;
            this._potCount = 0;          
            this._players = players;
            this._maxCommitted = 0;
            this._publicCards = new List<Card>();
            this._sb = startingChip;
            this._bb = _sb*2;
            this._sidePots = new List<Tuple<int, List<Player>>>();
            _gameNumber++;
            _gameReplay = new GameReplay(_id.GetHashCode(), 0); // thats how we get the int from GUID?
            this._gm = new GameManager(this);
         }

        public override List<Player> _players { get; set; }
        public override Guid _id { get; set; }
        public override List<Spectetor> _spectatores { get; set; }
        public override int _dealerPos { get; set; }
        public override int _maxCommitted { get; set; }
        public override int _actionPos { get; set; }
        public override int _potCount { get; set; }
        public override int _bb { get; set; }
        public override int _sb { get; set; }
        public override Deck _deck { get; set; }
        public override HandStep _handStep { get; set; }
        public override List<Card> _publicCards { get; set; }
        public override bool _isGameOver { get; set; }
        public override List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public override int _gameRoles { get; set; }

        

        public override Player NextToPlay()
        {
            return _players[_actionPos];
        }

        public override int ToCall()
        {
            return _maxCommitted - _players[_actionPos]._gameChip;

        }
        public override void AddNewPublicCard()
        {
            foreach (Player player in _players)
                player.AddCard(_deck.ShowCard());
            _publicCards.Add(_deck.Draw());
        }
        public override void UpdateGameState()
        {
            // next player picked

            do { _actionPos = (_actionPos + 1) % _players.Count; }
            while (!_players[_actionPos]._isActive);

            UpdateMaxCommitted();
        }

        public override void ClearPublicCards()
        {
            _publicCards.Clear();
        }

        public override void UpdateMaxCommitted()
        {
            foreach (Player player in _players)
                if (player._gameChip > _maxCommitted)
                    _maxCommitted = player._gameChip;
        }
        public override void EndTurn()
        {
            MoveChipsToPot();
            ResetActionPos();

            _sb = 0;
            _maxCommitted = 0;

            foreach (Player player in _players)
                if (player._isActive)
                    player._lastAction = "";


        }
        public override void ResetActionPos()
        {
            int offset = 1;
            if (_handStep == HandStep.River)
                offset = 3;
            if (_players.Count == 0) _actionPos = 0;
            else
             { _actionPos = (_dealerPos + offset) % _players.Count;
                while (!_players[_actionPos]._isActive)
                    _actionPos = (_actionPos + 1) % _players.Count;
            }
        }
        public override void MoveChipsToPot()
        {
            foreach (Player player in _players)
            {
                _potCount += player._gameChip;
                player._gameChip = 0;
            }
        }
        public override int PlayersInGame()
        {
            int playersInGame = 0;
            foreach (Player player in _players)
                if (player._isActive)
                    playersInGame++;
            return playersInGame;

        }

        public override int PlayersAllIn()
        {
            int playersAllIn = 0;
            foreach (Player player in _players)
                if (player.IsAllIn())
                    playersAllIn++;
            return playersAllIn;
        }

        public override bool AllDoneWithTurn()
        {
            bool allDone = true;
            foreach (Player player in _players)
                if (!(player._isActive == false || (player._lastAction == "call" || player._lastAction == "check" || player._lastAction == "bet" || player._lastAction == "raise") && player._gameChip == _maxCommitted))
                    allDone = false;
            return allDone;



        }

        public override bool newSplitPot(Player allInPlayer)
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
