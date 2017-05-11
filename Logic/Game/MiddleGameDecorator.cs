using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Logic
{

    public class MiddleGameDecorator : Decorator
    {
        private Decorator NextDecorator;
        public GameMode GameMode { get; set; }
        public int BB { get; set; }
        public int SB { get; set; }

        public void SetNextDecorator(Decorator d)
        {
            NextDecorator = d;
        }

        public MiddleGameDecorator(GameMode gameModeChosen, int bb, int sb)
        {
            this.GameMode = gameModeChosen;
            this.BB = bb;
            this.SB = sb;
        }

        public bool CanStartTheGame(int numOfPlayers)
        {
            throw new NotImplementedException();
        }

        public bool CanSpectatble()
        {
            throw new NotImplementedException();
        }

        public int GetMinBetInRoom()
        {
            throw new NotImplementedException();
        }

        public int GetEnterPayingMoney()
        {
            throw new NotImplementedException();
        }

        public int GetStartingChip()
        {
            throw new NotImplementedException();
        }


        public int GetMaxAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            switch (GameMode)
            {
                case GameMode.Limit:
                    switch (step)
                    {
                        case GameRoom.HandStep.Flop:
                        case GameRoom.HandStep.PreFlop:
                            return BB;
                        case GameRoom.HandStep.River:
                        case GameRoom.HandStep.Turn:
                            return BB * 2;
                    }
                    break;
                case GameMode.NoLimit:
                    return int.MaxValue;
                case GameMode.PotLimit:
                    return maxCommited;
                default:
                    break;
            }
            //not spouse to arrive here
            return -1;
        }

        public int GetMinAllowedRaise(int maxCommited, GameRoom.HandStep step)
        {
            if (this.GameMode == GameMode.NoLimit)
            {
                return maxCommited;
            }
            return 0;
        }

        public bool CanRaise(int currentPlayerBet, int maxBetInRound, GameRoom.HandStep step)
        {
            if (currentPlayerBet <= GetMaxAllowedRaise(maxBetInRound, step) && currentPlayerBet > 0)
                return true;
            return false;
        }

        //check if the amount is in the range
        public bool CanJoin(int playersCount, int amount, IUser user)
        {
            throw new NotImplementedException();
        }

        public bool IsGameModeEqual(GameMode gm)
        {
            return this.GameMode == gm;
        }

        public bool IsGameBuyInPolicyEqual(int buyIn)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMinPlayerEqual(int min)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMaxPlayerEqual(int max)
        {
            throw new NotImplementedException();
        }

        public bool IsGameMinBetEqual(int nimBet)
        {
            throw new NotImplementedException();
        }

        public bool IsGameStartingChipEqual(int startingChip)
        {
            throw new NotImplementedException();
        }

        public int GetMinPlayerInRoom()
        {
            throw new NotImplementedException();
        }

        public int GetMaxPlayerInRoom()
        {
            throw new NotImplementedException();
        }

        public GameMode GetGameMode()
        {
            throw new NotImplementedException();
        }
    }
}



//public int PlayerPlay()
//{
//    int toReturn = -3;
//    int maxRaise = MaxRaiseInThisRound;
//    int minRaise = MinRaiseInThisRound;
//    int fold = -1;
//    bool isLimit = (this.MyDecorator.GetGameMode() == GameMode.Limit);
//    GameMode gm;


//    int playerMoney = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
//    //raise - <Raise,bool is limit,maxRaise, minRaise> - if true must raise equal to max.
//    //bet - <Bet,bool is limit,maxRaise, minRaise> - if true must Bet equal to max.
//    //check <Check, false, 0, 0>
//    //fold - <Fold, false,-1,-1>
//    //call - <Call, false,call amount, 0>
//    List<Tuple<GameMove, bool, int, int>> moveToSend = new List<Tuple<GameMove, bool, int, int>>();
//    int callAmount = maxRaise - this.CurrentPlayer._payInThisRound;
//    bool canCheck = (this.maxBetInRound == 0);
//    try
//    {

//        switch (this.Hand_Step)
//        {
//            case (GameRoom.HandStep.PreFlop):
//                if (this.CurrentPlayer == this.SbPlayer && this.CurrentPlayer._payInThisRound == 0)//start of game - small blind bet
//                {
//                    toReturn = this.Sb;
//                    return toReturn;
//                }

//                if (this.CurrentPlayer == this.BbPlayer && this.CurrentPlayer._payInThisRound == 0) // start game big blind bet
//                {
//                    toReturn = this.Bb;
//                    return toReturn;
//                }
//                if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
//                {
//                    maxRaise = this.Bb;
//                    if (this.CurrentPlayer == this.SbPlayer && this.CurrentPlayer._payInThisRound == this.Sb)
//                    {
//                        callAmount = this.Bb - this.Sb;
//                        maxRaise = callAmount + maxRaise;
//                    }
//                    else if (this.CurrentPlayer == this.BbPlayer && this.CurrentPlayer._payInThisRound == this.Bb)
//                    {
//                        maxRaise = this.Bb;
//                    }
//                    else if (this.CurrentPlayer._payInThisRound == 0)
//                    {
//                        callAmount = this.Bb;
//                        maxRaise = this.Bb + callAmount;
//                    }
//                    else if ((this.CurrentPlayer != this.BbPlayer || this.CurrentPlayer != this.SbPlayer) &&
//                             this.CurrentPlayer._payInThisRound != 0)
//                    {
//                        callAmount = maxRaise - this.CurrentPlayer._payInThisRound;
//                        maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
//                    }

//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
//                {
//                    //todo - yarden - max money all in equal to this?
//                    maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
//                    minRaise = LastRaise;
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
//                {
//                    maxRaise = GetRaisePotLimit(this.CurrentPlayer);
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                Tuple<GameMove, bool, int, int> RaisePreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> CallPreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
//                Tuple<GameMove, bool, int, int> FoldPreFlop = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
//                moveToSend.Add(RaisePreFlop);
//                moveToSend.Add(CallPreFlop);
//                moveToSend.Add(FoldPreFlop);
//                break;


//            case (GameRoom.HandStep.Flop):

//                if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "small bet" - equal to big blind
//                {
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                    if (this.CurrentPlayer._payInThisRound != 0)
//                    {
//                        maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
//                    }
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
//                {
//                    //todo - yarden - max money all in equal to this?
//                    maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
//                    minRaise = LastRaise;
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
//                {
//                    maxRaise = GetRaisePotLimit(this.CurrentPlayer);
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                if (canCheck)
//                {
//                    Tuple<GameMove, bool, int, int> CheckFlop =
//                        new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
//                    moveToSend.Add(CheckFlop);
//                }
//                Tuple<GameMove, bool, int, int> RaiseFlop = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> BetFlop = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> CallFlop = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
//                Tuple<GameMove, bool, int, int> FoldFlop = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
//                moveToSend.Add(RaiseFlop);
//                moveToSend.Add(CallFlop);
//                moveToSend.Add(FoldFlop);
//                moveToSend.Add(BetFlop);
//                break;


//            case (GameRoom.HandStep.Turn):
//                if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
//                {
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                    if (this.CurrentPlayer._payInThisRound != 0)
//                    {
//                        maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
//                    }

//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
//                {
//                    //todo - yarden - max money all in equal to this?
//                    maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
//                    minRaise = LastRaise;
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
//                {
//                    maxRaise = GetRaisePotLimit(this.CurrentPlayer);
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                if (canCheck)
//                {
//                    Tuple<GameMove, bool, int, int> CheckTurn =
//                        new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
//                    moveToSend.Add(CheckTurn);
//                }
//                Tuple<GameMove, bool, int, int> RaiseTurn = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> BetTurn = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> CallTurn = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
//                Tuple<GameMove, bool, int, int> FoldTurn = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
//                moveToSend.Add(RaiseTurn);
//                moveToSend.Add(CallTurn);
//                moveToSend.Add(FoldTurn);
//                moveToSend.Add(BetTurn);

//                break;
//            case (GameRoom.HandStep.River):
//                if (this.MyDecorator.GetGameMode() == GameMode.Limit)// raise/Be must be "big bet" - equal to big blind times 2
//                {
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                    if (this.CurrentPlayer._payInThisRound != 0)
//                    {
//                        maxRaise = maxRaise - this.CurrentPlayer._payInThisRound;
//                    }
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.NoLimit) //can do all in, min raise / bet must be equal to priveus raise
//                {
//                    //todo - yarden - max money all in equal to this?
//                    maxRaise = this.CurrentPlayer.TotalChip - this.CurrentPlayer.RoundChipBet;
//                    minRaise = LastRaise;
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                else if (this.MyDecorator.GetGameMode() == GameMode.PotLimit)//Max raise is equal to the size of the pot include the sum need to call.
//                {
//                    maxRaise = GetRaisePotLimit(this.CurrentPlayer);
//                    callAmount = LastRaise - this.CurrentPlayer._payInThisRound;
//                }
//                if (canCheck)
//                {
//                    Tuple<GameMove, bool, int, int> CheckRiver =
//                        new Tuple<GameMove, bool, int, int>(GameMove.Check, false, 0, 0);
//                    moveToSend.Add(CheckRiver);
//                }
//                Tuple<GameMove, bool, int, int> RaiseRiver = new Tuple<GameMove, bool, int, int>(GameMove.Raise, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> BetRiver = new Tuple<GameMove, bool, int, int>(GameMove.Bet, isLimit, maxRaise, minRaise);
//                Tuple<GameMove, bool, int, int> CallRiver = new Tuple<GameMove, bool, int, int>(GameMove.Call, false, callAmount, 0);
//                Tuple<GameMove, bool, int, int> FoldRiver = new Tuple<GameMove, bool, int, int>(GameMove.Fold, false, -1, -1);
//                moveToSend.Add(RaiseRiver);
//                moveToSend.Add(CallRiver);
//                moveToSend.Add(FoldRiver);
//                moveToSend.Add(BetRiver);
//                break;
//            default:
//                ErrorLog log = new ErrorLog("error in roung in room: " + this.Id + "the tound is not prefop / flop / turn / river");
//                //GameCenter.Instance.AddErrorLog(log);
//                _logControl.AddErrorLog(log);
//                break;

//        }
//        if (!IsTestMode)
//        {
//            Tuple<GameMove, int> getSelectedFromPlayer =
//                GameCenter.Instance.SendUserAvailableMovesAndGetChoosen(moveToSend);
//            toReturn = getSelectedFromPlayer.Item2;
//        }
//        else
//        {
//            toReturn = this.CurrentPlayer.moveForTest;
//        }
//    }
//    catch (Exception e)
//    {
//        ErrorLog log = new ErrorLog("error in play of player with Id: " + this.CurrentPlayer.user.Id() + " somthing went wrong");
//        // GameCenter.Instance.AddErrorLog(log);
//        _logControl.AddErrorLog(log);
//    }

//    return toReturn;
//}
