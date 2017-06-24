using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Service
{
    public class ReplayHandler
    {
        private ReplayManager _RM ;

        public ReplayHandler(ReplayManager replays)
        {
            _RM = replays;
        }

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


        public Tuple<bool, string> ShowFirstGameReplay( int roomID, int userId)
        {
           return _RM.GetGameReplayForUserSearch(roomID,userId );
            
        }

    }
}
