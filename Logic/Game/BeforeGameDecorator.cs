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
      
        private GameCenter GameCenter;

        public BeforeGameDecorator( int minBetInRoom, int startingChip, bool isSpectetor,
             int minPlayersInRoom, int maxPlayersInRoom,
            int enterPayingMoney, Decorator d) : base(d)
        {
            this.IsSpectetor = isSpectetor;
            this.StartingChip = startingChip;
            this.MaxPlayersInRoom = maxPlayersInRoom;
            this.MinPlayersInRoom = minPlayersInRoom;
            this.EnterPayingMoney = enterPayingMoney;
            this.MinBetInRoom = minBetInRoom;
            
        }

        public override bool CanBeSpectatble()
        {
            return IsSpectetor;
        }

        public override bool CanStartTheGame(int numOfPlayers)
        {
            return numOfPlayers >= this.MinPlayersInRoom ?  true : false;
        }

        public override bool CanRaise()
        {
            return NextDecorator.CanRaise();
        }

        public override bool CanCheck()
        {
            return NextDecorator.CanCheck();
        }

        public override bool CanFold()
        {
            return NextDecorator.CanFold();
        }

        public override bool CanSpectatble()
        {
            throw new NotImplementedException();
        }

        public override int GetMinBetInRoom()
        {
            return this.MinBetInRoom;
        }

        public override int GetMaxAllowedRaise(int BB, int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMaxAllowedRaise(BB, maxCommited, step);
        }

        public override int GetMinAllowedRaise(int BB, int maxCommited, GameRoom.HandStep step)
        {
            return NextDecorator.GetMinAllowedRaise(BB, maxCommited, step);
        }

         public override int GetEnterPayingMoney()
        {
            return this.EnterPayingMoney;
        }

        public int GetStartingChip()
        {
            return this.StartingChip;
        }
        
        public bool CanAddMorePlayer(int currNumOfPlayers)
        {
            return currNumOfPlayers < MaxPlayersInRoom ?   true :  false;
        }

       
    }
}
