using System.Collections.Generic;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class SearchResponseCommMessage : ResponeCommMessage
    {
        public List<ClientGame> Games;

        public SearchResponseCommMessage() : base(-1){ }//for parsing

        public SearchResponseCommMessage(List<ClientGame> _games, long sid, int id, bool success, 
            CommunicationMessage originalMsg) : base(id, sid, success, originalMsg)
        {
            Games = _games;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override void Notify(IResponseNotifier notifier, ResponeCommMessage msg)
        {
            notifier.Notify(OriginalMsg, this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(SearchResponseCommMessage))
            {
                var afterCasting = (SearchResponseCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && 
                       Games.TrueForAll(g => afterCasting.Games.Contains(g));
            }
            return false;
        }
    }
}
