using TexasHoldemShared.CommMessages.ServerToClient;

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
        public string SearchByString;
        public int SearchByInt;
        public GameMode SearchByGameMode;

        public SearchCommMessage() : base(-1, -1) { } //for parsing

        public SearchCommMessage(int userId, long sid, SearchType _searchType, string _searchByString,
            int _searchByInt, GameMode _searchByGameMode) : base(userId, sid)
        {
            SearchByGameMode = _searchByGameMode;
            SearchByInt = _searchByInt;
            SearchByString = _searchByString;
            searchType = _searchType;
            SessionId = sid;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        //{
        //    notifier.Notify(response.OriginalMsg, response);
        //}

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(SearchCommMessage))
            {
                var afterCasting = (SearchCommMessage)other;
                return searchType == afterCasting.searchType && SearchByString.Equals(afterCasting.SearchByString) &&
                       UserId == afterCasting.UserId && SearchByInt == afterCasting.SearchByInt;
            }
            return false;
        }
    }
}
