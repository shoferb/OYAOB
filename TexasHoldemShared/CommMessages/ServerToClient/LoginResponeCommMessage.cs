namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class LoginResponeCommMessage : ResponeCommMessage
    {
        

        public string name;
        public string username;
        public string password;
        public string avatar;//- image path
        public int money;
        public string email;
        public string leauge;

        public LoginResponeCommMessage() : base(-1) { } //for parsing

        public LoginResponeCommMessage(int id, string _name, string _username, string _password,
            string _avatar, int _money, string _email,string _leauge, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.name = _name;
            this.username = _username;
            this.password = _password;
            this.avatar = _avatar;
            this.money = _money;
            this.email = _email;
            this.leauge = _leauge;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(LoginResponeCommMessage))
            {
                var afterCasting = (LoginResponeCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && name.Equals(afterCasting.name) &&
                       username.Equals(afterCasting.username) && password.Equals(afterCasting.password) &&
                       avatar.Equals(afterCasting.avatar) && email.Equals(afterCasting.email) &&
                       leauge.Equals(afterCasting.leauge) && money == afterCasting.money;
            }
            return false;
        }
    }
}
