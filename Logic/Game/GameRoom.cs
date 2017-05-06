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
        private List<Player> Players;
        public enum HandStep { PreFlop, Flop, Turn, River }
        public int Id { get; set; }
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
        private List<Tuple<int, List<Player>>> SidePots; //TODO use that in all in
        private GameReplay GameReplay;
        private ReplayManager ReplayManager;
        private GameCenter GameCenter;
        private Player CurrentPlayer;
        private Player DealerPlayer;
        private Player BbPlayer;
        private Player SbPlayer;
        private Decorator MyDecorator;
        private int MaxRaiseInThisRound;
        private int MinRaiseInThisRound;
        private int LastRaise;  
        private LogControl _logControl;
        private int GameNumber;
        private Player lastPlayerRaisedInRound;
        private Player FirstPlayerInRound;
        private int currentPlayerPos;
        private bool someOneRaised;
        private int MinBetInRoom;
        private int MaxRank;
        private int MinRank;

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
            SetTheBlinds();
            SidePots = new List<Tuple<int, List<Player>>>();
            Tuple<int,int> tup = GameCenter.UserLeageGapPoint(players[0].user.Id());
            MinRank = tup.Item1;
            MaxRank = tup.Item2;
            DealerPlayer = null;
            _logControl = LogControl.Instance;
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


        public bool DoAction(IUser user, ActionType action, int amount)
        {
            if (action == ActionType.Join)
            {
                return Join(user, amount);
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
            if (amount == 0)
            {
                return Check(player);
            }
            if (amount > 0)
            {
                return CallOrRaise(player, amount);
            }
            return false;
        }

        private bool Leave(Player player)
        {
            List<Player> relevantPlayers = new List<Player>();
            LeaveAction leave = new LeaveAction(player);
            GameReplay.AddAction(leave);
            SystemLog log = new SystemLog(Id, "Player with user Id: "
                + player.user.Id() + " left succsfully from room: " +Id);
            _logControl.AddSystemLog(log);
            player.user.EditUserMoney(player.user.Money() + (player.TotalChip - player.RoundChipBet));
            player.user.RemoveRoomFromActiveGameList(this);
            foreach (Player p in this.Players)
            {
                if (p.user.Id() != player.user.Id())
                {
                    relevantPlayers.Add(p);
                }
            }
            Players = relevantPlayers;
            return AfterAction();
        }

        private bool Join(IUser user, int amount)
        {
            if (CanJoinGameAsPlayer(user, amount))
            {
                int moneyToReduce = MyDecorator.GetEnterPayingMoney() + amount;
                if (user.ReduceMoneyIfPossible(moneyToReduce)){
                    Player p = new Player(user, amount, this.Id);
                    this.Players.Add(p);
                    return true;
                }
                return false;
            }
            return false;
        }

        //TODO: checking before calling to this function that this user&room ID are exist
        public bool CanJoinGameAsPlayer(IUser user, int amount)
        {
            if (user == null)
            {
                ErrorLog log = new ErrorLog("Error while tring to add player to room - " +
                    "invalid input - null user");
                _logControl.AddErrorLog(log);
                return false;
            }
            if (!MyDecorator.CanJoin(Players.Count , amount)) //check if the amount is in the range
            {
                return false;
            }

            int userMoneyAfterFeeAndEnter = user.Money() - MyDecorator.GetEnterPayingMoney() - amount;
            if (userMoneyAfterFeeAndEnter < 0)
            {
                ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: "
                    + user.Id() + " to room: " + Id + "insufficient money");
                _logControl.AddErrorLog(log);
                return false;
            }

            //User cant be spectator & player in the same room
            foreach (Spectetor s in Spectatores)
            {
                if (s.user.Id() == user.Id())
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with Id: " +
                        user.Id() + " to room: " + Id + " user is a spectetor in this room");
                    this._logControl.AddErrorLog(log);
                    return false;
                }      
            }

            if (!this.MyDecorator.CanAddMorePlayer(Players.Count))
            {
                ErrorLog log = new ErrorLog("Error while trying to add player: " + user.Id() +
                  " to the room: " + Id +" - room is full");
                this._logControl.AddErrorLog(log);
                return false;
            }

            if (!IsBetweenRanks(user.Points()))
            {
                ErrorLog log = new ErrorLog("Error while trying to add player, user with Id: "
                    + user.Id() + " to room: " + Id + "user point: " + user.Points() + 
                    " doest not met the game critiria");
                this._logControl.AddErrorLog(log);
                return false;
            }
            return true;
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
            return AfterAction();
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
            return AfterAction();
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
            return AfterAction();
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
                EndGame();
            }
            if (AllDoneWithTurn() )
            {
                return NextRound();
            }
            return NextCurrentPlayer();
        }

        private bool NextRound()
        {
            MoveChipsToPot();

            lastPlayerRaisedInRound = null;
            LastRaise = 0;
            InitializePlayerRound();
            //TODO: check that
            MaxRaiseInThisRound = MyDecorator.GetMaxAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);
            MinRaiseInThisRound = MyDecorator.GetMinAllowedRaise(this.Bb, this.maxBetInRound, this.Hand_Step);

            if (Hand_Step == HandStep.River) 
            {
                return EndGame(); 
            }

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
            return false;
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

        //public void Start()
        //{
        //    if (RoomThread != null && !this.IsActiveGame)
        //    {
        //        try
        //        {
        //            RoomThread.Start();
        //            Play();

        //        }
        //        catch (Exception e)
        //        {
        //            ErrorLog log = new ErrorLog("Room number " + this.Id + " was attempted to start but has allready been started.");
        //            _logControl.AddErrorLog(log);
        //        }
        //    }
        //}

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

        private int GetRaisePotLimit(Player p)
        {

            int potSize = this.PotCount;
            int lastRise = this.maxBetInRound;
            int playerPayInRound = p._payInThisRound;
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
                    _logControl.AddErrorLog(log);
                    break;
                }
                return false;
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
                    break;
                }
                return true;
            }
            return false;
        }

        public bool IsBetweenRanks(int playerRank)
        {
            return (playerRank <= this.MaxRank) && (playerRank >= this.MinRank);
        }
    }
}
