using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexasHoldem.Logic.Actions;
namespace TexasHoldem.Logic.Replay
{
    public class GameReplay
    {
        private int _gameRoomID;
        private int _gameNumber;
        private List<Action> _actions;

        public GameReplay()
        {
            _actions = new List<Action>();
        }

        public GameReplay(int gameRoomID, int gameNumber)
        {
            _gameRoomID = gameRoomID;
            _gameNumber = gameNumber;
            _actions = new List<Action>();
        }

        public void AddAction(Action action)
        {
            _actions.Add(action);
        }

        public int GameRoomID { get => _gameRoomID; set => _gameRoomID = value; }
        public int GameNumber { get => _gameNumber; set => _gameNumber = value; }
        public List<Action> Actions { get => _actions; set => _actions = value; }

        public bool RightGame(int gameRoomID, int gameNumber)
        {
            return (gameRoomID == _gameRoomID && gameNumber == _gameNumber);
        }

        public bool ReplayGame()
        {
            if (_actions == null || _actions.Count == 0)
            {
                return false;
            }
            foreach (Action a in _actions)
            {
                a.DoAction();
            }
            return true;
        }
    }
}
