using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;

using System.Windows.Documents;
using TexasHoldem.Database.EntityFramework.Model;
using TexasHoldem.Database.NewEntity;
using UserTable = TexasHoldem.Database.EntityFramework.Model.UserTable;

namespace TexasHoldem.Database.EntityFramework.Controller
{
    public class UserController 
    {

        public void AddNewUser(NewEntity.UserTable objUser)
        {
            try
            {
                using (var context = new DataBaseSadnaEntities())
                {
                    
                        var ob = new NewEntity.UserTable
                    {
                        userId  = objUser.userId,
                    username =   objUser.username,name =  objUser.name,email =  objUser.email,
                        password =  objUser.password,
                        avatar = objUser.avatar, points = objUser.points,money = objUser.money,
                        gamesPlayed = objUser.gamesPlayed,leagueName = objUser.leagueName,
                        winNum =  objUser.winNum, HighestCashGainInGame = objUser.HighestCashGainInGame,
                        TotalProfit = objUser.TotalProfit, inActive = objUser.inActive
                    };
                    context.UserTables.Add(ob);
                    context.SaveChanges();
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query entityFramework");
                Console.WriteLine(e);
                return;
            }


        }
        
        public void iii(UserTable objUser)
        {
            using (IDbConnection db = new SqlConnection
            (ConfigurationManager.ConnectionStrings
                ["DataBaseSadnaEntitiesNewest"].ConnectionString))
            {
                db.Open();
                string readSp = "AddNewUser";
                db.Execute(readSp, objUser, commandType: CommandType.StoredProcedure);
                Console.WriteLine("error after adding");
                db.Close();
            }
            DataBaseSadnaEntitiesNewest dbs = new DataBaseSadnaEntitiesNewest();
            List<UserTable> y =  dbs.UserTables.ToList();
        }
    }
}
