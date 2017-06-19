using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //Sent from Client to Server and represents a player's action, as in Fold, Raise, Join a game, etc.
    public class ActionCommMessage : CommunicationMessage
    {
        public ActionType MoveType;
        public int Amount; //only filled when relevant
        public int RoomId;

        public ActionCommMessage() : base(-1, -1) { } //for parsing

        public ActionCommMessage(int id, long sid, ActionType moveType, int amount, int roomId) : base(id, sid)
        {
            MoveType = moveType;
            Amount = amount;
            RoomId = roomId;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        //{
        //    notifier.Notify(response.OriginalMsg, response);
        //}

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
