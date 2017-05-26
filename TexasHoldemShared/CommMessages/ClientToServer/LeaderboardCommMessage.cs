using Newtonsoft.Json;

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

        public LeaderboardCommMessage() : base(-1) { }

        public LeaderboardCommMessage(int id, SortingOption sortBy) : base(id)
        {
            SortedBy = sortBy;
        }

        public override string Handle(IEventHandler handler)
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
