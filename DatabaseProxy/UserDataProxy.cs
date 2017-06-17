using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.Security;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.Security;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;

namespace TexasHoldem.DatabaseProxy
{
    public class UserDataProxy
    {
        readonly UserDataControler _userDataControler = new UserDataControler();
        
        public void Login(IUser user)
        {
            try
            {
                var toLogin = convertToUserT(user);
                _userDataControler.EditUserIsActive(toLogin.userId, true);
            }
            catch (Exception)
            {
                return;
            }
        }

        private static int GetLeagueNum(LeagueName league)
        {
            var toReturn = 0;
            switch (league)
            {
                    case LeagueName.A:
                        toReturn = 1;
                    break;
                case LeagueName.B:
                    toReturn = 2;
                    break;
                case LeagueName.C:
                    toReturn = 3;
                    break;
                case LeagueName.D:
                    toReturn = 4;
                    break;
                case LeagueName.E:
                    toReturn = 5;
                    break;
                case LeagueName.Unknow:
                    toReturn = 6;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(league), league, null);
            }
            return toReturn;
        }
        private static LeagueName GetLeagueName(int league)
        {
           LeagueName toReturn = 0;
            switch (league)
            {
                case 1:
                    toReturn = LeagueName.A;
                    break;
                case 2:
                    toReturn = LeagueName.B;
                    break;
                case 3:
                    toReturn = LeagueName.C;
                    break;
                case 4:
                    toReturn = LeagueName.D;
                    break;
                case 5:
                    toReturn = LeagueName.E;
                    break;
                case 6:
                    toReturn = LeagueName.Unknow;
                    break;

            }
            return toReturn;
        }


        private UserTable convertToUserT(IUser user)
        {
            var toReturn = new UserTable
            {
                userId = user.Id(),
                username = user.MemberName(),
                name = user.Name(),
                password = user.Password(),
                email = user.Email(),
                HighestCashGainInGame = user.HighestCashGainInGame,
                TotalProfit = user.TotalProfit,
                avatar = user.Avatar(),
                inActive = user.IsLogin(),
                leagueName = GetLeagueNum(user.GetLeague()),
                gamesPlayed = user.GetNumberOfGamesUserPlay(),
                money = user.Money(),
                winNum = user.WinNum,
                points = user.Points()
            };
            return toReturn;
        }

        private static IUser ConvertToIUser(UserTable user)
        {
           // string decryptpassword = PasswordSecurity.Decrypt(user.password, "securityPassword");
            IUser toResturn = new User(user.userId, user.name, user.username, user.password, user.points,
                user.money, user.email, user.winNum, 0, user.HighestCashGainInGame, user.TotalProfit,user.avatar
                ,user.gamesPlayed,user.inActive,GetLeagueName(user.leagueName));
            return toResturn;
        }

        public void Logout(IUser user)
        {
            try
            {
                var toLogin = convertToUserT(user);
                _userDataControler.EditUserIsActive(toLogin.userId, false);
            }
            catch (Exception)
            {
                return;
            }
        }

        public IUser GetUserById(int userid)
        {
 
            try
            {
                var user = _userDataControler.GetUserById(userid);
               // Console.WriteLine("inside proxy get user by id ut Id " + user.userId);
             //   Console.WriteLine("!!inside proxy get user by ut password " + user.password);
              
                var toResturn = ConvertToIUser(user);
               // var toDec = user.password;
               // Console.WriteLine("!!inside proxy get user by IUSER password " + toResturn.Password());
                //string decryptpassword = PasswordSecurity.Decrypt(toDec, "securityPassword");
                //Console.WriteLine("!!inside proxy get user by id DEC PASSWORD " + decryptpassword);
                //toResturn.EditPassword(decryptpassword);
                //Console.WriteLine("!!inside proxy get user by id IUSER  DEC PASSWORD " + toResturn.Password());
                return toResturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IUser GetUserByUserName(string username)
        {
            IUser toReturn = null;
            try
            {
                var temp = _userDataControler.GetUserByUserName(username);
                toReturn = ConvertToIUser(temp);
                return toReturn;
            }
            catch (Exception )
            {
                return toReturn;
            }
        }

        public List<IUser> GetAllUser()
        {

            var toreturn = new List<IUser>();
            try
            {
                var temp = _userDataControler.GetAllUser();
                foreach (var user in temp)
                {
                    var toAdd = ConvertToIUser(user);
                    toreturn.Add(toAdd);
                }
                return toreturn;
            }
            catch (Exception)
            {
                return toreturn;
            }
        }

        public void DeleteUserByUserName(string username)
        {
            try
            {
                _userDataControler.DeleteUserByUsername(username);
            }
            catch (Exception)
            {
                return;
            }

        }

        public void DeleteUserById(int id)
        {
            try
            {
                _userDataControler.DeleteUserById(id);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void AddNewUser(IUser toAdd)
        {
            try
            {
                UserTable toAddUt = convertToUserT(toAdd);
               // string pass = toAdd.Password();
                //string encryptedstring = PasswordSecurity.Encrypt(pass, "securityPassword");
                //toAddUT.password = encryptedstring;
                _userDataControler.AddNewUser(toAddUt);
            }
            catch (Exception e)
            {
                return;
            }

        }

        public void EditUserId(int oldId, int newId)
        {
            try
            {
               _userDataControler.EditUserId(oldId,newId);

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
                _userDataControler.EditUserPoints(id,newPoints);

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
                _userDataControler.EditUserAvatar(id, newAvatar);
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
               _userDataControler.EditUserName(id,newUserName);
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
                _userDataControler.EditName(id,newName);
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
                _userDataControler.EditEmail(id,newEmail);
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
                // string encryptedPassword = PasswordSecurity.Encrypt(newPassword, "securityPassword");
                //  userDataControler.EditPassword(Id, encryptedPassword);
                _userDataControler.EditPassword(id, newPassword);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void EditUserHighestCashGainInGame(int id, int newHighestCashGainInGame)
        {
            try
            {
                _userDataControler.EditUserHighestCashGainInGame(id,newHighestCashGainInGame);
            }
            catch (Exception)
            {
                return;
            }
        }


      

        public void EditUserLeagueName(int id, LeagueName newLeagueName)
        {
            try
            {
                var league = new Database.LinqToSql.LeagueName
                {
                    League_Value = GetLeagueNum(newLeagueName)
                };

                _userDataControler.EditUserLeagueName(id,league);
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
                _userDataControler.EditUserMoney(id,newMoney);
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
                _userDataControler.EditUserNumOfGamesPlayed(id,newEditUserNumOfGamesPlayed);
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
                _userDataControler.EditUserTotalProfit(id,newTotalProfit);
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
               _userDataControler.EditUserWinNum(id,newWinNum);
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
                _userDataControler.DeleteSpectetorGameOfUSer(userId, roomId, gameId);

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
                _userDataControler.DeleteActiveGameOfUser(userId,roomId,gameId);

            }
            catch (Exception)
            {
                return;
            }
        }

        public void AddGameToUserActiveGames(int userId, int roomId, int gameId)
        {
            try
            {
                _userDataControler.AddGameToUserActiveGames(userId, roomId, gameId);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void AddGameToUserSpectetorGames(int userId, int roomId, int gameId)
        {
            try
            {
                _userDataControler.AddGameToUserSpectetorGames(userId, roomId, gameId);
            }
            catch (Exception)
            {
                return;
            }
        }


    }
}
