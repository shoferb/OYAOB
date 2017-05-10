using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;

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
        public LeagueName league { get; set; }
        private Decorator NextDecorator;

        public BeforeGameDecorator(int minBetInRoom, int startingChip, bool isSpectetor,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, LeagueName league)
        {
            this.IsSpectetor = isSpectetor;
            this.StartingChip = startingChip;
            this.MaxPlayersInRoom = maxPlayersInRoom;
            this.MinPlayersInRoom = minPlayersInRoom;
            this.EnterPayingMoney = enterPayingMoney;
            this.BB = minBetInRoom;
            SB = BB / 2;
            this.league = league;
        }

        public void SetNextDecorator(Decorator d)
        {
            NextDecorator = d;
        }

        public bool CanSpectatble()
        {
            return IsSpectetor;
        }

        public  bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            return NextDecorator.CanRaise(currentPlayerBet, maxBetInRound, step);
        }

        public  bool CanJoin(int playersCount, int amount, IUser user)
        {
            if (CanAddMorePlayer(playersCount) && amount >= StartingChip 
                && HasEnoughMoney(user, amount) && SuitableLeague(user))
            {
                return true;
            }
            return false;
        }


        public bool IsGameModeEqual(GameMode gm)
        {
            return NextDecorator.IsGameModeEqual(gm);
        }

        public bool IsGameBuyInPolicyEqual(int buyIn)
        {
            return EnterPayingMoney == buyIn;
        }

        public bool IsGameMinPlayerEqual(int min)
        {
            return MinPlayersInRoom == min;
        }

        public bool IsGameMaxPlayerEqual(int max)
        {
            return MaxPlayersInRoom == max;
        }

        public bool IsGameMinBetEqual(int minBet)
        {
            return BB == minBet;
        }

        public bool IsGameStartingChipEqual(int startingChip)
        {
            return StartingChip == startingChip;
        }

        public int GetMinPlayerInRoom()
        {
            return this.MinPlayersInRoom;
        }

        public int GetMaxPlayerInRoom()
        {
            return  this.MaxPlayersInRoom;
        }

        public GameMode GetGameMode()
        {
            return this.NextDecorator.GetGameMode();
        }

        public bool CanStartTheGame(int numOfPlayers)
        {
            return numOfPlayers >= this.MinPlayersInRoom ? true : false;
        }

        public int GetMinBetInRoom()
        {
            return BB;
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
            return EnterPayingMoney;
        }

        public int GetStartingChip()
        {
            return StartingChip;
        }

        private bool CanAddMorePlayer(int currNumOfPlayers)
        {
            return currNumOfPlayers < MaxPlayersInRoom ? true : false;
        }

        private bool SuitableLeague(IUser user)
        {
            return (user.IsUnKnow() || user.GetLeague() == league);
        }

        private bool HasEnoughMoney(IUser user, int amount)
        {
            return user.HasEnoughMoney(amount, EnterPayingMoney);
        }

        private bool CanAddAnotherPlayer(int currNumOfPlayer)
        {
            return currNumOfPlayer >= this.MaxPlayersInRoom && currNumOfPlayer <= this.MaxPlayersInRoom;
        }

    }
}
