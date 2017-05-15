using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class GameReplayClientReview
    {
        public int gameId;
        public string replay;

        public GameReplayClientReview(int _gameId, string _replay)
        {
            this.gameId = _gameId;
            this.replay = _replay;
        }
    }
}
