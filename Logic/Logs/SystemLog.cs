using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class SystemLog : Log
    {
        private int roomId;
        private string msg;

        public SystemLog(int roomId, string ms) : base()
        {
            this.roomId = roomId;
            this.msg = ms;
        }

        //getter setter
        public int RoomId
        {
            get
            {
                return roomId;
            }

            set
            {
                roomId = value;
            }
        }

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
            toReturn = toReturn + " msg is: " + msg + "to room Id: " + roomId;
            return toReturn;
        }

       
    }
}
