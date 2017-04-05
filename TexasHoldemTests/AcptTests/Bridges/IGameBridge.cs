using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemTests.AcptTests.Bridges
{
    interface IGameBridge
    {
        bool CreateGame();
        bool CreateGame(int id);
        bool RemoveGame(int id);
        int GetNextFreeId();
        bool DoesGameExist(int id);
    }
}
