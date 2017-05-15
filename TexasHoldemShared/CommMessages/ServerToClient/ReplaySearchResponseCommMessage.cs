using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ReplaySearchResponseCommMessage : ResponeCommMessage
    {
        public List<string> replaysAsked;

        public ReplaySearchResponseCommMessage() : base(-1) { } //for parsing

        public ReplaySearchResponseCommMessage(List<string> replayesToRet, int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.replaysAsked = replayesToRet;

        }
        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

    }

    //TODO: Aske oded if needed equal methos;
}
