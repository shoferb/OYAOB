namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ResponeCommMessage : CommunicationMessage
    {
        public bool Success;
        public CommunicationMessage OriginalMsg;

        public ResponeCommMessage() : base(-1) { } //for parsing

        public ResponeCommMessage(int id) : base(id)
        {
            
        }

        public ResponeCommMessage(int id, bool success, CommunicationMessage originalMsg) : base(id)
        {
            Success = success;
            OriginalMsg = originalMsg;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(ResponeCommMessage))
            {
                var afterCasting = (ResponeCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
