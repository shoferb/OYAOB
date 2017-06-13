﻿using System;
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

        public bool InsertSpec(SpectetorGamesOfUser toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertSpecToDb(toAdd.userId, toAdd.roomId,toAdd.Game_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertPublicCard(Public_Card toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InserPubCardToDb(toAdd.room_Id, toAdd.card, toAdd.Game_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertPlayer(Player toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertPlayerToDb(toAdd.room_Id, toAdd.user_Id, toAdd.is_player_active,
                        toAdd.player_name, toAdd.Total_chip, toAdd.Round_chip_bet, toAdd.Player_action_the_round,
                        toAdd.first_card, toAdd.secund_card, toAdd.Game_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<LinqToSql.Card> GetPublicCardsByRoomId(int roomId)
        {
            List<LinqToSql.Card> toRet = new List<LinqToSql.Card>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetPublicCardsByRoomId(roomId).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(getDBCardByVal(v.card));
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool InsertGameReply(GameReplay toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertGameReplayToDb(toAdd.replay, toAdd.game_Id, toAdd.room_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertDeck(LinqToSql.Deck toAdd)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertDeckToDb(toAdd.card_value, toAdd.game_Id, toAdd.room_Id);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public int GetCardValByShapeAndRealVal(string v, int value)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetCardValByShapeAndRealVal(v,value);
                    return temp.First().Card_Value;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        public List<SpectetorGamesOfUser> GetSpectOfRoom(int roomId)
        {
            List<SpectetorGamesOfUser> toRet = new List<SpectetorGamesOfUser>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetSpecsByRoomId(roomId).ToList();
                    foreach (var v in temp)
                    {
                        SpectetorGamesOfUser toAdd = new SpectetorGamesOfUser();
                        toAdd.Game_Id= v.Game_Id;
                        toAdd.roomId = v.roomId;
                        toAdd.userId = v.userId;
                        toRet.Add(toAdd);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Player> GetPlayersOfRoom(int roomId)
        {
            List<Player> toRet = new List<Player>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetPlayerssByRoomId(roomId).ToList();
                    foreach (var v in temp)
                    {
                        Player toAdd = new Player();
                        toAdd.first_card = v.first_card;
                        toAdd.is_player_active = v.is_player_active;
                        toAdd.Player_action_the_round = v.Player_action_the_round;
                        toAdd.player_name = v.player_name;
                        toAdd.room_Id = v.room_Id;
                        toAdd.Round_chip_bet = v.Round_chip_bet;
                        toAdd.secund_card = v.secund_card;
                        toAdd.Total_chip = v.Total_chip;
                        toAdd.user_Id = v.user_Id;
                        toRet.Add(toAdd);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
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

        public Database.LinqToSql.Card getDBCardByVal(int val)
        {
            Database.LinqToSql.Card toRet = new LinqToSql.Card();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetCardByVal(val);
                    toRet = ConvertCard(temp);
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private LinqToSql.Card ConvertCard(ISingleResult<GetCardByValResult> temp)
        {
            Database.LinqToSql.Card toRet = new LinqToSql.Card();
            foreach (var cardd in temp)
            {
                toRet.Card_Real_Value = cardd.Card_Real_Value;
                toRet.Card_Shpe = cardd.Card_Shpe;
            }
            return toRet;
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

        public List<LinqToSql.Card> getDeckCards(int roomId)
        {
            List<Database.LinqToSql.Card> toRet = new List<LinqToSql.Card>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetDeckByRoomId(roomId);
                    foreach (var aCard in temp)
                    {
                        Database.LinqToSql.Card toAdd = getDBCardByVal(aCard.card_value);
                        toRet.Add(toAdd);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public LinqToSql.Deck getDeckByRoomId(int roomId)
        {
            
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetDeckByRoomId(roomId);
                    return ConvertToDeck(temp);
                 }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private LinqToSql.Deck ConvertToDeck(ISingleResult<GetDeckByRoomIdResult> singleResult)
        {
            LinqToSql.Deck deck = new LinqToSql.Deck();
            foreach (var v in singleResult)
            {
                deck.room_Id = v.room_Id;
                deck.index = v.index;
                deck.game_Id = v.game_Id;
            }
            return deck;
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

        public int GetHandStepValByName(string name)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetHandStepValByName(name);
                    return temp.First().hand_Step_value;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }

    }
}