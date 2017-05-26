namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class UserStatisticsResponseCommMessage : ResponeCommMessage
    {
        public double AvgCashGain;
        public double AvgGrossProfit;

        public UserStatisticsResponseCommMessage() { }

        public UserStatisticsResponseCommMessage(int id, bool success, CommunicationMessage originalMsg, 
            double avgCashGain, double avgGrossProfit) 
            : base(id, success, originalMsg)
        {
            AvgCashGain = avgCashGain;
            AvgGrossProfit = avgGrossProfit;
        }

        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            bool ans = false;
            if (other.GetType() == typeof(UserStatisticsResponseCommMessage))
            {
                var afterCast = (UserStatisticsResponseCommMessage)other;
                ans = base.Equals(afterCast) && Equals(AvgCashGain, afterCast.AvgCashGain) &&
                      Equals(AvgGrossProfit, afterCast.AvgGrossProfit);
            }
            return ans;
        }
    }
}
