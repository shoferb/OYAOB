namespace TexasHoldemShared.CommMessages
{
    public abstract class CommunicationMessage
    {
        public enum ActionType
        {
            Fold,
            Call,
            Raise,
            Check,

            Join,
            Leave,

            StartGame,
        }

        //TODO: add fields here
        public int UserId;

        protected CommunicationMessage(int id)
        {
            UserId = id;
        }
    }
}
