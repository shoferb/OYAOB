using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class SystemLog : Log
    {
        private String msg;

        public SystemLog(int lodId,string msg) : base()
        {
            this.msg = msg;
        }


        //getter setter
        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }
    }
}
