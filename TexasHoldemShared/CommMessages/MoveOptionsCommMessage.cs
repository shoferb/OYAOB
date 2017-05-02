namespace TexasHoldemShared.CommMessages
{
    class MoveOptionsCommMessage : CommunicationMessage
    {
        public ActionType[] Options;

        public MoveOptionsCommMessage(ActionType[] options)
        {
            this.Options = options;
        }
    }
}
