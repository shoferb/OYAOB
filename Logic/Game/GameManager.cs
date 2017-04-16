using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class GameManager
    {
        public int _forTest;
        public int _verifyAction;      
        public bool _gameOver = false;
        public Player _currentPlayer;
        public Player _dealerPlayer;
        public Player _bbPlayer;
        public Player _sbPlayer;
        public List<HandEvaluator> _winners;

        private int buttonPos;
        ConcreteGameRoom _state;
        //change to gameroom
        public GameManager(ConcreteGameRoom state)
        {
            this._forTest = 0;
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

            foreach (Player player in this._state._players)
            {
                player._isActive = true;
                player.AddHoleCards(deck.Draw(), deck.Draw());
            }
            this._state.UpdateMaxCommitted();

           // Play(state);

        }

        public bool Play()
        {   
            if (this._state._players.Count < 2) return false;
            else
            {
                this._state._dealerPos = 0;
                SetRoles();
                while (!this._state.AllDoneWithTurn())
                {
                    this._state.NextToPlay().Play(this._state);
                    this._forTest++;

                    this._state.UpdateGameState();
                }

                if (this._state.AllDoneWithTurn() || this._state.PlayersInHand() < 2)
                    if (ProgressHand(this._state._handStep))
                    {
                        // progresses _hand and returns whether _hand is over (the last _handStep was river)
                        EndHand();
                        return true;
                    }
                    else
                        Play();
                if (!this._state._isGameOver)
                {
                    _currentPlayer = this._state.NextToPlay();
                    _dealerPlayer = this._state._players[(this._state._players.IndexOf(_dealerPlayer) + 1)% this._state._players.Count];
                    _sbPlayer = this._state._players[(this._state._players.IndexOf(_sbPlayer) + 1)% this._state._players.Count];
                    _bbPlayer = this._state._players[(this._state._players.IndexOf(_bbPlayer) + 1)% this._state._players.Count];

                }
            }
            return true;
        }


        public bool ProgressHand(ConcreteGameRoom.HandStep previousStep)
        {            
            List<Player> playersWhoWentAllIn = new List<Player>();
            foreach (Player player in this._state._players)
                if (player.IsAllIn() && player._chipsCommitted > 0)
                    playersWhoWentAllIn.Add(player);

            while (playersWhoWentAllIn.Count > 0)
            {
                int minAllIn = 10000000;
                Player minAllInPlayer = playersWhoWentAllIn[0];

                foreach (Player player in playersWhoWentAllIn) // find player who has the smallest all in
                {
                    if (player._chipsCommitted < minAllIn)
                    {
                        minAllInPlayer = player;
                        minAllIn = player._chipsCommitted;
                    }
                }
                if (minAllInPlayer != null)
                {
                    this._state.newSplitPot(minAllInPlayer);
                    playersWhoWentAllIn.Remove(minAllInPlayer);
                }

            }
            // moves chips to center, rests _actionPos, _maxCommitted = 0, resets last actions, resets last raise



            if (this._state.PlayersInHand() < 2)
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

            if (this._state.PlayersInHand() - this._state.PlayersAllIn() < 2)
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
            List<Player> playersLeftInGame = new List<Player>();
           

            foreach (Player player in this._state._players)
                if (player._chipCount != 0)
                    playersLeftInGame.Add(player);
                else
                {
                   // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player._isActive = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }
            this._state._players = playersLeftInGame;
            this._state.EndTurn();
            _winners= FindWinner(this._state._publicCards, playersLeftInGame);
            
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
                SetRoles();
            }
            else
            {
                this._state._isGameOver = true;
                if (!_currentPlayer.OutOfMoney())
                    this._state._players[0]._isActive = false; // so if human wins doesn't try to display cards
                //GameOver
            }


        }
        //TODO: Aviv G - it's all yours:)
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
    }
 }

