using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.GameControl;

namespace TexasHoldem.Logic.Game_Control
{
    public class League
    {
        private LeagueName name;
        
        public League(LeagueName _name)
        {
            this.name = _name;
            
        }


        //getter setter name
        public LeagueName getName()
        {
            return name;
        }
        public void setName(LeagueName newName)
        {
            name = newName;
        }
       

    }
}
