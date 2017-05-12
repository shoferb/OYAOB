using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;
using static TexasHoldemShared.CommMessages.CommunicationMessage;

namespace TexasHoldem.Logic.Game
{
    public class GameRoom : IGame
    {
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int Id { get; set; }

        private List<Player> Players;
        private List<Spectetor> Spectatores;
        private int DealerPos;
        private int maxBetInRound;
        private int PotCount;
        private int Bb;
        private int Sb;
        private Deck Deck;
        private GameRoom.HandStep Hand_Step;
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

        private LeagueName league;
        private static readonly object padlock = new object();

        public GameRoom(List<Player> players, int ID)
        {
            Id = ID;
            GameNumber = 0;
            IsActiveGame = false;
            PotCount = 0;          
            maxBetInRound = 0;
            PublicCards = new List<Card>();
            Players = players;
            Spectatores = new List<Spectetor>();
            Bb = 0;
            Sb = 0;
            SidePots = new List<Tuple<int, Player>>();
            DealerPlayer = null;
            logControl = LogControl.Instance;
            league = GetLeagueFromPlayer(Players);
            ReplayManager = ReplayManager.ReplayManagerInstance;
            GameCenter = GameCenter.Instance;
        }

        private LeagueName GetLeagueFromPlayer(List<Player> players)
        {
            if (players == null || players.Count == 0)
            {
                return LeagueName.A; //defult league
            }
            return players[0].user.GetLeague();
        }

        private void SetTheBlinds()
        {
            this.Bb = MyDecorator.GetMinBetInRoom();
            this.Sb = (int)Bb / 2;
            
        }

        public void AddDecorator(Decorator d)
        {
            this.MyDecorator = d;
            SetTheBlinds();
        }

        public bool DoAction(IUser user, ActionType action, int amount)
        {
            lock (padlock)
            {       
                if (action == ActionType.Join)
                {
                    if (IsUserInGame(user))
                    {
                        return false;
                    }
                    return Join(user, amount);
                }
                if (!IsUserInGame(user))
                {
                    return IrellevantUser(user, action);
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
                if (!IsActiveGame) { return false; }
                if (player != CurrentPlayer)
                {
                    return IrellevantUser(user, action);
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
            return false;
        }

        private bool Leave(Player player)
        {
            GameData gameData = GetGameData();

            List<Player> relevantPlayers = new List<Player>();
            if (IsActiveGame)
            {
                LeaveAction leave = new LeaveAction(player);
                GameReplay.AddAction(leave);
            }
            SystemLog log = new SystemLog(Id, "Player with user Id: "
                + player.user.Id() + " left succsfully from room: " +Id);
            logControl.AddSystemLog(log);
            GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Leave, true);
            player.user.AddMoney(player.TotalChip - player.RoundChipBet);
            player.user.RemoveRoomFromActiveGameList(this);
            foreach (Player p in this.Players)
            {
                if (p.user.Id() != player.user.Id())
                {
                    relevantPlayers.Add(p);
                }
            }
            Players = relevantPlayers;
            if (Players.Count == 0)
            {
                GameCenter.RemoveRoom(Id);
                return true;
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
                    return NextRound();
                }
            }
            return true; 
        }

        private GameData GetGameData()
        {
            string dealerName = "";
            string sbName = "";
            string bbName = "";
            if (DealerPlayer != null)
            {
                dealerName = DealerPlayer.name;
            }
            if (SbPlayer != null)
            {
                sbName = SbPlayer.name;
            }
            if (BbPlayer != null)
            {
                bbName = BbPlayer.name;
            }
            GameData gd = new GameData(PublicCards, MyDecorator.GetStartingChip(), PotCount, Players, dealerName,
                        bbName, sbName);
            return gd;
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

        private bool Join(IUser user, int amount)
        {
            Player p = new Player(user, amount, this.Id);
            GameData gameData = GetGameData();

            if (IsUserASpectator(user))
            {
                GameCenter.SendMessageToClient(p, Id, gameData, ActionType.Join, false);
                return false;
            }
            if (MyDecorator.CanJoin(Players.Count , amount, user))
            {
                int moneyToReduce = MyDecorator.GetEnterPayingMoney() + amount;
                if (user.ReduceMoneyIfPossible(moneyToReduce)){
                    this.Players.Add(p);
                    GameCenter.SendMessageToClient(p, Id, gameData, ActionType.Join, true);
                    return true;
                }
                GameCenter.SendMessageToClient(p, Id, gameData, ActionType.Join, false);
                return false;
            }
            GameCenter.SendMessageToClient(p, Id, gameData, ActionType.Join, false);
            return false;
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

        private bool StartGame(Player player)
        {
            GameData gameData = GetGameData();
            if (!MyDecorator.CanStartTheGame(Players.Count))
            {
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.StartGame, false);
                return false;
            }
            if (IsActiveGame == true) //can't start an already active game
            {
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.StartGame, false);
                return false;
            }
            Hand_Step = HandStep.PreFlop;
            Deck = new Deck();
            GameReplay = new GameReplay(Id, GameNumber);
            SystemLog log = new SystemLog(Id, "Game Started");
            logControl.AddSystemLog(log);
            SetRoles();
            StartGame startAction = new StartGame(this.Players, DealerPlayer, SbPlayer, BbPlayer);
            this.GameReplay.AddAction(startAction);
            SystemLog log2 = new SystemLog(this.Id, startAction.ToString());
            logControl.AddSystemLog(log2);
            GameCenter.SendMessageToClient(player, Id, gameData, ActionType.StartGame, true);
            MoveBbnSBtoPot();
            maxBetInRound = Bb;

            HandCardsAndInitPlayers();
            IncGamesCounterForPlayers();
            IsActiveGame = true;
            return true;
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

        private bool CallOrRaise(Player player, int bet)
        {
            if (player.RoundChipBet + bet == maxBetInRound)
            {
                return Call(player, bet);
            }
            return Raise(player, bet);
        }

        private bool Raise(Player player, int bet)
        {
            GameData gameData = GetGameData();

            int currentPlayerBet = player.RoundChipBet + bet;
            if (!MyDecorator.CanRaise(currentPlayerBet, maxBetInRound, Hand_Step))
            {
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Bet, false);
                return false;
            }
            if (player.TotalChip < bet) //not enough chips for bet maybe change to all in 
            {
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Bet, false);
                return false;  
            }
            maxBetInRound = currentPlayerBet;
            player.PlayedAnActionInTheRound = true;
            player.CommitChips(bet);
            RaiseAction raise = new RaiseAction(player, player._firstCard,
                 player._secondCard, currentPlayerBet);
            GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this.Id, raise.ToString());
            logControl.AddSystemLog(log);
            GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Bet, true);

            foreach (Player p in Players) //they all need to make another action in this round
            {
                if (p != player)
                {
                    p.PlayedAnActionInTheRound = false;
                }
            }
            return AfterAction();
        }

        private bool Call(Player player, int bet)
        {
            GameData gameData = GetGameData();
            player.PlayedAnActionInTheRound = true;
            bet = Math.Min(bet, player.TotalChip); // if can't afford that many chips in a call, go all in           
            player.CommitChips(bet);
            CallAction call = new CallAction(player, player._firstCard,
                player._secondCard, bet);
            GameReplay.AddAction(call);
            SystemLog log = new SystemLog(this.Id, call.ToString());
            logControl.AddSystemLog(log);
            GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Bet, true);
            return AfterAction();
        }

        private bool Check(Player player)
        {
            GameData gameData = GetGameData();
            player.PlayedAnActionInTheRound = true;
            CheckAction check = new CheckAction(player, player._firstCard,
                 player._secondCard);
            SystemLog log = new SystemLog(this.Id, check.ToString());
            logControl.AddSystemLog(log);
            GameReplay.AddAction(check);
            GameCenter.SendMessageToClient(player, Id, gameData, ActionType.Bet, true);
            return AfterAction();
        }

        private bool Fold(Player player)
        {
            GameData gameData = GetGameData();
            player.PlayedAnActionInTheRound = true;
            player.isPlayerActive = false;
            FoldAction fold = new FoldAction(player, player._firstCard,
                player._secondCard);
            GameReplay.AddAction(fold);
            SystemLog log = new SystemLog(this.Id, fold.ToString());
            logControl.AddSystemLog(log);
            GameCenter.SendMessageToClient(player, this.Id, gameData, ActionType.Fold, true);

            return AfterAction();
        }

        private bool AfterAction()
        {
            if (IsGameOver())
            {
                EndGame();
            }
            if (AllDoneWithTurn() )
            {
                return NextRound();
            }
            return NextCurrentPlayer(1);
        }

        private bool NextRound()
        {
            MoveChipsToPot();

            InitializePlayerRound();

            if (Hand_Step == HandStep.River) 
            {
                return EndGame(); 
            }

            ProgressHand();
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
            this.GameNumber++;
            List<Player> playersLeftInGame = new List<Player>();
            foreach (Player player in this.Players)
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
                int amount = this.PotCount / Winners.Count;

                foreach (HandEvaluator h in Winners)
                {
                    h._player.Win(amount);
                }
            }
            playersLeftInGame = new List<Player>();
            foreach (Player player in this.Players)
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
            GameReplay = new GameReplay(Id, GameNumber);
            return true;
        }

        private void ProgressHand()
        {
            int nextStep = (int)Hand_Step + 1;
            Hand_Step = (GameRoom.HandStep)nextStep;

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

            if (this.ActivePlayersInGame() - this.PlayersAllIn() < 2)
            {
                ProgressHand(); // recursive, runs until we'll hit the river
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

        private void HandCardsAndInitPlayers()
        {
            GameData gameData = GetGameData();
            foreach (Player player in this.Players)
            {
                player.InitForNewGame();
                player.Add2Cards(Deck.Draw(), Deck.Draw());
                HandCards hand = new HandCards(player, player._firstCard,
                    player._secondCard);
                GameReplay.AddAction(hand);
                SystemLog log = new SystemLog(this.Id, hand.ToString());
                logControl.AddSystemLog(log);
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.HandCard, true);

            }
        }
        
       private void AddNewPublicCard()
        {
            GameData gameData = GetGameData();
            Card c = Deck.ShowCard();
            foreach (Player player in Players)
            {
                player.AddPublicCardToPlayer(c);
                GameCenter.SendMessageToClient(player, Id, gameData, ActionType.HandCard, true);

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
                PotCount += player.RoundChipBet;
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
            foreach (Player player in this.Players)
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
                    this.PotCount, table, winners[0]._relevantCards);
                this.GameReplay.AddAction(win);
                SystemLog log = new SystemLog(this.Id, win.ToString());
                logControl.AddSystemLog(log);
                return winners;
            }
            return EvalTies(winners, table);
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
                    else if (playerOneCards.ElementAt(j)._value < playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i - 1);
                        tie = false;
                        break;
                    }
                }
                if (tie == true)
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
                     (int)this.PotCount / winners.Count, table, h._relevantCards);
                this.GameReplay.AddAction(win);
                SystemLog log = new SystemLog(this.Id, win.ToString());
                // this.this._gameCenter.AddSystemLog(log);
                logControl.AddSystemLog(log);
            }
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

        private int GetRaisePotLimit(Player p)
        {

            int potSize = this.PotCount;
            int lastRise = this.maxBetInRound;
            int playerPayInRound = p.RoundChipBet;
            int toReturn = (lastRise - playerPayInRound) + potSize;
            return toReturn;
        }

        public bool AddSpectetorToRoom(IUser user)
        {           
            //if user is player in room cant be also spectetor
            foreach (Player p in Players)
            {
                if (p.user.Id() == user.Id())
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: "
                        + user.Id() + " to room: " +Id + " user is already a player in this room");
                    logControl.AddErrorLog(log);
                    return false;
                }
            }
           
            user.AddRoomToSpectetorGameList(this);
            Spectetor spectetor = new Spectetor(user, Id);
            Spectatores.Add(spectetor);               
            return true;
       }

        public bool RemoveSpectetorFromRoom(IUser user)
        {          
            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == user.Id())
                {
                    SystemLog log =
                        new SystemLog(this.Id, "Spcetator with user Id: " + user.Id() + ", Removed succsfully from room: " + Id);
                    Spectatores.Remove(s);
                    user.RemoveRoomFromSpectetorGameList(this);
                    return true;
                }
            }
            return false;
        }

        public bool CanJoin(IUser user)
        {
            return MyDecorator.CanJoin(Players.Count, user.Money(), user);
        }

        public bool IsGameActive()
        {
            return this.IsActiveGame;
        }


        public bool IsSpectatable()
        {
            return MyDecorator.CanSpectatble();
        }

        public bool IsPotSizeEqual(int potSize)
        {
            return this.PotCount == potSize;
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
            return this.Players;
        }


        public List<Spectetor> GetSpectetorInRoom()
        {
            return this.Spectatores;
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
            return this.PotCount;
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
    }
}
