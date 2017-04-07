using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.User;

namespace TexasHoldem.Logic.Actions
{
    public abstract class Action
    {

        private Player _player;
        private int _roomID;
        private int _gameNumber;

        public Action(Player player, int roomID, int gameNumber)
        {
            _player = player;
            _roomID = roomID;
            _gameNumber = gameNumber;
        }

        public Player Player { get; set; }
        public int RoomID { get; set; }
        public int GameNumber { get; set; }

        public abstract String DoAction();

    }
}
