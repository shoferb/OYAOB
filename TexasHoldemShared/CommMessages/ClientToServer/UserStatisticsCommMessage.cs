using System;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class UserStatisticsCommMessage : CommunicationMessage
    {
        public string UserName;

        public UserStatisticsCommMessage(int id, string name) : base(id)
        {
            UserName = name;
        }

        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            throw new NotImplementedException();
        }
    }
}
