using System.Xml.Serialization;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages
{
    [XmlInclude(typeof(CommunicationMessage))]
    [XmlInclude(typeof(ActionCommMessage))]
    [XmlInclude(typeof(ChatCommMessage))]
    [XmlInclude(typeof(CreatrNewRoomMessage))]
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

    public abstract class CommunicationMessage
    {
        public enum ActionType
        {
            Fold,
            Bet,
            //check = Bet with amount 0
            //Call =
            //Raise =?
            Join,
            Leave,
            Spectate,
            StartGame,
            HandCard,
            PlayerBrodcast,
            SpectetorBrodcast,
            PlayerWhisper,
            SpectetorWhisper

        }

        //TODO: add fields here
        public int UserId;

        protected CommunicationMessage(int id)
        {
            UserId = id;
        }

        public abstract void Handle(IEventHandler handler);

        public abstract bool Equals(CommunicationMessage other);
    }
}
