using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Replay
{
    public class ReplayManager
    {
        private List<GameReplay> gamesActions;

        public List<GameReplay> GamesActions { get => gamesActions; set => gamesActions = value; }

        public bool ReplayGame(int gameRoomID, int GameNumber) { return false; }
    }
}
