namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class SearchCommMessage : CommunicationMessage
    {

        public enum SearchType
        {
            ActiveGamesByUserName,
            SpectetorGameByUserName,
            ByRoomId,
            AllSepctetorGame,
            GamesUserCanJoin,
            ByPotSize,
            ByGameMode,
            ByBuyInPolicy,
            ByMinPlayer,
            ByMaxPlayer,
            ByStartingChip,
            ByMinBet
        }

        
        public SearchType searchType;
        public string searchByString;
        public int searchByInt;
        public GameMode searchByGameMode;

        public SearchCommMessage() : base(-1) { } //for parsing

        public SearchCommMessage(int userId, SearchType _searchType, string _searchByString, int _searchByInt,GameMode _searchByGameMode) : base(userId)
        {
            this.searchByGameMode = _searchByGameMode;
            this.searchByInt = _searchByInt;
            this.searchByString = _searchByString;
            this.searchType = _searchType;
        }

        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(SearchCommMessage))
            {
                var afterCasting = (SearchCommMessage)other;
                return searchType == afterCasting.searchType && searchByString.Equals(afterCasting.searchByString) &&
                       UserId == afterCasting.UserId && searchByInt == afterCasting.searchByInt;
            }
            return false;
        }
    }
}
