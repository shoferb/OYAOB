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

        public Player _player { set; get; }
        public int _roomID { set; get; }
        public int _gameNumber { set; get; }

        public Action(Player player, int roomID, int gameNumber)
        {
            _player = player;
            _roomID = roomID;
            _gameNumber = gameNumber;
        }

        public abstract String DoAction();

    }
}
