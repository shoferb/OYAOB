using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;

namespace TexasHoldem.Logic
{
    public class BeforeGameDecorator : Decorator
    {
        public bool IsSpectetor { get; set; }
        public int MinPlayersInRoom { get; set; }
        public int MaxPlayersInRoom { get; set; }
        public int EnterPayingMoney { get; set; }
        public int StartingChip { get; set; }
        public int BB { get; set; }
        public int SB { get; set; }

        private Decorator NextDecorator;
        private GameCenter GameCenter;

        public BeforeGameDecorator(int minBetInRoom, int startingChip, bool isSpectetor,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney)
        {
            this.IsSpectetor = isSpectetor;
            this.StartingChip = startingChip;
            this.MaxPlayersInRoom = maxPlayersInRoom;
            this.MinPlayersInRoom = minPlayersInRoom;
            this.EnterPayingMoney = enterPayingMoney;
            this.BB = minBetInRoom;
            SB = BB / 2;
        }

        public void SetDecorator(Decorator d)
        {
            NextDecorator = d;
        }

        public bool CanSpectatble()
        {
            return IsSpectetor;
        }

        public  bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            return this.NextDecorator.CanRaise(currentPlayerBet, maxBetInRound, step);
        }

        public  bool CanJoin(int playersCount, int amount)
        {
            if (CanAddMorePlayer(playersCount) && amount >= StartingChip)
            {
                return true;
            }
            return false;
        }


        public bool IsGameModeEqual(GameMode gm)
        {
            return this.NextDecorator.IsGameModeEqual(gm);
        }

        public bool IsGameBuyInPolicyEqual(int buyIn)
        {
            return this.EnterPayingMoney == buyIn;
        }

        public bool IsGameMinPlayerEqual(int min)
        {
            return this.MinPlayersInRoom == min;
        }

        public bool IsGameMaxPlayerEqual(int max)
        {
            return this.MaxPlayersInRoom == max;
        }

        public bool IsGameMinBetEqual(int minBet)
        {
            return this.BB == minBet;
        }

        public bool IsGameStartingChipEqual(int startingChip)
        {
            return this.StartingChip == startingChip;
        }

        public bool CanUserJoinGameWithMoney(int userMoney)
        {
            return userMoney - (this.EnterPayingMoney + this.StartingChip) > 0;
        }

        public bool CanAddAnotherPlayer(int currNumOfPlayer)
        {
            return currNumOfPlayer >= this.MaxPlayersInRoom && currNumOfPlayer <= this.MaxPlayersInRoom;
        }

        public bool CanStartTheGame(int numOfPlayers)
        {
            return numOfPlayers >= this.MinPlayersInRoom ? true : false;
        }

        public int GetMinBetInRoom()
        {
            return this.BB;
        }

        public int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMaxAllowedRaise(maxCommited, step);
        }

        public int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMinAllowedRaise(maxCommited, step);
        }

        public int GetEnterPayingMoney()
        {
            return this.EnterPayingMoney;
        }

        public int GetStartingChip()
        {
            return this.StartingChip;
        }

        private bool CanAddMorePlayer(int currNumOfPlayers)
        {
            return currNumOfPlayers < MaxPlayersInRoom ? true : false;
        }



    }
}
