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

        public LinqToSql.GameRoomPreferance GetPrefByRoomId(int roomId)
        {
            LinqToSql.GameRoomPreferance toRet = new GameRoomPreferance();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGamePreferencesByRoomId(roomId).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Game_Id = v.Game_Id;
                        toRet.Game_Mode = v.Game_Mode;
                        toRet.is_Spectetor = v.is_Spectetor;
                        toRet.League_name = v.League_name;
                        toRet.max_player_in_room = v.max_player_in_room;
                        toRet.Min_player_in_room = v.Min_player_in_room;
                        toRet.room_id = v.room_id;
                        toRet.Sb = v.Sb;
                        toRet.starting_chip = v.starting_chip;
                        toRet.enter_paying_money = v.enter_paying_money;
                        toRet.Bb = v.Bb;
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool InsertGameRoom(GameRoom toIns)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertGameRoomToDb(toIns.Bb, toIns.Bb_Player, toIns.curr_Player, toIns.curr_player_position,
                        toIns.Dealer_Player, toIns.Dealer_position, toIns.First_Player_In_round, toIns.first_player_in_round_position,
                        toIns.game_id, toIns.hand_step, toIns.is_Active_Game, toIns.last_rise_in_round, toIns.league_name,
                        toIns.Max_Bet_In_Round, toIns.Pot_count, toIns.room_Id, toIns.Sb, toIns.SB_player);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertPref(GameRoomPreferance toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertPrefToDb(toAdd.room_id, toAdd.is_Spectetor, toAdd.Min_player_in_room,
                        toAdd.max_player_in_room, toAdd.enter_paying_money, toAdd.starting_chip,
                        toAdd.Bb, toAdd.Sb, toAdd.League_name, toAdd.Game_Mode, toAdd.Game_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public int GetGameModeValByName(string v)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameModeValByName(v);
                    return temp.First().Game_mode_value;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public GameMode GetGameModeByVal(int val)
        {
            GameMode toRet = new GameMode();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameModeNameByVal(val).ToList();
                    toRet.game_mode_name = temp.First().game_mode_name;
                    return toRet;
                }                
            }
            catch (Exception e)
            {
                return null;
            }
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

        public int GetLeagueValByName(string name)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetLeageValByName(name);
                    return temp.First().League_Value;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
