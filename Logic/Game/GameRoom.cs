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

namespace TexasHoldem.Logic.Game
{
    public class GameRoom : IGame
    {
        public List<Player> Players { get; set; }
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int Id { get; set; }
        public List<Spectetor> Spectatores { get; set;}
        public int DealerPos { get; set; }
        public int MaxCommitted { get; set; }
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
        public bool IsTestMode { get; set; }
        public Decorator MyDecorator;
        public int MaxRaiseInThisRound { get; set; } //מה המקסימום raise / bet שיכול לבצע בסיבוב הנוכחי 
        public int MinRaiseInThisRound { get; set; } //המינימום שחייב לבצע בסיבוב הנוכחי
        public int LastRaise { get; set; }  //change to maxCommit
        public Thread RoomThread { get; set; }
        //new after log control change
        private LogControl _logControl = LogControl.Instance;
        public int MaxRank { get; set; }
        public int MinRank { get; set; }
        public int GameNumber=0;
        public int MinBetInRoom { get; set; }
        private int _currLoaction { get; set; }
        private int _roundCounter { get; set; }
        public GameRoom(List<Player> players, int ID)
        {
            this.Id = ID;
            this.IsActiveGame = false;
            this.PotCount = 0;          
            this.Players = players;
            this.MaxCommitted = 0;
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
                    // this._gameCenter.errorLog.Add(log);
                    _logControl.AddErrorLog(log);
                }
            }
        }

        private void SetRoles()
        {
            if (this.DealerPlayer == null)
            {
                this.DealerPos = 0;
            }
            Deck deck = new Deck();
            this.Deck = deck;
            this._buttonPos = this.DealerPos;

            if (this.Players.Count > 2)
            {
                //delaer
                this.DealerPlayer = this.Players[this._buttonPos];
                // small blind
                this.SbPlayer = this.Players[(this._buttonPos + 1) % this.Players.Count];
                this.Players[(this._buttonPos + 1) % this.Players.Count].CommitChips(this.Bb / 2);
                // big blind
                this.BbPlayer = this.Players[(this._buttonPos + 2) % this.Players.Count];
                this.Players[(this._buttonPos + 2) % this.Players.Count].CommitChips(this.Bb);
                //actionPos will keep track on the curr player.
                this.ActionPos = (this._buttonPos + 3) % this.Players.Count;

            }
            else
            {
                this.ActionPos = (this._buttonPos) % this.Players.Count;
                // small blind
                this.DealerPlayer = this.Players[(this._buttonPos) % this.Players.Count];
                this.SbPlayer = this.Players[(this._buttonPos) % this.Players.Count];
                this.Players[(this._buttonPos) % this.Players.Count].CommitChips(this.Bb / 2);
                // big blind
                this.BbPlayer = this.Players[(this._buttonPos + 1) % this.Players.Count];
                this.Players[(this._buttonPos + 1) % this.Players.Count].CommitChips(this.Bb);
            }

            this.UpdateMaxCommitted();
            this.MoveBbnSBtoPot(this.BbPlayer, this.SbPlayer);
            switch (this.Players.Count)
            {
                case 2:
                case 3:
                    this.CurrentPlayer = this.BbPlayer;
                    this._currLoaction = Players.FindIndex((p => p.user.Id() == this.CurrentPlayer.user.Id()));
                    break;
                default:
                    this.CurrentPlayer = this.Players[this.ActionPos];
                    this._currLoaction = Players.FindIndex((p => p.user.Id() == this.CurrentPlayer.user.Id()));
                    break;
            }

        }

        public bool WorkingPlay()
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
                //Orellie functions
                InitializePlayerRound();
                RaiseFieldAtEveryRound();

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


        public bool Play()
        {
            //we arrive here only at the very begging so we need to check min players 
            if (!this.MyDecorator.CanStartTheGame(this.Players.Count))
            {
                return false;              
            }
            else
            {
                //need to do before we starting a new game
                this.Hand_Step = HandStep.PreFlop;
                this.GameReplay = new GameReplay(this.Id, this.GameNumber);
                SystemLog log = new SystemLog(this.Id, this.GameReplay.ToString());
                _logControl.AddSystemLog(log);
                SetRoles();
                HandCards();
                this.IsActiveGame = true;
                this._roundCounter = 1;
                while (this._roundCounter<=4)
                {
                    DoRound();
                    //Orellie functions
                    InitializePlayerRound();
                    RaiseFieldAtEveryRound();

                    this.MoveChipsToPot();
                    if (this.ActivePlayersInGame() >= 2)
                    {
                        if (!ProgressHand(this.Hand_Step))
                        {
                            EndHand();
                            return true;
                        }
                        else
                        {
                            this.ActionPos = this._currLoaction;
                            this.CurrentPlayer = this.NextToPlay();
                         }

                    }
                    else
                    {
                        EndHand();
                    }

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
                player.AddHoleCards(this.Deck.Draw(), this.Deck.Draw());
                HandCards hand = new HandCards(player, player._hand._firstCard,
                    player._hand._seconedCard);
                this.GameReplay.AddAction(hand);
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
                player.AddCard(c);
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
                if (player.RoundChipBet > MaxCommitted)
                    MaxCommitted = player.RoundChipBet;
        }
        private void AfterFinishedRound()
        {
            MoveChipsToPot();

           foreach (Player player in Players)
                if (player.isPlayerActive)
                    player.PlayedAnActionInTheRound = false;
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
                if (player.IsAllIn())
                    playersAllIn++;
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

        private void MoveBbnSBtoPot(Player bbPlayer, Player sbPlayer)
        {
            PotCount = Bb + Sb;
            bbPlayer.RoundChipBet = bbPlayer.RoundChipBet + Bb;
            sbPlayer.RoundChipBet = sbPlayer.RoundChipBet + Sb;
        }

        private void RaiseFieldAtEveryRound()
        {

            if (this.MyDecorator.GetGameMode() == GameMode.Limit && (this.Hand_Step == GameRoom.HandStep.Flop ||
                                                       this.Hand_Step == GameRoom.HandStep.PreFlop))
            {
                MaxRaiseInThisRound = this.Bb;
            }
            if (this.MyDecorator.GetGameMode() == GameMode.Limit && (this.Hand_Step == GameRoom.HandStep.River ||
                                                       this.Hand_Step == GameRoom.HandStep.Turn))
            {
                MaxRaiseInThisRound = this.Bb * 2;
            }
            if (this.MyDecorator.GetGameMode() == GameMode.NoLimit)
            {
                MinRaiseInThisRound = this.MaxCommitted;
            }
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
            bool canCheck = (this.MaxCommitted == 0);
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
                    if (move == MaxCommitted)
                        Call(MaxCommitted);
                    else
                    {
                        Raise(move);
                        StartNewRoundAfterRaise();
                    }
                    break;
            }
        }

        private void Fold()
        {
            this.CurrentPlayer.PlayedAnActionInTheRound = true;
            this.CurrentPlayer.isPlayerActive = false;
            FoldAction fold = new FoldAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                this.CurrentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this.Id, fold.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            _logControl.AddSystemLog(log);
            this.GameReplay.AddAction(fold);
        }

        private void Check()
        {
            this.CurrentPlayer.PlayedAnActionInTheRound = true;
            CheckAction check = new CheckAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                 this.CurrentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this.Id, check.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            _logControl.AddSystemLog(log);
            this.GameReplay.AddAction(check);
        }

        private void Call(int additionalChips)
        {
            this.CurrentPlayer.PlayedAnActionInTheRound = true;
            additionalChips = Math.Min(additionalChips, this.CurrentPlayer.TotalChip); // if can't afford that many chips in a call, go all in           
            this.CurrentPlayer.CommitChips(additionalChips);
            CallAction call = new CallAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                this.CurrentPlayer._hand._seconedCard, additionalChips);
            this.GameReplay.AddAction(call);
            SystemLog log = new SystemLog(this.Id, call.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            _logControl.AddSystemLog(log);
        }

       private void Raise(int additionalChips)
        {
            this.MaxCommitted += additionalChips;
            this.CurrentPlayer.PlayedAnActionInTheRound = true;
            this.CurrentPlayer.CommitChips(additionalChips);
            RaiseAction raise = new RaiseAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                 this.CurrentPlayer._hand._seconedCard, additionalChips);
            this.GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this.Id, raise.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            _logControl.AddSystemLog(log);
        }
        private bool ProgressHand(GameRoom.HandStep previousStep)
        {

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
                    break;
            }


            int numNextStep = (int)previousStep + 1;
            this.Hand_Step = (GameRoom.HandStep)numNextStep;

            if (this.ActivePlayersInGame() - this.PlayersAllIn() < 2)
            {
                return ProgressHand(this.Hand_Step); // recursive, runs until we'll hit the river
            }
            else
            {
                this.AfterFinishedRound();
            }

            return true;

        }

        private void EndHand()
        {
            this.GameNumber++;
            List<Player> playersLeftInGame = new List<Player>();
            foreach (Player player in this.Players)
            {
                player.user.AddGameAvailableToReplay(this.Id, this.GameNumber);
                if (player.isPlayerActive)
                    playersLeftInGame.Add(player);
                else
                {
                    player.isPlayerActive = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }
            }
            this.AfterFinishedRound();
            this.Winners = FindWinner(this.PublicCards, playersLeftInGame);
            //TODO : by AvivG
            this.ReplayManager.AddGameReplay(this.GameReplay);
            if (this.Winners.Count > 0) // so there are winners at the end of the game
            {
                var amount = this.PotCount / this.Winners.Count;

                foreach (HandEvaluator h in this.Winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in playersLeftInGame)
            {
                player.ClearCards(); // gets rid of cards of players still alive
            }
            this.IsActiveGame = false;
            if (this.Players.Count > 1)
            {
                // sets next DealerPos - if we want to "run" for a new game immediantly
                this.DealerPos = this.DealerPos+1 % this.Players.Count;
                // put new turns for the next round
                this.ClearPublicCards();
                this.GameReplay = new GameReplay(this.Id, this.GameNumber);
                SystemLog log = new SystemLog(this.Id, this.GameReplay.ToString());
                _logControl.AddSystemLog(log);
                SetRoles();
            }
            else
            {
                
                if (!this.CurrentPlayer.OutOfMoney())
                    this.Players[0].isPlayerActive = false;
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
                playerCards.AddRange(p._hand.GetCards());
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
                    winners[0]._player._hand._firstCard, winners[0]._player._hand._seconedCard,
                    this.PotCount, table, winners[0]._relevantCards);
                this.GameReplay.AddAction(win);
                SystemLog log = new SystemLog(this.Id, win.ToString());
                // this.this._gameCenter.AddSystemLog(log);
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
                     h._player._hand._firstCard, h._player._hand._seconedCard,
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
                if (move > this.MaxCommitted)
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
            int lastRise = this.MaxCommitted;
            int playerPayInRound = p._payInThisRound;
            int toReturn = (lastRise - playerPayInRound) + potSize;
            return toReturn;
        }
    }
}
