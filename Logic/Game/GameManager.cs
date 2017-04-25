using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldem.Logic.Actions;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class GameManager
    {
        public int _verifyAction;      
        public bool _gameOver = false;
        public Player _currentPlayer;
        public Player _dealerPlayer;
        public Player _bbPlayer;
        public Player _sbPlayer;
        public List<HandEvaluator> _winners;
        public static bool _firstEnter = true;
        private int buttonPos;
        private bool _backFromRaise;
        ConcreteGameRoom _state; //game
        public bool isTestMode { get; set; }

        public int maxRaiseInThisRound{ get; set; } //מה המקסימום raise / bet שיכול לבצע בסיבוב הנוכחי 
        public int minRaiseInThisRound { get; set; } //המינימום שחייב לבצע בסיסוב הנוכחי
        public int lastRaise { get; set; }  //change to maxCommit

       
        //change to gameroom
        public GameManager(ConcreteGameRoom state)
        {
           this._state = state;
            InitRaiseField();
        }

        public void Start()
        {
            Play();
        }
        public void SetRoles()
        {
            this._state._handStep = ConcreteGameRoom.HandStep.PreFlop;
            Deck deck = new Deck();
            this._state._deck = deck;
            this._state._players = this._state._players;
            this.buttonPos = this._state._dealerPos;

            if (this._state._players.Count > 2)
            {
                //delaer
                this._dealerPlayer = this._state._players[this.buttonPos];
                // small blind
                this._sbPlayer = this._state._players[(this.buttonPos + 1)% this._state._players.Count];
                this._state._players[(this.buttonPos + 1) % this._state._players.Count].CommitChips(this._state._bb / 2);
                // big blind
                this._bbPlayer = this._state._players[(this.buttonPos + 2)% this._state._players.Count];
                this._state._players[(this.buttonPos + 2) % this._state._players.Count].CommitChips(this._state._bb);
                //actionPos will keep track on the curr player.
                this._state._actionPos = (this.buttonPos + 3) % this._state._players.Count;

            }
            else
            {
                this._state._actionPos = (this.buttonPos) % this._state._players.Count; 
                // small blind
                this._dealerPlayer = this._state._players[(this.buttonPos) % this._state._players.Count];
                this._sbPlayer = this._state._players[(this.buttonPos) % this._state._players.Count];
                this._state._players[(this.buttonPos) % this._state._players.Count].CommitChips(this._state._bb / 2);
                // big blind
                this._bbPlayer = this._state._players[(this.buttonPos + 1)% this._state._players.Count];
                this._state._players[(this.buttonPos + 1) % this._state._players.Count].CommitChips(this._state._bb);
            }

            StartGame startAction = new StartGame(_state._players, _dealerPlayer, _sbPlayer, _bbPlayer);
            SystemLog log = new SystemLog(this._state._id, startAction.ToString());
            this._state._gameCenter.AddSystemLog(log);
            _state._gameReplay.AddAction(startAction);

            foreach (Player player in this._state._players)
            {
                player.isPlayerActive = true;
                player.AddHoleCards(deck.Draw(), deck.Draw());
                HandCards hand = new HandCards(player, player._hand._firstCard,
                    player._hand._seconedCard);
                _state._gameReplay.AddAction(hand);
                log = new SystemLog(this._state._id, hand.ToString());
                this._state._gameCenter.AddSystemLog(log);
            }
            this._state.UpdateMaxCommitted();
            this._state.moveBBnSBtoPot(this._bbPlayer, this._sbPlayer);
            switch (_state._players.Count)
            {
                case 2:
                case 3:
                    this._currentPlayer = this._bbPlayer;
                    break;
                default:
                    this._currentPlayer = this._state._players[this._state._actionPos];
                    break;
            }

        }

        public bool Play() 
        {
            
            if (this._state._players.Count < this._state._minPlayersInRoom) return false;
            else
            {
                //add to users list of available game to replay
                foreach (Player p in _state._players)
                {
                    p.AddGameAvailableToReplay(_state._id, _state._gameNumber);
                }

                if (_firstEnter)
                {
                    StartTheGame();
                }       
                while (!this._state.AllDoneWithTurn())
                {
                   int move;
                   this._currentPlayer = this._state.NextToPlay();
                    //move = this.playTurn(player)
                    move = Play(this._currentPlayer);
                    PlayerDesicion(move);
                    if (_backFromRaise)
                    {
                        _backFromRaise = false;
                        break;
                    }
                   this._state.UpdateGameState();
                    this._state.CheckIfPlayerWantToLeave();
                }
                this._state.MoveChipsToPot();

                if (this._state.AllDoneWithTurn() || this._state.PlayersInGame() < 2)
                {
                    if (ProgressHand(this._state._handStep))
                    {
                        // progresses _hand and returns whether _hand is over (the last _handStep was river)
                        EndHand();
                        return true;
                    }
                    else
                    {
                        _currentPlayer = this._state.NextToPlay();
                        _dealerPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_dealerPlayer) + 1) % this._state._players.Count];
                        _sbPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_sbPlayer) + 1) % this._state._players.Count];
                        _bbPlayer =
                            this._state._players[
                                (this._state._players.IndexOf(_bbPlayer) + 1) % this._state._players.Count];
                        return Play();
                    }
                       
                }
               
            }
            return true;
            
        }

        // return -1 if player select fold
        // return -2 if player selsct exit
        //return positive number bigger than 0 for call / raise 
        // return 0 for check
        //return -3 error
        private int Play(Player _currentPlayer)
        {
            int toReturn = -3;
            if (!isTestMode)
            {
                
            }
            int maxRaise = maxRaiseInThisRound;
            int minRaise = minRaiseInThisRound;
            int fold = -1;
            bool isLimit = (_state._gameMode == GameMode.Limit);
            GameMode gm;
            
      
            int playerMoney = _currentPlayer._totalChip - _currentPlayer._gameChip;
            //raise - <Raise,bool is limit,maxRaise, minRaise> - if true must raise equal to max.
            //bet - <Bet,bool is limit,maxRaise, minRaise> - if true must Bet equal to max.
            //check <Check, false, 0, 0>
            //fold - <Fold, false,-1,-1>
            //call - <Call, false,call amount, 0>
            List<Tuple<Action,bool,int,int>> moveToSend = new List<Tuple<Action,bool, int, int>>(); 
            int callAmount = maxRaise - _currentPlayer._payInThisRound;
            bool canCheck  = (_state._maxCommitted == 0);
            try
            {
                
                switch (_state._handStep)
                {
                    case (ConcreteGameRoom.HandStep.PreFlop):
                        if (_currentPlayer == _sbPlayer && _currentPlayer._payInThisRound ==0)//start of game - small blind bet
                        {
                            toReturn = _state._sb;
                            return toReturn;
                        }
                       
                        if (_currentPlayer == _bbPlayer && _currentPlayer._payInThisRound == 0) // start game big blind bet
                        {
                            toReturn = _state._bb;
                            return toReturn;
                        }
                        if (_state._gameMode == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
                        {
                            maxRaise = _state._bb;
                            if (_currentPlayer == _sbPlayer && _currentPlayer._payInThisRound == _state._sb)
                            {
                                callAmount = _state._bb - _state._sb;
                                maxRaise = callAmount + maxRaise;
                            }
                           else if (_currentPlayer == _bbPlayer && _currentPlayer._payInThisRound == _state._bb)
                            {
                                maxRaise = _state._bb;
                            }
                            else if (_currentPlayer._payInThisRound == 0)
                            {
                                callAmount = _state._bb;
                                maxRaise = _state._bb + callAmount;
                            }
                            else if ((_currentPlayer != _bbPlayer || _currentPlayer != _sbPlayer) &&
                                     _currentPlayer._payInThisRound != 0)
                            {
                                callAmount = maxRaise - _currentPlayer._payInThisRound;
                                maxRaise = maxRaise - this._currentPlayer._payInThisRound;
                            }
                           
                        }
                        else if (_state._gameMode == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = _currentPlayer._totalChip - _currentPlayer._gameChip;
                            minRaise = lastRaise;
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                        }
                        else if (_state._gameMode == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(_currentPlayer);
                            callAmount = lastRaise - this._currentPlayer._payInThisRound;
                        }
                        Tuple<Action, bool, int, int>  RaisePreFlop = new Tuple<Action,bool, int, int>(Action.Raise,isLimit,maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallPreFlop = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldPreFlop = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaisePreFlop);
                        moveToSend.Add(CallPreFlop);
                        moveToSend.Add(FoldPreFlop);
                        break;


                    case (ConcreteGameRoom.HandStep.Flop):
                       
                       if (_state._gameMode == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
                       {
                           callAmount = lastRaise - _currentPlayer._payInThisRound;
                           if (_currentPlayer._payInThisRound != 0)
                           {
                               maxRaise = maxRaise - _currentPlayer._payInThisRound;
                           }
                       }
                        else if (_state._gameMode == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = _currentPlayer._totalChip - _currentPlayer._gameChip;
                            minRaise = lastRaise;
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                        }
                         else if (_state._gameMode == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                         {
                             maxRaise = GetRaisePotLimit(_currentPlayer);
                             callAmount = lastRaise - this._currentPlayer._payInThisRound;
                        }
                        if (canCheck)
                        {
                            Tuple<Action, bool, int, int> CheckFlop =
                                new Tuple<Action, bool, int, int>(Action.Check, false, 0, 0);
                            moveToSend.Add(CheckFlop);
                        }
                        Tuple<Action, bool, int, int> RaiseFlop = new Tuple<Action, bool, int, int>(Action.Raise, isLimit,  maxRaise , minRaise);
                        Tuple<Action, bool, int, int> BetFlop = new Tuple<Action, bool, int, int>(Action.Bet, isLimit, maxRaise, minRaise);
                        Tuple<Action, bool, int, int> CallFlop = new Tuple<Action, bool, int, int>(Action.Call, false, callAmount, 0);
                        Tuple<Action, bool, int, int> FoldFlop = new Tuple<Action, bool, int, int>(Action.Fold, false, -1, -1);
                        moveToSend.Add(RaiseFlop);
                        moveToSend.Add(CallFlop);
                        moveToSend.Add(FoldFlop);
                        moveToSend.Add(BetFlop);
                        break;


                    case (ConcreteGameRoom.HandStep.Turn):
                        if (_state._gameMode == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                            if (_currentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - _currentPlayer._payInThisRound;
                            }
                           
                        }
                        else if (_state._gameMode == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = _currentPlayer._totalChip - _currentPlayer._gameChip;
                            minRaise = lastRaise;
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                        }
                        else if (_state._gameMode == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(_currentPlayer);
                            callAmount = lastRaise - this._currentPlayer._payInThisRound;
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
                    case (ConcreteGameRoom.HandStep.River):
                        if (_state._gameMode == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
                        {
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                            if (_currentPlayer._payInThisRound != 0)
                            {
                                maxRaise = maxRaise - _currentPlayer._payInThisRound;
                            }
                        }
                        else if (_state._gameMode == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
                        {
                            //todo - yarden - max money all in equal to this?
                            maxRaise = _currentPlayer._totalChip - _currentPlayer._gameChip;
                            minRaise = lastRaise;
                            callAmount = lastRaise - _currentPlayer._payInThisRound;
                        }
                        else if (_state._gameMode == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
                        {
                            maxRaise = GetRaisePotLimit(_currentPlayer);
                            callAmount = lastRaise - this._currentPlayer._payInThisRound;
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
                        ErrorLog log = new ErrorLog("error in roung in room: "+_state._id+ "the tound is not prefop / flop / turn / river");
                        GameCenter.Instance.AddErrorLog(log);
                        break;
    
                }
                if (!isTestMode)
                {
                    Tuple<Action, int> getSelectedFromPlayer =
                        GameCenter.Instance.SendUserAvailableMovesAndGetChoosen(moveToSend);
                    toReturn = getSelectedFromPlayer.Item2;
                }
                else
                {
                    toReturn = _currentPlayer.moveForTest;
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog("error in play of player with id: "+_currentPlayer.Id + " somthing went wrong");
                GameCenter.Instance.AddErrorLog(log);
            }

            return toReturn;
        }

        
       
        private void InitRaiseField()
        {

            this.maxRaiseInThisRound = 0;
            this.minRaiseInThisRound = 0;
            this.lastRaise = 0;

        }

        //Todo - YARDEN  - add call at the biginning of each round
        private void InitializePlayerRound()
        {
            foreach (Player player in _state._players)
            {
                player.InitPayInRound();
            }
        }

        //Todo - YARDEN add call at the biginning of every Round - after change the enum of round
        private void RaiseFieldAtEveryRound()
        {

            if (_state._gameMode == GameMode.Limit && (_state._handStep == ConcreteGameRoom.HandStep.Flop ||
                                                       _state._handStep == ConcreteGameRoom.HandStep.PreFlop))
            {
                maxRaiseInThisRound = _state._bb;
            }
            if (_state._gameMode == GameMode.Limit && (_state._handStep == ConcreteGameRoom.HandStep.River ||
                                                       _state._handStep == ConcreteGameRoom.HandStep.Turn))
            {
                maxRaiseInThisRound = _state._bb * 2;
            }
            if (_state._gameMode == GameMode.NoLimit)
            {
                minRaiseInThisRound = _state._maxCommitted;
            }
        }


        //return the max limit for player
        private int GetRaisePotLimit(Player p)
        {
            
            int potSize = _state._potCount;
            int lastRise= _state._maxCommitted;
            int playerPayInRound = p._payInThisRound;
            int toReturn = (lastRise - playerPayInRound) + potSize;
            return toReturn;
        }

        public void PlayerDesicion(int move)
        {
            int max = this._state._maxCommitted;
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

        private void StartNewRoundAfterRaise()
        {
            if (this._state.PlayersInGame() < 2)
                EndHand();

            Player tempPlayer = this._currentPlayer;
            foreach (Player p in _state._players)
            {
                if (p != tempPlayer)
                {
                    int move;
                    this._currentPlayer = this._state.NextToPlay();
                    //todo - Yarden - change to new Play
                    //move = this._currentPlayer.Play(this._state._sb, this._state._handStep);
                    //PlayerDesicion(move);

                    this._state.UpdateGameState();
                }
            }
            _backFromRaise = true;
        }

        private void StartTheGame()
        {
            _state._gameReplay = new GameReplay(_state._id, _state._gameNumber);
            SystemLog log = new SystemLog(this._state._id, _state._gameReplay.ToString());
            this._state._gameCenter.AddSystemLog(log);

            this._state._dealerPos = 0;
            SetRoles();
            _firstEnter = false;
            _state._isActiveGame = true;
        }


        public bool ProgressHand(ConcreteGameRoom.HandStep previousStep)
        {            
            

            if (this._state.PlayersInGame() < 2)
                return true;

            switch (previousStep)
            {
                case ConcreteGameRoom.HandStep.PreFlop:
                    for (int i = 0; i <= 2; i++)
                        this._state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.Flop:
                    this._state.AddNewPublicCard();
                  break;
                case ConcreteGameRoom.HandStep.Turn:
                    this._state.AddNewPublicCard();
                    break;
                case ConcreteGameRoom.HandStep.River:
                   return true;

                default:
                    break;
            }


            int numNextStep = (int)previousStep + 1;
            this._state._handStep = (ConcreteGameRoom.HandStep)numNextStep;

            if (this._state.PlayersInGame() - this._state.PlayersAllIn() < 2)
            {
                ProgressHand(this._state._handStep); // recursive, runs until we'll hit the river
                return true;
            }
            else
                this._state.EndTurn(); 

            return false;

        }

        public void EndHand()
        {
            this._state._gameNumber++;
            List<Player> playersLeftInGame = new List<Player>();        
            foreach (Player player in this._state._players)
                if (player._totalChip != 0)
                    playersLeftInGame.Add(player);
                else
                {
                   // RulesAndMethods.AddToLog("Player " + player.name + " was eliminated.");
                    player.isPlayerActive = false;
                    player.ClearCards(); // gets rid of cards for people who are eliminated
                }

            this._state.EndTurn();
            _winners= FindWinner(this._state._publicCards, playersLeftInGame);
            _state._replayManager.AddGameReplay(_state._gameReplay);
            int amount;
            if (_winners.Count > 0) // so there are winners at the end of the game
            {
                amount = this._state._potCount/_winners.Count;

                foreach (HandEvaluator h in _winners)
                {
                    h._player.Win(amount);
                }
            }
            foreach (Player player in this._state._players)
            {
                player.ClearCards(); // gets rid of cards of _players still alive
            }
            if (this._state._players.Count > 1)
            {
                // sets next _dealerPos - if we want to "run" for a new game immediantly

                this._state._dealerPos++;
                this._state._dealerPos = this._state._dealerPos % this._state._players.Count;

                this._state.ClearPublicCards();
                _state._gameReplay = new GameReplay(_state._id, _state._gameNumber);
                SystemLog log = new SystemLog(this._state._id, _state._gameReplay.ToString());
                this._state._gameCenter.AddSystemLog(log);
                SetRoles();
            }
            else if (_state._players.Count == 1)
            {
                this._state._isActiveGame = false;
                _firstEnter = true;
                if (!_currentPlayer.OutOfMoney())
                    this._state._players[0].isPlayerActive = false; 
            }
            else //no players at all
            {
                this._state._isActiveGame = false;
                _firstEnter = true;
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
                else if(h._rank.CompareTo(winners.ElementAt(0)._rank) == 0)
                {
                    winners.Add(h);
                }
            }
            if (winners.Count() == 1)
            {
                WinAction win = new WinAction(winners[0]._player, 
                    winners[0]._player._hand._firstCard, winners[0]._player._hand._seconedCard,
                    _state._potCount, table, winners[0]._relevantCards);
                _state._gameReplay.AddAction(win);
                 SystemLog log = new SystemLog(this._state._id, win.ToString());
                this._state._gameCenter.AddSystemLog(log);

                return winners;
            }
            return EvalTies(winners, table);
        }

        private List<HandEvaluator> EvalTies(List<HandEvaluator> winners, List<Card> table)
        {
            List<Card> playerOneCards;
            List<Card> playerTwoCards;
            int i = 1;
            bool tie;
            while (winners.Count >= 2 && i < winners.Count() )
            {
                playerOneCards = winners.ElementAt(i-1)._relevantCards;
                playerTwoCards = winners.ElementAt(i)._relevantCards;
                FixAceTo14(playerOneCards);
                FixAceTo14(playerTwoCards);
                playerOneCards = playerOneCards.OrderByDescending(o => o._value).ToList();
                playerTwoCards = playerTwoCards.OrderByDescending(o => o._value).ToList();
                tie = true;
                for (int j=0; j < playerOneCards.Count && j < playerTwoCards.Count; j++)
                {
                    if (playerOneCards.ElementAt(j)._value > playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i);
                        tie = false;
                        break;
                    }
                    else if(playerOneCards.ElementAt(j)._value < playerTwoCards.ElementAt(j)._value)
                    {
                        winners.RemoveAt(i-1);
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
                     (int)_state._potCount/winners.Count, table, h._relevantCards);
                     _state._gameReplay.AddAction(win);
                SystemLog log = new SystemLog(this._state._id, win.ToString());
                this._state._gameCenter.AddSystemLog(log);
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

        public void Fold()
        {
            this._currentPlayer._lastAction = "fold";
            this._currentPlayer.isPlayerActive = false;
            FoldAction fold = new FoldAction(_currentPlayer, _currentPlayer._hand._firstCard,
                _currentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this._state._id, fold.ToString());
            this._state._gameCenter.AddSystemLog(log);
            _state._gameReplay.AddAction(fold);
        }

        public void Check()
        {
            this._currentPlayer._lastAction = "check";
            CheckAction check = new CheckAction(_currentPlayer, _currentPlayer._hand._firstCard,
                 _currentPlayer._hand._seconedCard);
            SystemLog log = new SystemLog(this._state._id, check.ToString());
            this._state._gameCenter.AddSystemLog(log);
            _state._gameReplay.AddAction(check);
        }

        public void Call(int additionalChips)
        {
            this._currentPlayer._lastAction = "call";
            additionalChips = Math.Min(additionalChips, this._currentPlayer._totalChip); // if can't afford that many chips in a call, go all in           
            this._currentPlayer.CommitChips(additionalChips);
            CallAction call = new CallAction(_currentPlayer, _currentPlayer._hand._firstCard,
                _currentPlayer._hand._seconedCard, additionalChips);
            _state._gameReplay.AddAction(call);
            SystemLog log = new SystemLog(this._state._id, call.ToString());
            this._state._gameCenter.AddSystemLog(log);
        }

        public void Call()
        {
            Call(this._state.ToCall());

        }


       public void Raise(int additionalChips)
        {
            _state._maxCommitted += additionalChips;
            this._currentPlayer._lastAction = "raise";
            this._currentPlayer.CommitChips(additionalChips);
            RaiseAction raise = new RaiseAction(_currentPlayer, _currentPlayer._hand._firstCard,
                 _currentPlayer._hand._seconedCard, additionalChips);
            _state._gameReplay.AddAction(raise);
            SystemLog log = new SystemLog(this._state._id, raise.ToString());
            this._state._gameCenter.AddSystemLog(log);
        }


    }
 }

