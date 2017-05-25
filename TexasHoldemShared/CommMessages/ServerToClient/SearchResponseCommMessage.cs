using System.Collections.Generic;
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
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(SearchResponseCommMessage))
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
