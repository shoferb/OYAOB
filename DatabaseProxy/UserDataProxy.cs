using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Database.DataControlers;
using TexasHoldem.Database.LinqToSql;
using TexasHoldem.Logic.Users;
using LeagueName = TexasHoldem.Logic.GameControl.LeagueName;

namespace TexasHoldem.DatabaseProxy
{
    public class UserDataProxy
    {
        UserDataControler userDataControler = new UserDataControler();
        public void Login(IUser user)
        {
            UserTable toLogin = convertToUserT(user);
            userDataControler.EditUserIsActive(toLogin.userId,true);

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
            IUser toResturn = new User(user.userId, user.name, user.username, user.password, user.points,
                user.money, user.email, user.winNum, 0, user.HighestCashGainInGame, user.TotalProfit,user.avatar
                ,user.gamesPlayed,user.inActive,GetLeagueName(user.leagueName));
            return toResturn;
        }
        public void Logout(IUser user)
        {
            UserTable toLogin = convertToUserT(user);
            userDataControler.EditUserIsActive(toLogin.userId, false);
        }

        public IUser GetUserById(int userid)
        {
            IUser toReturn = null;
            UserTable temp = userDataControler.GetUserById(userid);
            toReturn = convertToIUser(temp);
            return toReturn;
        }

        public IUser GetUserByUserName(string username)
        {
            IUser toReturn = null;
            UserTable temp = userDataControler.GetUserByUserName(username);
            toReturn = convertToIUser(temp);
            return toReturn;
        }

        public List<IUser> GetAllUser()
        {
            List<IUser> toreturn = new List<IUser>();
            List<UserTable> temp = userDataControler.GetAllUser();
            foreach (UserTable user in temp)
            {
                IUser toAdd = convertToIUser(user);
                toreturn.Add(toAdd);
            }
            return toreturn;
        }

        public void DeleteUserByUserName(string username)
        {
            userDataControler.DeleteUserByUsername(username);
        }

        public void DeleteUserById(int Id)
        {
            userDataControler.DeleteUserById(Id);
        }

        public void AddNewUser(IUser toAdd)
        {
            UserTable toAddUT = convertToUserT(toAdd);
            userDataControler.AddNewUser(toAddUT);
        }
    }
}
