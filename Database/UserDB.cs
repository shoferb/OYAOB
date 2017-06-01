using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TexasHoldem.Database.DatabaseObject;
using TexasHoldem.Logic.GameControl;

namespace TexasHoldem.Database
{
    public class UserDB
    {
        public List<userDatabaseOb> GetAllUser()
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "GetAllUser";
                   List<userDatabaseOb> toReturn = db.Query<userDatabaseOb>(readSp,
                        commandType: CommandType.StoredProcedure).ToList();
                    db.Close();
                    return toReturn;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error get all user");
                return null;
            }

        }

        public userDatabaseOb GetUserById()
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "GetUserByUserId";
                    userDatabaseOb toReturn = db.Query<userDatabaseOb>(readSp,
                        commandType: CommandType.StoredProcedure).ToList().First();
                    db.Close();
                    return toReturn;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error get user by user Id");
                return null;
            }

        }

        public userDatabaseOb GetUserByUserName()
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "GetUserByUserName";
                    userDatabaseOb toReturn =  db.Query<userDatabaseOb>(readSp,
                        commandType: CommandType.StoredProcedure).ToList().First();
                    db.Close();
                    return toReturn;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error get user by user name");
                return null;
            }
        
        }

        public void AddNewUser(userDatabaseOb objUser)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "AddNewUser";
                    db.Execute(readSp, objUser, commandType: CommandType.StoredProcedure);
                    db.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error add new User query");
                return;
            }
           

        }

        public void EditUserId(int oldId, int newId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserId";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newId);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user Id");
                return;
            }
        }

        public void EditUserName(int oldId, string newUserName)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUsername";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newUserName);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user name (memberName) ");
                return;
            }
        }

        public void EditName(int oldId, string newName)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditName";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newName);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user name");
                return;
            }
        }

        public void EditEmail(int oldId, string newEmail)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditEmail";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newEmail);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user email");
                return;
            }
        }

        public void EditPassword(int oldId, string newPassword)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditPassword";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newPassword);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user password");
                return;
            }
        }

        public void EditUserHighestCashGainInGame(int oldId, string newHighestCashGainInGame)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserHighestCashGainInGame";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newHighestCashGainInGame);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user HighestCashGainInGame");
                return;
            }
        }

        //todo in proxy convert bool to bit 1= true 0 = false
        public void EditUserIsActive(int oldId, int newIsActive)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserIsActive";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newIsActive);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user is active");
                return;
            }
        }
        //todo in proxy convert league name to int 
        public void EditUserLeagueName(int oldId, int newLeagueName)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserIsActive";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newLeagueName);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user league name");
                return;
            }
        }


        public void EditUserMoney(int oldId, int newMoney)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserMoney";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newMoney);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user Money");
                return;
            }
        }

        public void EditUserNumOfGamesPlayed(int oldId, int newEditUserNumOfGamesPlayed)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserNumOfGamesPlayed";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newEditUserNumOfGamesPlayed);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user newEditUserNumOfGamesPlayed");
                return;
            }
        }


        public void EditUserTotalProfit(int oldId, int newTotalProfit)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserTotalProfit";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newTotalProfit);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user newTotalProfit");
                return;
            }
        }

        public void EditUserWinNum(int oldId, int newWinNum)
        {
            try
            {
                using (IDbConnection db = new SqlConnection
                (ConfigurationManager.ConnectionStrings
                    ["DataBaseSadna"].ConnectionString))
                {
                    db.Open();
                    string readSp = "EditUserWinNum";
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(oldId);
                    dbParams.AddDynamicParams(newWinNum);
                    db.Execute(readSp, dbParams, commandType: CommandType.StoredProcedure);
                    db.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user newWinNum");
                return;
            }
        }
    }
}
