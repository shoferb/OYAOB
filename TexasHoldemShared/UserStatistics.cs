using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared
{
    public class UserStatistics
    {
        public double AvgCashGain;
        public double AvgGrossProfit;

        public UserStatistics(double avgCashGain, double avgGrossProfit)
        {
            AvgCashGain = avgCashGain;
            AvgGrossProfit = avgGrossProfit;
        }
    }
}
