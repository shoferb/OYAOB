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
        public ConcreteGameRoom(List<Player> players, int startingChip, int ID, ReplayManager rm , bool isSpectetor, GameMode gameModeChosen) : base(players, startingChip, ID, rm, isSpectetor, gameModeChosen)
        {
            this._isActiveGame = false;
            this._potCount = 0;          
            this._players = players;
            this._maxCommitted = 0;
            this._publicCards = new List<Card>();
            this._sb = (int) _bb / 2;
            this._bb = startingChip;
            this._sidePots = new List<Tuple<int, List<Player>>>();
            this._gm = new GameManager(this);
         }


        
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
            while (!_players[_actionPos].isPlayerActive);

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
                if (player.isPlayerActive)
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
                if (player.isPlayerActive)
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
                if (!(player.isPlayerActive == false || (player._lastAction == "call" || player._lastAction == "check" || player._lastAction == "bet" || player._lastAction == "raise") && player._gameChip == _maxCommitted))
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


        public void CheckIfPlayerWantToLeave()
        {
            foreach (Player p in this._players)
            {
                if (p._isInRoom == false)
                {
                    this._players.Remove(p);
                }
            }
        }
    }
}
