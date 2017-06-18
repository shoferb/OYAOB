namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class RegisterResponeCommMessage : ResponeCommMessage
    {

        public string Name;
        public string Username;
        public string Password;
        public string Avatar;//- image path
        public int Money;
        public string Email;
        public string Leauge;

        public RegisterResponeCommMessage() : base(-1) { } //for parsing

        public RegisterResponeCommMessage(long sid, int id, string _name, string _username, string 
            _password, string _avatar, int _money, string _email, string _leauge, bool success, 
            CommunicationMessage originalMsg) : base(id, sid, success, originalMsg)
        {
            Name = _name;
            Username = _username;
            Password = _password;
            Avatar = _avatar;
            Money = _money;
            Email = _email;
            Leauge = _leauge;
        }

        //visitor pattern
        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override void Notify(IResponseNotifier notifier)
        {
            notifier.Notify(OriginalMsg, this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(RegisterResponeCommMessage))
            {
                var afterCasting = (RegisterResponeCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && Name.Equals(afterCasting.Name) &&
                       Username.Equals(afterCasting.Username) && Password.Equals(afterCasting.Password) &&
                       Avatar.Equals(afterCasting.Avatar) && Email.Equals(afterCasting.Email) &&
                       Leauge.Equals(afterCasting.Leauge) && Money == afterCasting.Money;
            }
            return false;
        }
    }
}
