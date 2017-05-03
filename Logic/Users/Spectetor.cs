using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Users
{
    public class Spectetor 
    {
        public int roomId { get; }
        public IUser user;

        public Spectetor(IUser User, int RoomId)
        {
            this.user = User;
            this.roomId = RoomId;
        }

        public Spectetor(int id, string name, string memberName, string password, int points, int money,String email,int roomId) :
            base(id, name, memberName, password, points, money, email)
        {
            
            this.roomId = roomId;
            
        }

        //getter setter
        public int RoomId
        {
            get
            {
                return roomId;
            }

           
        }

        
    }
}
