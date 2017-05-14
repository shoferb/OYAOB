
namespace TexasHoldemShared.CommMessages.ClientToServer
{
    //Sent from client to server and represents a user's wish to register to the system
    public class RegisterCommMessage : CommunicationMessage
    {
        public string Name;
        public string MemberName;
        public string Password;
        public int Money;
        public string Email;

        public RegisterCommMessage() : base(-1) { } //for parsing

        public RegisterCommMessage(int id, string name, string memberName, string password, int money, string email) : base(id)
        {
            Name = name;
            MemberName = memberName;
            Password = password;
            Money = money;
            Email = email;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public bool Equals(RegisterCommMessage other)
        {
            return Name.Equals(other.Name) && MemberName.Equals(other.MemberName) && Password.Equals(other.Password) &&
                   Money == other.Money && Email.Equals(other.Email);
        }
    }
}
