using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Notifications_And_Logs
{
    public class SystemLog : Log
    {
        private int _roomId;
        private string _msg;
        private int _gameId;

        public SystemLog(int roomId, string ms, int gameId) : base()
        {
            _roomId = roomId;
            _msg = ms;
            _gameId = gameId;
        }

        //getter setter
        public int RoomId
        {
            get
            {
                return _roomId;
            }

            set
            {
                _roomId = value;
            }
        }

        public int GameId
        {
            get
            {
                return _gameId;
            }

            set
            {
                _gameId = value;
            }
        }
        public string Msg
        {
            get
            {
                return _msg;
            }

            set
            {
                _msg = value;
            }
        }

        public string ToString()
        {
            string toReturn = base.ToString();
            toReturn = toReturn + " msg is: " + _msg + "to room Id: " + _roomId;
            return toReturn;
        }

       
    }
}
