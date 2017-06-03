﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;

namespace TexasHoldem.DatabaseProxy
{
    class GameDataProxy
    {
        private GameDataControler _controller;
        private SystemControl _systemControl;
        private LogControl logControl;
        private ReplayManager replayManager;

        public GameDataProxy()
        {
            _controller = new GameDataControler();
        }
        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();
            List<Database.LinqToSql.GameRoom> dbGames= _controller.getAllGames();
            foreach(Database.LinqToSql.GameRoom g in dbGames)
            {
                Logic.Game.GameRoom toAdd = new Logic.Game.GameRoom(_players,g.room_Id,_decorator,gameCenter,LogDataControler,repManager,sender)
                //deck
                //public cards 
            }

            return toRet;
        }

    }
}
