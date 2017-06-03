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

                List<Card> pubCards = new List<Card>();
                List<Database.LinqToSql.Card> dbPubCards = _controller.GetPublicCardsByRoomId(g.room_Id);
                foreach(var aCard in dbPubCards)
                {
                    pubCards.Add(getCardByVal(aCard.Card_Value));
                }
                List<Database.LinqToSql.SpectetorGamesOfUser> dbSpecs = _controller.GetSpectOfRoom(g.room_Id);
                if (dbSpecs == null)
                {
                    return null;
                }
                List<Logic.Users.Spectetor> SpecssLst = ConvertSpecsList(dbSpecs);
                Logic.Users.Player currentPlayer = playersLst.First();
                Logic.Users.Player bbPlayer = playersLst.First();
                Logic.Users.Player sbPlayer = playersLst.First();
                Logic.Users.Player firstPlayerInRound = playersLst.First();
                Logic.Users.Player dealerPlayer = playersLst.First();

                foreach (Logic.Users.Player p in playersLst)
                {
                    if(p.user.Id()==g.curr_Player)
                    {
                        currentPlayer = p;
                    }
                    if (p.user.Id() == g.Bb_Player)
                    {
                        bbPlayer = p;
                    }
                    if (p.user.Id() == g.SB_player)
                    {
                        sbPlayer = p;
                    }
                    if (p.user.Id() == g.First_Player_In_round)
                    {
                        firstPlayerInRound = p;
                    }
                    if (p.user.Id() == g.Dealer_Player)
                    {
                        dealerPlayer = p;
                    }
                }

                Logic.Game.GameRoom toAdd = new Logic.Game.GameRoom(playersLst, g.room_Id, /*Decorator*/ decorator, _gameCenter, _logControl,
               _replayManager, _sender, g.game_id, g.is_Active_Game, g.Pot_count, g.Max_Bet_In_Round,
                 pubCards, SpecssLst,  dealerPlayer, /*LeagueName*/ leagueOf, g.last_rise_in_round,
                /* Player*/ currentPlayer, /*Player*/ bbPlayer,/* Player*/ sbPlayer, /*Player*/ firstPlayerInRound, g.Bb ,g.Sb,
                g.Dealer_position, g.curr_player_position, g.first_player_in_round_position, /*GameReplay */gr, /*GameRoom.HandStep*/ hs, /*Deck*/ d);
            }

            return toRet;
        }

        private List<Logic.Users.Spectetor> ConvertSpecsList(List<Database.LinqToSql.SpectetorGamesOfUser> dbSpecs)
        {
            List<Logic.Users.Spectetor> toRet = new List<Logic.Users.Spectetor>();
            foreach (Database.LinqToSql.SpectetorGamesOfUser s in dbSpecs)
            {
                User user; //= UserDataProxy.GetUserById(dbPlayer.user_Id);
                Logic.Users.Spectetor toAdd = new Logic.Users.Spectetor(user, s.roomId);
            }
            return toRet;
        }

        private List<Logic.Users.Player> ConvertPlayerList(List<Database.LinqToSql.Player> dbPlayers)
        {
            List<Logic.Users.Player> toRet = new List<Logic.Users.Player>();
            foreach (Database.LinqToSql.Player dbPlayer in dbPlayers)
            {
                User user; //= UserDataProxy.GetUserById(dbPlayer.user_Id);
                Card fCard = getCardByVal(dbPlayer.first_card);
                Card sCard = getCardByVal(dbPlayer.secund_card);
                Logic.Users.Player toAdd = new Logic.Users.Player(/*IUser user*/ null, dbPlayer.Total_chip, dbPlayer.room_Id, dbPlayer.Round_chip_bet,
                    dbPlayer.is_player_active, fCard, sCard, dbPlayer.Player_action_the_round);
            }
            return toRet;
        }

        private Card getCardByVal(int val)
        {
            Database.LinqToSql.Card dbCard = _controller.getDBCardByVal(val);
            Suits s = new Suits();
            if(dbCard.Card_Shpe.Equals("Clubs"))
            {
                s = Suits.Clubs;
            }
            else if (dbCard.Card_Shpe.Equals("Diamonds"))
            {
                s = Suits.Diamonds;
            }
            else if (dbCard.Card_Shpe.Equals("Hearts"))
            {
                s = Suits.Hearts;
            }
            else if (dbCard.Card_Shpe.Equals("Spades"))
            {
                s = Suits.Spades;
            }
            else if (dbCard.Card_Shpe.Equals("None"))
            {
                s = Suits.None;
            }
            return new Card(s, dbCard.Card_Real_Value);
        }
    }
}
