using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public interface IResponseNotifier
    {
        bool GeneralCase(GameDataCommMessage msgGameData);
        bool ObserverNotify(ResponeCommMessage msg);
        bool NotifyChat(ResponeCommMessage msg);
        bool NotifyReturnAsPlayer(ResponeCommMessage msg);
        bool NotifyReturnAsSpec(ResponeCommMessage msg);
        bool NotifySearch(ResponeCommMessage msg);
        bool ReceivedJoin(ResponeCommMessage msg);
        bool ReceivedSpectate(ResponeCommMessage msg);
        bool NotifyAction(ResponeCommMessage msg);

        bool Default(ResponeCommMessage msg);
    }
}
