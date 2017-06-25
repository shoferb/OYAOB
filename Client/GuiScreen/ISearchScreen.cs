
using System.Collections.Generic;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.GuiScreen
{
    public interface ISearchScreen
    {
        void EmptySearch();
        void ResultRecived(List<ClientGame> games);
        void JoinOkay(GameDataCommMessage msgGameData);
        void JoinOkayAsSpectate(GameDataCommMessage msgGameData);
    }
}
