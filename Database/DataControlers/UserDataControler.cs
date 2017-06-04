using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.Security;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;

namespace TexasHoldem.Database.DataControlers
{
    public class UserDataControler
    {

        public List<UserTable> GetAllUser()
        {
            List<UserTable> toReturn = new List<UserTable>();
            try
            {

                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    var temp = db.GetAllUser().ToList();
                    foreach (var v in temp)
                    {
                        UserTable toAdd = convertToUser(v);
                        toReturn.Add(toAdd);
                    }
                    return toReturn;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error get all user");
                return null;
            }

        }

       

        public UserTable GetUserById(int id)
       {
           UserTable toReturn = null;
           try
           {

               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   var temp = db.GetUserByUserId(id).ToList().First();
                   toReturn = convertToUser(temp);
                    //Console.WriteLine("in data user ,user id: "+toReturn.userId);
                   //Console.WriteLine("in data user ,user password id: " + toReturn.password);
                   //Console.WriteLine("in data user ,%%%%%% Try dec password");
                   //string toDec = toReturn.password;
                  // string decryptpassword = PasswordSecurity.Decrypt(toDec, "securityPassword");

                   //Console.WriteLine("in data user ,%%%%%% AFTER &&&&&& dec password");
                   //toReturn.password = decryptpassword;
                    return toReturn;
               }
           }
           catch (Exception e)
           {
               Console.WriteLine("error get user by Id");
               return null;
           }

        }
        
       public UserTable GetUserByUserName(string username)
       {
           UserTable toReturn = null;
           try
           {

               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   var temp = db.GetUserByUserName(username).ToList().First();
                   toReturn = convertToUser(temp);
                   Console.WriteLine(toReturn.userId);
                   return toReturn;
               }
           }
           catch (Exception e)
           {
               Console.WriteLine("error get user by name");
               return null;
           }
        }
        
       public void AddNewUser(UserTable toAddUser)
       {
           UserTable toReturn = null;
           try
           {

               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                    db.AddNewUser(toAddUser.userId, toAddUser.username, toAddUser.name, toAddUser.email, toAddUser.password, toAddUser.avatar, toAddUser.points, toAddUser.money, toAddUser.gamesPlayed, toAddUser.leagueName, toAddUser.winNum, toAddUser.HighestCashGainInGame, toAddUser.TotalProfit, toAddUser.inActive);
                    //db.UserTables.InsertOnSubmit(toAddUser);
                //    db.SubmitChanges();
                }
           }
           catch (Exception e)
           {
               Console.WriteLine("error Add new User exeaption: "+e);
               return;
           }
        }

        public void DeleteUserById(int userId)
        {
            
            try
            {

                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    // db.AddNewUser(toAddUser.userId, toAddUser.username, toAddUser.name, toAddUser.email, toAddUser.password, toAddUser.avatar, toAddUser.points, toAddUser.money, toAddUser.gamesPlayed, toAddUser.leagueName, toAddUser.winNum, toAddUser.HighestCashGainInGame, toAddUser.TotalProfit, toAddUser.inActive);
                    db.DeleteUserById(userId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error Delete user By Id");
                return;
            }
        }

        public void DeleteUserByUsername(string username)
        {

            try
            {

                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    // db.AddNewUser(toAddUser.userId, toAddUser.username, toAddUser.name, toAddUser.email, toAddUser.password, toAddUser.avatar, toAddUser.points, toAddUser.money, toAddUser.gamesPlayed, toAddUser.leagueName, toAddUser.winNum, toAddUser.HighestCashGainInGame, toAddUser.TotalProfit, toAddUser.inActive);
                    db.DeleteUserByUserName(username);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error Delete user By Id");
                return;
            }
        }
        public void EditUserId(int oldId, int newId)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserId(newId, oldId);
               }
               
            }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user Id");
               return;
           }
       }
        
       public void EditUserName(int Id, string newUserName)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUsername(Id,newUserName);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  username");
               return;
           }
        }
        
       public void EditName(int Id, string newName)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditName(Id,newName);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  name");
               return;
           }
        }

       public void EditEmail(int Id, string newEmail)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditEmail(Id,newEmail);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user email");
               return;
           }
        }

       public void EditPassword(int Id, string newPassword)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditPassword(Id, newPassword);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user passsword");
               return;
           }
        }

       public void EditUserHighestCashGainInGame(int Id, int newHighestCashGainInGame)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserHighestCashGainInGame(Id,newHighestCashGainInGame);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user Highest Cash Gain In Game");
               return;
           }
        }

       
       public void EditUserIsActive(int Id, bool newIsActive)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserIsActive(Id,newIsActive);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user active mode");
               return;
           }
        }
      
       public void EditUserLeagueName(int Id, LinqToSql.LeagueName newLeagueName)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserLeagueName(Id,newLeagueName.League_Value);
               }
           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user League name");
               return;
           }
        }


       public void EditUserMoney(int Id, int newMoney)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserMoney(Id,newMoney);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user money");
               return;
           }
        }

       

        public void EditUserNumOfGamesPlayed(int Id, int newEditUserNumOfGamesPlayed)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserNumOfGamesPlayed(Id, newEditUserNumOfGamesPlayed);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user num og games played");
               return;
           }
        }


       public void EditUserTotalProfit(int Id, int newTotalProfit)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserTotalProfit(Id, newTotalProfit);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user Total profit");
               return;
           }
        }

       public void EditUserWinNum(int Id, int newWinNum)
       {
           try
           {
               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   db.EditUserWinNum(Id, newWinNum);
               }

           }
           catch (Exception e)
           {
               Console.WriteLine("error edit  user win num");
               return;
           }
        }

        public void EditUserPoints(int Id, int newPoints)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.EditUserPoints(Id, newPoints);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user win num");
                return;
            }
        }

        public void EditUserAvatar(int Id, string newAvatar)
        {
            try
            {
                using (connectionsLinqDataContext db = new connectionsLinqDataContext())
                {
                    db.EditAvatar(Id, newAvatar);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user avatar");
                return;
            }
        }
        private UserTable convertToUser(GetAllUserResult v)
        {
            UserTable toReturn = new UserTable();
            toReturn.userId = v.userId;
            toReturn.username = v.username;
            toReturn.name = v.name;
            toReturn.password = v.password;
            toReturn.email = v.email;
            toReturn.HighestCashGainInGame = v.HighestCashGainInGame;
            toReturn.TotalProfit = v.TotalProfit;
            toReturn.avatar = v.avatar;
            toReturn.inActive = v.inActive;
            toReturn.leagueName = v.leagueName;
            toReturn.gamesPlayed = v.gamesPlayed;
            toReturn.money = v.money;
            toReturn.winNum = v.winNum;
            toReturn.points = v.points;
            return toReturn;
        }

        private UserTable convertToUser(GetUserByUserIdResult v)
        {
            UserTable toReturn = new UserTable();
            toReturn.userId = v.userId;
            toReturn.username = v.username;
            toReturn.name = v.name;
            toReturn.password = v.password;
            toReturn.email = v.email;
            toReturn.HighestCashGainInGame = v.HighestCashGainInGame;
            toReturn.TotalProfit = v.TotalProfit;
            toReturn.avatar = v.avatar;
            toReturn.inActive = v.inActive;
            toReturn.leagueName = v.leagueName;
            toReturn.gamesPlayed = v.gamesPlayed;
            toReturn.money = v.money;
            toReturn.winNum = v.winNum;
            toReturn.points = v.points;
            return toReturn;
        }

        private UserTable convertToUser(GetUserByUserNameResult v)
        {
            UserTable toReturn = new UserTable();
            toReturn.userId = v.userId;
            toReturn.username = v.username;
            toReturn.name = v.name;
            toReturn.password = v.password;
            toReturn.email = v.email;
            toReturn.HighestCashGainInGame = v.HighestCashGainInGame;
            toReturn.TotalProfit = v.TotalProfit;
            toReturn.avatar = v.avatar;
            toReturn.inActive = v.inActive;
            toReturn.leagueName = v.leagueName;
            toReturn.gamesPlayed = v.gamesPlayed;
            toReturn.money = v.money;
            toReturn.winNum = v.winNum;
            toReturn.points = v.points;
            return toReturn;
        }
    }
}
