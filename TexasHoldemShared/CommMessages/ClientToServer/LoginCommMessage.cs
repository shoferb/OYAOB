namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //sent from client to server and represents a user's wish to login / logout
    public class LoginCommMessage : CommunicationMessage
    {
        public bool IsLogin; //true = login, false = logout
        public string UserName;
        public string Password;

        //TODO: add more fields

        public LoginCommMessage() : base(-1) { } //for parsing

        public LoginCommMessage(int userId, bool isLogin, string name, string passWord) : base(userId)
        {
            IsLogin = isLogin;
            UserName = name;
            Password = passWord;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
