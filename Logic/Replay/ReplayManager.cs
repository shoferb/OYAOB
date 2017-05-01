using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Actions;

namespace TexasHoldem.Logic.Replay
{
    public class ReplayManager
    {
        public SortedDictionary<GameReplay, List<int>> _gamesActions { set; get; }

        public ReplayManager()
        {
            _gamesActions = new SortedDictionary<GameReplay, List<int>>();
        }


        public bool AddGameReplay(GameReplay gr, List<int> ids)
        {
            if (gr._gameNumber < 0 || gr._gameRoomID < 0 || IsExist(gr))
            {
                return false;
            }
            if (ids == null || ids.Count == 0)
            {
                return false;
            }
            _gamesActions.Add(gr, ids);
            return true;
        }

        public bool IsExist(GameReplay gr)
        {
            foreach (KeyValuePair<GameReplay, List<int>> entry in _gamesActions)
            {
                if (entry.Key.RightGame(gr._gameRoomID, gr._gameNumber))
                {
                    return true;
                }
            }
            return false;
        }

        public GameReplay GetGameReplay(int gameRoomID, int gameNumber) {

            foreach (GameReplay gr in _gamesActions)
            {
                if (gr.RightGame(gameRoomID, gameNumber))
                {
                    return gr;               
                }
            }
            return null;
        }

        public Action GetActionFromGameReplay(int gameRoomID, int gameNumber, int actionNumber)
        {
            GameReplay gr = GetGameReplay(gameRoomID, gameNumber);
            if (gr == null)
            {
                return null;
            }
            return gr.GetActionAt(actionNumber);
        }
    }
}
