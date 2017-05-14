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
        } //TODO: make sure all fields are here

        public EditField FieldToEdit;
        public string NewValue;

        public EditCommMessage() : base(-1)
        {
        } //for parsing

        public EditCommMessage(int userId, EditField field, string value) : base(userId)
        {
            FieldToEdit = field;
            NewValue = value;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(EditCommMessage))
            {
                var afterCasting = (EditCommMessage) other;
                return FieldToEdit == afterCasting.FieldToEdit && NewValue == afterCasting.NewValue &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
