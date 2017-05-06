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
        public int MinBetInRoom { get; set; }
        public int BB { get; set; }
        public int SB { get; set; }

        private GameCenter GameCenter;

        public BeforeGameDecorator( int minBetInRoom, int startingChip, bool isSpectetor,
             int minPlayersInRoom, int maxPlayersInRoom,
            int enterPayingMoney, int bb, int sb, Decorator d) : base(d)
        {
            this.IsSpectetor = isSpectetor;
            this.StartingChip = startingChip;
            this.MaxPlayersInRoom = maxPlayersInRoom;
            this.MinPlayersInRoom = minPlayersInRoom;
            this.EnterPayingMoney = enterPayingMoney;
            this.MinBetInRoom = minBetInRoom;
            this.BB = bb;
            this.SB = sb;

        }

        public override bool CanSpectatble()
        {
            return IsSpectetor;
        }

        public override bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            return this.NextDecorator.CanRaise(currentPlayerBet, maxBetInRound, step);
        }

        public override bool CanJoin(int playersCount, int amount)
        {
            if (CanAddMorePlayer(playersCount) && amount >= StartingChip)
                return true;
            return false;
        }


        public override bool IsGameModeEqual(GameMode gm)
        {
            return this.NextDecorator.IsGameModeEqual(gm);
        }

        public override bool IsGameBuyInPolicyEqual(int buyIn)
        {
            return this.EnterPayingMoney == buyIn;
        }

        public override bool IsGameMinPlayerEqual(int min)
        {
            return this.MinPlayersInRoom == min;
        }

        public override bool IsGameMaxPlayerEqual(int max)
        {
            return  this.MaxPlayersInRoom == max;
        }

        public override bool IsGameMinBetEqual(int minBet)
        {
            return this.MinBetInRoom == minBet;
        }

        public override bool IsGameStartingChipEqual(int startingChip)
        {
            throw new NotImplementedException();
        }

        public override bool CanUserJoinGame(int userMoney, int userPoints, bool isUnKnow)
        {
            throw new NotImplementedException();
        }

        public override bool CanStartTheGame(int numOfPlayers)
        {
            return numOfPlayers >= this.MinPlayersInRoom ?  true : false;
        }

        public override int GetMinBetInRoom()
        {
            return this.MinBetInRoom;
        }

        public override int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMaxAllowedRaise(maxCommited, step);
        }

        public override int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMinAllowedRaise(maxCommited, step);
        }

         public override int GetEnterPayingMoney()
        {
            return this.EnterPayingMoney;
        }

        public override int GetStartingChip()
        {
            return this.StartingChip;
        }
        
        private bool CanAddMorePlayer(int currNumOfPlayers)
        {
            return currNumOfPlayers < MaxPlayersInRoom ?   true :  false;
        }

       
    }
}
