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

        public Player Player { get => _player; set => _player = value; }
        public int RoomID { get => _roomID; set => _roomID = value; }
        public int GameNumber { get => _gameNumber; set => _gameNumber = value; }

        public abstract String DoAction();

    }
}
