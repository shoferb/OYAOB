namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class JoinResponseCommMessage : ResponeCommMessage
    {
            public GameDataCommMessage GameData;

            public JoinResponseCommMessage(): base(-1)//for parsing
            {
            }

            public JoinResponseCommMessage(long sid, int id, bool success, CommunicationMessage originalMsg, 
                GameDataCommMessage gameData) : base(id, sid, success, originalMsg)
            {
                GameData = gameData;
            }

            //visitor pattern
            public override ResponeCommMessage Handle(IEventHandler handler)
            {
                return handler.HandleEvent(this);
            }

            public override bool Equals(CommunicationMessage other)
            {
                if (other != null && other.GetType() == typeof(JoinResponseCommMessage))
                {
                    var afterCasting = (JoinResponseCommMessage)other;
                    return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                           UserId == afterCasting.UserId && GameData.Equals(afterCasting.GameData);
                }
                return false;
            }
        }
    }


