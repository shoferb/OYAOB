using System.Collections.Generic;
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

        public Card[] PlayerCards = new Card[2];
        public List<Card> TableCards;

        public int TotalChips;
        public int PotSize;

        public List<string> AllPlayerNames;
        public string DealerName;
        public string BbName;
        public string SbName;

        public bool isSucceed;

        public GameDataCommMessage() : base(-1) { } //for parsing

        public GameDataCommMessage(int userId, int roomId, Card card1, Card card2,
            List<Card> tableCards, int chips, int pot, List<string> allPlayerNames, string dealerName,
            string bbName, string sbName, bool success) : base(userId)
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
            this.isSucceed = success;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
