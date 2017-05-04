using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.ServiceInterfaces
{
    interface IUserService
    {
        bool EditName(int userId, string newName);
        bool EditUserEmail(int userId, string newEmail);
        bool EditUserPassword(int userId, string newPassword);
        bool EditUserAvatar(int userId, string newAvatar);
    }
}
