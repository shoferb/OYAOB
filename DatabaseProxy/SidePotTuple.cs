using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.DatabaseProxy
{
    public class SidePotTuple
    {
        public int item1;
        public PlayerXML item2;

        public SidePotTuple() { }

        public SidePotTuple(int a , PlayerXML b)
        {
            item1 = a;
            item2 = b;
        }
    }
}
