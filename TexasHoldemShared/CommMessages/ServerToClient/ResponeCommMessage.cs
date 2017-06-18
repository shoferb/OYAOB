namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ResponeCommMessage : CommunicationMessage
    {
        public bool Success;
        public CommunicationMessage OriginalMsg;
        public GameDataCommMessage GameData;

        public ResponeCommMessage() : base(-1, -1) { } //for parsing

        public ResponeCommMessage(int id) : base(id, -1)
        {
            
        }

        public void SetGameData(GameDataCommMessage gameData)
        {
            GameData = gameData;
        }

        public ResponeCommMessage(int id, long sid, bool success, CommunicationMessage originalMsg) : base(id, sid)
        {
            Success = success;
            OriginalMsg = originalMsg;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //another visitor
        public override void Notify(IResponseNotifier notifier, ResponeCommMessage msg)
        {
            notifier.Notify(OriginalMsg, this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ResponeCommMessage))
            {
                var afterCasting = (ResponeCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
