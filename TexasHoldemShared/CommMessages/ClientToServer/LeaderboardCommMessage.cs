using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class LeaderboardCommMessage : CommunicationMessage
    {
        /// <summary>
        /// can be one of:
        ///     totGrossProfit
        ///     highCashGain
        ///     numGamesPlayed
        /// </summary>
        public string SortBy;

        public LeaderboardCommMessage(int id, string sortBy) : base(id)
        {
            SortBy = sortBy;
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
                ans = UserId == afterCast.UserId && SortBy.Equals(afterCast.SortBy);
            }
            return ans;
        }
    }
}
