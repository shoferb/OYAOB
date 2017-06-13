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
           return  _controller.InsertGameRoom(toIns);
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
            Database.LinqToSql.GameRoomPreferance toAdd= new GameRoomPreferance();
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
            return _controller.InsertPref(toAdd);
        }

        private bool InsertNewGameRoom(Logic.Game.GameRoom v)
        {
            return InsertNewGameRoom(v) & InsertGamePref(v);
        }

        public List<IGame> GetAllGames()
        {
            List<IGame> toRet = new List<IGame>();
            List<Database.LinqToSql.GameRoom> dbGames= _controller.getAllGames();
            if(dbGames == null)
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
            if(gameMode.game_mode_name.Equals("Limit"))
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



        private Logic.Replay.GameReplay ConvertGameReplay(Database.LinqToSql.GameReplay repDB)
        {
            Logic.Replay.GameReplay toRet = new Logic.Replay.GameReplay();
            toRet._gameRoomID = repDB.room_Id;
            toRet._gameNumber = repDB.game_Id;
            //TODO: string of actions???
            return toRet;
                
        }

        private Logic.Game.GameRoom.HandStep ConvertSpecsList(Database.LinqToSql.HandStep hsDB)
        {
            if (hsDB.hand_Step_name.Equals("Pre-Flop"))
            {
                return Logic.Game.GameRoom.HandStep.PreFlop;
            }
            else if (hsDB.hand_Step_name.Equals("River"))
            {
                return Logic.Game.GameRoom.HandStep.River;
            }
            else if (hsDB.hand_Step_name.Equals("Turn"))
            {
                return Logic.Game.GameRoom.HandStep.Turn;
            }
            else
            {
                return Logic.Game.GameRoom.HandStep.Flop;
            }
           
        }


        private Logic.GameControl.LeagueName ConvertSpecsList(Database.LinqToSql.LeagueName leagueDB)
        {
            if(leagueDB.League_Name.Equals("A"))
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

        private List<Logic.Users.Spectetor> ConvertSpecsList(List<Database.LinqToSql.SpectetorGamesOfUser> dbSpecs)
        {
            List<Logic.Users.Spectetor> toRet = new List<Logic.Users.Spectetor>();
            foreach (Database.LinqToSql.SpectetorGamesOfUser s in dbSpecs)
            {
                //TODO
                User user; //= UserDataProxy.GetUserById(dbPlayer.user_Id);
                Logic.Users.Spectetor toAdd = new Logic.Users.Spectetor(/*user*/ null, s.roomId);
            }
            return toRet;
        }

        private List<Logic.Users.Player> ConvertPlayerList(List<Database.LinqToSql.Player> dbPlayers)
        {
            List<Logic.Users.Player> toRet = new List<Logic.Users.Player>();
            foreach (Database.LinqToSql.Player dbPlayer in dbPlayers)
            {
                //TODO
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
