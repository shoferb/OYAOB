using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem
{
    class ConcreteGameRoom : GameRoom
    {
        public ConcreteGameRoom(string name, int sb, int bb, int minMoney, int maxMoney) : base(name, sb, bb, minMoney, maxMoney)
        {
        }


    }
}
