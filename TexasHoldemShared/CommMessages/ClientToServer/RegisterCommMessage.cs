
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

        public RegisterCommMessage() : base(-1, -1) { } //for parsing

        public RegisterCommMessage(int id, string name, string memberName, string password, int money, string email) : base(id, -1)
        {
            Name = name;
            MemberName = memberName;
            Password = password;
            Money = money;
            Email = email;
        }

        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(RegisterCommMessage))
            {
                var afterCasting = (RegisterCommMessage)other;
                return Money == afterCasting.Money && Name.Equals(afterCasting.Name) &&
                       MemberName.Equals(afterCasting.MemberName) &&
                       UserId == afterCasting.UserId && Password.Equals(afterCasting.Password);
            }
            return false;
        }
    }
}
