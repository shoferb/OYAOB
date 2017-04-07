using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Replay
{
    public class ReplayManager
    {
        private List<GameReplay> _gamesActions;

        public ReplayManager()
        {
            _gamesActions = new List<GameReplay>();
        }

        public List<GameReplay> GamesActions { get => _gamesActions; set => _gamesActions = value; }

        public bool AddGameReplay(GameReplay gr)
        {
            if (gr.GameNumber < 0 || gr.GameRoomID < 0 || isExist(gr))
            {
                return false;
            }
            _gamesActions.Add(gr);
            return true;
        }

        private bool isExist(GameReplay gr)
        {
            foreach (GameReplay g in _gamesActions)
            {
                if (g.RightGame(gr.GameRoomID, gr.GameNumber))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ReplayGame(int gameRoomID, int gameNumber) {

            foreach (GameReplay gr in _gamesActions)
            {
                if (gr.RightGame(gameRoomID, gameNumber))
                {
                    return gr.ReplayGame();               
                }
            }
            return false;
        }
    }
}
