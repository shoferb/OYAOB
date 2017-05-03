using System.Collections.Generic;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    //This class is used to pass game data / table updates to the client
    public class GameDataCommMessage : CommunicationMessage
    {
        //data types:
        public enum Suits //TODO: duplicated
        {
            Hearts,
            Diamonds,
            Spades,
            Clubs,
            None
        }

        public class GameDataCard
        {
            public int Value;
            public Suits Suit;

            public GameDataCard(int value, Suits s) //TODO: sort of dup
            {
                Value = value;
                Suit = s;
            }
        }

        //fields:
        public int RoomId;

        public GameDataCard[] PlayerCards = new GameDataCard[2];
        public List<GameDataCard> TableCards;

        public int TotalChips;
        public int PotSize;

        public string[] AllPlayerNames;
        public string DealerName;
        public string BbName;
        public string SbName;

        public GameDataCommMessage(int userId, int roomId, GameDataCard card1, GameDataCard card2,
            List<GameDataCard> tableCards, int chips, int pot, string[] allPlayerNames, string dealerName,
            string bbName, string sbName) : base(userId)
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
        }
    }
}
