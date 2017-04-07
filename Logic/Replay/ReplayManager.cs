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

        public List<GameReplay> GamesActions { get => _gamesActions; set => _gamesActions = value; }

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
