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
        private int _index;
        public List<Action> _actions { get; set; }

        public GameReplay()
        {
            _actions = new List<Action>();
            _index = 0;
        }

        public GameReplay(int gameRoomID, int gameNumber)
        {
            _index = 0;
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

        public Action GetNextAction() //return current action or null if done
        {
            if (_actions == null || _actions.Count == 0 || _index >= _actions.Count)
            {
                return null;
            }
            _index++;
            return _actions.ElementAt(_index -1);
        }

        public void StartOver()
        {
            _index = 0;
        }

        public string ToString()
        {
            string gameReplay = "";
            Action action = GetNextAction();
            while (action != null)
            {
                gameReplay += action.ToString();
            }
            return gameReplay;
        }

    }
}
