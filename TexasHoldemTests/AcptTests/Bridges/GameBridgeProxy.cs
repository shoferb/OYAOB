using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class GameBridgeProxy : IGameBridge
    {
        public bool CreateGame(int userId, int gameId)
        {
            return true;
        }

        public bool CreateGame(int id)
        {
            return true;
        }

        public int GetNextFreeGameId()
        {
            return 1;
        }

        public bool RemoveGame(int id)
        {
            return true;
        }

        public bool DoesGameExist(int id)
        {
            return true;
        }

        public bool IsUserInGame(int userId, int gameId)
        {
            return true;
        }
    }
}
