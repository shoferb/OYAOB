using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class ClientUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }//- image path
        public int money { get; set; }
        public string email { get; set; }
        public string leauge { get; }

        public ClientUser(int _id, string _name, string _username, string _password,
            string _avatar, int _money, string _email,string _leauge)
        {
            this.id = _id;
            this.name = _name;
            this.username = _username;
            this.password = _password;
            this.avatar = _avatar;
            this.money = _money;
            this.email = _email;
            this.leauge = _leauge;
        }

    }
}
