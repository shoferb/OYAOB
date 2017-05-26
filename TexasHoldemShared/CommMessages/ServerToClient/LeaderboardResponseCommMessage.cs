using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class LeaderboardResponseCommMessage : ResponeCommMessage
    {
        public List<LeaderboardLineData> Results;

        public LeaderboardResponseCommMessage() { }

        public LeaderboardResponseCommMessage(int id, bool success, CommunicationMessage originalMsg, 
            List<LeaderboardLineData> results) : base(id, success, originalMsg)
        {
            Results = results;
        }

        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            bool ans = false;
            if (other.GetType() == typeof(LeaderboardResponseCommMessage))
            {
                var afterCast = (LeaderboardResponseCommMessage)other;
                ans = base.Equals(afterCast) && Results.TrueForAll(line => afterCast.Results.Contains(line));
            }
            return ans;
        }
    }
}
