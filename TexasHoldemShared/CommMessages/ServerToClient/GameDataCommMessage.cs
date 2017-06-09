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
        public string actionPlayerName;
        public int betAmount;
        public CommunicationMessage.ActionType action;
        public Card[] PlayerCards = new Card[2];
        public List<Card> TableCards;

        public int TotalChips;
        public int PotSize;

        public List<string> AllPlayerNames;
        public string DealerName;
        public string BbName;
        public string SbName;

        public string CurrPlayerTurn;
        public bool IsSucceed;

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
            actionPlayerName = other.actionPlayerName;
            betAmount = other.betAmount;
            action = other.action;
            SessionId = other.SessionId;
            TableCards = new List<Card>();
            AllPlayerNames = other.AllPlayerNames;
            other.TableCards.ForEach(c => TableCards.Add(c));
        }

        public GameDataCommMessage(int userId, int roomId, long sid, Card card1, Card card2, List<Card> tableCards, 
            int chips, int pot, List<string> allPlayerNames, string dealerName, string bbName, string sbName, bool success, 
            string currPlayer, string actionPlayer, int bet, ActionType actionType) : base(userId, sid)
        {
            RoomId = roomId;
            TableCards = tableCards;
            TotalChips = chips;
            PotSize = pot;
            PlayerCards[0] = card1;
            PlayerCards[1] = card2;
            AllPlayerNames = allPlayerNames;
            DealerName = dealerName;
            BbName = bbName;
            SbName = sbName;
            IsSucceed = success;
            CurrPlayerTurn = currPlayer;
            actionPlayerName = actionPlayer;
            betAmount = bet;
            action = actionType;
            SessionId = sid;
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
                       TotalChips == afterCasting.TotalChips && BbName.Equals(afterCasting.BbName) &&
                       SbName.Equals(afterCasting.SbName) && CurrPlayerTurn.Equals(afterCasting.CurrPlayerTurn);
                good = PlayerCards.Aggregate(good, (current, card) => current && afterCasting.PlayerCards.Contains(card));
                good = AllPlayerNames.Aggregate(good, (current, name) => current && afterCasting.AllPlayerNames.Contains(name));
                good = TableCards.Aggregate(good, (current, card) => current && afterCasting.TableCards.Contains(card));
                good = good && afterCasting.actionPlayerName.Equals(actionPlayerName);
                good = good && afterCasting.actionPlayerName.Equals(actionPlayerName);
                good = good && afterCasting.betAmount == this.betAmount;
                good = good && afterCasting.action.Equals(action);
                return good;
            }
            return false;
        }
    }
}
