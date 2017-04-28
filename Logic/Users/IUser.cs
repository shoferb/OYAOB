using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;

namespace TexasHoldem.Logic.Users
{
    public interface IUser
    {
        bool Login();

        bool Logout();

        bool EditEmail(string email);

        bool EditPassword(string password);

        bool  EditUserName(string username);

        bool EditAvatar(string path);

        bool EditUserPoint(int point);

        bool EditUserMoney(int money);

        bool RemoveRoomFromActiveGameList(IGame game);
        
        bool RemoveRoomFromSpectetorGameList(IGame game);

        bool HasThisActiveGame(IGame game);

        bool HasThisSpectetorGame(IGame game);

        bool AddRoomFromActiveGameList(IGame game);

        bool AddRoomFromSpectetorGameList(IGame game);

    }
}
