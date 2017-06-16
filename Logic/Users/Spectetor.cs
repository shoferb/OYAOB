using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Users
{
    public class Spectetor 
    {
        public int roomId { get; set; }
        public IUser user;

        public Spectetor(IUser User, int RoomId)
        {
            this.user = User;
            this.roomId = RoomId;
        }

        public Spectetor(int rId)
        {
            this.roomId = rId;
        }

    }
}
