using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.DatabaseProxy
{
    public class SpecXML
    {
        public int roomId { get; set; }
        public int userId;

        public SpecXML(Logic.Users.Spectetor s)
        {
            this.roomId = s.roomId;
            this.userId = s.user.Id();
        }
    }
}
