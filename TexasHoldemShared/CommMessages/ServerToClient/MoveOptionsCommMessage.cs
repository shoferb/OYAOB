namespace TexasHoldemShared.CommMessages.ServerToClient
{
    //Sent from server to client in order to pass the user a list of his possible moves 
    public class MoveOptionsCommMessage : CommunicationMessage
    {
        public ActionType[] Options;

        public MoveOptionsCommMessage(int id, ActionType[] options) : base(id)
        {
            Options = options;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
