using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.DatabaseProxy
{
    public class GameDataProxy
    {

        GameDataControler _controller;


        public GameDataProxy()
        {
            _controller = new GameDataControler();
        }

        static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        private bool InsertGameRoom(Logic.Game.GameRoom v)
        {
            Database.LinqToSql.GameRoom toIns = new Database.LinqToSql.GameRoom();
            toIns.GameId = v.GetGameNum();
            toIns.isActive = v.IsGameActive();
            toIns.RoomId = v.Id;
            toIns.GameXML = GameRoomToXElement(v);
            toIns.Replay = v.GetGameReplay();
            return _controller.InsertGameRoom(toIns);
        }

        private XElement GameRoomToXElement(Logic.Game.GameRoom obj)
        {

            string xmlString = ConvertObjectToXMLString(obj);
            XElement xElement = XElement.Parse(xmlString);
            return xElement;

        }

        private Logic.Game.GameRoom GameRoomFromXElement(XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(Logic.Game.GameRoom));
            return (Logic.Game.GameRoom)xmlSerializer.Deserialize(xElement.CreateReader());
        }


        private bool InsertGamePref(Logic.Game.GameRoom v)
        {
            Database.LinqToSql.GameRoomPreferance toAdd = new GameRoomPreferance();
            toAdd.GameId = v.GetGameNum();
            toAdd.BuyInPolicy = v.GetBuyInPolicy();
            toAdd.GameMode = _controller.GetGameModeValByName(v.GetGameMode().ToString());
            toAdd.CanSpectate = v.IsSpectatable();
            toAdd.League = _controller.GetLeagueValByName(v.GetLeagueName().ToString());
            toAdd.MaxPlayers = v.GetMaxPlayer();
            toAdd.MinPlayers = v.GetMinPlayer();
            toAdd.Roomid = v.Id;
            toAdd.EnterGamePolicy = v.GetStartingChip();
            toAdd.MinBet = v.GetMinBet();
            toAdd.PotSize = v.GetPotSize();
            return _controller.InsertPref(toAdd);
        }

        public bool UpdateGameRoomPotSize(int newPot, int roomId)
        {
            return _controller.UpdateGameRoomPotSize(newPot,roomId);
        }

        public bool InsertNewGameRoom(Logic.Game.GameRoom v)
        {
            bool ans = InsertGameRoom(v);
            bool ans2 = InsertGamePref(v);
            return ans&ans2 ;
        }


        public bool UpdateGameRoom(Logic.Game.GameRoom g)
        {
            return _controller.UpdateGameRoom( g.Id, g.GetGameNum(),  GameRoomToXElement(g), g.IsGameActive(),  g.GetGameReplay());
        }

        public bool DeleteGameRoom(int roomId, int gameId)
        {
            return _controller.DeleteGameRoom(roomId, gameId);
        }

        public bool DeleteGameRoomPref(int roomId)
        {
            return _controller.DeleteGameRoomPref(roomId);
        }

        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();
            List<Database.LinqToSql.GameRoom> dbGames = _controller.getAllGames();
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (Database.LinqToSql.GameRoom g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g.GameXML));
            }
            return toRet;
        }

        public List<IGame> GetAllActiveGameRooms()
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetAllActiveGameRooms();
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetAllSpectatebleGameRooms()
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetAllSpectatebleGameRooms();
            if (dbGames.Capacity ==0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByBuyInPolicy(int bipol)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByBuyInPolicy(bipol);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }
        private int GetGameModeValByName(TexasHoldemShared.CommMessages.GameMode mode)
        {
            return _controller.GetGameModeValByName(mode.ToString());
        }

        public List<IGame> GetGameRoomsByGameMode(TexasHoldemShared.CommMessages.GameMode mode)
        {
            List<IGame> toRet = new List<IGame>();
            int modeVal = GetGameModeValByName(mode);
            List<XElement> dbGames = _controller.GetGameRoomsByGameMode(modeVal);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByMaxPlayers(int max)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByMaxPlayers(max);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByMinPlayers(int min)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByMinPlayers(min);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByMinBet(int min)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByMinBet(min);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByPotSize(int pot)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByPotSize(pot);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public List<IGame> GetGameRoomsByStartingChip(int sc)
        {
            List<IGame> toRet = new List<IGame>();
            List<XElement> dbGames = _controller.GetGameRoomsByStartingChip(sc);
            if (dbGames.Capacity == 0)
            {
                return null;
            }
            foreach (XElement g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g));
            }
            return toRet;
        }

        public IGame GetGameRoombyId(int roomid)
        {
            List<IGame> toRet = new List<IGame>();
            XElement g = _controller.GetGameRoomById(roomid);
            if (g==null)
            {
                return null;
            }
            return GameRoomFromXElement(g);
        }

        public string GetGameRoomReplyById(int roomid, int gameid)
        {
            List<IGame> toRet = new List<IGame>();
            string rep = _controller.GetGameRoomReplyById(roomid, gameid);
            if (rep == null)
            {
                return null;
            }
            return rep;
        }

        private TexasHoldemShared.CommMessages.GameMode ConvertGameModeChosen(Database.LinqToSql.GameMode gameMode)
        {
            if (gameMode.game_mode_name.Equals("Limit"))
            {
                return TexasHoldemShared.CommMessages.GameMode.Limit;
            }
            else if (gameMode.game_mode_name.Equals("NoLimit"))
            {
                return TexasHoldemShared.CommMessages.GameMode.NoLimit;
            }
            else
            {
                return TexasHoldemShared.CommMessages.GameMode.PotLimit;
            }
        }



        private Logic.GameControl.LeagueName ConvertLeague(Database.LinqToSql.LeagueName leagueDB)
        {
            if (leagueDB.League_Name.Equals("A"))
            {
                return Logic.GameControl.LeagueName.A;
            }
            else if (leagueDB.League_Name.Equals("B"))
            {
                return Logic.GameControl.LeagueName.B;
            }
            else if (leagueDB.League_Name.Equals("C"))
            {
                return Logic.GameControl.LeagueName.C;
            }
            else if (leagueDB.League_Name.Equals("D"))
            {
                return Logic.GameControl.LeagueName.D;
            }
            else if (leagueDB.League_Name.Equals("E"))
            {
                return Logic.GameControl.LeagueName.E;
            }
            else
            {
                return Logic.GameControl.LeagueName.Unknow;
            }
        }

    }   
}
