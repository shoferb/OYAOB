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
                    var temp = db.GetAllGameRooms().ToList();
                    foreach (var v in temp)
                    {
                        GameRoom toAdd = ConvertToGameRoom(v);
                       
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

        private GameRoom ConvertToGameRoom(GetAllGameRoomsResult v)
        {
            GameRoom toRet = new GameRoom();
            toRet.GameId = v.GameId;
            toRet.GameXML = v.GameXML;
            toRet.isActive = v.isActive;
            toRet.Replay = v.Replay;
            toRet.RoomId = v.RoomId;
            return toRet;
        }

    /*    public LinqToSql.GameRoomPreferance GetPrefByRoomId(int roomId)
        {
            LinqToSql.GameRoomPreferance toRet = new GameRoomPreferance();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.getga(roomId).ToList();
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
        }*/

        public bool InsertGameRoom(GameRoom toIns)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.InsertGameRoomToDb(toIns.RoomId, toIns.GameId, toIns.Replay, 
                        toIns.GameXML, toIns.isActive);
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
                    db.InsertPrefToDb(toAdd.Roomid, toAdd.CanSpectate, toAdd.MinPlayers,
                         toAdd.MaxPlayers, toAdd.BuyInPolicy, toAdd.EnterGamePolicy,
                         toAdd.MinBet, toAdd.League, toAdd.GameMode, toAdd.GameId, toAdd.PotSize);
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

       /* public GameMode GetGameModeByVal(int val)
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
        }*/

       
      

       
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
