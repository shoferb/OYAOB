using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Replay
{
    public class GameReplay
    {
        private int _GameRoomID;
        private int _GameNumber;
        private List<Action> _actions;

        public int GameRoomID { get => _GameRoomID; set => _GameRoomID = value; }
        public int GameNumber { get => _GameNumber; set => _GameNumber = value; }
        public List<Action> Actions { get => _actions; set => _actions = value; }

        public bool ReplayGame() { return false; }
    }
}
