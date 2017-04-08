using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class UserServiceHandler : ServiceHandler
    {
        public const int InitialMoneyAmount = 100;
        public const int InitialPointsAmount = 0;

        public abstract User GetUserFromId(int userId);
        public abstract Player GetPlayer(int userId, int roomId);
        public abstract int GetNextUserId();
        public abstract int IncUserId();

        public abstract User CreateNewUser(int id, string name, string memberName,
            string password, string email);

        public abstract bool EditUserPassword(string password /*TODO*/);
        public abstract bool EditUserEmail(string newEmail);
    }
}