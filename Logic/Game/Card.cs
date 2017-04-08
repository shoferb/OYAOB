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
        private string _value { set; get; }
        private Suits _suit; 
      
        public Card(Suits s, string val)
        {
            this._suit = s;
            this._value = val;
        }
    }
}
