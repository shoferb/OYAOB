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
        private List<Player> Players;
        private readonly List<Spectetor> Spectatores;
        private int DealerPos;
        private int maxBetInRound;
        private int PotCount;
        private int Bb;
        private int Sb;
        private Deck Deck;
        private HandStep Hand_Step;
        private List<Card> PublicCards;
        private bool IsActiveGame;
        private List<Tuple<int, Player>> SidePots; //TODO use that in all in
        private GameReplay GameReplay;
        private ReplayManager ReplayManager;
        private GameCenter GameCenter;
        private Player CurrentPlayer;
        private Player DealerPlayer;
        private Player BbPlayer;
        private Player SbPlayer;
        private Decorator MyDecorator;
        private LogControl logControl;
        private int GameNumber;
        private Player FirstPlayerInRound;
        private int currentPlayerPos;
        private int firstPlayerInRoundPoistion;
        private int lastRaiseInRound;
        private bool useCommunication;
        private SessionIdHandler sidHandler;

        private LeagueName league;
        private static readonly object padlock = new object();


        public GameRoomXML() { }

        public GameRoomXML ( int _roomid, List<Player> _players,
                           List<Spectetor> _spectatores, int _dealerPos, int _maxBetInRound,
                           int _potCount,int _bb, int _sb, Deck _deck,HandStep _handStep,
                           List<Card> _publicCards, bool _isActiveGame, List<Tuple<int, Player>> _idePots,
                           GameReplay _gameReplay, ReplayManager _replayManager, GameCenter _gameCenter,
                           Player _currentPlayer,Player _dealerPlayer, Player _bbPlayer, Player _sbPlayer,
                           Decorator _myDecorator, LogControl _logControl,int _gameNumber, Player _firstPlayerInRound,
                           int _currentPlayerPos, int _firstPlayerInRoundPoistion,int _lastRaiseInRound,
                           bool _useCommunication, SessionIdHandler _sidHandler, LeagueName _league)
        {
            Id = _roomid;
            Players = _players;
            Spectatores = _spectatores;
            DealerPos = _dealerPos;
            maxBetInRound = _maxBetInRound;
            PotCount = _potCount;
            Bb = _bb;
            Sb = _sb;
            Deck = _deck;
            Hand_Step = _handStep;
            PublicCards = _publicCards;
            IsActiveGame = _isActiveGame;
            SidePots = _idePots;
            GameReplay = _gameReplay;
            league = _league;
            ReplayManager = _replayManager;
            GameCenter = _gameCenter;
            CurrentPlayer = _currentPlayer;
            DealerPlayer = _dealerPlayer;
            BbPlayer = _bbPlayer;
            SbPlayer = _sbPlayer;
            MyDecorator = _myDecorator;
            logControl = _logControl;
            GameNumber = _gameNumber;
            FirstPlayerInRound = _firstPlayerInRound;
            currentPlayerPos = _currentPlayerPos;
            firstPlayerInRoundPoistion = _firstPlayerInRoundPoistion;
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
