using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;

namespace TexasHoldem.communication.Impl
{
    public class SessionIdHandler : ISessionIdHandler
    {
        private readonly IDictionary<int, long> _uidToSidMap = new Dictionary<int, long>();

        public bool AddSid(int userId, long sessionId)
        {
            if (!ContainsSid(sessionId) && !ContainsUserId(userId))
            {
                _uidToSidMap.Add(userId, sessionId);
                return true;
            }
            return false;
        }

        public bool ContainsSid(long sid)
        {
            return _uidToSidMap.Values.Contains(sid);
        }

        public bool ContainsUserId(int userId)
        {
            return _uidToSidMap.ContainsKey(userId);
        }

        public long GetSessionIdByUserId(int userId)
        {
            if (_uidToSidMap.ContainsKey(userId))
            {
                return _uidToSidMap[userId];
            }
            return -1;
        }
    }
}
