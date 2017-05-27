namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class LoginResponeCommMessage : ResponeCommMessage
    {
        

        public string Name;
        public string Username;
        public string Password;
        public string Avatar;//- image path
        public int Money;
        public string Email;
        public string Leauge;

        public LoginResponeCommMessage() : base(-1) { } //for parsing

        public LoginResponeCommMessage(int id, long sid, string _name, string _username, 
            string _password, string _avatar, int _money, string _email, string _leauge,
            bool success, CommunicationMessage originalMsg) : base(id, sid, success, originalMsg)
        {
            this.Name = _name;
            this.Username = _username;
            this.Password = _password;
            this.Avatar = _avatar;
            this.Money = _money;
            this.Email = _email;
            this.Leauge = _leauge;
        }

        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(LoginResponeCommMessage))
            {
                var afterCasting = (LoginResponeCommMessage)other;
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
