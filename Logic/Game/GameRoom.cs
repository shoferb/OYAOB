using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.communication.Converters;
using static TexasHoldemShared.CommMessages.CommunicationMessage;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.Logic.Game
{
    public class GameRoom : IGame
    {
        public List<Player> Players { get; set; }
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int Id { get; set; }
        public List<Spectetor> Spectatores { get; set;}
        public int DealerPos { get; set; }
        public int maxBetInRound { get; set; } //TODO: should move to decorator
        public int ActionPos { get; set; }
        public int PotCount { get; set; }
        public int Bb { get; set; }
        public int Sb { get; set; }
        public Deck Deck { get; set; }
        public GameRoom.HandStep Hand_Step { get; set; }
        public List<Card> Cards { get; set; }
        public List<Card> PublicCards { get; set; }
        public bool IsActiveGame { get; set; }
        public List<Tuple<int, List<Player>>> SidePots { get; set; }
        public GameReplay GameReplay { get; set; }
        public ReplayManager ReplayManager;
        public GameCenter GameCenter; 
        public int VerifyAction { get; set; }
        public bool GameOver = false;
        public Player CurrentPlayer;
        public Player DealerPlayer;
        public Player BbPlayer;
        public Player SbPlayer;
        public List<HandEvaluator> Winners;
        private int _buttonPos;
        private bool _backFromRaise;
        public bool IsTestMode { get; set; } //TODO: maybe not relevant anymore?
        public Decorator MyDecorator;
        public int MaxRaiseInThisRound { get; set; }  
        public int MinRaiseInThisRound { get; set; } 
        public int LastRaise { get; set; }  //change to maxCommit
        public Thread RoomThread { get; set; }
        //new after log control change
        private LogControl _logControl = LogControl.Instance;
        public int GameNumber=0;
        private Player lastPlayerRaisedInRound;
        private Player FirstPlayerInRound;
        private int currentPlayerPos;
        private bool someOneRaised;

        public int MinBetInRoom { get; set; }
        private int _currLoaction { get; set; }
        private int _roundCounter { get; set; }
        public int MaxRank { get; set; }
        public int MinRank { get; set; }
        public GameRoom(List<Player> players, int ID)
        {
            this.Id = ID;
            this.IsActiveGame = false;
            this.PotCount = 0;          
            this.Players = players;
            this.maxBetInRound = 0;
            this.PublicCards = new List<Card>();
            SetTheBlinds();
            this.SidePots = new List<Tuple<int, List<Player>>>();
            Tuple<int,int> tup = GameCenter.UserLeageGapPoint(players[0].user.Id());
            this.MinRank = tup.Item1;
            this.MaxRank = tup.Item2;
            this.DealerPlayer = null;
        }

        private void SetTheBlinds()
        {
            this.Bb = MyDecorator.GetMinBetInRoom();
            this.Sb = (int)Bb / 2;
            
        }

        public void AddDecorator(Decorator d)
        {
            this.MyDecorator = d;
        }

        //set the room's thread
        public void SetThread(Thread thread)
        {
            RoomThread = thread;
        }

        public bool DoAction(IUser user, ActionType action, int bet)
        {
            if (action == ActionType.Join)
            {
                return Join(user);
            }
            if (!IsUserInGame(user))
            {
                return IrellevantUser(user, action);
            }

            Player player = GetInGamePlayerFromUser(user);
            if(action == ActionType.StartGame)
            {
                return StartGame(player);
            }
            if (action == ActionType.Leave)
            {
                return Leave(player);
            }
            if (player != CurrentPlayer)
            {
                return IrellevantUser(user, action);
            }
            if (action == ActionType.Fold)
            {
                return Fold(player);
            }
            if (bet == 0)
            {
                return Check(player);
            }
            if (bet > 0)
            {
                return CallOrRaise(player, bet);
            }
            return false;
        }

        private bool Leave(Player player)
        {
            List<Player> relevantPlayers = new List<Player>();
            LeaveAction leave = new LeaveAction(player);
            GameReplay.AddAction(leave);
            foreach (Player p in this.Players)
            {
                if (p.user.Id() != player.user.Id())
                {
                    relevantPlayers.Add(p);
                }
            }
            Players = relevantPlayers;
            return true;
        }

        //TODO create player and add to game 
        private bool Join(IUser user)
        {
            throw new NotImplementedException();
        }

        private bool StartGame(Player player)
        {
            if (!MyDecorator.CanStartTheGame(Players.Count))
            {
                return false;
            }
            if (IsActiveGame == true) //can't start an already active game
            {
                return false;
            }

            Hand_Step = HandStep.PreFlop;
            Deck = new Deck();
            GameReplay = new GameReplay(Id, GameNumber);
            SystemLog log = new SystemLog(Id, "Game Started");
            _logControl.AddSystemLog(log);
            SetRoles();
            StartGame startAction = new StartGame(this.Players, DealerPlayer, SbPlayer, BbPlayer);
            this.GameReplay.AddAction(startAction);
            SystemLog log2 = new SystemLog(this.Id, startAction.ToString());
            _logControl.AddSystemLog(log2);

            MoveBbnSBtoPot();
            maxBetInRound = Bb;

            HandCards();
            IsActiveGame = true;
            _roundCounter = 1;
            someOneRaised = false;
            return true;
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
            int currentPlayerBet = player.RoundChipBet + bet;
            if (!MyDecorator.CanRaise(currentPlayerBet, maxBetInRound))
            {
                return false;
            }
            if (player.TotalChip < bet) //not enough chips for bet
            {
                return false;  
            }
            maxBetInRound = currentPlayerBet;
            player.PlayedAnActionInTheRound = true;
            player.CommitChips(bet);
            RaiseAction raise = new RaiseAction(player, player._firstCard,
                 player._secondCard, currentPlayerBet);
            GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this.Id, raise.ToString());
            _logControl.AddSystemLog(log);
            lastPlayerRaisedInRound = player;
            someOneRaised = true;
            foreach (Player p in Players) //they all need to make another action in this round
            {
                if (p != player)
                {
                    p.PlayedAnActionInTheRound = false;
                }
            }
            return true;
        }

        private bool Call(Player player, int bet)
        {
            player.PlayedAnActionInTheRound = true;
            bet = Math.Min(bet, player.TotalChip); // if can't afford that many chips in a call, go all in           
            player.CommitChips(bet);
            CallAction call = new CallAction(player, player._firstCard,
                player._secondCard, bet);
            GameReplay.AddAction(call);
            SystemLog log = new SystemLog(this.Id, call.ToString());
            _logControl.AddSystemLog(log);
            return true;
        }

        private bool Check(Player player)
        {
            if (!MyDecorator.CanCheck())
            {
                return false;
            }
            player.PlayedAnActionInTheRound = true;
            CheckAction check = new CheckAction(player, player._firstCard,
                 player._secondCard);
            SystemLog log = new SystemLog(this.Id, check.ToString());
            _logControl.AddSystemLog(log);
            GameReplay.AddAction(check);
            return true;
        }

        private bool Fold(Player player)
        {
            player.PlayedAnActionInTheRound = true;
            player.isPlayerActive = false;
            FoldAction fold = new FoldAction(player, player._firstCard,
                player._secondCard);
            GameReplay.AddAction(fold);
            SystemLog log = new SystemLog(this.Id, fold.ToString());
            _logControl.AddSystemLog(log);
            return AfterAction();
        }

        private bool AfterAction()
        {
            if (IsGameOver())
            {

            }
            if (AllDoneWithTurn() )
            {
                return NextRound();
            }
            return NextCurrentPlayer();
        }

        private bool NextRound()
        {
            if (Hand_Step == HandStep.River) 
            {
                return EndGame(); 
            }
            MoveChipsToPot();

            lastPlayerRaisedInRound = null;
            LastRaise = 0;
            InitializePlayerRound();
            //TODO: check that
            MaxRaiseInThisRound = MyDecorator.GetMaxAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);
            MinRaiseInThisRound = MyDecorator.GetMinAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);

            ProgressHand();
            return true;
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
            this.InitPlayersLastAction();
            this.Winners = FindWinner(this.PublicCards, playersLeftInGame);
            List<int> ids = new List<int>();
            foreach (Player player in playersLeftInGame)
            {
                ids.Add(player.user.Id());
            }
            this.ReplayManager.AddGameReplay(this.GameReplay, ids);
            if (this.Winners.Count > 0) // so there are winners at the end of the game
            {
                var amount = this.PotCount / this.Winners.Count;

                foreach (HandEvaluator h in this.Winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in this.Players)
            {
                player.ClearCards(); // gets rid of cards of all players
                if (player.OutOfMoney())
                {
                    player.isPlayerActive = false;
                    this.Players.Remove(player);
                }
            }
            this.IsActiveGame = false;
            if (this.Players.Count > 1)
            {
                // sets next DealerPos - for the next run 
                this.DealerPos = this.DealerPos + 1 % this.Players.Count;
                // put new turns for the next round
                this.ClearPublicCards();
                this.GameReplay = new GameReplay(this.Id, this.GameNumber);
                SetRoles();
            }
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

    private bool NextCurrentPlayer()
        {
            int i = 1;
            while(i <= Players.Count)
            {
                int newPosition = (currentPlayerPos + i) % Players.Count;
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
            if (!IsActiveGame)
            {
                return false;
            }
            if (ActivePlayersInGame() < 2)
            {
                return true;
            }
            //TODO last round finished?
        }

        //@TODO send a message to user saying he is not part of the game and cant do action
        private bool IrellevantUser(IUser user, ActionType action)
        {
            throw new NotImplementedException();
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

        public void Start()
        {
            if (RoomThread != null && !this.IsActiveGame)
            {
                try
                {
                    RoomThread.Start();
                    Play();

                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Room number " + this.Id + " was attempted to start but has allready been started.");
                    _logControl.AddErrorLog(log);
                }
            }
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
            CurrentPlayer = FirstPlayerInRound;
        }

        //TODO: restart deck between rounds
        public bool Play()
        {
            if (!this.MyDecorator.CanStartTheGame(this.Players.Count))
            {
                return false;
            }
            this.Hand_Step = HandStep.PreFlop;
            this.GameReplay = new GameReplay(this.Id, this.GameNumber);
            SystemLog log = new SystemLog(this.Id, "Game Started");
            _logControl.AddSystemLog(log);
            SetRoles();
            StartGame startAction = new StartGame(this.Players, DealerPlayer, SbPlayer, BbPlayer);
            this.GameReplay.AddAction(startAction);
            SystemLog log2 = new SystemLog(this.Id, startAction.ToString());
            _logControl.AddSystemLog(log2);
            HandCards();
            this.IsActiveGame = true;
            this._roundCounter = 1;
            while (this._roundCounter <= 4)
            {
                InitializePlayerRound();
                MaxRaiseInThisRound = MyDecorator.GetMaxAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);
                MinRaiseInThisRound = MyDecorator.GetMinAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);

                DoRound();

                MoveChipsToPot(); 

                if (this.ActivePlayersInGame() >= 2)
                {
                    if (!ProgressHand(this.Hand_Step))
                    {
                        EndHand();
                        return true;
                    }
                    else
                    {
                        this.InitPlayersLastAction();
                        this.ActionPos = this._currLoaction;
                        this.CurrentPlayer = this.NextToPlay();
                    }

                }
                else
                {
                    EndHand();
                }

            }
            return true;

        }

        private void DoRound()
        {
            while (!this.AllDoneWithTurn())
            {
                this.CurrentPlayer = NextToPlay();
                int move = PlayerPlay();
                PlayerDesicion(move);
                if (_backFromRaise)
                {
                    _backFromRaise = false;
                    break;
                }
                this.UpdateGameState();
                this.CheckIfPlayerWantToLeave();
            }
        }

        private void HandCards()
        {
            foreach (Player player in this.Players)
            {
                player.isPlayerActive = true;
                player.Add2Cards(Deck.Draw(), Deck.Draw());
                HandCards hand = new HandCards(player, player._firstCard,
                    player._secondCard);
                GameReplay.AddAction(hand);
                SystemLog log = new SystemLog(this.Id, hand.ToString());
                _logControl.AddSystemLog(log);
            }
        }
        
        private Player NextToPlay()
        {
            return Players[ActionPos];
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

        private void UpdateGameState()
        {
            // next player picked

            do { ActionPos = (ActionPos + 1) % Players.Count; }
            while (!Players[ActionPos].isPlayerActive);

            UpdateMaxCommitted();
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

     
        private void CheckIfPlayerWantToLeave()
        {
            List<Player> players = new List<Player>();
            foreach (Player p in this.Players)
            {
                if (p._isInRoom == true)
                {
                    players.Add(p);
                }
                else
                {
                    LeaveAction leave = new LeaveAction(p);
                    GameReplay.AddAction(leave);
                }
            }
            if (players.Count < this.Players.Count)
            {
                this.Players = players;
            }
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

        public int PlayerPlay()
        {
            int toReturn = -3;
            int maxRaise = MaxRaiseInThisRound;
            int minRaise = MinRaiseInThisRound;
            int fold = -1;
            bool isLimit = (this.MyDecorator.GetGameMode() == GameMode.Limit);
            GameMode gm;


            int playerMoney = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
            //raise - <Raise,bool is limit,maxRaise, minRaise> - if true must raise equal to max.
            //bet - <Bet,bool is limit,maxRaise, minRaise> - if true must Bet equal to max.
            //check <Check, false, 0, 0>
            //fold - <Fold, false,-1,-1>
            //call - <Call, false,call amount, 0>
            List<Tuple<GameMove, bool, int, int>> moveToSend = new List<Tuple<GameMove, bool, int, int>>();
            int callAmount = maxRaise - this.CurrentPlayer._payInThisRound;
            bool canCheck = (this.maxBetInRound == 0);
            try
            {

                switch (this.Hand_Step)
                {
                    case (GameRoom.HandStep.PreFlop):
                        if (this.CurrentPlayer == this.SbPlayer && this.CurrentPlayer._payInThisRound == 0)//start of game - small blind bet
                        {
                            toReturn = this.Sb;
                            return toReturn;
                        }

                        if (this.CurrentPlayer == this.BbPlayer && this.CurrentPlayer._payInThisRound == 0) // start game big blind bet
                        {
                            toReturn = this.Bb;
                            return toReturn;
                        }
                        if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
                        {
                            maxRaise = this.Bb;
                            if (this.CurrentPlayer == this.SbPlayer && this.CurrentPlayer._payInThisRound == this.Sb)
                            {
                                callAmount = this.Bb - this.Sb;
                                maxRaise = callAmount + maxRaise;
                            }
                            else if (this.CurrentPlayer == this.BbPlayer && this.CurrentPlayer._payInThisRound == this.Bb)
                            {
                                maxRaise = this.Bb;
                            }
                            else if (this.CurrentPlayer._payInThisRound == 0)
                            {
                                callAmount = this.Bb;
                                maxRaise = this.Bb + callAmount;
                            }
                            else if ((this.CurrentPlayer != this.BbPlayer || this.CurrentPlayer != this.SbPlayer) &&
                                     this.CurrentPlayer._payInThisRound != 0)
                            {
                                callAmount = maxRaise - this.CurrentPlayer._payInThisRound;
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }

                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        Tuple<GameMove, bool, int, int> RaisePreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> CallPreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
                        Tuple<GameMove, bool, int, int> FoldPreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
                        moveToSend.Add(RaisePreFlop);
                        moveToSend.Add(CallPreFlop);
                        moveToSend.Add(FoldPreFlop);
                        break;


                    case (GameRoom.HandStep.Flop):

                        if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<GameMove, bool, int, int> CheckFlop =
                                new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
                            moveToSend.Add(CheckFlop);
                        }
                        Tuple<GameMove, bool, int, int> RaiseFlop = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> BetFlop = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> CallFlop = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
                        Tuple<GameMove, bool, int, int> FoldFlop = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
                        moveToSend.Add(RaiseFlop);
                        moveToSend.Add(CallFlop);
                        moveToSend.Add(FoldFlop);
                        moveToSend.Add(BetFlop);
                        break;


                    case (GameRoom.HandStep.Turn):
                        if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }

                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<GameMove, bool, int, int> CheckTurn =
                                new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
                            moveToSend.Add(CheckTurn);
                        }
                        Tuple<GameMove, bool, int, int> RaiseTurn = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> BetTurn = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> CallTurn = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
                        Tuple<GameMove, bool, int, int> FoldTurn = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
                        moveToSend.Add(RaiseTurn);
                        moveToSend.Add(CallTurn);
                        moveToSend.Add(FoldTurn);
                        moveToSend.Add(BetTurn);

                        break;
                    case (GameRoom.HandStep.River):
                        if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<GameMove, bool, int, int> CheckRiver =
                                new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
                            moveToSend.Add(CheckRiver);
                        }
                        Tuple<GameMove, bool, int, int> RaiseRiver = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> BetRiver = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
                        Tuple<GameMove, bool, int, int> CallRiver = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
                        Tuple<GameMove, bool, int, int> FoldRiver = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
                        moveToSend.Add(RaiseRiver);
                        moveToSend.Add(CallRiver);
                        moveToSend.Add(FoldRiver);
                        moveToSend.Add(BetRiver);
                        break;
                    default:
                        ErrorLog log = new ErrorLog("error in roung in room: " + this.Id + "the tound is not prefop / flop / turn / river");
                        //GameCenter.Instance.AddErrorLog(log);
                        _logControl.AddErrorLog(log);
                        break;

                }
                if (!IsTestMode)
                {
                    Tuple<GameMove, int> getSelectedFromPlayer =
                        GameCenter.Instance.SendUserAvailableMovesAndGetChoosen(moveToSend);
                    toReturn = getSelectedFromPlayer.Item2;
                }
                else
                {
                    toReturn = this.CurrentPlayer.moveForTest;
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog("error in play of player with Id: " + this.CurrentPlayer.user.Id() + " somthing went wrong");
                // GameCenter.Instance.AddErrorLog(log);
                _logControl.AddErrorLog(log);
            }

            return toReturn;
        }

        private void PlayerDesicion(int move)
        {
            switch (move)
            {
                case -1:
                    Fold();
                    break;
                case 0:
                    Check();
                    break;
                default:
                    if (move == maxBetInRound)
                        Call(maxBetInRound);
                    else
                    {
                        Raise(move);
                        StartNewRoundAfterRaise();
                    }
                    break;
            }
        }


       private void Raise(int additionalChips)
        {
            this.maxBetInRound += additionalChips;
            this.CurrentPlayer.PlayedAnActionInTheRound = true;
            this.CurrentPlayer.CommitChips(additionalChips);
            RaiseAction raise = new RaiseAction(this.CurrentPlayer, this.CurrentPlayer._firstCard,
                 this.CurrentPlayer._secondCard, additionalChips);
            this.GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this.Id, raise.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            _logControl.AddSystemLog(log);
        }
        private bool ProgressHand(GameRoom.HandStep previousStep)
        {
            int numNextStep = (int)previousStep + 1;
            this.Hand_Step = (GameRoom.HandStep)numNextStep;

            switch (previousStep)
            {   //never spouse to be in the "pre flop" case
                case HandStep.PreFlop:               
                    break;
                case HandStep.Flop:
                    for (int i = 0; i <= 2; i++)
                    {
                        this.AddNewPublicCard();
                    }
                    this._roundCounter++;
                    break;
                case HandStep.Turn:
                    this.AddNewPublicCard();
                    this._roundCounter++;
                    break;
                case HandStep.River:
                    this.AddNewPublicCard();
                    this._roundCounter++;
                    break;
                
                default:
                    return false;
            }

            if (this.ActivePlayersInGame() - this.PlayersAllIn() < 2)
            {
                return ProgressHand(this.Hand_Step); // recursive, runs until we'll hit the river
            }
            return true;
        }

        private void EndHand()
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
            this.InitPlayersLastAction();
            this.Winners = FindWinner(this.PublicCards, playersLeftInGame);
            List<int> ids = new List<int>();
            foreach (Player player in playersLeftInGame)
            {
                ids.Add(player.user.Id());
            }
            this.ReplayManager.AddGameReplay(this.GameReplay, ids);
            if (this.Winners.Count > 0) // so there are winners at the end of the game
            {
                var amount = this.PotCount / this.Winners.Count;

                foreach (HandEvaluator h in this.Winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in this.Players)
            {
                player.ClearCards(); // gets rid of cards of all players
                if (player.OutOfMoney())
                {
                    player.isPlayerActive = false;
                    this.Players.Remove(player);
                }
            }
            this.IsActiveGame = false;
            if (this.Players.Count > 1)
            {
                // sets next DealerPos - for the next run 
                this.DealerPos = this.DealerPos+1 % this.Players.Count;
                // put new turns for the next round
                this.ClearPublicCards();
                this.GameReplay = new GameReplay(this.Id, this.GameNumber);
                SetRoles();
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
                _logControl.AddSystemLog(log);
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
                _logControl.AddSystemLog(log);
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

        private void StartNewRoundAfterRaise()
        {
            if (this.ActivePlayersInGame() < 2)
            {
                EndHand();
            }

            UpdateGameState();
            Player playerWhoRaise = this.CurrentPlayer;
            Player nextPlayer = NextToPlay();
            while (nextPlayer != null && nextPlayer != playerWhoRaise)
            {
                this.CurrentPlayer = nextPlayer;
                int move = PlayerPlay();
                if (move > this.maxBetInRound)
                {
                    StartNewRoundAfterRaise();
                    break;
                }
                    PlayerDesicion(move);                  
                     UpdateGameState();
                     nextPlayer = this.NextToPlay();
            }
            
            _backFromRaise = true;
        }

        private int GetRaisePotLimit(Player p)
        {

            int potSize = this.PotCount;
            int lastRise = this.maxBetInRound;
            int playerPayInRound = p._payInThisRound;
            int toReturn = (lastRise - playerPayInRound) + potSize;
            return toReturn;
        }
        //TODO: checking before calling to this function that this user&room ID are exist
        public bool AddPlayerToRoom(int userId)
        {
            SystemControl sc = SystemControl.SystemControlInstance;
            IUser user = sc.GetUserWithId(userId);
            if (user == null)
            {
                ErrorLog log = new ErrorLog("Error while tring to add player to room - invalid input - there is no user with user Id: " + userId + "(user with Id: " + userId + " to room: " + this.Id);
                this._logControl.AddErrorLog(log);
                return false;
            }

            int EntrancePayingMoney = user.Money() - this.MyDecorator.GetEnterPayingMoney();
            int AfterReduceTheStartingChip = EntrancePayingMoney - this.MyDecorator.GetStartingChip();
            if (EntrancePayingMoney < 0)
            {
                ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: " + userId + " to room: " + this.Id + "user dont have money to pay the buy in policey of this room");
                this._logControl.AddErrorLog(log);
                return false;
            }
            if (AfterReduceTheStartingChip < 0)
            {
                ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: " + userId + " to room: " + this.Id + " user dont have money to get sarting chip and buy in policey");
                this._logControl.AddErrorLog(log);
                return false;
            }
            //User cant be spectator & player in the same room
            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == userId)
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: " + userId + " to room: " + this.Id + " user is a spectetor in this room need to leave first than join game");
                    this._logControl.AddErrorLog(log);
                    continue;
                }
                return false;
            }
          
            if (!this.MyDecorator.CanAddMorePlayer(this.Players.Count))
            {
                ErrorLog log = new ErrorLog("Error while trying to add player to room thaere is no place in the room - max amount of player tight now: " + this.Players.Count + "(user with Id: " + userId + " to room: " + this.Id);
                this._logControl.AddErrorLog(log);
                return false;
            }
            if (!IsBetweenRanks(user.Points()))
            {
                ErrorLog log =
                    new ErrorLog("Error while trying to add player, user with Id: " + userId + " to room: " + this.Id +
                                 "user point: " + user.Points() + " are not in this game critiria");
                this._logControl.AddErrorLog(log);
                return false;
            }
            user.EditUserMoney(AfterReduceTheStartingChip);

            Player p = new Player(user, AfterReduceTheStartingChip, this.MyDecorator.GetStartingChip(), this.Id);
            this.Players.Add(p);

            return true;
        
    }

        public bool AddSpectetorToRoom(int userId)
        {           
            SystemControl sc = SystemControl.SystemControlInstance;
            IUser user = sc.GetUserWithId(userId);
            //if user is player in room cant be also spectetor
            foreach (Player p in Players)
            {
                if (p.user.Id() == userId)
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: " + userId + " to room: " + this.Id + " user is a spectetor in this room need to leave first than join game");
                    this._logControl.AddErrorLog(log);
                    continue;
                }
                return false;
            }
           
                user.AddRoomToSpectetorGameList(this);
                Spectetor spectetor = new Spectetor(user, this.Id);
                this.Spectatores.Add(spectetor);
                   
                    return true;
       }


        public bool RemovePlayerFromRoom(int userId)
        {                       
            SystemControl sc = SystemControl.SystemControlInstance;
            IUser user = sc.GetUserWithId(userId);

            foreach (Player p in Players)
            {
                if (p.user.Id() == userId)
                {
                    SystemLog log =
                        new SystemLog(this.Id, "The Player with user Id " + userId + " Removed succsfully from room" + this.Id);
                    this.Players.Remove(p);
                    user.EditUserMoney(user.Money() + (p.TotalChip - p.RoundChipBet));
                    user.RemoveRoomFromActiveGameList(this);
                    continue;
                }
                return true;
            }
            return false;
        }

        public bool RemoveSpectetorFromRoom(int userId)
        {
            
            SystemControl sc = SystemControl.SystemControlInstance;     
            IUser user = sc.GetUserWithId(userId);

            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == userId)
                {
                    SystemLog log =
                        new SystemLog(this.Id, "The Spcetator with user Id " + userId + " Removed succsfully from room" + this.Id);
                    this.Spectatores.Remove(s);
                    user.RemoveRoomFromSpectetorGameList(this);
                    continue;
                }
                return true;
            }
            return false;
        }

        public bool IsBetweenRanks(int playerRank)
        {
            return (playerRank <= this.MaxRank) && (playerRank >= this.MinRank) ? true : false;
        }
    }
}
