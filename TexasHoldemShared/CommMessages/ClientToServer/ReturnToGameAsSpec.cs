using System;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReturnToGameAsSpec : CommunicationMessage
    {
        public int RoomId;

        public ReturnToGameAsSpec(int id, long sid, int roomId) : base(id, sid)
        {
            RoomId = roomId;
        }

        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            throw new NotImplementedException();
        }
    }
}
