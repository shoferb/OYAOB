using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic;
using TexasHoldem.Logic.Users;
using TexasHoldem.communication.Impl;
using static TexasHoldem.Logic.Game.GameRoom;

namespace TexasHoldem.DatabaseProxy
{
    public class GameRoomXML
    {
       
        public int Id { get; set; }
        public List<PlayerXML> Players;
        public  List<SpecXML> Spectatores;
        public int DealerPos;
        public int maxBetInRound;
        public int PotCount;
        public int Bb;
        public int Sb;
        public Deck Deck;
        public HandStep Hand_Step;
        public List<Card> PublicCards;
        public bool IsActiveGame;
        private List<Tuple<int, PlayerXML>> SidePots; //TODO use that in all in
        public List<SidePotTuple> sidePotsXML;
        private GameReplay GameReplay;
        private ReplayManager ReplayManager;
        private GameCenter GameCenter;
        public PlayerXML CurrentPlayer;
        public PlayerXML DealerPlayer;
        public PlayerXML BbPlayer;
        public PlayerXML SbPlayer;
        private Decorator MyDecorator;
        private LogControl logControl;
        public int GameNumber;
        public PlayerXML FirstPlayerInRound;
        public int currentPlayerPos;
        public int firstPlayerInRoundPoistion;
        public int lastRaiseInRound;
        public bool useCommunication;
        private SessionIdHandler sidHandler;
        public LeagueName league;
        public object padlock = new object();
        
        public GameRoomXML() { }

        public GameRoomXML (GameRoom g)
        {
            Id = g.Id;
            Players = new List<PlayerXML>();
            foreach (Player p in g.GetPlayersInRoom())
            {
                Players.Add(new PlayerXML(p));
            }
            
            Spectatores =new List<SpecXML>() ;
            foreach (Spectetor p in g.GetSpectetorInRoom())
            {
                Spectatores.Add(new SpecXML(p));
            }

            DealerPos = g.GetDealerPos();
            maxBetInRound = g.GetMaxBetInRound();
            PotCount = g.GetPotSize();
            Bb = g.getBBnum();
            Sb = g.getSBNUM();
            Deck = g.GetDeck();
            Hand_Step = g.GetHandStep();
            PublicCards = g.GetPublicCards();
            IsActiveGame = g.IsGameActive();
            if (g.GetSidePots() != null)
            {
                sidePotsXML = new List<SidePotTuple>();
                SidePots = new List<Tuple<int, PlayerXML>>();
                foreach (var p in g.GetSidePots())
                {
                    PlayerXML pp = new PlayerXML(p.Item2);
                    SidePots.Add(new Tuple<int, PlayerXML>(p.Item1, pp));
                    sidePotsXML.Add(new SidePotTuple( p.Item1, pp));
                }
            }
               
            GameReplay = g.GetGameRepObj();
            league = g.GetLeagueName();
            ReplayManager = g.GetRepManager();
         
            CurrentPlayer = new PlayerXML(g.GetCurrPlayer());
            DealerPlayer = new PlayerXML(g.GetDealer());
            BbPlayer = new PlayerXML(g.GetBb());
            SbPlayer = new PlayerXML(g.GetSb());
          //  MyDecorator = g.GetDecorator();
        
            GameNumber = g.GetGameNum();
            FirstPlayerInRound = new PlayerXML(g.GetFirstPlayerInRound());
            currentPlayerPos = g.GetCurrPosition();
            firstPlayerInRoundPoistion = g.GetFirstPlayerInRoundPos();
            lastRaiseInRound = g.GetlastRaiseInRound();
            useCommunication = g.GetUseComm();
      
    }

        public GameRoom ConvertToLogicGR(GameCenter gc)
        {
            List<Player> playersToL = new List<Player>();
            foreach(var pXML in Players)
            {
                Player toAdd = GetPlayerFromXML(pXML, gc);
                if (toAdd != null)
                {
                    playersToL.Add(toAdd);
                }
            }
            List<Spectetor> SpectatoresToL = new List<Spectetor>();
            foreach (var s in Spectatores)
            {
                Spectetor toAdd = GetSpecFromXML(s, gc);
                if (toAdd != null)
                {
                    SpectatoresToL.Add(toAdd);
                }
            }
            List<Tuple<int, Player>>  SidePotstoL = new List<Tuple<int, Player>>();
            foreach (var p in sidePotsXML)
            {
                Player toAdd = GetPlayerFromXML(p.item2, gc);
                if (toAdd != null)
                {
                    SidePotstoL.Add(new Tuple<int, Player>(p.item1, toAdd));


                }
            }
            return new GameRoom(playersToL, Id, gc, gc.GetLogControl(),
            gc.GetRepMan(), GameNumber, IsActiveGame, PotCount, maxBetInRound,
            PublicCards, SpectatoresToL, GetPlayerFromXML(DealerPlayer, gc), league, lastRaiseInRound,
           GetPlayerFromXML(CurrentPlayer, gc), GetPlayerFromXML(BbPlayer, gc), GetPlayerFromXML(SbPlayer, gc) , GetPlayerFromXML(FirstPlayerInRound, gc), Bb, Sb,
           DealerPos,  currentPlayerPos,  firstPlayerInRoundPoistion, GameReplay , Hand_Step, Deck, gc.GetSessionIdHandler(), useCommunication, SidePotstoL);

        }

        private Player GetPlayerFromXML(PlayerXML p, GameCenter gc)
        {
            Player toRet = new Player(p.name,p.TotalChip, p.roomId, p.RoundChipBet, p.isPlayerActive, p._firstCard,
                p._secondCard, p.PlayedAnActionInTheRound, p._publicCards);
            toRet.user = gc.GetSysControl().GetUserWithId(p.userId);
            if (toRet.user == null)
            {
                return null;
            }
            return toRet;

        }
        private Spectetor GetSpecFromXML(SpecXML p, GameCenter gc)
        {
            Spectetor toRet = new Spectetor(p.roomId);
            toRet.user = gc.GetSysControl().GetUserWithId(p.userId);
            if (toRet.user == null)
            {
                return null;
            }
            return toRet;
        }


    }
}
