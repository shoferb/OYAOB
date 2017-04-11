using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    class HandOfPoker
    {
        public bool _gameOver = false;

        public Player _currentPlayer;


        int buttonPos = 0;
        ConcreteGameRoom state;

        public HandOfPoker(ConcreteGameRoom state)
        {
            NewHand(state);
        }

        public void NewHand(ConcreteGameRoom state)
        {
            this.state = state;
            state.handStep = ConcreteGameRoom.HandStep.PreFlop;
            Deck deck = new Deck();
            state.deck = deck;
            state.players = state.players;
            this.buttonPos = state.buttonPos;

            if (state.players.Count > 2)
            {
                state.actionPos = (state.buttonPos + 3) % state.players.Count;
                // small blind
                state.players[(buttonPos + 1) % state.players.Count].CommitChips(state.bb / 2);
                // big blind
                state.players[(buttonPos + 2) % state.players.Count].CommitChips(state.bb);
            } 
            else
            {
                state.actionPos = (state.buttonPos) % state.players.Count;
                // small blind
                state.players[(state.buttonPos) % state.players.Count].CommitChips(state.bb / 2);
                // big blind
                state.players[(state.buttonPos + 1) % state.players.Count].CommitChips(state.bb);
            }

            foreach (Player player in state.players)
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
                if (ProgressHand(state.handStep))
                { // progresses hand and returns whether hand is over (the last handStep was river)
                    EndHand(state);
                    return;
                }
                else
                    Play(state);
            if (!state.gameOver)
                    _currentPlayer = state.NextToPlay();


        }


        public bool ProgressHand(ConcreteGameRoom.HandStep previousStep)
        {
            List<Player> playersWhoWentAllIn = new List<Player>();
            foreach (Player player in state.players)
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
            // moves chips to center, rests actionPos, maxCommitted = 0, resets last actions, resets last raise



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
            state.handStep = (ConcreteGameRoom.HandStep)numNextStep;

            if (state.PlayersInHand() - state.PlayersAllIn() < 2)
            {
                ProgressHand(state.handStep); // recursive, runs until you hit the river, which will immediately return true
                return true;
            }
            else
                state.EndTurn(); // need to be able to tell who went all in during a particular phase, so do this later in this case

            return false;

        }




        public void EndHand(ConcreteGameRoom state)
        {
            MessageBox.Show("Hand over.");


            List<Player> playersLeftInGame = new List<Player>();
            List<Player> playersLeftInHand = new List<Player>();
            foreach (Player player in state.players)
                if (player.inHand && player.chipsCommitted > 0)  // player.chipsCommitted to exclude people who went allin earlier
                    playersLeftInHand.Add(player);

            state.EndTurn();
            DecideWinnersAndPayChips(new Tuple<int, List<Player>>(state.potCount, playersLeftInHand));

            foreach (Tuple<int, List<Player>> sidePot in state.sidePots)
                DecideWinnersAndPayChips(sidePot);



            foreach (Player player in state.players)
                if (player.chipCount != 0)
                    playersLeftInGame.Add(player);
                else
                {
                   // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player.inHand = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }
            state.players = playersLeftInGame;

            foreach (Player player in state.players)
                player.ClearCards(); // gets rid of cards of players still alive



            if (state.players.Count > 1)
            {
                // sets next buttonPos

                state.buttonPos++;
                state.buttonPos = state.buttonPos % state.players.Count;

                state.ClearPublicCards();
                NewHand(state);
            }
            else
            {
                state.gameOver = true;
                if (!_currentPlayer.OutOfMoney())
                    state.players[0].inHand = false; // so if human wins doesn't try to display cards
                MessageBox.Show("GAME OVER.");
            }


        }

        public void DecideWinnersAndPayChips(Tuple<int, List<Player>> pot)
        {
            /*
                string message = "";
                if (winners.Count < 2)
                {
                    message = "Player " + winners. + " won the pot of " + pot.Item1 + " chips.";
                    if (pot.Item2.Count > 1)
                    { message += " He won with a hand of: " + winner.hand.ToString(); }
                }
                else
                {
                    message = "A split pot between: ";
                    message += "Player " + winners[0].name + " who had a hand of " + winners[0].hand.ToString();
                    for (int i = 1; i < winners.Count; i++)
                        message += ", Player " + winners[i].name + " who had a hand of " + winners[i].hand.ToString();
                    message += ". They split a pot of " + pot.Item1 + " " + winners.Count + " ways, for " + pot.Item1 / winners.Count + " each.";

                }


    */

             
            }
        }
    }

