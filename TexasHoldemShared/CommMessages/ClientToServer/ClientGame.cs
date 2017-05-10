namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ClientGame
    {
        private bool isActive;
        private bool isSpectetor;
        private GameMode gameMode;
        private int roomId;
        private int minPlayer;
        private int maxPlayer;
        private int minBet;
        private int startingChip;
        private int buyInPolicy;
        private string LeagueName;
        private int potSize;

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