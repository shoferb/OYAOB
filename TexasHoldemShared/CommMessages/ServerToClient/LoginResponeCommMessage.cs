using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class LoginResponeCommMessage : ResponeCommMessage
    {
        //todo - this class is te message responce we get when login

        private string name;
        private string username;
        private string password;
        private string avatar;//- image path
        private int money;
        private string email;

        public LoginResponeCommMessage(int id, string _name, string _username, string _password,
            string _avatar, int _money, string _email, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            this.name = _name;
            this.username = _username;
            this.password = _password;
            this.avatar = _avatar;
            this.money = _money;
            this.email = _email;
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
