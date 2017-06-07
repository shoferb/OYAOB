using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public class ActionResultInfo
    {
        public int Id;
        public GameDataCommMessage GameData;

        public ActionResultInfo(int id, GameDataCommMessage gameData)
        {
            Id = id;
            GameData = gameData;
        }
    }
}
