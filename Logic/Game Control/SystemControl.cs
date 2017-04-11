using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Game_Control
{
    class SystemControl
    {
        public struct DataLogin
        {
            public DataLogin(string username, string password)
            {
                UsernameData = username;
                PasswordData = password;
            }

            public string UsernameData { get; set; }
            public string PasswordData { get; set; }
        }
    }
}
