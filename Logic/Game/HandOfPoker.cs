using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class HandOfPoker
    {
        public bool _gameOver = false;
        public Player _currentPlayer;
        public Player _dealerPlayer;
        public int _dealerIdx;
        public Player _bbPlayer;
        public Player _sbPlayer;
        

        int buttonPos = 0;
        ConcreteGameRoom state;

        public HandOfPoker(ConcreteGameRoom state)
        {
            if (state._players.Count < 8)
                NewHand(state);
            else
            {
                //TODO: write to log and throw an exception.
            }
        } 

        public void NewHand(ConcreteGameRoom state)
        {
            this.state = state;
            state._handStep = ConcreteGameRoom.HandStep.PreFlop;
            Deck deck = new Deck();
            state._deck = deck;
            state._players = state._players;
            this.buttonPos = state._buttonPos;

            if (state._players.Count > 2)
            {
                //delaer
                this._dealerPlayer = state._players[this.buttonPos];
                // small blind
                this._sbPlayer = state._players[(this.buttonPos + 1)%state._players.Count];
                state._players[(this.buttonPos + 1) % state._players.Count].CommitChips(state._bb / 2);
                // big blind
                this._bbPlayer = state._players[(this.buttonPos + 2)%state._players.Count];
                state._players[(this.buttonPos + 2) % state._players.Count].CommitChips(state._bb);
                //actionPos will keep track on the curr player.
                state._actionPos = (this.buttonPos + 3) % state._players.Count;

            }
            else
            {
                state._actionPos = (this.buttonPos) % state._players.Count; // TODO: if only 2 who starts?
                // small blind
                this._dealerPlayer = state._players[(this.buttonPos) %state._players.Count];
                this._sbPlayer = state._players[(this.buttonPos) %state._players.Count];
                state._players[(this.buttonPos) % state._players.Count].CommitChips(state._bb / 2);
                // big blind
                this._bbPlayer = state._players[(this.buttonPos + 1)%state._players.Count];
                state._players[(this.buttonPos + 1) % state._players.Count].CommitChips(state._bb);
            }

            foreach (Player player in state._players)
            {
                player.inHand = true;
                player.AddHoleCards(deck.Draw(), deck.Draw());
            }
            state.UpdateMaxCommitted();


            Play(state);
        }

        public void Play(ConcreteGameRoom state)
        {

            while (!state.AllDoneWithTurn())
            {
                state.NextToPlay().Play(state);


                state.UpdateGameState();
            }

            if (state.AllDoneWithTurn() || state.PlayersInHand() < 2)
                if (ProgressHand(state._handStep))
                { // progresses hand and returns whether hand is over (the last _handStep was river)
                    EndHand(state);
                    return;
                }
                else
                    Play(state);
            if (!state._isGameOver)
            {
                _currentPlayer = state.NextToPlay();
                _dealerPlayer = state._players[(state._players.IndexOf(_dealerPlayer) + 1) % state._players.Count];
                _sbPlayer = state._players[(state._players.IndexOf(_sbPlayer) + 1) % state._players.Count];
                _bbPlayer = state._players[(state._players.IndexOf(_bbPlayer) + 1) % state._players.Count];

            }

        }


        public bool ProgressHand(ConcreteGameRoom.HandStep previousStep)
        {
            List<Player> playersWhoWentAllIn = new List<Player>();
            foreach (Player player in state._players)
                if (player.IsAllIn() && player.chipsCommitted > 0)
                    playersWhoWentAllIn.Add(player);

            while (playersWhoWentAllIn.Count > 0)
            {
                int minAllIn = 10000000;
                Player minAllInPlayer = playersWhoWentAllIn[0];

                foreach (Player player in playersWhoWentAllIn) // find player who has the smallest all in
                {
                    if (player.chipsCommitted < minAllIn)
                    {
                        minAllInPlayer = player;
                        minAllIn = player.chipsCommitted;
                    }
                }
                if (minAllInPlayer != null)
                {
                    state.newSplitPot(minAllInPlayer);
                    playersWhoWentAllIn.Remove(minAllInPlayer);
                }

            }
            // moves chips to center, rests _actionPos, _maxCommitted = 0, resets last actions, resets last raise



            if (state.PlayersInHand() < 2)
                return true;

            switch (previousStep)
            {
                case ConcreteGameRoom.HandStep.PreFlop:
                    for (int i = 0; i <= 2; i++)
                        state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.Flop:
                    state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.Turn:
                    state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.River:
                    return true;

                default:
                    break;
            }


            int numNextStep = (int)previousStep + 1;
            state._handStep = (ConcreteGameRoom.HandStep)numNextStep;

            if (state.PlayersInHand() - state.PlayersAllIn() < 2)
            {
                ProgressHand(state._handStep); // recursive, runs until we'll hit the river
                return true;
            }
            else
                state.EndTurn(); 

            return false;

        }




        public void EndHand(ConcreteGameRoom state)
        {
            List<Player> playersLeftInGame = new List<Player>();
           

            foreach (Player player in state._players)
                if (player.chipCount != 0)
                    playersLeftInGame.Add(player);
                else
                {
                   // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player.inHand = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }
            state._players = playersLeftInGame;
            state.EndTurn();
            List<HandEvaluator> winners= FindWinner(state._publicCards, playersLeftInGame);
            //TODO: pay chips, give rank
            foreach (Player player in state._players)
                player.ClearCards(); // gets rid of cards of _players still alive

            if (state._players.Count > 1)
            {
                // sets next _buttonPos

                state._buttonPos++;
                state._buttonPos = state._buttonPos % state._players.Count;

                state.ClearPublicCards();
                NewHand(state);
            }
            else
            {
                state._isGameOver = true;
                if (!_currentPlayer.OutOfMoney())
                    state._players[0].inHand = false; // so if human wins doesn't try to display cards
            }


        }
        //TODO: Aviv G - it's all yours:)
        public List<HandEvaluator> FindWinner(List<Card> statePublicCards, List<Player> playersLeftInHand)
        {
            List<HandEvaluator> winners = new List<HandEvaluator>();
            foreach (Player p in playersLeftInHand)
            {
                HandEvaluator h = new HandEvaluator(p);
                List<Card> playerCards = statePublicCards;
                playerCards.AddRange(p.hand.GetCards());
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
                return winners;
            }
            return EvalTies(winners);
        }

        public List<HandEvaluator> EvalTies(List<HandEvaluator> winners)
        {
            List<Card> playerOneCards;
            List<Card> playerTwoCards;
            int i = 1;
            bool tie;
            while (winners.Count >= 2 && i < winners.Count() )
            {
                playerOneCards = winners.ElementAt(i-1)._relevantCards;
                playerTwoCards = winners.ElementAt(i)._relevantCards;
                playerOneCards.OrderBy(o => o._value).ToList();
                playerTwoCards.OrderBy(o => o._value).ToList();
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
            }
            return winners;
        }
    }
    }

