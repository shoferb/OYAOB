using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.DatabaseProxy
{
    class GameDataProxy
    {
        private GameDataControler _controller;
        private SystemControl _systemControl;
        private LogControl _logControl;
        private Logic.Replay.ReplayManager _replayManager;
        private GameCenter _gameCenter;
        private ServerToClientSender _sender;

        public GameDataProxy(SystemControl sysCon, LogControl lc, Logic.Replay.ReplayManager rm, GameCenter gc)
        {
            _controller = new GameDataControler();
            _systemControl = sysCon;
            _logControl = lc;
            _replayManager = rm;
            _gameCenter = gc;
           _sender = new ServerToClientSender(_gameCenter, _systemControl, _logControl, _replayManager);
        }
        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();
            List<Database.LinqToSql.GameRoom> dbGames= _controller.getAllGames();
            foreach(Database.LinqToSql.GameRoom g in dbGames)
            {
                List<Database.LinqToSql.Player> dbPlayers = _controller.GetPlayersOfRoom(g.room_Id);
                if(dbPlayers==null)
                {
                    return null;
                }
                List<Logic.Users.Player> playersLst = ConvertPlayerList(dbPlayers);


            Logic.Game.GameRoom toAdd = new Logic.Game.GameRoom(/*List < Player >*/ playersLst, g.room_Id, /*Decorator*/ decorator, _gameCenter, _logControl,
           _replayManager, _sender, g.game_id, g.is_Active_Game, g.Pot_count, g.Max_Bet_In_Round,
            /*List < Card >*/ pubCards, /*List < Spectetor >*/ specs, /*Player*/ dealerPlayer, /*LeagueName*/ leagueOf, g.last_rise_in_round)
                //deck
                //public cards 
            }

            return toRet;
        }

        private List<Logic.Users.Player> ConvertPlayerList(List<Database.LinqToSql.Player> dbPlayers)
        {
            List<Logic.Users.Player> toRet = new List<Logic.Users.Player>();
            foreach (Database.LinqToSql.Player dbPlayer in dbPlayers)
            {
                Logic.Users.Player toAdd = new Logic.Users.Player()
            }
            return toRet;
        }
    }
}
