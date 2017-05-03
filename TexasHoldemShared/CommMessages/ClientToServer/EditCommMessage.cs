namespace TexasHoldemShared.CommMessages
{
    //sent from client to server and represents a user's wish to change the value of a field, as in User Name, Password, etc.
    class EditCommMessage : CommunicationMessage
    {
        public enum EditField {UserName, Password, Email, Avatar} //TODO: make sure all fields are here

        public EditField FieldToEdit;
        public string NewValue;

        public EditCommMessage(int userId, EditField field, string value) : base(userId)
        {
            FieldToEdit = field;
            NewValue = value;
        }
    }
}
