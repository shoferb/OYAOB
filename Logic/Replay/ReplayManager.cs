using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Replay
{
    public class ReplayManager
    {
        public List<GameReplay> _gamesActions { set; get; }

        public ReplayManager()
        {
            _gamesActions = new List<GameReplay>();
        }


        public bool AddGameReplay(GameReplay gr)
        {
            if (gr._gameNumber < 0 || gr._gameRoomID < 0 || IsExist(gr))
            {
                return false;
            }
            _gamesActions.Add(gr);
            return true;
        }

        public bool IsExist(GameReplay gr)
        {
            foreach (GameReplay g in _gamesActions)
            {
                if (g.RightGame(gr._gameRoomID, gr._gameNumber))
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
    }
}
