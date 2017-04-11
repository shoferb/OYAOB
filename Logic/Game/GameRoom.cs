 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class GameRoom 
    {
        private int _id { get; set; } //base has ID allready
        public enum HandStep { PreFlop, Flop, Turn, River }
        public List<Player> _players { get; set; }
        public int _buttonPos { get; set; }
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
       
        public GameRoom(List<Player> players, int buttonPos)
        {
            //TODO : generate random this._id ;
            _isGameOver = false;
            this._potCount = 0;
            this._actionPos = (buttonPos + 3) % players.Count;
            this._players = players;
            this._buttonPos = buttonPos;
            this._maxCommitted = 0;
            _publicCards = new List<Card>();
            _sb = 0;
            _sidePots = new List<Tuple<int, List<Player>>>();
        }

        public void AddNewPublicCard()
        {
            foreach (Player player in _players)
                player.AddCard(_deck.ShowCard());
            _publicCards.Add(_deck.Draw());
        }

        public Player NextToPlay()
        {
            return _players[_actionPos];
        }

        public Player DelaerPosition()
        {
            return null;
        }
        public int ToCall()
        {
            return _maxCommitted - _players[_actionPos].chipsCommitted;

        }
        public void UpdateGameState()
        {
            // next player picked

            do { _actionPos = (_actionPos + 1) % _players.Count; }
            while (!_players[_actionPos].inHand);

            UpdateMaxCommitted();
        }

        public void ClearPublicCards()
        {
            _publicCards.Clear();
        }

        public void UpdateMaxCommitted()
        {
            foreach (Player player in _players)
                if (player.chipsCommitted > _maxCommitted)
                    _maxCommitted = player.chipsCommitted;
        }
        public void EndTurn()
        {
            MoveChipsToPot();
            ResetActionPos();

            _sb = 0;
            _maxCommitted = 0;

            foreach (Player player in _players)
                if (player.inHand)
                    player.lastAction = "";


        }
        public void ResetActionPos()
        {
            int offset = 1;
            if (_handStep == HandStep.River)
                offset = 3;

            _actionPos = (_buttonPos + offset) % _players.Count;
            while (!_players[_actionPos].inHand)
                _actionPos = (_actionPos + 1) % _players.Count;
        }
        public void MoveChipsToPot()
        {
            foreach (Player player in _players)
            {
                _potCount += player.chipsCommitted;
                player.chipsCommitted = 0;
            }
        }
        public int PlayersInHand()
        {
            int playersInHand = 0;
            foreach (Player player in _players)
                if (player.inHand)
                    playersInHand++;
            return playersInHand;

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
                if (!(player.inHand == false || player.IsAllIn() || (player.lastAction == "call" || player.lastAction == "check" || player.lastAction == "bet" || player.lastAction == "raise") && player.chipsCommitted == _maxCommitted))
                    allDone = false;
            return allDone;



        }

        public void newSplitPot(Player allInPlayer)
        {
            List<Player> eligiblePlayers = new List<Player>();
            int sidePotCount = 0;
            int chipsToMatch = allInPlayer.chipsCommitted;
            foreach (Player player in _players)
            {
                if (player.inHand && player.chipsCommitted > 0)
                {
                    player.chipsCommitted -= chipsToMatch;
                    sidePotCount += chipsToMatch;
                    eligiblePlayers.Add(player);
                }
            }
            sidePotCount += _potCount;
            _potCount = 0;

            if (sidePotCount > 0)
                _sidePots.Add(new Tuple<int, List<Player>>(sidePotCount, eligiblePlayers));


        }



    }
}
