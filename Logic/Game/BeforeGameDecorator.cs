using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic
{
    class BeforeGameDecorator : Decorator
    {
        public bool IsSpectetor { get; set; }
        public int MinPlayersInRoom { get; set; }
        public int MaxPlayersInRoom { get; set; }
        public int EnterPayingMoney { get; set; }
        public int StartingChip { get; set; }
        public BeforeGameDecorator(int startingChip, bool isSpectetor,
             int minPlayersInRoom, int maxPlayersInRoom,
            int enterPayingMoney, Decorator d) : base(d)
        {
            this.IsSpectetor = isSpectetor;
            this.StartingChip = startingChip;
            this.MaxPlayersInRoom = maxPlayersInRoom;
            this.MinPlayersInRoom = minPlayersInRoom;
            this.EnterPayingMoney = enterPayingMoney;
        }

        public bool CanBeSpectatble()
        {
            return IsSpectetor;
        }

        public bool CanStartTheGame()
        {
            return false;
        }

        public bool CanRaise()
        {
            NextDecorator.CanRaise();
        }

        public void Check()
        {
            NextDecorator.Check();
        }

        public void Fold()
        {
            NextDecorator.Fold();
        }

        public int GetMinPlayersInRoom()
        {
            return 0;
        }
        public GameMode? GetGameMode()
        {
            NextDecorator.GetGameMode();
        }
    }
}
