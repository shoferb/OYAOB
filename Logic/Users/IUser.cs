using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.Users
{
    public interface IUser
    {
        
        int Id();
       
        String Name();
       
        String MemberName();
        
        string Password();
       
        String Avatar();
        
        int Points();
     
        int Money();

         List<Notification> WaitListNotification();

        string Email();

        List<Tuple<int, int>> GamesAvailableToReplay();

        List<IGame> ActiveGameList();


        List<IGame> SpectateGameList();

        List<Actions.Action> FavActions();

        int WinNum();

        bool IncWinNum();

        int Rank();

        bool Login();

        bool Logout();

        bool EditId(int Id);

        bool EditEmail(string email);

        bool EditPassword(string password);

        bool  EditUserName(string username);

        bool EditName(string name);

        bool EditAvatar(string path);

        bool EditUserPoints(int point);

        bool EditUserMoney(int money);

        bool EditUserRank(int Rank);

        bool RemoveRoomFromActiveGameList(IGame game);
        
        bool RemoveRoomFromSpectetorGameList(IGame game);

        bool HasThisActiveGame(IGame game);

        bool HasThisSpectetorGame(IGame game);

        bool AddRoomToActiveGameList(IGame game);

        bool AddRoomToSpectetorGameList(IGame game);

        bool IsLogin();


        bool SendNotification(Notification toSend);


        bool AddNotificationToList(Notification toAdd);


        bool AddGameAvailableToReplay(int roomID, int gameID);

        bool AddActionToFavorite(Actions.Action action);

    }
}
