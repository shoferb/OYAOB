namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ReplaySearchResponseCommMessage : ResponeCommMessage
    {
        public int roomID;
        public string replay;

        public ReplaySearchResponseCommMessage() : base(-1) { } //for parsing

        public ReplaySearchResponseCommMessage(int roomID, int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.roomID = roomID;

        }
        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ReplaySearchResponseCommMessage))
            {
                var afterCasting = (ReplaySearchResponseCommMessage)other;
                bool same = afterCasting.roomID == roomID && afterCasting.UserId == UserId;
                return same;
            }
            return false;
        }

    }
}
