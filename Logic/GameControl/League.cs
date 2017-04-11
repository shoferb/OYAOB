using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game_Control
{
    public class League
    {
        private string name;
        private int minRank;
        private int maxRank;


        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int MinRank
        {
            get
            {
                return minRank;
            }

            set
            {
                minRank = value;
            }
        }

        public int MaxRank
        {
            get
            {
                return maxRank;
            }

            set
            {
                maxRank = value;
            }
        }
    }
}
