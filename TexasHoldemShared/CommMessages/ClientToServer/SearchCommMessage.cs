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
            ByStartingChip
        }


        public SearchCommMessage(int userId, SearchType searchType, string searchByString, int searchByInt) : base(userId)
        {
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
