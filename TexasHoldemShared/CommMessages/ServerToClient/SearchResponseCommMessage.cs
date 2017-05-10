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

        public SearchResponseCommMessage(List<ClientGame> _games, int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.games = _games;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
