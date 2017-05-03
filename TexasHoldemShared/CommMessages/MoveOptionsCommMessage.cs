namespace TexasHoldemShared.CommMessages
{
    //Sent from server to client in order to pass the user a list of his possible moves 
    class MoveOptionsCommMessage : CommunicationMessage
    {
        public ActionType[] Options;

        public MoveOptionsCommMessage(int id, ActionType[] options) : base(id)
        {
            Options = options;
        }
    }
}
