using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.LinqToSql;


namespace TexasHoldem.Database.DataControlers
{
    class GameDataControler
    {

        public GameDataControler() { }

        public List<GameRoom> getAllGames()
        {
            List<GameRoom> toRet = new List<GameRoom>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetAllGames().ToList();
                    foreach (var v in temp)
                    {
                        GameRoom toAdd = ConvertToGameRoom(v);
                        LinqToSql.GameReplay toAddGameReplay = ConvertToReplay(db.GetGameReplayByRoomId(toAdd.room_Id));
                        toAdd = SetGameReplayToGame(toAdd, toAddGameReplay);
                        LinqToSql.LeagueName toAddLeagueName = ConvertToLeague(db.GetLeagueNameByVal(toAdd.league_name));
                        toAdd = SetLeagueToGame(toAdd, toAddLeagueName);
                        LinqToSql.HandStep toAddHandStep = ConvertToHandStep(db.GetHandStepNameByVal(toAdd.hand_step));
                        toAdd = SetHandStepToGame(toAdd , toAddHandStep);
                        //  toAdd = SetDeckToGame(toAdd); //TODO NOT HERE
                        // toAdd = SetPlayersToGame(toAdd); //TODO NOT HERE
                        // toAdd = SetPublicCardsToGame(toAdd); //TODO NOT HERE
                        //  toAdd = SetSpectetorsToGame(toAdd);  //TODO NOT HERE
                        toRet.Add(toAdd);
                    }
                    return toRet;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        private GameRoom SetHandStepToGame(GameRoom g, HandStep h)
        {
            g.HandStep.hand_Step_name = h.hand_Step_name;
            g.HandStep.hand_Step_value = h.hand_Step_value;
            return g;
        }

        private HandStep ConvertToHandStep(ISingleResult<GetHandStepNameByValResult> singleResult)
        {
            HandStep toAddHandStep = new HandStep();
            foreach (var v in singleResult)
            {
                toAddHandStep.hand_Step_name = v.hand_Step_name;
                toAddHandStep.hand_Step_value = v.hand_Step_value;
            }
            return toAddHandStep;
        }

        private GameRoom SetLeagueToGame(GameRoom g, LeagueName l)
        {
            g.LeagueName.League_Name = l.League_Name;
            g.LeagueName.League_Value = l.League_Value;
            return g;
        }

        private LeagueName ConvertToLeague(ISingleResult<GetLeagueNameByValResult> singleResult)
        {
            LeagueName toAddLeague = new LeagueName();
            foreach (var v in singleResult)
            {
                toAddLeague.League_Name = v.League_Name;
                toAddLeague.League_Value = v.League_Value;
            }
            return toAddLeague;
        }

        private GameRoom SetGameReplayToGame(GameRoom g, GameReplay gr)
        {
            g.GameReplay.room_Id = gr.room_Id;
            g.GameReplay.replay = gr.replay;
            g.GameReplay.index = gr.index;
            g.GameReplay.game_Id = gr.game_Id;
            return g;
        }

        private GameReplay ConvertToReplay(ISingleResult<GetGameReplayByRoomIdResult> singleResult)
        {
            GameReplay toAddRep = new GameReplay();
            foreach( var v in singleResult)
            {
                toAddRep.game_Id = v.game_Id;
                toAddRep.index = v.index;
                toAddRep.replay = v.replay;
                toAddRep.room_Id = v.room_Id;
            }
            return toAddRep;
        }

       

    /*    private LinqToSql.Deck ConvertToDeck(ISingleResult<GetDeckByRoomIdResult> singleResult)
        {
            LinqToSql.Deck deck = new LinqToSql.Deck();
            foreach (var v in singleResult)
            {
                deck.room_Id = v.room_Id;
                deck.index = v.index;
                deck.game_Id = v.game_Id;
                deck.
            }*/

    

        private GameRoom ConvertToGameRoom(GetAllGamesResult v)
        {
            GameRoom toRet = new GameRoom();
            toRet.Bb = v.Bb;
            toRet.Bb_Player = v.Bb_Player;
            toRet.curr_Player = v.curr_Player;
            toRet.curr_player_position = v.curr_player_position;
            toRet.Dealer_Player = v.Dealer_Player;
            toRet.Dealer_position = v.Dealer_position;
          
            toRet.First_Player_In_round = v.First_Player_In_round;
            toRet.first_player_in_round_position = v.first_player_in_round_position;
          
           
            toRet.game_id = v.game_id;
          
            toRet.hand_step = v.hand_step;
            toRet.is_Active_Game = v.is_Active_Game;
            toRet.last_rise_in_round = v.last_rise_in_round;
         
            toRet.league_name = v.league_name;
            toRet.Max_Bet_In_Round = v.Max_Bet_In_Round;
           
            toRet.Pot_count = v.Pot_count;
          
            toRet.room_Id = v.room_Id;
            toRet.Sb = v.Sb;
            toRet.SB_player = v.SB_player;
           

            return toRet;

        }
    }
}
