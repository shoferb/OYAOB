using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class Notification
    {
        private int roomId;
        private String msg;

        public Notification(int roomId, string msg)
        {
            this.roomId = roomId;
            this.msg = msg;
        }


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
            string toReturn = "this is a notification with  massage: " + msg + "to room Id: " + roomId;
            return toReturn;
        }
    }
}
