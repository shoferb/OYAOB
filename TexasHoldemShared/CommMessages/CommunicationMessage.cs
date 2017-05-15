using System.Xml.Serialization;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages
{
    [XmlInclude(typeof(CommunicationMessage))]
    [XmlInclude(typeof(ChatCommMessage))]
    [XmlInclude(typeof(RegisterCommMessage))]
    [XmlInclude(typeof(ResponeCommMessage))]
    [XmlInclude(typeof(RegisterResponeCommMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]
    //[XmlInclude(typeof(CommunicationMessage))]

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

        public int UserId;

        protected CommunicationMessage(int id)
        {
            UserId = id;
        }

        public abstract void Handle(IEventHandler handler);

        public abstract bool Equals(CommunicationMessage other);
    }
}
