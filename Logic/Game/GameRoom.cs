using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;
using static TexasHoldemShared.CommMessages.CommunicationMessage;

namespace TexasHoldem.Logic.Game
{
    public class GameRoom : IGame
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int Id { get; set; }
        private List<Player> Players;
        private readonly List<Spectetor> Spectatores;
        private int DealerPos;
        private int maxBetInRound;
        private int PotCount;
        private int Bb;
        private string winner = "";
        private string ReplayStr = "";
        public int GetGameNum()
        {
            return this.GameNumber;
        }

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
        public int GameNumber { get; set; }
        private Player FirstPlayerInRound;
        private int currentPlayerPos;
        private int firstPlayerInRoundPoistion;
        private int lastRaiseInRound;
        private bool useCommunication;
        private SessionIdHandler sidHandler;

        private LeagueName league;
        private static readonly object padlock = new object();

        public GameRoom() { } // for XML

        public int GetSSB() //for test
        {
            return this.Sb;
        }

        public int GetDealerPos()
        {
            return this.DealerPos;
        }

        public void SetBB(int j)
        {
            this.Bb = j;
        }
        public int GetBBNUM()
        {
            return this.Bb;
        }

      
        public int GetMaxBetInRound()
        {
            return this.maxBetInRound;
        }
        public GameRoom(List<Player> players, int ID, Decorator decorator, GameCenter gc, LogControl log, 
            ReplayManager replay, SessionIdHandler sidH)
        {
            MyDecorator = decorator;
            SetTheBlinds();
            Id = ID;
            GameNumber = 0;
            IsActiveGame = false;
            PotCount = 0;          
            maxBetInRound = 0;
            PublicCards = new List<Card>();
            Players = players;
            if (Players.Count > 0)
            {
                Players[0].name = Players[0].user.MemberName();
            }
            Spectatores = new List<Spectetor>();
            SidePots = new List<Tuple<int, Player>>();
            DealerPlayer = null;
            logControl = log;
            league = GetLeagueFromPlayer(Players);
            ReplayManager = replay;
            GameCenter = gc;
            lastRaiseInRound = 0;
            ReduceFeeAndStatringChipFromPlayers();
            useCommunication = true;
            sidHandler = sidH;
        }
        public void SetDeco(int minBet, int startingChip, bool canSpectate, int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, GameMode gameModeChosen, LeagueName league)
        {
            Decorator mid = new MiddleGameDecorator(gameModeChosen, this.Bb, this.Sb);
            Decorator before = new BeforeGameDecorator(minBet, startingChip, canSpectate, minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, league);
            before.SetNextDecorator(mid);
            this.MyDecorator =before;
        }


        public GameRoom(List<Player> players, int ID, GameCenter gc, LogControl log,
          ReplayManager replay, int gameNum, bool isActiveGame, int potCount, int mmaxBetInRound,
           List<Card> pubCards, List<Spectetor> specs, Player dealerPlayer, LeagueName leagueOf, int lastRaiseInRoundd,
           Player currentPlayer, Player bbPlayer, Player sbPlayer, Player firstPlayerInRound, int bb, int sb,
           int dealerPos, int currentPlayerPoss, int firstPlayerInRoundPoistionn, GameReplay gr, GameRoom.HandStep hs, Deck d, SessionIdHandler _sidHandler, bool _useCom, List<Tuple<int, Player>> _sp)
        {
           // Decorator mid = new MiddleGameDecorator(GameMode.NoLimit,bb, sb);
           // Decorator before = new BeforeGameDecorator(minBetInRoom, 1000, true, 2, 4, 20, LeagueName.A);
          //  before.SetNextDecorator(mid);
          //  MyDecorator= before;
            Id = ID;
            GameNumber = gameNum;
            IsActiveGame = isActiveGame;
            PotCount = potCount;
            maxBetInRound = mmaxBetInRound;
            PublicCards = pubCards;
            Players = players;
            if (Players.Count > 0)
            {
                Players[0].name = Players[0].user.MemberName();
            }
            Spectatores = specs;
            SidePots = _sp;
            DealerPlayer = dealerPlayer;
            logControl = log;
            league = leagueOf;
            ReplayManager = replay;
            GameCenter = gc;
            lastRaiseInRound = lastRaiseInRoundd;
            useCommunication = true;
            sidHandler = _sidHandler;
            this.CurrentPlayer = currentPlayer;
            this.BbPlayer = bbPlayer;
            this.SbPlayer = sbPlayer;
            this.FirstPlayerInRound = firstPlayerInRound;
            this.Bb = bb;
            this.Sb = sb;
            this.DealerPos = dealerPos;
            this.currentPlayerPos = currentPlayerPoss;
            this.firstPlayerInRoundPoistion = firstPlayerInRoundPoistionn;
            this.GameReplay = gr;
            this.Hand_Step = hs;
            this.Deck = d;
        }

        public string GetGameReplay()
        {
            if (this.GameReplay == null)
            {
                return "";
            }
           return string.Concat("", this.GameReplay.ToString());
        }
        public void SetPotSize(int newPot) //forTsets
        {
            this.PotCount = newPot;
        }
        public void SetIsActive(bool n)//for tests
        {
            this.IsActiveGame = n;
        }
        private void ReduceFeeAndStatringChipFromPlayers()
        {
            foreach (Player p in Players)
            {
                
                p.user.ReduceMoneyIfPossible(MyDecorator.GetStartingChip() +
                    MyDecorator.GetEnterPayingMoney());
            }
        }

        private LeagueName GetLeagueFromPlayer(List<Player> players)
        {
            if (players == null || players.Count == 0)
            {
                return LeagueName.A; //defult league
            }
            return players[0].user.GetLeague();
        }
        public int getSBNUM()
        {
            return this.Sb;
        }

        public List<Tuple<int, Player>> GetSidePots()
        {
            return this.SidePots;
        }

        public Player GetFirstPlayerInRound()
        {
            return this.FirstPlayerInRound;
        }

        public SessionIdHandler GetsidHandler()
        {
            return this.sidHandler;
        }

        public bool GetUseComm()
        {
            return this.useCommunication;
        }
        public int GetlastRaiseInRound()
        {
            return this.lastRaiseInRound;
        }

        public int GetFirstPlayerInRoundPos()
        {
            return this.firstPlayerInRoundPoistion;
        }

        public LogControl GetLogControl()
        {
            return this.logControl;
        }

        public Decorator GetDecorator()
        {
            return this.MyDecorator;
        }

        public GameCenter GetGameCenter()
        {
            return this.GameCenter;
        }

        public ReplayManager GetRepManager()
        {
            return this.ReplayManager;
        }
        public GameReplay GetGameRepObj()
        {
            return this.GameReplay;
        }

        public Deck GetDeck()
        {
            return this.Deck;
        }

        public HandStep GetHandStep()
        {
            return this.Hand_Step;
        }
        private void SetTheBlinds()
        {
            Bb = MyDecorator.GetMinBetInRoom();
            Sb = Bb / 2;
            
        }

        public void SetDecorator(Decorator d)
        {
            MyDecorator = d;
            SetTheBlinds();
        }

        private IEnumerator<ActionResultInfo> GetEnumeratorToSend(List<Player> players, List<Spectetor> spectators,
            GameDataCommMessage gameData)
        {
            List<ActionResultInfo> actions = new List<ActionResultInfo>();
            foreach (var player in players)
            {
                //copy constructor deep copy
                var gmData = new GameDataCommMessage(gameData)
                {
                    PlayerCards =
                    {
                        [0] = player._firstCard,
                        [1] = player._secondCard
                    },
                    TotalChips = player.TotalChip,
                    UserId = player.user.Id()
                };

                //id of the user to send
                var action = new ActionResultInfo(player.user.Id(), gmData);
                actions.Add(action);
            }
            foreach (var spect in spectators)
            {
                //copy constructor deep copy
                var gmData = new GameDataCommMessage(gameData)
                {
                    UserId = spect.user.Id()
                };

                //id of the user to send
                var action = new ActionResultInfo(spect.user.Id(), gmData);
                actions.Add(action);
            }
            return actions.GetEnumerator();
        }

        public IEnumerator<ActionResultInfo> DoAction(IUser user, ActionType action, 
            int amount, bool useCommunication)
        {
            GameReplay = new GameReplay(Id, GameNumber);
            GameDataCommMessage gameData;
            List<ActionResultInfo> list;
            lock (padlock)
            {
                this.useCommunication = useCommunication;
                if (action == ActionType.SpectatorLeave)
                {
                    return RemoveSpectetorFromRoom(user);
                }
                if (action == ActionType.Join)
                {
                    if (IsUserInGame(user))
                    {
                        gameData = GetGameData(GetInGamePlayerFromUser(user), amount, false, ActionType.Join);
                        list = new List<ActionResultInfo> {new ActionResultInfo(user.Id(), gameData)};
                        return list.GetEnumerator();
                    }
                    return Join(user, amount);
                }
                if (!IsUserInGame(user))
                {
                    gameData = GetGameData(null, amount, false, ActionType.Join);
                    gameData.UserId = user.Id();
                    list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                    return list.GetEnumerator();
                }

                Player player = GetInGamePlayerFromUser(user);
                if (action == ActionType.StartGame)
                {
                    return StartGame(player);
                }
                if (action == ActionType.Leave)
                {
                    return Leave(player);
                }
                if (!IsActiveGame || (player.user.Id() != CurrentPlayer.user.Id()))
                {
                    gameData = GetGameData(player, amount, false, action);
                    list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                    return list.GetEnumerator();
                }
                if (action == ActionType.Fold)
                {
                    return Fold(player);
                }
                if (amount == 0)
                {
                    return Check(player);
                }
                if (amount > 0)
                {
                    return CallOrRaise(player, amount);
                }
            }
            gameData = GetGameData(null, amount, false, action);
            list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
            return list.GetEnumerator();
            
        }

        private IEnumerator<ActionResultInfo> Leave(Player player)
        {

            List<Player> relevantPlayers = new List<Player>();
            if (IsActiveGame)
            {
                LeaveAction leave = new LeaveAction(player);
                GameReplay.AddAction(leave);
            }
            SystemLog log = new SystemLog(Id, "Player with user Id: "
                                              + player.user.Id() + " left succsfully from room: " +Id, GameNumber);
            logControl.AddSystemLog(log);
            player.user.AddMoney(player.TotalChip - player.RoundChipBet);
            player.user.RemoveRoomFromActiveGameList(this);
            foreach (Player p in Players)
            {
                if (p.user.Id() != player.user.Id())
                {
                    relevantPlayers.Add(p);
                }
            }
            Players = relevantPlayers;
            GameDataCommMessage gameData;
            if (Players.Count == 0)
            {
                GameCenter.RemoveRoom(Id);
                gameData = GetGameData(player, 0, true, ActionType.Leave);
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator();
            }
            if (IsGameOver())
            {
                EndGame();
            }
            if (IsActiveGame)
            {
                FixRoles(player);
                if (AllDoneWithTurn())
                {
                    bool success = NextRound(player);
                    gameData = GetGameData(player, 0, success, ActionType.Leave);
                    List<Player> tP = new List<Player>();
                    tP.Add(player);
                    tP.AddRange(Players);
                    return GetEnumeratorToSend(tP, Spectatores, gameData);
                }
            }

            gameData = GetGameData(player, 0 , true, ActionType.Leave);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private List<int> GetAllPlayersAndSpectatoresIds()
        {
            List<int> ids = new List<int>();
            foreach(Player p in Players)
            {
                ids.Add(p.user.Id());
            }
            foreach(Spectetor s in Spectatores)
            {
                ids.Add(s.user.Id());
            }
            return ids;
        }

        private GameDataCommMessage GetGameDataFromSpectator(Spectetor s, int bet, bool success, ActionType action)
        {
            int userId = 0;
            string dealerName = "";
            string sbName = "";
            string bbName = "";
            string currName = "";
            string playerName = "";
            int money = 0;
            Card card1 = null, card2 = null;
            if (DealerPlayer != null)
            {
                dealerName = DealerPlayer.user.MemberName();
            }
            if (SbPlayer != null)
            {
                sbName = SbPlayer.name;
            }
            if (BbPlayer != null)
            {
                bbName = BbPlayer.name;
            }
            if (CurrentPlayer != null)
            {
                currName = CurrentPlayer.user.MemberName();
            }
            if (s != null)
            {
                userId = s.user.Id();
                playerName = s.user.MemberName();
            }
            List<string> allPlayerNames = GetPlayersNames();
            List<string> allSpecatatorNames = GetSpectatorNames();
            GameDataCommMessage gd = new GameDataCommMessage(userId, Id, sidHandler.GetSessionIdByUserId(userId),
                card1, card2, PublicCards, money, PotCount, allPlayerNames, allSpecatatorNames, dealerName,
                bbName, sbName, success, currName, playerName, bet, action, Hand_Step.ToString(), winner);
            return gd;
        }

        private GameDataCommMessage GetGameData(Player player, int bet, bool success, ActionType action)
        {
            int userId = 0;
            string dealerName = "";
            string sbName = "";
            string bbName = "";
            string currName = "";
            string playerName = "";
            int money = 0;
            Card card1 = null, card2 = null;
            if (DealerPlayer != null)
            {
                dealerName = DealerPlayer.user.MemberName();
            }
            if (SbPlayer != null)
            {
                sbName = SbPlayer.name;
            }
            if (BbPlayer != null)
            {
                bbName = BbPlayer.name;
            }
            if (CurrentPlayer != null)
            {
                currName = CurrentPlayer.user.MemberName();
            }
            if (player != null)
            {
                card1 = player._firstCard;
                card2 = player._secondCard;
                userId = player.user.Id();
                playerName = player.user.MemberName();
                money = player.TotalChip;
            }
            List<string> allPlayerNames = GetPlayersNames();
            List<string> allSpecatatorNames = GetSpectatorNames();
            GameDataCommMessage gd = new GameDataCommMessage(userId, Id, sidHandler.GetSessionIdByUserId(userId), 
                card1, card2, PublicCards , money, PotCount , allPlayerNames, allSpecatatorNames, dealerName,
                bbName, sbName, success, currName , playerName, bet, action, Hand_Step.ToString(), winner);
            return gd;
        }

        private List<string> GetPlayersNames()
        {
            return Players.ConvertAll(p => p.user.MemberName());
        }

        private List<string> GetSpectatorNames()
        {
            return Spectatores.ConvertAll(s => s.user.MemberName());
        }

        private bool FixRoles(Player playerLeaved)
        {
            if (playerLeaved == DealerPlayer)
            {
                DealerPos = (DealerPos + 1) % Players.Count;
                DealerPlayer = Players[DealerPos];
            }
            if (playerLeaved == SbPlayer)
            {
                SbPlayer = Players[(DealerPos + 1) % Players.Count];
            }
            if (playerLeaved == BbPlayer)
            {
                BbPlayer = Players[(DealerPos + 2) % Players.Count];
            }
            if (playerLeaved == CurrentPlayer)
            {
                return NextCurrentPlayer(0);
            }
            if (playerLeaved == FirstPlayerInRound)
            {
                firstPlayerInRoundPoistion = (firstPlayerInRoundPoistion) % Players.Count;
                FirstPlayerInRound = Players[firstPlayerInRoundPoistion];              
            }
  
            return true;
        }

        private IEnumerator<ActionResultInfo> Join(IUser user, int amount)
        {
            Player p = new Player(user, amount, Id);
            GameDataCommMessage gameData = GetGameData(p, amount, false, ActionType.Join);
            if (IsUserASpectator(user))
            {
                var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                return list.GetEnumerator(); 
            }
            if (MyDecorator.CanJoin(Players.Count , amount, user))
            {
                int moneyToReduce = MyDecorator.GetEnterPayingMoney() + amount;
                if (user.ReduceMoneyIfPossible(moneyToReduce))
                {
                    p.name = user.MemberName();
                    Players.Add(p);
                    gameData = GetGameData(p, amount, true, ActionType.Join);
                    return GetEnumeratorToSend(Players, Spectatores, gameData);
                }
                var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                return list.GetEnumerator(); 
            }
            var list2 = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
            return list2.GetEnumerator(); 
        }

        private bool IsUserASpectator(IUser user)
        {
            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == user.Id())
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator<ActionResultInfo> StartGame(Player player)
        {
            GameDataCommMessage gameData = GetGameData(player, 0, false, ActionType.StartGame);
            if (!MyDecorator.CanStartTheGame(Players.Count))
            {
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator();
            }
            if (IsActiveGame) //can't start an already active game
            {
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator();
            }
            Hand_Step = HandStep.PreFlop;
            Deck = new Deck();
            GameReplay = new GameReplay(Id, GameNumber);
            SystemLog log = new SystemLog(Id, "Game Started", GameNumber);
            logControl.AddSystemLog(log);
            SetRoles();
            StartGame startAction = new StartGame(Players, DealerPlayer, SbPlayer, BbPlayer);
            GameReplay.AddAction(startAction);
            SystemLog log2 = new SystemLog(Id, startAction.ToString(), GameNumber);
            logControl.AddSystemLog(log2);
            maxBetInRound = Bb;

            HandCardsAndInitPlayers(player);
            MoveBbnSBtoPot();

            IncGamesCounterForPlayers();
            IsActiveGame = true;
            foreach (Player p in Players)
            {
                p.user.AddRoomToActiveGameList(this);
            }
            gameData = GetGameData(player, 0, true, ActionType.StartGame);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private void IncGamesCounterForPlayers()
        {
            foreach (Player p in Players)
            {
                if (p.isPlayerActive)
                {
                    p.user.IncGamesPlay();
                }
            }
        }

        private IEnumerator<ActionResultInfo> CallOrRaise(Player player, int bet)
        {
            if (player.RoundChipBet + bet < maxBetInRound && !player.OutOfMoney()) // for all in
            {
                var gameData = GetGameData(player, bet, false, ActionType.Bet);
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator(); // need to bet atless maxBetInRound value
            }
            if (player.RoundChipBet + bet == maxBetInRound)
            {
                return Call(player, bet);
            }
            return Raise(player, bet);
        }

        private IEnumerator<ActionResultInfo> Raise(Player player, int bet)
        {
            GameDataCommMessage gameData = GetGameData(player, bet, false, ActionType.Bet);
           // ids.Add(player.user.Id());
            int currentPlayerBet = player.RoundChipBet + bet;
            int currentPlayerRaise = currentPlayerBet - maxBetInRound;
            if (!MyDecorator.CanRaise(lastRaiseInRound, currentPlayerRaise, maxBetInRound, player.RoundChipBet, PotCount, Hand_Step))
            {
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator(); // need to bet atless maxBetInRound value
            }
            if (player.TotalChip < bet) //not enough chips for bet maybe change to all in 
            {
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator(); // need to bet atless maxBetInRound value
            }
            maxBetInRound = currentPlayerBet;
            player.PlayedAnActionInTheRound = true;
            player.CommitChips(bet);
            PotCount += bet;
            RaiseAction raise = new RaiseAction(player, player._firstCard,
                 player._secondCard, currentPlayerBet);
            GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(Id, raise.ToString(), GameNumber);
            logControl.AddSystemLog(log);
            lastRaiseInRound = currentPlayerRaise;
            foreach (Player p in Players) //they all need to make another action in this round
            {
                if (p != player)
                {
                    p.PlayedAnActionInTheRound = false;
                }
            }
            bool afterAction = AfterAction(player);
            gameData = GetGameData(player, bet, afterAction, ActionType.Bet);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private IEnumerator<ActionResultInfo> Call(Player player, int bet)
        {
            player.PlayedAnActionInTheRound = true;
            bet = Math.Min(bet, player.TotalChip); // if can't afford that many chips in a call, go all in           
            player.CommitChips(bet);
            PotCount += bet;
            CallAction call = new CallAction(player, player._firstCard,
                player._secondCard, bet);
            GameReplay.AddAction(call);
            SystemLog log = new SystemLog(Id, call.ToString(), GameNumber);
            logControl.AddSystemLog(log);
            List<int> ids = GetAllPlayersAndSpectatoresIds();
            ids.Remove(player.user.Id());
            bool afterAction = AfterAction(player);
            GameDataCommMessage gameData = GetGameData(player, bet, afterAction, ActionType.Bet);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private IEnumerator<ActionResultInfo> Check(Player player)
        {
            GameDataCommMessage gameData;
            if (player.RoundChipBet < maxBetInRound && !player.OutOfMoney()) // for all in
            {
                gameData = GetGameData(player, 0, false, ActionType.Bet);
                var list = new List<ActionResultInfo> { new ActionResultInfo(player.user.Id(), gameData) };
                return list.GetEnumerator(); // need to bet atless maxBetInRound value
            }
            player.PlayedAnActionInTheRound = true;
            CheckAction check = new CheckAction(player, player._firstCard,
                 player._secondCard);
            SystemLog log = new SystemLog(Id, check.ToString(), GameNumber);
            logControl.AddSystemLog(log);
            GameReplay.AddAction(check);
            bool afterAction = AfterAction(player);
            gameData = GetGameData(player, 0, afterAction, ActionType.Bet);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private IEnumerator<ActionResultInfo> Fold(Player player)
        {
            player.PlayedAnActionInTheRound = true;
            player.isPlayerActive = false;
            FoldAction fold = new FoldAction(player, player._firstCard,
                player._secondCard);
            GameReplay.AddAction(fold);
            SystemLog log = new SystemLog(Id, fold.ToString(), GameNumber);
            logControl.AddSystemLog(log);
            List<int> ids = GetAllPlayersAndSpectatoresIds();
            ids.Remove(player.user.Id());
            bool afterAction = AfterAction(player);
            GameDataCommMessage gameData = GetGameData(player, 0, afterAction, ActionType.Fold);
            
            return GetEnumeratorToSend(Players, Spectatores, gameData);
        }

        private bool AfterAction(Player doNotSend)
        {
            if (IsGameOver())
            {
                EndGame();
                return true;
            }
            else if (AllDoneWithTurn() )
            {
                return NextRound(doNotSend);
            }
            return NextCurrentPlayer(1);
        }

        private bool NextRound(Player doNotSend)
        {
            lastRaiseInRound = 0;
            maxBetInRound = 0;
            InitializePlayerRound();

            if (Hand_Step == HandStep.River) 
            {
                return EndGame(); 
            }

            ProgressHand(doNotSend);
            FindFirstPlayerInRound();
            return true;
        }

        private void FindFirstPlayerInRound()
        {
            if (Players.Contains(FirstPlayerInRound))
            {
                CurrentPlayer = FirstPlayerInRound;
                currentPlayerPos = firstPlayerInRoundPoistion;
                return;
            }
            int i = 0;
            while (i <= Players.Count)
            {
                int newPosition = (firstPlayerInRoundPoistion + i) % Players.Count;
                if (Players[newPosition].isPlayerActive)
                {
                    currentPlayerPos = newPosition;
                    CurrentPlayer = Players[newPosition];
                    FirstPlayerInRound = CurrentPlayer;
                    firstPlayerInRoundPoistion = newPosition;
                    return;
                }
            }
        }

        private bool EndGame()
        {
            GameCenter.UpdateExistingGameDB(this);
            GameNumber++;
            List<Player> playersLeftInGame = new List<Player>();
            foreach (Player player in Players)
            {
                if (player.isPlayerActive)
                {
                    playersLeftInGame.Add(player);
                }
            }
            List<HandEvaluator> Winners = FindWinner(PublicCards, playersLeftInGame);
            List<int> ids = new List<int>();
            foreach (Player player in Players)
            {
                ids.Add(player.user.Id());
            }
            ReplayManager.AddGameReplay(GameReplay, ids);
            if (Winners.Count > 0) // so there are winners at the end of the game
            {
                int amount = PotCount / Winners.Count;

                foreach (HandEvaluator h in Winners)
                {
                    h._player.Win(amount);
                }
            }
            var winningPlayers = Winners.ConvertAll(win => win._player);
            var loosingPlayers = Players.Except(winningPlayers);
            foreach (var loser in loosingPlayers)
            {
                loser.Lose();
            }
            playersLeftInGame = new List<Player>();
            foreach (Player player in Players)
            {
                player.ClearCards(); // gets rid of cards of all players
                player.isPlayerActive = false;
                if (!player.OutOfMoney())
                {
                    playersLeftInGame.Add(player);
                }
            }
            Players = playersLeftInGame;
            IsActiveGame = false;
            ClearPublicCards();
            maxBetInRound = 0;
            lastRaiseInRound = 0;
            GameReplay = new GameReplay(Id, GameNumber);
            //Hand_Step = HandStep.PreFlop;
            GameCenter.InsertNewGameInExistingRoomDB(this);
            return true;
        }

        private void ProgressHand(Player doNotSend)
        {
            int nextStep = (int)Hand_Step + 1;
            Hand_Step = (HandStep)nextStep;

            switch (Hand_Step)
            {   //wont get to "pre flop" case
                case HandStep.PreFlop:
                    break;
                case HandStep.Flop:
                    for (int i = 0; i <= 2; i++)
                    {
                       AddNewPublicCard();
                    }
                    break;
                case HandStep.Turn:
                    AddNewPublicCard();
                    break;
                case HandStep.River:
                    AddNewPublicCard();
                    break;

                default:
                    return;
            }

            if (ActivePlayersInGame() - PlayersAllIn() < 2)
            {
                ProgressHand(doNotSend); // recursive, runs until we'll hit the river
            }
        }

        private bool NextCurrentPlayer(int startingIndex)
        {
            while(startingIndex <= Players.Count)
            {
                int newPosition = (currentPlayerPos + startingIndex) % Players.Count;
                if (Players[newPosition].isPlayerActive)
                {
                    currentPlayerPos = newPosition;
                    CurrentPlayer = Players[newPosition];
                    return true;
                }
            }
            return false;
        }

        private bool IsGameOver()
        {
            if (!IsActiveGame || ActivePlayersInGame() < 2)
            {
                this.IsActiveGame = false;
                return true;
            }
            return false;
        }

        //@TODO send a message to user saying he is not part of the game and cant do action
        private bool IrellevantUser(IUser user, ActionType action)
        {
            return false;
        }

        private bool IsUserInGame(IUser user)
        {
            return (GetInGamePlayerFromUser(user) != null);
        }

        private Player GetInGamePlayerFromUser(IUser user)
        {
            foreach(Player player in Players)
            {
                if (player.user.Id() == user.Id())
                {
                    return player;
                }
            }
            return null;
        }

        private void SetRoles()
        {
            if (DealerPlayer == null)
            {
                DealerPos = 0;
            }
            else
            {
                DealerPos = (DealerPos + 1) % Players.Count;
            }

            DealerPlayer = Players[DealerPos];
            SbPlayer = Players[(DealerPos + 1) % Players.Count];
            BbPlayer = Players[(DealerPos + 2) % Players.Count];
            currentPlayerPos = (DealerPos + 3) % Players.Count;
            FirstPlayerInRound = Players[currentPlayerPos];
            firstPlayerInRoundPoistion = currentPlayerPos;
            CurrentPlayer = FirstPlayerInRound;          
        }

        private void HandCardsAndInitPlayers(Player doNotSend)
        {
            foreach (Player player in Players)
            {
                player.InitForNewGame();
                player.Add2Cards(Deck.Draw(), Deck.Draw());
                HandCards hand = new HandCards(player, player._firstCard,
                    player._secondCard);
                GameReplay.AddAction(hand);
                SystemLog log = new SystemLog(Id, hand.ToString(), GameNumber);
                logControl.AddSystemLog(log);
                //if (player.user.Id() != doNotSend.user.Id())
                //{
                //    GameDataCommMessage gameData = GetGameData(player, 0, true, ActionType.HandCard);
                //    List<int> ids = new List<int> {player.user.Id()};
                //    clientSender.SendMessageToClient(this, gameData, ids, useCommunication); 
                //}
            }
        }
        
       private void AddNewPublicCard()
        {
            Card c = Deck.ShowCard();
            foreach (Player player in Players)
            {
                player.AddPublicCardToPlayer(c);
            }
            PublicCards.Add(Deck.Draw());
            DrawCard draw = new DrawCard(c, PublicCards, PotCount);
            GameReplay.AddAction(draw);
        }

        private void ClearPublicCards()
        {
            PublicCards.Clear();
        }

        private void UpdateMaxCommitted()
        {
            foreach (Player player in Players)
                if (player.RoundChipBet > maxBetInRound)
                    maxBetInRound = player.RoundChipBet;
        }

        private void InitPlayersLastAction()
        {
            foreach (Player player in Players)
            {
                if (player.isPlayerActive)
                {
                    player.PlayedAnActionInTheRound = false;
                }
            }
        }

        private void MoveChipsToPot()
        {
            foreach (Player player in Players)
            {
                player.TotalChip -= player.RoundChipBet;
                player.RoundChipBet = 0;               
            }
        }

        private int ActivePlayersInGame()
        {
            int playersInGame = 0;
            foreach (Player player in Players)
            {
                if (player.isPlayerActive)
                {
                    playersInGame++;
                }
            }
            return playersInGame;

        }

        private int PlayersAllIn()
        {
            int playersAllIn = 0;
            foreach (Player player in Players)
            {
                if (player.IsAllIn())
                {
                    playersAllIn++;
                }
            }
            return playersAllIn;
        }

        private bool AllDoneWithTurn()
        {
            bool allDone = true;
            foreach (Player player in Players)
            {
                if (player.isPlayerActive && !player.PlayedAnActionInTheRound)
                {
                    allDone = false;
                }
            }
            return allDone;
        }
  
        private void MoveBbnSBtoPot()
        {
            PotCount = Bb + Sb;
            SbPlayer.CommitChips(Sb);
            BbPlayer.CommitChips(Bb);
        }

        private void InitializePlayerRound()
        {
            foreach (Player player in Players)
            {
                player.InitPayInRound();
            }
        }

        public List<HandEvaluator> FindWinner(List<Card> table, List<Player> playersLeftInHand)
        {
            List<HandEvaluator> winners = new List<HandEvaluator>();
            foreach (Player p in playersLeftInHand)
            {
                HandEvaluator h = new HandEvaluator(p);
                List<Card> playerCards = new List<Card>();
                playerCards.AddRange(table);
                playerCards.AddRange(p.GetCards());
                Card[] cards = playerCards.ToArray();
                h.DetermineHandRank(cards);
                if (winners.Count == 0)
                {
                    winners.Add(h);
                    continue;
                }
                if (h._rank.CompareTo(winners.ElementAt(0)._rank) > 0)
                {
                    winners.Clear();
                    winners.Add(h);
                }
                else if (h._rank.CompareTo(winners.ElementAt(0)._rank) == 0)
                {
                    winners.Add(h);
                }
            }
            if (winners.Count() == 1)
            {
                WinAction win = new WinAction(winners[0]._player,
                    winners[0]._player._firstCard, winners[0]._player._secondCard,
                    PotCount, table, winners[0]._relevantCards);
                GameReplay.AddAction(win);
                SystemLog log = new SystemLog(Id, win.ToString(), GameNumber);
                logControl.AddSystemLog(log);
                winner = GetWinnerString(winners);
                return winners;
            }
            return EvalTies(winners, table);
        }

        private string GetWinnerString(List<HandEvaluator> winners)
        {
            var winnerNames = winners.ConvertAll<string>(w => w._player.user.MemberName());
            string names = "";
            foreach (string w in winnerNames)
            {
                names += w + ", ";
            }
            if (names.Length > 2)
            {
                return names.Substring(0, names.Length - 2);
            }
            return names;
        }

        private List<HandEvaluator> EvalTies(List<HandEvaluator> winners, List<Card> table)
        {
            int i = 1;
            while (winners.Count >= 2 && i < winners.Count())
            {
                var playerOneCards = winners.ElementAt(i - 1)._relevantCards;
                var playerTwoCards = winners.ElementAt(i)._relevantCards;
                FixAceTo14(playerOneCards);
                FixAceTo14(playerTwoCards);
                playerOneCards = playerOneCards.OrderByDescending(o => o._value).ToList();
                playerTwoCards = playerTwoCards.OrderByDescending(o => o._value).ToList();
                var tie = true;
                for (int j = 0; j < playerOneCards.Count && j < playerTwoCards.Count; j++)
                {
                    if (playerOneCards.ElementAt(j)._value > playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i);
                        tie = false;
                        break;
                    }
                    if (playerOneCards.ElementAt(j)._value < playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i - 1);
                        tie = false;
                        break;
                    }
                }
                if (tie)
                {
                    i++;
                }
                FixAceTo1(playerOneCards);
                FixAceTo1(playerTwoCards);
            }
            foreach (HandEvaluator h in winners)
            {
                WinAction win = new WinAction(h._player,
                     h._player._firstCard, h._player._secondCard,
                     PotCount / winners.Count, table, h._relevantCards);
                GameReplay.AddAction(win);
                SystemLog log = new SystemLog(Id, win.ToString(), GameNumber);
                // this.this._gameCenter.AddSystemLog(log);
                logControl.AddSystemLog(log);
            }
            winner = GetWinnerString(winners);
            return winners;
        }

        private void FixAceTo14(List<Card> cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == 1) //ACE
                {
                    c._value = 14;
                }
            }
        }

        private void FixAceTo1(List<Card> cards)
        {
            foreach (Card c in cards)
            {
                if (c._value == (14)) //ACE
                {
                    c._value = 1;
                }
            }
        }

        public IEnumerator<ActionResultInfo> AddSpectetorToRoom(IUser user)
        {           
            //if user is player in room cant be also spectetor
            GameDataCommMessage gameData;
            foreach (Player p in Players)
            {
                if (p.user.Id() == user.Id())
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: "
                        + user.Id() + " to room: " +Id + " user is already a player in this room");
                    logControl.AddErrorLog(log);
                    gameData = GetGameData(p, 0, false, ActionType.Spectate);
                    var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                    return list.GetEnumerator();
                }
            }
            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == user.Id())
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: "
                        + user.Id() + " to room: " + Id + " user is already a player in this room");
                    logControl.AddErrorLog(log);
                    gameData = GetGameDataFromSpectator(s, 0, false, ActionType.Spectate);
                    var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
                    return list.GetEnumerator();        
                }
            }

            //user.AddRoomToSpectetorGameList(this);
            Spectetor spectetor = new Spectetor(user, Id);
            Spectatores.Add(spectetor);
            spectetor.user.AddRoomToSpectetorGameList(this);
            Player player = Players.Find(p => p.user.Id() == user.Id());
            gameData = GetGameData(player, 0, true, ActionType.Spectate);
            return GetEnumeratorToSend(Players, Spectatores, gameData);
       }

        private IEnumerator<ActionResultInfo> RemoveSpectetorFromRoom(IUser user)
        {
            GameDataCommMessage gameData;

            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == user.Id())
                {
                    SystemLog log =
                        new SystemLog(Id, "Spcetator with user Id: " + user.Id() + ", Removed succsfully from room: " + Id, GameNumber);
                    logControl.AddSystemLog(log);
                    Spectatores.Remove(s);
                    user.RemoveRoomFromSpectetorGameList(this);
                    gameData = GetGameDataFromSpectator(s, 0, true, ActionType.SpectatorLeave);
                    return GetEnumeratorToSend(Players, Spectatores, gameData);
                }
            }

            Spectetor spectetor = new Spectetor(user, Id);
            gameData = GetGameDataFromSpectator(spectetor, 0, false, ActionType.SpectatorLeave);
            var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
            return list.GetEnumerator();
        }

        public bool CanJoin(IUser user)
        {
            return MyDecorator.CanJoin(Players.Count, user.Money(), user);
        }

        public bool IsGameActive()
        {
            return IsActiveGame;
        }


        public bool IsSpectatable()
        {
            return MyDecorator.CanSpectatble();
        }

        public bool IsPotSizeEqual(int potSize)
        {
            return PotCount == potSize;
        }

        public bool IsGameModeEqual(GameMode gm)
        {
            return MyDecorator.IsGameModeEqual(gm);
        }

        public bool IsGameBuyInPolicyEqual(int buyIn)
        {
            return MyDecorator.IsGameBuyInPolicyEqual(buyIn);
        }

        public bool IsGameMinPlayerEqual(int min)
        {
            return MyDecorator.IsGameMinPlayerEqual(min);
        }

        public bool IsGameMaxPlayerEqual(int max)
        {
            return MyDecorator.IsGameMaxPlayerEqual(max);
        }

        public bool IsGameMinBetEqual(int minBet)
        {
            return MyDecorator.IsGameMinBetEqual(minBet);
        }

        public bool IsGameStartingChipEqual(int startingChip)
        {
            return MyDecorator.IsGameStartingChipEqual(startingChip);
        }

        public List<Player> GetPlayersInRoom()
        {
            return Players;
        }


        public List<Spectetor> GetSpectetorInRoom()
        {
            return Spectatores;
        }

      

        //Getters for search display in GUI
        public int GetMinPlayer()
        {
            return MyDecorator.GetMinPlayerInRoom();
        }

        public int GetMinBet()
        {
            return MyDecorator.GetMinBetInRoom();
        }

        public int GetMaxPlayer()
        {
            return MyDecorator.GetMaxPlayerInRoom();
        }

        public int GetPotSize()
        {
            return PotCount;
        }

        public int getBBnum()
        {
            return this.Bb;
        }
        public int GetBuyInPolicy()
        {
            return MyDecorator.GetEnterPayingMoney();
        }

        public int GetStartingChip()
        {
            return MyDecorator.GetStartingChip();
        }

        public GameMode GetGameMode()
        {
            return MyDecorator.GetGameMode();
        }

        public LeagueName GetLeagueName()
        {
            return MyDecorator.GetLeagueName();
        }

        public HandStep GetStep()
        {
            return Hand_Step;
        }

        //for chat
        public Player GetSb()
        {
            return SbPlayer;
        }

        public Player GetCurrPlayer()
        {
            return CurrentPlayer;
        }

        public int GetCurrPosition()
        {
            return currentPlayerPos;
        }

        public bool IsPlayerInRoom(IUser user)
        {
            GameReplay = new GameReplay(Id, GameNumber);
            GameDataCommMessage gameData;
            List<ActionResultInfo> list;
            bool toReturn = false;
            lock (padlock)
            {
                foreach(Player player in Players)
                {
                    if(player.user.Id() == user.Id())
                    {
                        toReturn = true;
                        return toReturn;
                    }
                }
            }
            return toReturn;
        }

        public bool IsSpectetorInRoom(IUser user)
        {
            bool toReturn = false;
            lock (padlock)
            {
                foreach (Spectetor spectetor in Spectatores)
                {
                    if (spectetor.user == user)
                    {
                        toReturn = true;
                        return toReturn;
                    }
                }
            }
            return toReturn;
        }

        public IEnumerator<ActionResultInfo> ReturnToGameAsPlayer(IUser user)
        {
            bool success = IsUserInGame(user);
            GameDataCommMessage gameData;
            if (success)
            {
                gameData = GetGameData(GetInGamePlayerFromUser(user), 0, true, ActionType.ReturnToGame);
            }
            else
            {
                gameData = GetGameData(null, 0, false, ActionType.ReturnToGame);
            }
            var list = new List<ActionResultInfo> {new ActionResultInfo(user.Id(), gameData)};
            return list.GetEnumerator();
        }

        public IEnumerator<ActionResultInfo> ReturnToGameAsSpec(IUser user)
        {
            Spectetor spect = Spectatores.Find(s => s.user.Id() == user.Id());
            bool success = spect != null;
            var gameData = GetGameData(null, 0, success, ActionType.ReturnToGame);
            var list = new List<ActionResultInfo> { new ActionResultInfo(user.Id(), gameData) };
            return list.GetEnumerator();
        }

        public List<Card> GetPublicCards()
        {
            return PublicCards;
        }

        public Player GetDealer()
        {
            return DealerPlayer;
        }

        public Player GetBb()
        {
            return BbPlayer;
        }
    }
}
