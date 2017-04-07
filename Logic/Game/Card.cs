using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    class Card
    {

        private int _value { set; get; }
        private enum _cards 
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades,
        };

        public Card(int val)
        {
            this._value = val;
        }
    }
}
