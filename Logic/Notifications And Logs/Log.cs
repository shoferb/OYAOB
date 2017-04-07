using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class Log
    {

        private int lodId;

        public Log(int lodId)
        {
            this.lodId = lodId;
        }


        //getter Setter
        public int LodId
        {
            get
            {
                return lodId;
            }

            set
            {
                lodId = value;
            }
        }
    }
}
