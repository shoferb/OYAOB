using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool AddNewGameToDB(Logic.Game.GameRoom gr)
        {
            bool ans = false;
            ans = InsertGameRoom(gr);
            return ans;
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

        private XElement GameRoomToXElement(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Logic.Game.GameRoom));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
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

        private bool InsertNewGameRoom(Logic.Game.GameRoom v)
        {
            return InsertNewGameRoom(v) & InsertGamePref(v);
        }


        public bool UpdateGameRoom(int roomId, int gameId, XElement newXML, bool newIsActive, string newRep)
        {
            return _controller.UpdateGameRoom( roomId, gameId,  newXML, newIsActive,  newRep);
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
            if (dbGames == null)
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
            List<Database.LinqToSql.GameRoom> dbGames = _controller.GetAllActiveGameRooms();
            if (dbGames == null)
            {
                return null;
            }
            foreach (Database.LinqToSql.GameRoom g in dbGames)
            {
                toRet.Add(GameRoomFromXElement(g.GameXML));
            }
            return toRet;
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
