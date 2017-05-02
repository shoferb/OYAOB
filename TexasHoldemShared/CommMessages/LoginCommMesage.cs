namespace TexasHoldemShared.CommMessages
{
    class LoginCommMesage : CommunicationMessage
    {
        public bool IsLogin; //true = login, false = logout
        public string UserName;
        public string Password;

        //TODO: add more fields

        public LoginCommMesage(int userId, bool isLogin, string name, string passWord) : base(userId)
        {
            IsLogin = isLogin;
            UserName = name;
            Password = passWord;
        }
    }
}
