namespace TexasHoldemShared.CommMessages
{
    //Sent from Client to Server and represents a player's action, as in Fold, Raise, Join a game, etc.
    class ActionCommMessage : CommunicationMessage
    {
        //TODO: consider spliting this class up

        public ActionType MoveType;
        public int Amount; //only filled when relevant
        public int RoomId;

        public ActionCommMessage(int id, ActionType moveType, int amount, int roomId) : base(id)
        {
            MoveType = moveType;
            Amount = amount;
            RoomId = roomId;
        }
    }
}
