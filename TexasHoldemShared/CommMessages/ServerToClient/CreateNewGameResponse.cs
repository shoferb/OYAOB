namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class CreateNewGameResponse : ResponeCommMessage
    {
        public GameDataCommMessage GameData;

        public CreateNewGameResponse() : base(-1) //for parsing
        {
        }

        public CreateNewGameResponse(int id, long sid, bool success, CommunicationMessage originalMsg, 
            GameDataCommMessage gameData) : base(id, sid, success, originalMsg)
        {
            GameData = gameData;
        }

        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
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
