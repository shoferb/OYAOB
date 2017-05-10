using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class SearchCommMessage : CommunicationMessage
    {

        public enum SearchType
        {
            ByUserName,
            ByRoomId,
            AllActiveGames,
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

        public SearchCommMessage(int userId, SearchType _searchType, string _searchByString, int _searchByInt,GameMode _searchByGameMode) : base(userId)
        {
            this.searchByGameMode = _searchByGameMode;
            this.searchByInt = _searchByInt;
            this.searchByString = _searchByString;
            this.searchType = _searchType;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
