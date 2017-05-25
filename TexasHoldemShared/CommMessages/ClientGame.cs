namespace TexasHoldemShared.CommMessages
{
    public class ClientGame
    {
        public bool isActive { get; set; }
        public bool isSpectetor { get; set; }
        public GameMode gameMode { get; set; }
        public int roomId { get; set; }
        public int minPlayer { get; set; }
        public int maxPlayer { get; set; }
        public int minBet { get; set; }
        public int startingChip { get; set; }
        public int buyInPolicy { get; set; }
        public string LeagueName { get; set; }
        public int potSize { get; set; }

        public ClientGame()  { } //for parsing

        public ClientGame(bool isActive, bool isSpectetor, GameMode gameMode, int roomId, int minPlayer, int maxPlayer, int minBet, int startingChip, int buyInPolicy, string leagueName, int potSize)
        {
            this.isActive = isActive;
            this.isSpectetor = isSpectetor;
            this.gameMode = gameMode;
            this.roomId = roomId;
            this.minPlayer = minPlayer;
            this.maxPlayer = maxPlayer;
            this.minBet = minBet;
            this.startingChip = startingChip;
            this.buyInPolicy = buyInPolicy;
            LeagueName = leagueName;
            this.potSize = potSize;
        }
    }
}