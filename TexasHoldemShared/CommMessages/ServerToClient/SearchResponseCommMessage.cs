using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class SearchResponseCommMessage : ResponeCommMessage
    {
        public List<ClientGame> games;

        public SearchResponseCommMessage() : base(-1){ }//for parsing

        public SearchResponseCommMessage(List<ClientGame> _games, int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.games = _games;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(SearchResponseCommMessage))
            {
                var afterCasting = (SearchResponseCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && 
                       games.TrueForAll(g => afterCasting.games.Contains(g));
            }
            return false;
        }
    }
}
