using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Users
{
    public class Player : Spectetor
    {
        private bool isActive;

        public Player(int id, string name, string memberName, int password, int points, int money,String email, int gameId, bool isActive) : base(id, name, memberName, password, points, money,email,gameId)
        {
            this.isActive = isActive;
            //sads dsajfh
        }

        //getter setter
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }
    }
}
