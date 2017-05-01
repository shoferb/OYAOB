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
        public bool FirstEnter { get; set; }
        private int _buttonPos;
        private bool _backFromRaise;
        public bool IsTestMode { get; set; }
        public List<Decorator> MyDecorators;
        public int MaxRaiseInThisRound { get; set; } //מה המקסימום raise / bet שיכול לבצע בסיבוב הנוכחי 
        public int MinRaiseInThisRound { get; set; } //המינימום שחייב לבצע בסיבוב הנוכחי
        public int LastRaise { get; set; }  //change to maxCommit
        public Thread RoomThread { get; set; }
        //new after log control change
        private LogControl logControl = LogControl.Instance;
        public int MaxRank { get; set; }
        public int MinRank { get; set; }
        public int GameNumber=0;
        public int MinBetInRoom { get; set; }

        public GameRoom(List<Player> players, int ID, int minBetInRoom, List<Decorator> decorators )
        {
            this.Id = ID;
            this.IsActiveGame = false;
            this.PotCount = 0;          
            this.Players = players;
            this.MaxCommitted = 0;
            this.PublicCards = new List<Card>();
            this.Sb = (int) Bb / 2;
            this.Bb = minBetInRoom;
            this.MyDecorators = decorators;
            this.SidePots = new List<Tuple<int, List<Player>>>();
            Tuple<int,int> tup = GameCenter.UserLeageGapPoint(players[0].user.Id());
            this.MinRank = tup.Item1;
            this.MaxRank = tup.Item2;
        }

        //set the room's thread
        public void SetThread(Thread thread)
        {
            RoomThread = thread;
        }

        public void Start()
        {
            //Play();
            if (RoomThread != null && !this.IsActiveGame)
            {
                try
                {
                    RoomThread.Start();
                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Room number " + this.Id + " was attempted to start but has allready been started.");
                    // this._gameCenter.errorLog.Add(log);
                    logControl.AddErrorLog(log);
                }
            }
        }

        private void SetRoles()
        {
            this.Hand_Step = GameRoom.HandStep.PreFlop;
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

            StartGame startAction = new StartGame(this.Players, DealerPlayer, SbPlayer, BbPlayer);
            SystemLog log = new SystemLog(this.Id, startAction.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
            this.GameReplay.AddAction(startAction);

            foreach (Player player in this.Players)
            {
                player.isPlayerActive = true;
                player.AddHoleCards(deck.Draw(), deck.Draw());
                HandCards hand = new HandCards(player, player._hand._firstCard,
                    player._hand._seconedCard);
                this.GameReplay.AddAction(hand);
                log = new SystemLog(this.Id, hand.ToString());
                // this.this._gameCenter.AddSystemLog(log);
                logControl.AddSystemLog(log);
            }
            this.UpdateMaxCommitted();
            this.MoveBbnSBtoPot(this.BbPlayer, this.SbPlayer);
            switch (this.Players.Count)
            {
                case 2:
                case 3:
                    this.CurrentPlayer = this.BbPlayer;
                    break;
                default:
                    this.CurrentPlayer = this.Players[this.ActionPos];
                    break;
            }

        }

       public bool Play()
        {

            if (this.Players.Count < this.MyDecorators[0].GetMinPlayersInRoom())
            {
                return false;
            }
            else
            {
                if (this.FirstEnter)
                {
                    StartTheGame();
                }
                while (!this.AllDoneWithTurn())
                {
                    RaiseFieldAtEveryRound();
                    InitializePlayerRound();
                    int move;
                    this.CurrentPlayer = NextToPlay();
                    //move = this.playTurn(player)
                    move = PlayerPlay();
                    PlayerDesicion(move);
                    if (_backFromRaise)
                    {
                        _backFromRaise = false;
                        break;
                    }
                    this.UpdateGameState();
                    this.CheckIfPlayerWantToLeave();
                }
                this.MoveChipsToPot();

                if (this.AllDoneWithTurn() || this.PlayersInGame() < 2)
                {
                    if (ProgressHand(this.Hand_Step))
                    {
                        // progresses _hand and returns whether _hand is over (the last Hand_Step was river)
                        EndHand();
                        return true;
                    }
                    else
                    {
                        this.CurrentPlayer = this.NextToPlay();
                        this.DealerPlayer =
                            this.Players[
                                (this.Players.IndexOf(this.DealerPlayer) + 1) % this.Players.Count];
                        this.SbPlayer =
                            this.Players[
                                (this.Players.IndexOf(this.SbPlayer) + 1) % this.Players.Count];
                        this.BbPlayer =
                            this.Players[
                                (this.Players.IndexOf(this.BbPlayer) + 1) % this.Players.Count];
                        return Play();
                    }

                }

            }
            return true;

        }

        private void StartTheGame()
        {
            this.GameReplay = new GameReplay(this.Id, this.GameNumber);
            SystemLog log = new SystemLog(this.Id, this.GameReplay.ToString());

            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
            this.DealerPos = 0;
            SetRoles();
            this.FirstEnter = false;
            this.IsActiveGame = true;

            foreach (Player p in this.Players)
            {
                p.isPlayerActive = true;
                p.user.AddGameAvailableToReplay(this.Id, this.GameNumber);
            }
        }

        private Player NextToPlay()
        {
            return Players[ActionPos];
        }

        private int ToCall()
        {
            return MaxCommitted - Players[ActionPos]._gameChip;

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
                if (player._gameChip > MaxCommitted)
                    MaxCommitted = player._gameChip;
        }
        private void EndTurn()
        {
            MoveChipsToPot();

           foreach (Player player in Players)
                if (player.isPlayerActive)
                    player._lastAction = "";
        }

        private void MoveChipsToPot()
        {
            foreach (Player player in Players)
            {
                PotCount += player._gameChip;
            }
        }
        private int PlayersInGame()
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
                if (!(player.isPlayerActive == false || (player._lastAction == "call" || player._lastAction == "check" || player._lastAction == "bet" || player._lastAction == "raise") && player._gameChip == MaxCommitted))
                    allDone = false;
            return allDone;
        }

        private bool newSplitPot(Player allInPlayer)
        {
            List<Player> eligiblePlayers = new List<Player>();
            int sidePotCount = 0;
            int chipsToMatch = allInPlayer._gameChip;
            foreach (Player player in Players)
            {
                if (player.isPlayerActive && player._gameChip > 0)
                {
                    player._gameChip -= chipsToMatch;
                    sidePotCount += chipsToMatch;
                    eligiblePlayers.Add(player);
                }
            }
            sidePotCount += PotCount;
            PotCount = 0;

            if (sidePotCount > 0)
            {
                SidePots.Add(new Tuple<int, List<Player>>(sidePotCount, eligiblePlayers));
            }
            return true;

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
            }
            if (players.Count < this.Players.Count)
            {
                this.Players = players;
            }
        }

        private void MoveBbnSBtoPot(Player bbPlayer, Player sbPlayer)
        {
            PotCount = Bb + Sb;
            bbPlayer._gameChip = bbPlayer._gameChip - Bb;
            sbPlayer._gameChip = sbPlayer._gameChip - Sb;
        }

        private void RaiseFieldAtEveryRound()
        {

            if (this.MyDecorators[0].GetGameMode() == GameMode.Limit && (this.Hand_Step == GameRoom.HandStep.Flop ||
                                                       this.Hand_Step == GameRoom.HandStep.PreFlop))
            {
                MaxRaiseInThisRound = this.Bb;
            }
            if (this.MyDecorators[0].GetGameMode() == GameMode.Limit && (this.Hand_Step == GameRoom.HandStep.River ||
                                                       this.Hand_Step == GameRoom.HandStep.Turn))
            {
                MaxRaiseInThisRound = this.Bb * 2;
            }
            if (this.MyDecorators[0].GetGameMode() == GameMode.NoLimit)
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
            if (!IsTestMode)
            {

            }
            int maxRaise = MaxRaiseInThisRound;
            int minRaise = MinRaiseInThisRound;
            int fold = -1;
            bool isLimit = (this.MyDecorators[0].GetGameMode() == GameMode.Limit);
            GameMode gm;


            int playerMoney = this.CurrentPlayer._totalChip - this.CurrentPlayer._gameChip;
            //raise - <Raise,bool is limit,maxRaise, minRaise> - if true must raise equal to max.
            //bet - <Bet,bool is limit,maxRaise, minRaise> - if true must Bet equal to max.
            //check <Check, false, 0, 0>
            //fold - <Fold, false,-1,-1>
            //call - <Call, false,call amount, 0>
            List<Tuple<Action, bool, int, int>> moveToSend = new List<Tuple<Action, bool, int, int>>();
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
                        if (this.MyDecorators[0].GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
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
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer._totalChip - this.CurrentPlayer._gameChip;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        Tuple<Action, bool, int, int> RaisePreFlop = new Tuple<Action, bool, int, int>(Action.Raise, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallPreFlop = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldPreFlop = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaisePreFlop);
                        moveToSend.Add(CallPreFlop);
                        moveToSend.Add(FoldPreFlop);
                        break;


                    case (GameRoom.HandStep.Flop):

                        if (this.MyDecorators[0].GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer._totalChip - this.CurrentPlayer._gameChip;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<Action, bool, int, int> CheckFlop =
                                new Tuple<Action, bool, int, int>(Action.Check, false, 0, 0);
                            moveToSend.Add(CheckFlop);
                        }
                        Tuple<Action, bool, int, int> RaiseFlop = new Tuple<Action, bool, int, int>(Action.Raise, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> BetFlop = new Tuple<Action, bool, int, int>(Action.Bet, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallFlop = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldFlop = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaiseFlop);
                        moveToSend.Add(CallFlop);
                        moveToSend.Add(FoldFlop);
                        moveToSend.Add(BetFlop);
                        break;


                    case (GameRoom.HandStep.Turn):
                        if (this.MyDecorators[0].GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }

                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer._totalChip - this.CurrentPlayer._gameChip;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<Action, bool, int, int> CheckTurn =
                                new Tuple<Action, bool, int, int>(Action.Check, false, 0, 0);
                            moveToSend.Add(CheckTurn);
                        }
                        Tuple<Action, bool, int, int> RaiseTurn = new Tuple<Action, bool, int, int>(Action.Raise, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> BetTurn = new Tuple<Action, bool, int, int>(Action.Bet, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallTurn = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldTurn = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaiseTurn);
                        moveToSend.Add(CallTurn);
                        moveToSend.Add(FoldTurn);
                        moveToSend.Add(BetTurn);

                        break;
                    case (GameRoom.HandStep.River):
                        if (this.MyDecorators[0].GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                            if (this.CurrentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
                            }
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = this.CurrentPlayer._totalChip - this.CurrentPlayer._gameChip;
                            minRaise = LastRaise;
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        else if (this.MyDecorators[0].GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(this.CurrentPlayer);
                            callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<Action, bool, int, int> CheckRiver =
                                new Tuple<Action, bool, int, int>(Action.Check, false, 0, 0);
                            moveToSend.Add(CheckRiver);
                        }
                        Tuple<Action, bool, int, int> RaiseRiver = new Tuple<Action, bool, int, int>(Action.Raise, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> BetRiver = new Tuple<Action, bool, int, int>(Action.Bet, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallRiver = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldRiver = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaiseRiver);
                        moveToSend.Add(CallRiver);
                        moveToSend.Add(FoldRiver);
                        moveToSend.Add(BetRiver);
                        break;
                    default:
                        ErrorLog log = new ErrorLog("error in roung in room: " + this.Id + "the tound is not prefop / flop / turn / river");
                        //GameCenter.Instance.AddErrorLog(log);
                        logControl.AddErrorLog(log);
                        break;

                }
                if (!IsTestMode)
                {
                    Tuple<Action, int> getSelectedFromPlayer =
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
                logControl.AddErrorLog(log);
            }

            return toReturn;
        }

        private void PlayerDesicion(int move)
        {
            int max = this.MaxCommitted;
            switch (move)
            {
                case -1:
                    Fold();
                    break;
                case 0:
                    Check();
                    break;
                default:
                    if (move == max)
                        Call(max);
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
            this.CurrentPlayer._lastAction = "fold";
            this.CurrentPlayer.isPlayerActive = false;
            FoldAction fold = new FoldAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                this.CurrentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this.Id, fold.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
            this.GameReplay.AddAction(fold);
        }

        private void Check()
        {
            this.CurrentPlayer._lastAction = "check";
            CheckAction check = new CheckAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                 this.CurrentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this.Id, check.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
            this.GameReplay.AddAction(check);
        }

        private void Call(int additionalChips)
        {
            this.CurrentPlayer._lastAction = "call";
            additionalChips = Math.Min(additionalChips, this.CurrentPlayer._totalChip); // if can't afford that many chips in a call, go all in           
            this.CurrentPlayer.CommitChips(additionalChips);
            CallAction call = new CallAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                this.CurrentPlayer._hand._seconedCard, additionalChips);
            this.GameReplay.AddAction(call);
            SystemLog log = new SystemLog(this.Id, call.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
        }

       private void Raise(int additionalChips)
        {
            this.MaxCommitted += additionalChips;
            this.CurrentPlayer._lastAction = "raise";
            this.CurrentPlayer.CommitChips(additionalChips);
            RaiseAction raise = new RaiseAction(this.CurrentPlayer, this.CurrentPlayer._hand._firstCard,
                 this.CurrentPlayer._hand._seconedCard, additionalChips);
            this.GameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this.Id, raise.ToString());
            //this.this._gameCenter.AddSystemLog(log);
            logControl.AddSystemLog(log);
        }

        private bool ProgressHand(GameRoom.HandStep previousStep)
        {


            if (this.PlayersInGame() < 2)
                return true;

            switch (previousStep)
            {
                case GameRoom.HandStep.PreFlop:

                    for (int i = 0; i <= 2; i++)
                        this.AddNewPublicCard();
                    break;
                case GameRoom.HandStep.Flop:

                    this.AddNewPublicCard();
                    break;
                case GameRoom.HandStep.Turn:

                    this.AddNewPublicCard();
                    break;
                case GameRoom.HandStep.River:

                    return true;

                default:
                    break;
            }


            int numNextStep = (int)previousStep + 1;
            this.Hand_Step = (GameRoom.HandStep)numNextStep;

            if (this.PlayersInGame() - this.PlayersAllIn() < 2)
            {
                ProgressHand(this.Hand_Step); // recursive, runs until we'll hit the river
                return true;
            }
            else
                this.EndTurn();

            return false;

        }

        private void EndHand()
        {
            this.GameNumber++;
            List<Player> playersLeftInGame = new List<Player>();
            foreach (Player player in this.Players)
            {
                player.user.AddGameAvailableToReplay(this.Id, this.GameNumber);
                if (player._totalChip != 0)
                    playersLeftInGame.Add(player);
                else
                {
                    // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player.isPlayerActive = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }
            }
            this.EndTurn();
            this.Winners = FindWinner(this.PublicCards, playersLeftInGame);
            //TODO : by AvivG
           // this.ReplayManager.AddGameReplay(this.GameReplay);
            int amount;
            if (this.Winners.Count > 0) // so there are winners at the end of the game
            {
                amount = this.PotCount / this.Winners.Count;

                foreach (HandEvaluator h in this.Winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in this.Players)
            {
                player.ClearCards(); // gets rid of cards of players still alive
            }
            if (this.Players.Count > 1)
            {
                // sets next DealerPos - if we want to "run" for a new game immediantly

                this.DealerPos++;
                this.DealerPos = this.DealerPos % this.Players.Count;

                this.ClearPublicCards();
                this.GameReplay = new GameReplay(this.Id, this.GameNumber);
                SystemLog log = new SystemLog(this.Id, this.GameReplay.ToString());
                // this.this._gameCenter.AddSystemLog(log);
                logControl.AddSystemLog(log);
                SetRoles();
            }
            else if (this.Players.Count == 1)
            {
                this.IsActiveGame = false;
                this.FirstEnter = true;
                if (!this.CurrentPlayer.OutOfMoney())
                    this.Players[0].isPlayerActive = false;
            }
            else //no players at all
            {
                this.IsActiveGame = false;
                this.FirstEnter = true;
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
                     h._player._hand._firstCard, h._player._hand._seconedCard,
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

        private void StartNewRoundAfterRaise()
        {
            if (this.PlayersInGame() < 2)
                EndHand();

            Player tempPlayer = this.CurrentPlayer;
            foreach (Player p in this.Players)
            {
                if (p != tempPlayer)
                {
                    int move;
                    this.CurrentPlayer = this.NextToPlay();
                     UpdateGameState();
                }
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
