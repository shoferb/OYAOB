using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic.Users
{
    public class Spectetor : User
    {
        private int gameId;

        public Spectetor(int id, string name, string memberName, int password, int points, int money,String email,int gameId) : base(id, name, memberName, password, points, money,email)
        {
            
            this.gameId = gameId;
            //new lskd
        }

        //getter setter
        public int GameId
        {
            get
            {
                return gameId;
            }

            set
            {
                gameId = value;
            }
        }
    }
}
