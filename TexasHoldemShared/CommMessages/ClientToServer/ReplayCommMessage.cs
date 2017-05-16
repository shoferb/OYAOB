using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReplayCommMessage : CommunicationMessage
    {
        public int roomID;


        public ReplayCommMessage() : base(-1) { } //for parsing

        public ReplayCommMessage(int _userid, int _roomId ) : base(_userid)
        {
            this.roomID = _roomId;
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() != this.GetType())
            {
                return false;
            }
            var afterCasting = (ReplayCommMessage)other;
            return roomID == afterCasting.roomID && UserId == afterCasting.UserId;
        }


        //TODO: ask Oded if needed Equal method? 


        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
