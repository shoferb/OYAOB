namespace TexasHoldem.communication.Interfaces
{
    public interface ISessionIdHandler
    {
        bool AddSid(int userId, long sessionId);
        bool ContainsSid(long sid);
        bool ContainsUserId(int userId);
        long GetSessionIdByUserId(int userId);
    }
}
