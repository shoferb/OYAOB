 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public Guid _id { get; private set; }
        public static int _gameNumber=0;
        public ConcreteGameRoom(List<Player> players, int buttonPos) : base(players, buttonPos)
        {
            this._id = Guid.NewGuid();
            _isGameOver = false;
            this._potCount = 0;
            this._actionPos = (buttonPos + 3) % players.Count;
            this._players = players;
            this._buttonPos = buttonPos;
            this._maxCommitted = 0;
            _publicCards = new List<Card>();
            _sb = 0;
            _sidePots = new List<Tuple<int, List<Player>>>();
            _gameNumber++;
        }

        public override List<Player> _players { get; set; }
        public override int _buttonPos { get; set; }
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

        public override void AddNewPublicCard()
        {
            foreach (Player player in _players)
                player.AddCard(_deck.ShowCard());
            _publicCards.Add(_deck.Draw());
        }

        public override Player NextToPlay()
        {
            return _players[_actionPos];
        }

        public override Player DelaerPosition()
        {
            return null;
        }
        public override int ToCall()
        {
            return _maxCommitted - _players[_actionPos].chipsCommitted;

        }
        public override void UpdateGameState()
        {
            // next player picked

            do { _actionPos = (_actionPos + 1) % _players.Count; }
            while (!_players[_actionPos].inHand);

            UpdateMaxCommitted();
        }

        public override void ClearPublicCards()
        {
            _publicCards.Clear();
        }

        public override void UpdateMaxCommitted()
        {
            foreach (Player player in _players)
                if (player.chipsCommitted > _maxCommitted)
                    _maxCommitted = player.chipsCommitted;
        }
        public override void EndTurn()
        {
            MoveChipsToPot();
            ResetActionPos();

            _sb = 0;
            _maxCommitted = 0;

            foreach (Player player in _players)
                if (player.inHand)
                    player.lastAction = "";


        }
        public override void ResetActionPos()
        {
            int offset = 1;
            if (_handStep == HandStep.River)
                offset = 3;
            //@TODO fix divide by 0 
            _actionPos = (_buttonPos + offset) % _players.Count;
            while (!_players[_actionPos].inHand)
                _actionPos = (_actionPos + 1) % _players.Count;
        }
        public override void MoveChipsToPot()
        {
            foreach (Player player in _players)
            {
                _potCount += player.chipsCommitted;
                player.chipsCommitted = 0;
            }
        }
        public override int PlayersInHand()
        {
            int playersInHand = 0;
            foreach (Player player in _players)
                if (player.inHand)
                    playersInHand++;
            return playersInHand;

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
                if (!(player.inHand == false || player.IsAllIn() || (player.lastAction == "call" || player.lastAction == "check" || player.lastAction == "bet" || player.lastAction == "raise") && player.chipsCommitted == _maxCommitted))
                    allDone = false;
            return allDone;



        }

        public override void newSplitPot(Player allInPlayer)
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
