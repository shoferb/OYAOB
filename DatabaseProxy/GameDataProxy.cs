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
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.DatabaseProxy
{
    public class GameDataProxy
    {
       
   

        public GameDataProxy() {  }

   
       
        public bool AddNewGameToDB(Logic.Game.GameRoom gr)
        {
           
            bool ans = false;
           ans = InsertGameRoom(gr);
            return ans;
        }

        private bool InsertGameRoom(Logic.Game.GameRoom v)
        {
            Database.LinqToSql.GameRoom toIns = new Database.LinqToSql.GameRoom();
            toIns.Bb = v.getBb();
            toIns.Bb_Player = v.getBbPlayer();
            toIns.curr_Player = v.getCurrPlayer();
            toIns.curr_player_position = v.getCurrPlayerPos();
            toIns.Dealer_Player = v.getDealerPlayer();
            toIns.Dealer_position = v.getDealerPos();
            toIns.First_Player_In_round = v.getFirstPlayerInRound();
            toIns.first_player_in_round_position = v.getFirstPlayerInRoundPos();
            toIns.game_id = v.getGameNum();
            toIns.is_Active_Game = v.IsGameActive();
            toIns.last_rise_in_round = v.GetLastRaiseInRound();
            toIns.Max_Bet_In_Round = v.GetMaxBetInRound();
            toIns.Pot_count = v.GetPotCount();
            toIns.room_Id = v.Id;
            toIns.Sb = v.GetSb();
            toIns.SB_player = v.GetSbPlayer();
            toIns.league_name = _controller.GetLeagueValByName(v.GetLeagueName().ToString());
            toIns.hand_step = _controller.GetHandStepValByName(v.GetHandStep().ToString());
            bool didSucceed = _controller.InsertGameRoom(toIns);
            bool successRel = true;
            bool deckSuccess = InsertGameDeck(v);
            bool gameRepSuccess = InsertGameReplay(v);
            bool pubCardsSuccess = InsertGamePublicCards(v);
            bool playersSuccess = InsertGamePlayers(v);
            bool SpecsSuccess = InsertGameSpecs(v);
            bool prefsSuccess = InsertGamePref(v);
          successRel = didSucceed & successRel & deckSuccess & gameRepSuccess & pubCardsSuccess
                & playersSuccess & SpecsSuccess & prefsSuccess;
            return successRel;  
        }

        private bool InsertGamePref(Logic.Game.GameRoom v)
        {
            bool ans = true;
            Database.LinqToSql.GameRoomPreferance toAdd= new GameRoomPreferance();
            toAdd.Game_Id = v.getGameNum();
            toAdd.Bb = v.getBb();
            toAdd.enter_paying_money = v.GetBuyInPolicy();
            toAdd.Game_Mode = _controller.GetGameModeValByName(v.GetGameMode().ToString());
            toAdd.is_Spectetor = v.IsSpectatable();
            toAdd.League_name = _controller.GetLeagueValByName(v.GetLeagueName().ToString());
            toAdd.max_player_in_room = v.GetMaxPlayer();
            toAdd.Min_player_in_room = v.GetMinPlayer();
            toAdd.room_id = v.Id;
            toAdd.Sb = v.GetSb();
            toAdd.starting_chip = v.GetStartingChip();
            ans = ans & (_controller.InsertPref(toAdd));
            
            return ans;
        }

        private bool InsertGameSpecs(Logic.Game.GameRoom v)
        {
            bool ans = true;
            if (v.GetSpectetorInRoom() != null)
            {
                List<Logic.Users.Spectetor> specss = v.GetSpectetorInRoom();
                foreach (var aSpec in specss)
                {
                    Database.LinqToSql.SpectetorGamesOfUser toAdd = new Database.LinqToSql.SpectetorGamesOfUser();
                    toAdd.Game_Id = v.getGameNum();
                    toAdd.roomId = v.Id;
                    toAdd.userId = aSpec.user.Id();
                    ans = ans & (_controller.InsertSpec(toAdd));
                }
            }
            return ans;
        }

        private bool InsertGamePlayers(Logic.Game.GameRoom v)
        {
            bool ans = true;
            List<Logic.Users.Player> players = v.GetPlayers();
           
                ans = true;
                foreach (var aPlayer in players)
                {
                if (aPlayer != null)
                {
                    Database.LinqToSql.Player toAdd = new Database.LinqToSql.Player();
                    toAdd.Game_Id = v.getGameNum();
                    toAdd.room_Id = v.Id;
                    toAdd.user_Id = aPlayer.user.Id();
                    toAdd.is_player_active = aPlayer.isPlayerActive;
                    toAdd.player_name = aPlayer.name;
                    toAdd.Total_chip = aPlayer.TotalChip;
                    toAdd.Round_chip_bet = aPlayer.RoundChipBet;
                    toAdd.Player_action_the_round = aPlayer.PlayedAnActionInTheRound;
                    try
                    {
                        toAdd.first_card = _controller.GetCardValByShapeAndRealVal(aPlayer._firstCard._suit.ToString(), aPlayer._firstCard._value);
                        toAdd.secund_card = _controller.GetCardValByShapeAndRealVal(aPlayer._secondCard._suit.ToString(), aPlayer._secondCard._value);
                    }
                    catch(Exception e)
                    {
                        toAdd.first_card = 0;
                        toAdd.secund_card = 0;
                    }
                    ans = ans & (_controller.InsertPlayer(toAdd));
                }
            }
            return ans;
        }

        private bool InsertGamePublicCards(Logic.Game.GameRoom v)
        {
            bool ans = true;
            if (v.GetPublicCards() != null)
            {
                List<Card> pubCards = v.GetPublicCards();
                foreach (var aCard in pubCards)
                {
                    Database.LinqToSql.Public_Card toAdd = new Database.LinqToSql.Public_Card();
                    toAdd.room_Id = v.Id;
                    toAdd.Game_Id = v.getGameNum();
                    toAdd.card = _controller.GetCardValByShapeAndRealVal(aCard._suit.ToString(), aCard._value);
                    ans = ans & (_controller.InsertPublicCard(toAdd));
                }
            }
            return ans;
        }

        private bool InsertGameReplay(Logic.Game.GameRoom v)
        {
            bool ans = true;
            Logic.Replay.GameReplay gameRep = v.GetGameReplay();
            if (gameRep != null)
            {
                ans = true;
                Database.LinqToSql.GameReplay toAdd = new Database.LinqToSql.GameReplay();
                toAdd.game_Id = gameRep._gameNumber;
                toAdd.room_Id = gameRep._gameRoomID;
                toAdd.replay = gameRep.ToString();
                ans = ans & (_controller.InsertGameReply(toAdd));
            }
            return ans;
        }

        private bool InsertGameDeck(Logic.Game.GameRoom v)
        {
            bool ans = true;
            Deck deck = v.GetDeck();
            if (deck != null)
            {
                ans = true;
            foreach (var aCard in v.GetDeck()._deck)
            {
                Database.LinqToSql.Deck toAdd = new Database.LinqToSql.Deck();
                toAdd.room_Id = v.Id;
                toAdd.game_Id = v.getGameNum();
                toAdd.card_value = _controller.GetCardValByShapeAndRealVal(aCard._suit.ToString(), aCard._value);
                ans = ans & (_controller.InsertDeck(toAdd));
            }
        }
            return ans;
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
                
                Deck d = ConverDBDeck(_controller.getDeckByRoomId(g.room_Id),_controller.getDeckCards(g.room_Id));
                Logic.Game.GameRoom.HandStep hs = ConvertSpecsList(g.HandStep);
                Logic.GameControl.LeagueName leagueOf = ConvertSpecsList(g.LeagueName);
                Logic.Replay.GameReplay gr = ConvertGameReplay(g.GameReplay);
                GameRoomPreferance  pref = _controller.GetPrefByRoomId(g.room_Id);

                Decorator decorator = _gameCenter.CreateDecorator(pref.Bb.Value, pref.starting_chip.Value, pref.is_Spectetor.Value, pref.Min_player_in_room.Value, pref.max_player_in_room.Value, pref.enter_paying_money.Value, ConvertGameModeChosen(_controller.GetGameModeByVal(pref.Game_Mode.Value)), leagueOf);
                Logic.Game.GameRoom toAdd = new Logic.Game.GameRoom(playersLst, g.room_Id, decorator, _gameCenter, _logControl,
               _replayManager, _sender, g.game_id, g.is_Active_Game, g.Pot_count, g.Max_Bet_In_Round,
                 pubCards, SpecssLst,  dealerPlayer,  leagueOf, g.last_rise_in_round, currentPlayer,  bbPlayer, sbPlayer,  
                 firstPlayerInRound, g.Bb ,g.Sb, g.Dealer_position, g.curr_player_position, g.first_player_in_round_position, 
                 gr,  hs,  d);
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

        private Deck ConverDBDeck(Database.LinqToSql.Deck deck, List<Database.LinqToSql.Card> list)
        {
            Deck toRet = new Deck();
            toRet._numOfCards = list.Count;
            List<Card> lst = new List<Card>();
            foreach (var aCard in list)
            {
                lst.Add(getCardByVal(aCard.Card_Value));
            }
            toRet._deck = lst;
            //todo cardRank????
            return toRet;
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
