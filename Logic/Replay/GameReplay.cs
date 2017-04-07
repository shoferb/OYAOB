using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexasHoldem.Logic.Actions;
namespace TexasHoldem.Logic.Replay
{
    public class GameReplay
    {
        public int _gameRoomID { get; set; }
        public int _gameNumber { get; set; }
        public List<Action> _actions { get; set; }

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
