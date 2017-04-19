using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class GameManager
    {
        public int _verifyAction;      
        public bool _gameOver = false;
        public Player _currentPlayer;
        public Player _dealerPlayer;
        public Player _bbPlayer;
        public Player _sbPlayer;
        public List<HandEvaluator> _winners;
        public static bool _firstEnter = true;
        private int buttonPos;
        private bool _backFromRaise;
        ConcreteGameRoom _state; //game
        //change to gameroom
        public GameManager(ConcreteGameRoom state)
        {
           this._state = state;

        } 

        public void SetRoles()
        {
            this._state._handStep = ConcreteGameRoom.HandStep.PreFlop;
            Deck deck = new Deck();
            this._state._deck = deck;
            this._state._players = this._state._players;
            this.buttonPos = this._state._dealerPos;

            if (this._state._players.Count > 2)
            {
                //delaer
                this._dealerPlayer = this._state._players[this.buttonPos];
                // small blind
                this._sbPlayer = this._state._players[(this.buttonPos + 1)% this._state._players.Count];
                this._state._players[(this.buttonPos + 1) % this._state._players.Count].CommitChips(this._state._bb / 2);
                // big blind
                this._bbPlayer = this._state._players[(this.buttonPos + 2)% this._state._players.Count];
                this._state._players[(this.buttonPos + 2) % this._state._players.Count].CommitChips(this._state._bb);
                //actionPos will keep track on the curr player.
                this._state._actionPos = (this.buttonPos + 3) % this._state._players.Count;

            }
            else
            {
                this._state._actionPos = (this.buttonPos) % this._state._players.Count; 
                // small blind
                this._dealerPlayer = this._state._players[(this.buttonPos) % this._state._players.Count];
                this._sbPlayer = this._state._players[(this.buttonPos) % this._state._players.Count];
                this._state._players[(this.buttonPos) % this._state._players.Count].CommitChips(this._state._bb / 2);
                // big blind
                this._bbPlayer = this._state._players[(this.buttonPos + 1)% this._state._players.Count];
                this._state._players[(this.buttonPos + 1) % this._state._players.Count].CommitChips(this._state._bb);
            }

            StartGame startAction = new StartGame(_state._players, _dealerPlayer, _sbPlayer, _bbPlayer);
            _state._gameReplay.AddAction(startAction);

            foreach (Player player in this._state._players)
            {
                player.isPlayerActive = true;
                player.AddHoleCards(deck.Draw(), deck.Draw());
                HandCards hand = new HandCards(player, player._hand._firstCard,
                    player._hand._seconedCard);
                _state._gameReplay.AddAction(hand);
            }
            this._state.UpdateMaxCommitted();
            this._state.MoveChipsToPot();
            switch (_state._players.Count)
            {
                case 2:
                case 3:
                    this._currentPlayer = this._bbPlayer;
                    break;
                default:
                    this._currentPlayer = this._state._players[this._state._actionPos];
                    break;
            }

        }

        public bool Play() 
        {
            
            if (this._state._players.Count < 2) return false;
            else
            {
                //add to users list of available game to replay
                foreach (Player p in _state._players)
                {
                    p.AddGameAvailableToReplay(_state._id, _state._gameNumber);
                }

                if (_firstEnter)
                {
                    StartTheGame();
                }       
                while (!this._state.AllDoneWithTurn())
                {
                   int move;
                   this._currentPlayer = this._state.NextToPlay();
                    //move = this.playTurn(player)
                    move = this._currentPlayer.Play(this._state._maxCommitted, this._state._handStep);
                    PlayerDesicion(move);
                    if (_backFromRaise)
                    {
                        _backFromRaise = false;
                        break;
                    }
                   this._state.UpdateGameState();
                    this._state.CheckIfPlayerWantToLeave();
                }
                this._state.MoveChipsToPot();

                if (this._state.AllDoneWithTurn() || this._state.PlayersInGame() < 2)
                {
                    if (ProgressHand(this._state._handStep))
                    {
                        // progresses _hand and returns whether _hand is over (the last _handStep was river)
                        EndHand();
                        return true;
                    }
                    else
                    {
                        _currentPlayer = this._state.NextToPlay();
                        _dealerPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_dealerPlayer) + 1) % this._state._players.Count];
                        _sbPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_sbPlayer) + 1) % this._state._players.Count];
                        _bbPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_bbPlayer) + 1) % this._state._players.Count];
                        return Play();
                    }
                       
                }
               
            }
            return true;
            
        }

        
        public void PlayerDesicion(int move)
        {
            int max = this._state._maxCommitted;
            switch (move)
            {
                case -1:
                    Fold();
                    break;
                case 0:
                    Check();
                    break;
                default:
                    if (move == max)
                        Call(max);
                    else
                    {
                        Raise(move);
                        StartNewRoundAfterRaise();
                    }
                    break;
            }
        }

        private void StartNewRoundAfterRaise()
        {
            if (this._state.PlayersInGame() < 2)
                EndHand();

            Player tempPlayer = this._currentPlayer;
            foreach (Player p in _state._players)
            {
                if (p != tempPlayer)
                {
                    int move;
                    this._currentPlayer = this._state.NextToPlay();
                    move = this._currentPlayer.Play(this._state._sb, this._state._handStep);
                    PlayerDesicion(move);

                    this._state.UpdateGameState();
                }
            }
            _backFromRaise = true;
        }

        private void StartTheGame()
        {
            _state._gameReplay = new GameReplay(_state._id, _state._gameNumber);
            this._state._dealerPos = 0;
            SetRoles();
            _firstEnter = false;
            _state._isActiveGame = true;
        }


        public bool ProgressHand(ConcreteGameRoom.HandStep previousStep)
        {            
            

            if (this._state.PlayersInGame() < 2)
                return true;

            switch (previousStep)
            {
                case ConcreteGameRoom.HandStep.PreFlop:
                    for (int i = 0; i <= 2; i++)
                        this._state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.Flop:
                    this._state.AddNewPublicCard();
                  break;
                case ConcreteGameRoom.HandStep.Turn:
                    this._state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.River:
                   return true;

                default:
                    break;
            }


            int numNextStep = (int)previousStep + 1;
            this._state._handStep = (ConcreteGameRoom.HandStep)numNextStep;

            if (this._state.PlayersInGame() - this._state.PlayersAllIn() < 2)
            {
                ProgressHand(this._state._handStep); // recursive, runs until we'll hit the river
                return true;
            }
            else
                this._state.EndTurn(); 

            return false;

        }

        public void EndHand()
        {
            this._state._gameNumber++;
            List<Player> playersLeftInGame = new List<Player>();        
            foreach (Player player in this._state._players)
                if (player._totalChip != 0)
                    playersLeftInGame.Add(player);
                else
                {
                   // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player.isPlayerActive = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }

            this._state.EndTurn();
            _winners= FindWinner(this._state._publicCards, playersLeftInGame);
            _state._replayManager.AddGameReplay(_state._gameReplay);
            int amount;
            if (_winners.Count > 0) // so there are winners at the end of the game
            {
                amount = this._state._potCount/_winners.Count;

                foreach (HandEvaluator h in _winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in this._state._players)
            {
                player.ClearCards(); // gets rid of cards of _players still alive
            }
            if (this._state._players.Count > 1)
            {
                // sets next _dealerPos - if we want to "run" for a new game immediantly

                this._state._dealerPos++;
                this._state._dealerPos = this._state._dealerPos % this._state._players.Count;

                this._state.ClearPublicCards();
                _state._gameReplay = new GameReplay(_state._id, _state._gameNumber);
                SetRoles();
            }
            else
            {
                this._state._isActiveGame = false;
                _firstEnter = true;
                if (!_currentPlayer.OutOfMoney())
                    this._state._players[0].isPlayerActive = false; 
            }


        }

        public List<HandEvaluator> FindWinner(List<Card> table, List<Player> playersLeftInHand)
        {
            List<HandEvaluator> winners = new List<HandEvaluator>();
            foreach (Player p in playersLeftInHand)
            {
                HandEvaluator h = new HandEvaluator(p);
                List<Card> playerCards = new List<Card>();
                playerCards.AddRange(table);
                playerCards.AddRange(p._hand.GetCards());
                Card[] cards = playerCards.ToArray();
                h.DetermineHandRank(cards);
                if (winners.Count == 0)
                {
                    winners.Add(h);
                    continue;
                }
                if (h._rank.CompareTo(winners.ElementAt(0)._rank) > 0)
                {
                    winners.Clear();
                    winners.Add(h);
                }
                else if(h._rank.CompareTo(winners.ElementAt(0)._rank) == 0)
                {
                    winners.Add(h);
                }
            }
            if (winners.Count() == 1)
            {
                WinAction win = new WinAction(winners[0]._player, 
                    winners[0]._player._hand._firstCard, winners[0]._player._hand._seconedCard,
                    _state._potCount, table, winners[0]._relevantCards);
                _state._gameReplay.AddAction(win);
                return winners;
            }
            return EvalTies(winners, table);
        }

        private List<HandEvaluator> EvalTies(List<HandEvaluator> winners, List<Card> table)
        {
            List<Card> playerOneCards;
            List<Card> playerTwoCards;
            int i = 1;
            bool tie;
            while (winners.Count >= 2 && i < winners.Count() )
            {
                playerOneCards = winners.ElementAt(i-1)._relevantCards;
                playerTwoCards = winners.ElementAt(i)._relevantCards;
                FixAceTo14(playerOneCards);
                FixAceTo14(playerTwoCards);
                playerOneCards = playerOneCards.OrderByDescending(o => o._value).ToList();
                playerTwoCards = playerTwoCards.OrderByDescending(o => o._value).ToList();
                tie = true;
                for (int j=0; j < playerOneCards.Count && j < playerTwoCards.Count; j++)
                {
                    if (playerOneCards.ElementAt(j)._value > playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i);
                        tie = false;
                        break;
                    }
                    else if(playerOneCards.ElementAt(j)._value < playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i-1);
                        tie = false;
                        break;
                    }           
                }
                if (tie == true)
                {
                    i++;
                }
                FixAceTo1(playerOneCards);
                FixAceTo1(playerTwoCards);
            }
            foreach (HandEvaluator h in winners)
            {
                WinAction win = new WinAction(h._player,
                     h._player._hand._firstCard, h._player._hand._seconedCard,
                     (int)_state._potCount/winners.Count, table, h._relevantCards);
                     _state._gameReplay.AddAction(win);
            }
            return winners;
        }

        private void FixAceTo14(List<Card> cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == 1) //ACE
                {
                    c._value = 14;
                }
            }
        }

        private void FixAceTo1(List<Card> cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == (14)) //ACE
                {
                    c._value = 1;
                }
            }
        }

        public void Fold()
        {
            this._currentPlayer._lastAction = "fold";
            this._currentPlayer.isPlayerActive = false;
            FoldAction fold = new FoldAction(_currentPlayer, _currentPlayer._hand._firstCard,
                _currentPlayer._hand._seconedCard);
            _state._gameReplay.AddAction(fold);
        }

        public void Check()
        {
            this._currentPlayer._lastAction = "check";
            CheckAction check = new CheckAction(_currentPlayer, _currentPlayer._hand._firstCard,
                 _currentPlayer._hand._seconedCard);
            _state._gameReplay.AddAction(check);
        }

        public void Call(int additionalChips)
        {
            this._currentPlayer._lastAction = "call";
            additionalChips = Math.Min(additionalChips, this._currentPlayer._totalChip); // if can't afford that many chips in a call, go all in           
            this._currentPlayer.CommitChips(additionalChips);
            CallAction call = new CallAction(_currentPlayer, _currentPlayer._hand._firstCard,
                _currentPlayer._hand._seconedCard, additionalChips);
            _state._gameReplay.AddAction(call);
        }

        public void Call()
        {
            Call(this._state.ToCall());

        }

       public void Raise(int additionalChips)
        {
            _state._maxCommitted += additionalChips;
            this._currentPlayer._lastAction = "raise";
            this._currentPlayer.CommitChips(additionalChips);
            RaiseAction raise = new RaiseAction(_currentPlayer, _currentPlayer._hand._firstCard,
                 _currentPlayer._hand._seconedCard, additionalChips);
            _state._gameReplay.AddAction(raise);
        }


    }
 }

