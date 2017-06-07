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

        public ErrorLog(string msg) : base()
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

        public string ToString()
        {
            string toReturn = base.ToString();
            toReturn = toReturn + " msg is: " + msg;
            return toReturn;
        }
    }
}
