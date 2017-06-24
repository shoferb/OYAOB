namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public abstract class ReturnToGameCommMsg : CommunicationMessage
    {
        public int RoomId;

        protected ReturnToGameCommMsg(int id, long sid) : base(id, sid)
        {
        }
    }
}
