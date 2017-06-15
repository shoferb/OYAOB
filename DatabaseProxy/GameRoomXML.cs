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
    class GameRoomXML
    {
       
        public int Id { get; set; }
        public List<Player> Players;
        public  List<Spectetor> Spectatores;
        public int DealerPos;
        public int maxBetInRound;
        public int PotCount;
        public int Bb;
        public int Sb;
        public Deck Deck;
        public HandStep Hand_Step;
        public List<Card> PublicCards;
        public bool IsActiveGame;
        public List<Tuple<int, Player>> SidePots; //TODO use that in all in
        public GameReplay GameReplay;
        public ReplayManager ReplayManager;
        public GameCenter GameCenter;
        public Player CurrentPlayer;
        public Player DealerPlayer;
        public Player BbPlayer;
        public Player SbPlayer;
        public Decorator MyDecorator;
        public LogControl logControl;
        public int GameNumber;
        public Player FirstPlayerInRound;
        public int currentPlayerPos;
        public int firstPlayerInRoundPoistion;
        public int lastRaiseInRound;
        public bool useCommunication;
        public SessionIdHandler sidHandler;
        public LeagueName league;
        public object padlock = new object();

        public GameRoomXML() { }

        public GameRoomXML (GameRoom g)
        {
            Id = g.Id;
            Players = g.GetPlayersInRoom();
            Spectatores = g.GetSpectetorInRoom();
            DealerPos = g.GetDealerPos();
            maxBetInRound = g.GetMaxBetInRound();
            PotCount = g.GetPotSize();
            Bb = g.getBBnum();
            Sb = g.getSBNUM();
            Deck = g.GetDeck();
            Hand_Step = g.GetHandStep();
            PublicCards = g.GetPublicCards();
            IsActiveGame = g.IsGameActive();
            SidePots = g.GetSidePots();
            GameReplay = g.GetGameRepObj();
            league = g.GetLeagueName();
            ReplayManager = g.GetRepManager();
            GameCenter = g.GetGameCenter();
            CurrentPlayer = g.GetCurrPlayer();
            DealerPlayer = g.GetDealer();
            BbPlayer = g.GetBb();
            SbPlayer = g.GetSb();
            MyDecorator = g.GetDecorator();
            logControl = g.GetLogControl();
            GameNumber = g.GetGameNum();
            FirstPlayerInRound = g.GetFirstPlayerInRound();
            currentPlayerPos = g.GetCurrPosition();
            firstPlayerInRoundPoistion = g.GetFirstPlayerInRoundPos();
            lastRaiseInRound = _lastRaiseInRound;
            useCommunication = _useCommunication;
            sidHandler = _sidHandler;
    }

        public GameRoom ConvertToLogicGR()
        {
            return new GameRoom(Players, Id, MyDecorator, GameCenter, logControl,
          ReplayManager , GameNumber, IsActiveGame, PotCount, maxBetInRound,
            PublicCards, Spectatores, DealerPlayer, league, lastRaiseInRound,
           CurrentPlayer, BbPlayer, SbPlayer, FirstPlayerInRound, Bb, Sb,
           DealerPos,  currentPlayerPos,  firstPlayerInRoundPoistion, GameReplay , Hand_Step, Deck, sidHandler, useCommunication,SidePots);

        }
    }
}
