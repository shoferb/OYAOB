using System;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class UserStatisticsCommMessage : CommunicationMessage
    {
        public UserStatisticsCommMessage() : base(-1, -1) { }

        public UserStatisticsCommMessage(int id, long sid) : base(id, sid)
        {
        }

        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        //{
        //    notifier.Notify(response.OriginalMsg, response);
        //}

        public override bool Equals(CommunicationMessage other)
        {
            return UserId == other.UserId && other.GetType() == typeof(UserStatisticsCommMessage);
        }
    }
}
