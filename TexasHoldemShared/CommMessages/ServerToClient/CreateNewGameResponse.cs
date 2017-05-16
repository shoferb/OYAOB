namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class CreateNewGameResponse : ResponeCommMessage
    {
        public GameDataCommMessage GameData;

        public CreateNewGameResponse() //for parsing
        {
        }

        public CreateNewGameResponse(int id, bool success, CommunicationMessage originalMsg,
            GameDataCommMessage gameData) : base(id, success, originalMsg)
        {
            GameData = gameData;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(CreateNewGameResponse))
            {
                var afterCasting = (CreateNewGameResponse)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && GameData.Equals(afterCasting.GameData);
            }
            return false;
        }
    }
}
