using System;
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
        private LogControl _logControl;
        private ReplayManager _replayManager;
        private GameCenter _gameCencer;

        public GameDataProxy(SystemControl sysCon, LogControl lc, ReplayManager rm, GameCenter gc)
        {
            _controller = new GameDataControler();
            _systemControl = sysCon;
            _logControl = lc;
            _replayManager = rm;
            _gameCencer = gc;
        }
        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();
            List<Database.LinqToSql.GameRoom> dbGames= _controller.getAllGames();
            foreach(Database.LinqToSql.GameRoom g in dbGames)
            {
                Logic.Game.GameRoom toAdd = new Logic.Game.GameRoom(List < Player > players, int ID, Decorator decorator, GameCenter gc, LogControl log,
           ReplayManager replay, ServerToClientSender sender, int gameNum, bool isActiveGame, int potCount, int mmaxBetInRound,
            List < Card > pubCards, List < Spectetor > specs, Player dealerPlayer, LeagueName leagueOf, int lastRaiseInRoundd, bool isuseCommunication)
                //deck
                //public cards 
            }

            return toRet;
        }

    }
}
