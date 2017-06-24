namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ReplaySearchResponseCommMessage : ResponeCommMessage
    {
        public int RoomId;
        public string Replay;

        public ReplaySearchResponseCommMessage() : base(-1) { } //for parsing

        public ReplaySearchResponseCommMessage(int roomId, int id, long sid, bool success, CommunicationMessage originalMsg)
            : base(id, sid, success, originalMsg)
        {
            RoomId = roomId;

        }
        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage msg)
        //{
        //    notifier.Notify(OriginalMsg, this);
        //}

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ReplaySearchResponseCommMessage))
            {
                var afterCasting = (ReplaySearchResponseCommMessage)other;
                bool same = afterCasting.RoomId == RoomId && afterCasting.UserId == UserId;
                return same;
            }
            return false;
        }

    }
}
