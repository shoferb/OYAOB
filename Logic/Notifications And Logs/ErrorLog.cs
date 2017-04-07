using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class ErrorLog : Log
    {
        private String msg;

        public ErrorLog(int lodId,string msg) : base(lodId)
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
