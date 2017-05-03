namespace TexasHoldemShared.CommMessages.ServerToClient
{
    class ResponeCommMessage : CommunicationMessage
    {
        public bool Success;
        public CommunicationMessage OriginalMsg; //TODO: maybe not needed

        public ResponeCommMessage(int id, bool success, CommunicationMessage originalMsg) : base(id)
        {
            Success = success;
            OriginalMsg = originalMsg;
        }
    }
}
