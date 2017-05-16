namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //Sent from Client to Server and represents a player's action, as in Fold, Raise, Join a game, etc.
    public class ActionCommMessage : CommunicationMessage
    {
        public ActionType MoveType;
        public int Amount; //only filled when relevant
        public int RoomId;

        public ActionCommMessage() : base(-1) { } //for parsing

        public ActionCommMessage(int id, ActionType moveType, int amount, int roomId) : base(id)
        {
            MoveType = moveType;
            Amount = amount;
            RoomId = roomId;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

       public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ActionCommMessage))
            {
                var afterCasting = (ActionCommMessage) other;
                return Amount == afterCasting.Amount && RoomId == afterCasting.RoomId && UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
