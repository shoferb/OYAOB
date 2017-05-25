using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public interface IEventHandler
    {
        //client to server:
        string HandleEvent(ActionCommMessage msg);
        string HandleEvent(EditCommMessage msg);
        string HandleEvent(LoginCommMessage msg);
        string HandleEvent(RegisterCommMessage msg);
        string HandleEvent(SearchCommMessage msg);
        string HandleEvent(ChatCommMessage msg);
        string HandleEvent(ReplayCommMessage msg); 
        string HandleEvent(UserStatisticsCommMessage msg);
        string HandleEvent(LeaderboardCommMessage msg);
        //server to client:
        string HandleEvent(GameDataCommMessage msg);
        string HandleEvent(ResponeCommMessage msg);
        string HandleEvent(CreateNewRoomMessage msg);

    }
}
