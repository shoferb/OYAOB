namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public abstract class ReturnToGameCommMsg : CommunicationMessage
    {
        protected ReturnToGameCommMsg(int id, long sid) : base(id, sid)
        {
        }
    }
}
