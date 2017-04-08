using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    abstract class UserServiceHandler : ServiceHandler
    {
        protected const int InitialMoneyAmount = 100;
        protected const int InitialPointsAmount = 0;

        public abstract User GetUserFromId(int userId);
        public abstract int GetNextUserId();
        public abstract int IncUserId();

        public abstract User CreateNewUser(int id, string name, string memberName,
            string password, string email);

        public abstract bool EditUserPassword(string password /*TODO*/);
        public abstract bool EditUserEmail(string newEmail);
    }
}