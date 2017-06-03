using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.DatabaseProxy
{
    class GameDataProxy
    {
        private GameDataControler _controller;

        public GameDataProxy()
        {
            _controller = new GameDataControler();
        }
        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();


            return toRet;
        }

    }
}
