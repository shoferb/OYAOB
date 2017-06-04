using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Database.Security;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.Security;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;

namespace TexasHoldem.DatabaseProxy
{
    public class UserDataProxy
    {
        UserDataControler userDataControler = new UserDataControler();
        private readonly ISecurity _security = new SecurityHandler();

        public void Login(IUser user)
        {
            try
            {
                UserTable toLogin = convertToUserT(user);
                userDataControler.EditUserIsActive(toLogin.userId, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy login fail");
                return;
            }
        }

        private int GetLeagueNum(LeagueName league)
        {
            int toReturn = 0;
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

            }
            return toReturn;
        }
        private LeagueName GetLeagueName(int league)
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
            UserTable toReturn = new UserTable();
            toReturn.userId = user.Id();
            toReturn.username = user.MemberName();
            toReturn.name = user.Name();
            toReturn.password = user.Password();
            toReturn.email = user.Email();
            toReturn.HighestCashGainInGame = user.HighestCashGainInGame;
            toReturn.TotalProfit = user.TotalProfit;
            toReturn.avatar = user.Avatar();
            toReturn.inActive = user.IsLogin();
            toReturn.leagueName = GetLeagueNum(user.GetLeague());
            toReturn.gamesPlayed = user.GetNumberOfGamesUserPlay();
            toReturn.money = user.Money();
            toReturn.winNum = user.WinNum;
            toReturn.points = user.Points();
            return toReturn;
        }

        private IUser convertToIUser(UserTable user)
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
                UserTable toLogin = convertToUserT(user);
                userDataControler.EditUserIsActive(toLogin.userId, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy logout fails");
                return;
            }
        }

        public IUser GetUserById(int userid)
        {
 
            try
            {
                UserTable user = userDataControler.GetUserById(userid);
                Console.WriteLine("inside proxy get user by id ut Id " + user.userId);
                Console.WriteLine("!!inside proxy get user by ut password " + user.password);
              
                IUser toResturn = convertToIUser(user);
                string toDec = user.password;
                Console.WriteLine("!!inside proxy get user by IUSER password " + toResturn.Password());
                //string decryptpassword = PasswordSecurity.Decrypt(toDec, "securityPassword");
                //Console.WriteLine("!!inside proxy get user by id DEC PASSWORD " + decryptpassword);
                //toResturn.EditPassword(decryptpassword);
                //Console.WriteLine("!!inside proxy get user by id IUSER  DEC PASSWORD " + toResturn.Password());
                return toResturn;
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy get user by id fails");
                Console.WriteLine("exeption: "+e);
                return null;
            }
        }

        public IUser GetUserByUserName(string username)
        {
            IUser toReturn = null;
            try
            {
                UserTable temp = userDataControler.GetUserByUserName(username);
                toReturn = convertToIUser(temp);
                return toReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine("error user proxy get user by user name fails");
                return toReturn;
            }
        }

        public List<IUser> GetAllUser()
        {

            List<IUser> toreturn = new List<IUser>();
            try
            {
                List<UserTable> temp = userDataControler.GetAllUser();
                foreach (UserTable user in temp)
                {
                    IUser toAdd = convertToIUser(user);
                    toreturn.Add(toAdd);
                }
                return toreturn;
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy: get all user fails");
                return toreturn;
            }
        }

        public void DeleteUserByUserName(string username)
        {
            try
            {
                userDataControler.DeleteUserByUsername(username);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy: Delete user by user name fails");
                return;
            }

        }

        public void DeleteUserById(int Id)
        {
            try
            {
                userDataControler.DeleteUserById(Id);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy: Delete user by user Id fails");
                return;
            }
        }

        public void AddNewUser(IUser toAdd)
        {
            try
            {
                UserTable toAddUT = convertToUserT(toAdd);
               // string pass = toAdd.Password();
                //string encryptedstring = PasswordSecurity.Encrypt(pass, "securityPassword");
                //toAddUT.password = encryptedstring;
                userDataControler.AddNewUser(toAddUT);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in user proxy: add new user fails fails");
                return;
            }

        }

        public void EditUserId(int oldId, int newId)
        {
            try
            {
               userDataControler.EditUserId(oldId,newId);

            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user Id in proxy");
                return;
            }
        }

        public void EditUserPoints(int Id, int newPoints)
        {
            try
            {
                userDataControler.EditUserPoints(Id,newPoints);

            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user points proxy");
                return;
            }
        }
        public void EditUserAvatar(int Id, string newAvatar)
        {
            try
            {
                userDataControler.EditUserAvatar(Id, newAvatar);
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit user avatar in proxy");
                return;
            }
        }
        public void EditUserName(int Id, string newUserName)
        {
            try
            {
               userDataControler.EditUserName(Id,newUserName);
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  username in proxy");
                return;
            }
        }

        public void EditName(int Id, string newName)
        {
            try
            {
                userDataControler.EditName(Id,newName);
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
                userDataControler.EditEmail(Id,newEmail);
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
                // string encryptedPassword = PasswordSecurity.Encrypt(newPassword, "securityPassword");
                //  userDataControler.EditPassword(Id, encryptedPassword);
                userDataControler.EditPassword(Id, newPassword);
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
                userDataControler.EditUserHighestCashGainInGame(Id,newHighestCashGainInGame);

            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user Highest Cash Gain In Game");
                return;
            }
        }


      

        public void EditUserLeagueName(int Id, LeagueName newLeagueName)
        {
            try
            {
                Database.LinqToSql.LeagueName league = new Database.LinqToSql.LeagueName();
                league.League_Value = GetLeagueNum(newLeagueName);
                
               userDataControler.EditUserLeagueName(Id,league);
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
                userDataControler.EditUserMoney(Id,newMoney);
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
                userDataControler.EditUserNumOfGamesPlayed(Id,newEditUserNumOfGamesPlayed);
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
                userDataControler.EditUserTotalProfit(Id,newTotalProfit);
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
               userDataControler.EditUserWinNum(Id,newWinNum);
            }
            catch (Exception e)
            {
                Console.WriteLine("error edit  user win num");
                return;
            }
        }
    }
}
