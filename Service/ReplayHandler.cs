using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;

namespace TexasHoldem.Service
{
    public class ReplayHandler
    {
        private ReplayManager _RM = ReplayManager.ReplayManagerInstance;

        public GameReplay GetGameReplayForUser(int gameRoomID, int gameNumber, int userID)
        {
            return _RM.GetGameReplayForUser(gameRoomID, gameNumber, userID);
        }

        public string ShowGameReplay(int gameRoomID, int gameNumber, int userID)
        {
            return _RM.ShowGameReplay(gameRoomID, gameNumber, userID);
        }

        public void DeleteGameReplay(int gameRoomID, int gameNumber)
        {
            _RM.DeleteGameReplay(gameRoomID, gameNumber);
        }
        public Tuple<int, string> ShowFirstGameReplay(int userID, int roomID)
        {
            string replay = _RM.ShowFirstGameReplayForUser(userID, roomID);
            Tuple<int,string> tup = new Tuple(roomID, replay);
            return tup;
        }

    }
}
