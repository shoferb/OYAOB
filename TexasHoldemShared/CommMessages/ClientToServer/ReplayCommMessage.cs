using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReplayCommMessage : CommunicationMessage
    {
        public int RoomId;
       
        public ReplayCommMessage() : base(-1, -1) { } //for parsing

        public ReplayCommMessage(int _userid, long sid, int _roomId) : base(_userid, sid)
        {
            RoomId = _roomId;
           
        }

        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ReplayCommMessage))
            {
                var afterCasting = (ReplayCommMessage)other;
                return RoomId == afterCasting.RoomId &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
