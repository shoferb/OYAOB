using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.GameControl
{
    public class UserControl
    {

        private List<IUser> users;
        private static UserControl instance = null;
        private static readonly object padlock = new object();

        //getter for Singelton
        public static UserControl SystemControlInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UserControl();
                    }
                    return instance;
                }
            }
        }

        //constractour for singelton
        private UserControl()
        {
            this.users = new List<IUser>();
        }

        
    }
}
