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
            var toReturn = new List<UserTable>();
            try
            {

                using (var db = new connectionsLinqDataContext())
                {
                    var temp = db.GetAllUser().ToList();
                    foreach (var v in temp)
                    {
                        UserTable toAdd = ConvertToUser(v);
                        var decryptedpassword = PasswordSecurity.Decrypt("securityPassword", toAdd.password, false);

                        toAdd.password = decryptedpassword;
                        toReturn.Add(toAdd);
                    }
                    return toReturn;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

       

        public UserTable GetUserById(int id)
       {
           
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   var temp = db.GetUserByUserId(id).ToList()[0];

                    var toReturn = ConvertToUser(temp);
                    var decryptedpassword = PasswordSecurity.Decrypt( "securityPassword",toReturn.password,false);

                    toReturn.password = decryptedpassword;

                    return toReturn;
               }
           }
           catch (Exception)
           {
               return null;
           }

        }
        
       public UserTable GetUserByUserName(string username)
       {
           try
           {

               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   var temp = db.GetUserByUserName(username).ToList().First();
                   var toReturn = ConvertToUser(temp);
                   var decryptedpassword = PasswordSecurity.Decrypt("securityPassword", toReturn.password, false);

                   toReturn.password = decryptedpassword;
                    return toReturn;
               }
           }
           catch (Exception e)
           {
               return null;
           }
        }
        
       public void AddNewUser(UserTable toAddUser)
       {
           try
           {

               using (connectionsLinqDataContext db = new connectionsLinqDataContext())
               {
                   var encryptedpassword = PasswordSecurity.Encrypt( "securityPassword", toAddUser.password,false);
                    db.AddNewUser(toAddUser.userId, toAddUser.username, toAddUser.name, toAddUser.email, encryptedpassword, toAddUser.avatar, toAddUser.points, toAddUser.money, toAddUser.gamesPlayed, toAddUser.leagueName, toAddUser.winNum, toAddUser.HighestCashGainInGame, toAddUser.TotalProfit, toAddUser.inActive);
                    //db.UserTables.InsertOnSubmit(toAddUser);
                //    db.SubmitChanges();
                }
           }
           catch (Exception e)
           {
               return;
           }
        }

        public void DeleteUserById(int userId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.DeleteUserById(userId);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void DeleteUserByUsername(string username)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.DeleteUserByUserName(username);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void EditUserId(int oldId, int newId)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserId(newId, oldId);
               }  
            }
           catch (Exception)
           {
               return;
           }
       }
        
       public void EditUserName(int id, string newUserName)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUsername(id,newUserName);
               }

           }
           catch (Exception)
           {
               return;
           }
        }
        
       public void EditName(int id, string newName)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditName(id,newName);
               }
           }
           catch (Exception)
           {
               return;
           }
        }

       public void EditEmail(int id, string newEmail)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditEmail(id,newEmail);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

       public void EditPassword(int id, string newPassword)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   var encryptedpassword = PasswordSecurity.Encrypt("securityPassword", newPassword, false);

                    db.EditPassword(id, encryptedpassword);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

       public void EditUserHighestCashGainInGame(int Id, int newHighestCashGainInGame)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserHighestCashGainInGame(Id,newHighestCashGainInGame);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

       
       public void EditUserIsActive(int id, bool newIsActive)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserIsActive(id,newIsActive);
               }

           }
           catch (Exception)
           {
               return;
           }
        }
      
       public void EditUserLeagueName(int id, LinqToSql.LeagueName newLeagueName)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserLeagueName(id,newLeagueName.League_Value);
               }
           }
           catch (Exception)
           {
               return;
           }
        }


       public void EditUserMoney(int id, int newMoney)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserMoney(id,newMoney);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

       

        public void EditUserNumOfGamesPlayed(int id, int newEditUserNumOfGamesPlayed)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserNumOfGamesPlayed(id, newEditUserNumOfGamesPlayed);
               }

           }
           catch (Exception)
           {
               return;
           }
        }


       public void EditUserTotalProfit(int id, int newTotalProfit)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserTotalProfit(id, newTotalProfit);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

       public void EditUserWinNum(int id, int newWinNum)
       {
           try
           {
               using (var db = new connectionsLinqDataContext())
               {
                   db.EditUserWinNum(id, newWinNum);
               }

           }
           catch (Exception)
           {
               return;
           }
        }

        public void EditUserPoints(int id, int newPoints)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.EditUserPoints(id, newPoints);
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public void EditUserAvatar(int id, string newAvatar)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.EditAvatar(id, newAvatar);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        private static UserTable ConvertToUser(GetAllUserResult v)
        {
            UserTable toReturn = new UserTable
            {
                userId = v.userId,
                username = v.username,
                name = v.name,
                password = v.password,
                email = v.email,
                HighestCashGainInGame = v.HighestCashGainInGame,
                TotalProfit = v.TotalProfit,
                avatar = v.avatar,
                inActive = v.inActive,
                leagueName = v.leagueName,
                gamesPlayed = v.gamesPlayed,
                money = v.money,
                winNum = v.winNum,
                points = v.points
            };
            return toReturn;
        }

        private static UserTable ConvertToUser(GetUserByUserIdResult v)
        {
            var toReturn = new UserTable
            {
                userId = v.userId,
                username = v.username,
                name = v.name,
                password = v.password,
                email = v.email,
                HighestCashGainInGame = v.HighestCashGainInGame,
                TotalProfit = v.TotalProfit,
                avatar = v.avatar,
                inActive = v.inActive,
                leagueName = v.leagueName,
                gamesPlayed = v.gamesPlayed,
                money = v.money,
                winNum = v.winNum,
                points = v.points
            };
            return toReturn;
        }

        private static UserTable ConvertToUser(GetUserByUserNameResult v)
        {
            var toReturn = new UserTable
            {
                userId = v.userId,
                username = v.username,
                name = v.name,
                password = v.password,
                email = v.email,
                HighestCashGainInGame = v.HighestCashGainInGame,
                TotalProfit = v.TotalProfit,
                avatar = v.avatar,
                inActive = v.inActive,
                leagueName = v.leagueName,
                gamesPlayed = v.gamesPlayed,
                money = v.money,
                winNum = v.winNum,
                points = v.points
            };
            return toReturn;
        }

      
        public void AddGameToUserActiveGames(int userId, int roomId,int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.AddUserActiveGame(userId, roomId, gameId);
                }

            }
            catch (Exception)
            {
                return ;
            }
        }

        public void AddGameToUserSpectetorGames(int userId, int roomId, int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.AddNewSpectetorGamesOfUser(userId, roomId, gameId);
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public void DeleteSpectetorGameOfUSer(int userId, int roomId, int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.DeleteUserSpectetorGame(userId, roomId, gameId);
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public void DeleteActiveGameOfUser(int userId, int roomId, int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    db.DeleteUserActiveGame(userId, roomId, gameId);
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public bool HasThisActiveGame(int userId, int roomId, int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    var allUserGames =  db.GetAllUserActiveGame(userId);
                    foreach (var entry in allUserGames)
                    {
                        if (entry.Game_Id == gameId && entry.roomId == roomId)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool HasThisSpectetorGame(int userId, int roomId, int gameId)
        {
            try
            {
                using (var db = new connectionsLinqDataContext())
                {
                    var allUserGames = db.GetUserSpectetorsGame(userId);
                    foreach (var entry in allUserGames)
                    {
                        if (entry.Game_Id == gameId && entry.roomId == roomId)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
