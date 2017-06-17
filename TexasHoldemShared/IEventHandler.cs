using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public interface IEventHandler
    {
        //client to server:
        ResponeCommMessage HandleEvent(ActionCommMessage msg);
        ResponeCommMessage HandleEvent(EditCommMessage msg);
        ResponeCommMessage HandleEvent(LoginCommMessage msg);
        ResponeCommMessage HandleEvent(RegisterCommMessage msg);
        ResponeCommMessage HandleEvent(SearchCommMessage msg);
        ResponeCommMessage HandleEvent(ChatCommMessage msg);
        ResponeCommMessage HandleEvent(ReplayCommMessage msg); 
        ResponeCommMessage HandleEvent(UserStatisticsCommMessage msg);
        ResponeCommMessage HandleEvent(LeaderboardCommMessage msg);
        ResponeCommMessage HandleEvent(ReturnToGameAsPlayer msg);
        ResponeCommMessage HandleEvent(ReturnToGameAsSpec msg);
        //server to client:
        ResponeCommMessage HandleEvent(GameDataCommMessage msg);
        ResponeCommMessage HandleEvent(ResponeCommMessage msg);
        ResponeCommMessage HandleEvent(CreateNewRoomMessage msg);

    }
}
