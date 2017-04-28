 using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Threading;
 using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom : GameRoom
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int _gameNumber=0;
        public ConcreteGameRoom(List<Player> players, int startingChip, int ID, bool isSpectetor,
            GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom,
            int enterPayingMoney, int minBetInRoom) : 
            base(players, startingChip, ID, isSpectetor, gameModeChosen, minPlayersInRoom, 
            maxPlayersInRoom, enterPayingMoney, minBetInRoom)
        {
            this._isActiveGame = false;
            this._potCount = 0;          
            this._players = players;
            this._maxCommitted = 0;
            this._publicCards = new List<Card>();
            this._sb = (int) _bb / 2;
            this._bb = minBetInRoom;
            this._sidePots = new List<Tuple<int, List<Player>>>();
            this._gm = new GameManager(this);
            //    this._minRank = _gameCenter.UserLeageGapPoint..
            Tuple<int,int> tup = _gameCenter.UserLeageGapPoint(players[0].user.Id());
            this._minRank = tup.Item1;
            this._maxRank = tup.Item2;
        }

        //set the room's thread
        public void SetThread(Thread thread)
        {
            _gm.RoomThread = thread;
        }

        public override Player NextToPlay()
        {
            return _players[_actionPos];
        }

        public override int ToCall()
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

        public override void UpdateGameState()
        {
            // next player picked

            do { _actionPos = (_actionPos + 1) % _players.Count; }
            while (!_players[_actionPos].isPlayerActive);

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

           foreach (Player player in _players)
                if (player.isPlayerActive)
                    player._lastAction = "";


        }
        
        public override void MoveChipsToPot()
        {
            foreach (Player player in _players)
            {
                _potCount += player._gameChip;
            }
        }
        public override int PlayersInGame()
        {
            int playersInGame = 0;
            foreach (Player player in _players)
            {
                if (player.isPlayerActive)
                {
                    playersInGame++;
                }
            }
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
                if (!(player.isPlayerActive == false || (player._lastAction == "call" || player._lastAction == "check" || player._lastAction == "bet" || player._lastAction == "raise") && player._gameChip == _maxCommitted))
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
                if (player.isPlayerActive && player._gameChip > 0)
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


        public override void CheckIfPlayerWantToLeave()
        {
            List<Player> players = new List<Player>();
            foreach (Player p in this._players)
            {
                if (p._isInRoom == true)
                {
                    players.Add(p);
                }
            }
            if (players.Count < _players.Count)
            {
                _players = players;
            }
        }

        public void moveBBnSBtoPot(Player bbPlayer, Player sbPlayer)
        {
            _potCount = _bb + _sb;
            bbPlayer._gameChip = bbPlayer._gameChip - _bb;
            sbPlayer._gameChip = sbPlayer._gameChip - _sb;
        }
    }
}
