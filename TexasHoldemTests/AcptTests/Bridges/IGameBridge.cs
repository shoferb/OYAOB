using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemTests.AcptTests.Bridges
{
    interface IGameBridge
    {
        bool CreateGame(int userId);
        bool CreateGame(int userId, int gameId);
        bool RemoveGame(int id);
        int GetNextFreeGameId();
        bool DoesGameExist(int id);
        bool IsUserInGame(int userId, int gameId);
        bool StartGame(int gameId);

    }
}
