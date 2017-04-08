using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game_Control
{
    public class League
    {
        private String name;
        private int minRank;
        private int maxRank;


        //getter setter name
        public String getName()
        {
            return name;
        }
        public void setName(String newName)
        {
            name = newName;
        }
        //getter setter minRank
        public int getMinRank()
        {
            return minRank;
        }
        public void setMinRank(int newMinRank)
        {
            minRank = newMinRank;
        }
        //getter setter maxRanl
        public int getMaxRank()
        {
            return maxRank;
        }
        public void setMaxRank(int newMaxRank)
        {
            maxRank = newMaxRank;
        }

    }
}
