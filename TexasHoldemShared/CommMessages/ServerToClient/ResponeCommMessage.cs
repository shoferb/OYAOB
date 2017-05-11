namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ResponeCommMessage : CommunicationMessage
    {
        public bool Success;
        public CommunicationMessage OriginalMsg; //TODO: maybe not needed

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
    }
}
