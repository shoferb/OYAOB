using Newtonsoft.Json;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    [JsonObject(Title = "LeaderboardCommMessage")]
    public class LeaderboardCommMessage : CommunicationMessage
    {
        public enum SortingOption
        {
            TotalGrossProfit,
            HighestCashGain,
            NumGamesPlayes
        }

        public SortingOption SortedBy;

        public LeaderboardCommMessage() : base(-1, -1) { }

        public LeaderboardCommMessage(int id, long sid, SortingOption sortBy) : base(id, sid)
        {
            SortedBy = sortBy;
        }

        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            bool ans = false;
            if (other.GetType() == typeof(LeaderboardCommMessage))
            {
                var afterCast = (LeaderboardCommMessage)other;
                ans = UserId == afterCast.UserId && SortedBy.Equals(afterCast.SortedBy);
            }
            return ans;
        }
    }
}
