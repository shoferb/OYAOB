using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem
{
    public class Card
    {
        public int _value { get; set; }
        public Suits _suit { get; set; }
      
        public Card(Suits s, int val)
        {
            this._suit = s;
            this._value = val;
        }

        public override string ToString()
        {
            return _value + " of " + _suit.ToString();
        }

    }
}
