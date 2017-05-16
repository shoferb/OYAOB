using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReplayCommMessage : CommunicationMessage
    {
        public int roomId;
       


        public ReplayCommMessage() : base(-1) { } //for parsing

        public ReplayCommMessage(int _userid, int _roomId ) : base(_userid)
        {
            this.roomId = _roomId;
           
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ReplayCommMessage))
            {
                var afterCasting = (ReplayCommMessage)other;
                return roomId == afterCasting.roomId &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }

        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
