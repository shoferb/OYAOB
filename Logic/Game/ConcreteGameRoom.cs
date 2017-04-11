 using System;
using System.Collections.Generic;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGameRoom 
    {
        private int _id { get; set; } //base has ID allready
        public enum HandStep { PreFlop, Flop, Turn, River }
        public List<Player> players { get; set; }
        public int buttonPos { get; set; }
        public int maxCommitted { get; set; }
        public int actionPos { get; set; }
        public int potCount { get; set; }
        public int bb { get; set; }
        public int lastRaise { get; set; } // in order to keep track of the minimum next raise
        public Deck deck { get; set; }
        public HandStep handStep { get; set; }
        public List<Card> publicCards { get; set; }
        public bool gameOver { get; set; }
        public List<Tuple<int, List<Player>>> sidePots { get; set; }
        public ConcreteGameRoom(List<Player> players, int buttonPos)
        {
            //TODO : generate random this._id ;
            gameOver = false;
            this.potCount = 0;
            this.actionPos = (buttonPos + 3) % players.Count;
            this.players = players;
            this.buttonPos = buttonPos;
            this.maxCommitted = 0;
            publicCards = new List<Card>();
            lastRaise = 0;
            sidePots = new List<Tuple<int, List<Player>>>();
        }

        public void AddNewPublicCard()
        {
            foreach (Player player in players)
                player.AddCard(deck.ShowCard());
            publicCards.Add(deck.Draw());
        }

        public Player NextToPlay()
        {
            return players[actionPos];
        }
        public int ToCall()
        {
            return maxCommitted - players[actionPos].chipsCommitted;

        }
        public void UpdateGameState()
        {
            // next player picked

            do { actionPos = (actionPos + 1) % players.Count; }
            while (!players[actionPos].inHand);

            UpdateMaxCommitted();



        }

        public void ClearPublicCards()
        {
            publicCards.Clear();
        }

        public void UpdateMaxCommitted()
        {
            foreach (Player player in players)
                if (player.chipsCommitted > maxCommitted)
                    maxCommitted = player.chipsCommitted;
        }
        public void EndTurn()
        {
            MoveChipsToPot();
            ResetActionPos();

            lastRaise = 0;
            maxCommitted = 0;

            foreach (Player player in players)
                if (player.inHand)
                    player.lastAction = "";


        }
        public void ResetActionPos()
        {
            int offset = 1;
            if (handStep == HandStep.River)
                offset = 3;

            actionPos = (buttonPos + offset) % players.Count;
            while (!players[actionPos].inHand)
                actionPos = (actionPos + 1) % players.Count;




        }
        public void MoveChipsToPot()
        {
            foreach (Player player in players)
            {
                potCount += player.chipsCommitted;
                player.chipsCommitted = 0;
            }
        }
        public int PlayersInHand()
        {
            int playersInHand = 0;
            foreach (Player player in players)
                if (player.inHand)
                    playersInHand++;
            return playersInHand;

        }

        public int PlayersAllIn()
        {
            int playersAllIn = 0;
            foreach (Player player in players)
                if (player.IsAllIn())
                    playersAllIn++;
            return playersAllIn;
        }

        public bool AllDoneWithTurn()
        {
            bool allDone = true;
            foreach (Player player in players)
                if (!(player.inHand == false || player.IsAllIn() || (player.lastAction == "call" || player.lastAction == "check" || player.lastAction == "bet" || player.lastAction == "raise") && player.chipsCommitted == maxCommitted))
                    allDone = false;
            return allDone;



        }

        public void newSplitPot(Player allInPlayer)
        {
            List<Player> eligiblePlayers = new List<Player>();
            int sidePotCount = 0;
            int chipsToMatch = allInPlayer.chipsCommitted;
            foreach (Player player in players)
            {
                if (player.inHand && player.chipsCommitted > 0)
                {
                    player.chipsCommitted -= chipsToMatch;
                    sidePotCount += chipsToMatch;
                    eligiblePlayers.Add(player);
                }
            }
            sidePotCount += potCount;
            potCount = 0;

            if (sidePotCount > 0)
                sidePots.Add(new Tuple<int, List<Player>>(sidePotCount, eligiblePlayers));


        }



    }
}
