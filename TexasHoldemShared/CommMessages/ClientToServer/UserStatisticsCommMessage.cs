using System;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class UserStatisticsCommMessage : CommunicationMessage
    {
        public UserStatisticsCommMessage() : base(-1, -1) { }

        public UserStatisticsCommMessage(int id, long sid) : base(id, sid)
        {
        }

        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            return UserId == other.UserId && other.GetType() == typeof(UserStatisticsCommMessage);
        }
    }
}
