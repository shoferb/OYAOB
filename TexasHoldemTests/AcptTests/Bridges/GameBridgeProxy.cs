using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class GameBridgeProxy : IGameBridge
    {
        public bool CreateGame()
        {
            throw new NotImplementedException();
        }

        public bool CreateGame(int id)
        {
            throw new NotImplementedException();
        }

        public int GetNextFreeId()
        {
            throw new NotImplementedException();
        }

        public bool RemoveGame(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesGameExist(int id)
        {
            throw new NotImplementedException();
        }
    }
}
