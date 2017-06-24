using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //sent from client to server and represents a user's wish to login / logout
    public class LoginCommMessage : CommunicationMessage
    {
        public bool IsLogin; //true = login, false = logout
        public string UserName;
        public string Password;

        public LoginCommMessage() : base(-1, -1)
        {
        } //for parsing

        public LoginCommMessage(int userId, bool isLogin, string name, string passWord) : base(userId, -1)
        {
            IsLogin = isLogin;
            UserName = name;
            Password = passWord;
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
            if (other != null && other.GetType() == typeof(LoginCommMessage))
            {
                var afterCasting = (LoginCommMessage) other;
                return IsLogin == afterCasting.IsLogin && UserName.Equals(afterCasting.UserName) &&
                       UserId == afterCasting.UserId && Password.Equals(afterCasting.Password);
            }
            return false;
        }
    }
}
