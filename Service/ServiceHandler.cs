using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;

//Service handler interface
namespace TexasHoldem.Service
{
    abstract class ServiceHandler
    {
        //user related:
        protected const int InitialMoneyAmount = 100;
        protected const int InitialPointsAmount = 0;

        public abstract int GetNextUserId();
        public abstract int IncUserId();
        public abstract User CreateNewUser(int id, string name, string memberName,
            string password, string email);
        public abstract bool EditUserPassword(string password /*TODO*/);
        public abstract bool EditUserEmail(string newEmail);
        //TODO: more user stuff


        //game relatad:
        protected abstract GamePrefDecorator CreateGameRoom(int id, string name, int sb,
            int bb, int minMoney, int maxMoney, int gameNum);
        protected abstract bool AddPlayerToRoom(Player player, /*TODO: maybe change this*/ GameRoom room);
        protected abstract bool AddSpectatorToRoom(Spectetor spectator, /*TODO: maybe change this*/ GameRoom room);
        protected abstract bool MakeRoomActive(GameRoom room);

        protected abstract bool Fold(Player player, GameRoom room);
        protected abstract bool Check(Player player, GameRoom room);
        protected abstract bool Call(Player player, GameRoom room);
        protected abstract bool Raise(Player player, GameRoom room, int sum);
        //TODO: more game stuff
        //TODO: how to do card dealing?

        //notification related:
        //TODO: more notification stuff

        //replay related:
        //TODO: more replay stuff

    }
}
