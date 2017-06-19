using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //sent from client to server and represents a user's wish to change the value of a field, as in User Name, Password, etc.
    public class EditCommMessage : CommunicationMessage
    {
        public enum EditField
        {
            UserName,
            Password,
            Email,
            Avatar,
            Id,
            Name,
            Money
        } 

        public EditField FieldToEdit;
        public string NewValue;

        public EditCommMessage() : base(-1, -1)
        {
        } //for parsing

        public EditCommMessage(int userId, long sid, EditField field, string value) : base(userId, sid)
        {
            FieldToEdit = field;
            NewValue = value;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        //{
        //    notifier.Notify(response.OriginalMsg, response);
        //}

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(EditCommMessage))
            {
                var afterCasting = (EditCommMessage) other;
                return FieldToEdit == afterCasting.FieldToEdit && NewValue == afterCasting.NewValue &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
