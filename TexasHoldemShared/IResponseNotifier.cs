using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public interface IResponseNotifier
    {
        bool Notify(ChatCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(LoginCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(RegisterCommMessage originalMsg, RegisterResponeCommMessage msg);
        bool Notify(CreateNewRoomMessage originalMsg, ResponeCommMessage msg);
        bool Notify(ReturnToGameAsPlayerCommMsg originalMsg, ResponeCommMessage msg);
        bool Notify(ReturnToGameAsSpecCommMsg originalMsg, ResponeCommMessage msg);
        bool Notify(SearchCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(ActionCommMessage originalMsg);

        //defaults:
        bool Notify(EditCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(LeaderboardCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(ReplayCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(UserStatisticsCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(ResponeCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(ReturnToGameResponseCommMsg originalMsg, ResponeCommMessage msg);
        bool Notify(SearchResponseCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(UserStatisticsResponseCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(CreateNewGameResponse originalMsg, ResponeCommMessage msg);
        bool Notify(GameDataCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(JoinResponseCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(LeaderboardResponseCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(LoginResponeCommMessage originalMsg, ResponeCommMessage msg);
        bool Notify(RegisterResponeCommMessage originalMsg, ResponeCommMessage msg);

        bool Default(ResponeCommMessage msg);
    }
}
