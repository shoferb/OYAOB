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
        public bool isSucceed;

        public GameDataCommMessage() : base(-1) { } //for parsing

        public GameDataCommMessage(int userId, int roomId, Card card1, Card card2,
            List<Card> tableCards, int chips, int pot, List<string> allPlayerNames, string dealerName,
            string bbName, string sbName, bool success ,string currPlayer, string actionPlayer, int bet, ActionType actionType) : base(userId)
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
            isSucceed = success;
            CurrPlayerTurn = currPlayer;
            actionPlayerName = actionPlayer;
            betAmount = bet;
            action = actionType;
        }
       
        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(GameDataCommMessage))
            {
                var afterCasting = (GameDataCommMessage)other;
                bool good = isSucceed == afterCasting.isSucceed && DealerName.Equals(afterCasting.DealerName) &&
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
