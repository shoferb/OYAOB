using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TexasHoldem.Database.LinqToSql;
using TexasHoldemShared.CommMessages;

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



        public bool UpdateGameRoomPotSize(int newPot, int roomId)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.UpdateGameRoomPotSize(newPot, roomId);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteGameRoom(int roomId, int gameId)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.DeleteGameRoom(roomId, gameId);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteGameRoomPref(int roomId)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.DeleteGameRoomPref(roomId);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<XElement> GetAllActiveGameRooms()
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetAllActiveGameRooms().ToList();
                    foreach (var v in temp)
                    {                
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetAllSpectatebleGameRooms()
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetAllSpectableGameRooms(true).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByBuyInPolicy(int bipol)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByBuyInPolicy(bipol).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByGameMode(int modeVal)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByGameMode(modeVal).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByMinPlayers(int min)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByMinPlayers(min).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByPotSize(int pot)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByPotSize(pot).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByStartingChip(int sc)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByStaringChip(sc).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByMinBet(int min)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByMinBet(min).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<XElement> GetGameRoomsByMaxPlayers(int max)
        {
            List<XElement> toRet = new List<XElement>();
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomsByMaxPlayers(max).ToList();
                    foreach (var v in temp)
                    {
                        toRet.Add(v.GameXML);
                    }
                    return toRet;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string GetGameRoomReplyById(int roomid)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomReplyById(roomid).ToList();
                    if (temp.Capacity == 0)
                    {
                        return null;
                    }
                    var res = temp.First();
                    return res.Replay;
                }
            
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public XElement GetGameRoomById(int roomid)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomById(roomid).ToList();
                    if(temp.Capacity==0)
                    {
                        return null;
                    }
                    var res = temp.First();
                    return res.GameXML;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public GameRoomPreferance GetGameRoomPrefById(int roomid)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetGameRoomPrefById(roomid).ToList();
                    if (temp.Capacity == 0)
                    {
                        return null;
                    }
                    return ConvertGamePref(temp.First());
                    
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private GameRoomPreferance ConvertGamePref(GetGameRoomPrefByIdResult p)
        {
            GameRoomPreferance toRet = new GameRoomPreferance();
            toRet.BuyInPolicy = p.BuyInPolicy;
            toRet.CanSpectate = p.CanSpectate;
            toRet.EnterGamePolicy = p.EnterGamePolicy;
            toRet.GameId = p.GameId;
            toRet.GameMode = p.GameMode;
            toRet.League = p.League;
            toRet.MaxPlayers = p.MaxPlayers;
            toRet.MinBet = p.MinBet;
            toRet.MinPlayers = p.MinPlayers;
            toRet.PotSize = p.PotSize;
            toRet.Roomid = p.Roomid;
            return toRet;
        }

        public bool UpdateGameRoom(int roomId, int gameId, XElement newXML, bool newIsActive, string newRep)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    string oldRep = db.GetGameRoomReplyById(roomId).First().Replay;
                    db.UpdateGameRoom(roomId, gameId, newXML, newIsActive, string.Concat(oldRep,newRep));
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

        public List<GetAllUserActiveGameResult> GetAllUserActiveGames(int userId)
        {
            List<GetAllUserActiveGameResult> toReturn = new List<GetAllUserActiveGameResult>();
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    toReturn = db.GetAllUserActiveGame(userId).ToList();
                    return toReturn;
                }

            }
            catch (Exception)
            {
                return toReturn;
            }
        }

        public List<GetUserSpectetorsGameResult> GetUserSpectetorsGameResult(int userId)
        {
            List<GetUserSpectetorsGameResult> toReturn = new List<GetUserSpectetorsGameResult>();
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    toReturn = db.GetUserSpectetorsGame(userId).ToList();
                    return toReturn;
                }

            }
            catch (Exception)
            {
                return toReturn;
            }
        }

    }
}
