namespace TexasHoldemShared.CommMessages
{
    public class LeaderboardLineData
    {
        public int Id;
        public string Name;
        public int Points;
        public int TotalGrossProfit;
        public int HighestCashGain;
        public int NumOfGamesPlayed;

        //for parsing
        public LeaderboardLineData() { }

        public LeaderboardLineData(int id, string name, int points, int totalGrossProfit, int highestCashGain, int numOfGamesPlayed)
        {
            Id = id;
            Name = name;
            Points = points;
            TotalGrossProfit = totalGrossProfit;
            HighestCashGain = highestCashGain;
            NumOfGamesPlayed = numOfGamesPlayed;
        }

        public bool Equals(LeaderboardLineData other)
        {
            return Id == other.Id && Name.Equals(other.Name) && Points == other.Points &&
                   TotalGrossProfit == other.TotalGrossProfit && HighestCashGain == other.HighestCashGain &&
                   NumOfGamesPlayed == other.NumOfGamesPlayed;
        }
    }
}
