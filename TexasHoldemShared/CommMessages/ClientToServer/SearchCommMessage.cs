using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
     public class SearchCommMessage : CommunicationMessage
    {
    
        public enum SearchField
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


         public SearchCommMessage(int id) : base(id)
         {
         }

         public override void Handle(IEventHandler handler)
         {
             throw new NotImplementedException();
         }
     }
}
