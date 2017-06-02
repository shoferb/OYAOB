using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Windows.Documents;
using TexasHoldem.Database.EntityFramework.Model;

namespace TexasHoldem.Database.EntityFramework.Controller
{
    public class UserController 
    {

        public void AddNewUser(UserTable objUser)
        {
            try
            {
                DataBaseSadnaEntitiesNewest db = new DataBaseSadnaEntitiesNewest();
                db.AddNewUser(objUser.userId, objUser.username, objUser.name, objUser.email, objUser.password,
                    objUser.avatar, objUser.points, objUser.money, objUser.gamesPlayed, objUser.leagueName,
                    objUser.winNum, objUser.HighestCashGainInGame, objUser.TotalProfit, objUser.inActive);
                db.UserTables.Add(objUser);
            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query entityFramework");
                Console.WriteLine(e);
                return;
            }


        }

        public void iii()
        {
            DataBaseSadnaEntitiesNewest db = new DataBaseSadnaEntitiesNewest();
            List<UserTable> y =  db.UserTables.ToList();
        }
    }
}
