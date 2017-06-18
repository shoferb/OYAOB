using System.Xml.Serialization;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages
{
    [XmlInclude(typeof(CommunicationMessage))]
    [XmlInclude(typeof(CreateNewGameResponse))]
    [XmlInclude(typeof(ActionCommMessage))]
    [XmlInclude(typeof(ChatCommMessage))]
    [XmlInclude(typeof(CreateNewRoomMessage))]
    [XmlInclude(typeof(EditCommMessage))]
    [XmlInclude(typeof(LoginCommMessage))]
    [XmlInclude(typeof(SearchCommMessage))]
    [XmlInclude(typeof(RegisterCommMessage))]
    [XmlInclude(typeof(ResponeCommMessage))]
    [XmlInclude(typeof(RegisterResponeCommMessage))]
    [XmlInclude(typeof(ChatResponceCommMessage))]
    [XmlInclude(typeof(GameDataCommMessage))]
    [XmlInclude(typeof(LoginResponeCommMessage))]
    [XmlInclude(typeof(SearchResponseCommMessage))]
    [XmlInclude(typeof(LeaderboardCommMessage))]
    [XmlInclude(typeof(LeaderboardResponseCommMessage))]
    [XmlInclude(typeof(UserStatisticsCommMessage))]
    [XmlInclude(typeof(UserStatisticsResponseCommMessage))]
    [XmlInclude(typeof(ReplayCommMessage))]
    [XmlInclude(typeof(ReturnToGameAsPlayerCommMsg))]
    [XmlInclude(typeof(ReturnToGameAsSpecCommMsg))]
    [XmlInclude(typeof(ReturnToGameResponseCommMsg))]
    [XmlInclude(typeof(ReturnToGameCommMsg))]

    public abstract class CommunicationMessage
    {
        public enum ActionType
        {
            CreateRoom,
            Fold,
            Bet,
            Join,
            Leave,
            Spectate,
            StartGame,
            HandCard,

            PlayerBrodcast,
            SpectetorBrodcast,
            PlayerWhisper,
            SpectetorWhisper,

            ReturnToGame

        }

        public int UserId;
        public long SessionId;

        protected CommunicationMessage(int id, long sid)
        {
            UserId = id;
            SessionId = sid;
        }

        public abstract ResponeCommMessage Handle(IEventHandler handler);

        public abstract bool Equals(CommunicationMessage other);
    }
}
