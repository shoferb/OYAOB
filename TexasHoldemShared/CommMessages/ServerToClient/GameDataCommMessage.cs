using System.Collections.Generic;
using System.Linq;
using TexasHoldem;
using TexasHoldem.Logic.Game;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    //This class is used to pass game data / table updates to the client
    public class GameDataCommMessage : CommunicationMessage
    {
        //data types:
       
        //fields:
        public int RoomId;
        public string ActionPlayerName;
        public int BetAmount;
        public ActionType Action;
        public Card[] PlayerCards = new Card[2];
        public List<Card> TableCards;

        public int TotalChips;
        public int PotSize;

        public List<string> AllPlayerNames;
        public List<string> AllSpectatorNames;
        public string DealerName;
        public string BbName;
        public string SbName;

        public string CurrPlayerTurn;
        public bool IsSucceed;

        public string CurrRound;

        public GameDataCommMessage() : base(-1, -1) { } //for parsing

        public GameDataCommMessage(GameDataCommMessage other) : base(other.UserId, other.SessionId)
        {
            RoomId = other.RoomId;
            TotalChips = other.TotalChips;
            PotSize = other.PotSize;
            PlayerCards[0] = other.PlayerCards[0];
            PlayerCards[1] = other.PlayerCards[1];
            DealerName = other.DealerName;
            BbName = other.BbName;
            SbName = other.SbName;
            IsSucceed = other.IsSucceed;
            CurrPlayerTurn = other.CurrPlayerTurn;
            ActionPlayerName = other.ActionPlayerName;
            BetAmount = other.BetAmount;
            Action = other.Action;
            SessionId = other.SessionId;
            TableCards = new List<Card>();
            AllPlayerNames = other.AllPlayerNames;
            AllSpectatorNames = other.AllSpectatorNames;
            other.TableCards.ForEach(c => TableCards.Add(c));
            CurrRound = other.CurrRound;
        }

        public GameDataCommMessage(int userId, int roomId, long sid, Card card1, Card card2, List<Card> tableCards, 
            int chips, int pot, List<string> allPlayerNames, List<string> allSpectatorNames, string dealerName, string bbName, string sbName, bool success, 
            string currPlayer, string actionPlayer, int bet, ActionType actionType, string round) : base(userId, sid)
        {
            RoomId = roomId;
            TableCards = tableCards;
            TotalChips = chips;
            PotSize = pot;
            PlayerCards[0] = card1;
            PlayerCards[1] = card2;
            AllPlayerNames = allPlayerNames;
            AllSpectatorNames = allSpectatorNames;
            DealerName = dealerName;
            BbName = bbName;
            SbName = sbName;
            IsSucceed = success;
            CurrPlayerTurn = currPlayer;
            ActionPlayerName = actionPlayer;
            BetAmount = bet;
            Action = actionType;
            SessionId = sid;
            CurrRound = round;
        }
       
        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(GameDataCommMessage))
            {
                var afterCasting = (GameDataCommMessage)other;
                bool good = IsSucceed == afterCasting.IsSucceed && DealerName.Equals(afterCasting.DealerName) &&
                       UserId == afterCasting.UserId && PotSize == afterCasting.PotSize && 
                       CurrRound.Equals(afterCasting.CurrRound) &&
                       TotalChips == afterCasting.TotalChips && BbName.Equals(afterCasting.BbName) &&
                       SbName.Equals(afterCasting.SbName) && CurrPlayerTurn.Equals(afterCasting.CurrPlayerTurn);
                good = PlayerCards.Aggregate(good, (current, card) => current && afterCasting.PlayerCards.Contains(card));
                good = AllPlayerNames.Aggregate(good, (current, name) => current && afterCasting.AllPlayerNames.Contains(name));
                good = AllSpectatorNames.Aggregate(good, (current, name) => current && afterCasting.AllSpectatorNames.Contains(name));
                good = TableCards.Aggregate(good, (current, card) => current && afterCasting.TableCards.Contains(card));
                good = good && afterCasting.ActionPlayerName.Equals(ActionPlayerName);
                good = good && afterCasting.ActionPlayerName.Equals(ActionPlayerName);
                good = good && afterCasting.BetAmount == this.BetAmount;
                good = good && afterCasting.Action.Equals(Action);
                return good;
            }
            return false;
        }
    }
}
