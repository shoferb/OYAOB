namespace TexasHoldemShared.CommMessages
{
    class ActionCommMessage : CommunicationMessage
    {
        //TODO: consider spliting this class up

        public ActionType MoveType;
        public int Amount; //only filled when relevant

        public ActionCommMessage(int id, ActionType moveType, int amount) : base(id)
        {
            MoveType = moveType;
            Amount = amount;
        }
    }
}
