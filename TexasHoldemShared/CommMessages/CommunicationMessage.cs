namespace TexasHoldemShared.CommMessages
{
    public abstract class CommunicationMessage
    {
        public enum ActionType
        {
            Fold,
            Bet,

            Join,
            Leave,

            StartGame,
            HandCard,
        }

        //TODO: add fields here
        public int UserId;

        protected CommunicationMessage(int id)
        {
            UserId = id;
        }

        public abstract void Handle(IEventHandler handler);
    }
}
