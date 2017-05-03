using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.communication.Interfaces
{
    public interface IEventHandler
    {
        void HandleEvent(ActionCommMessage msg);
        void HandleEvent(EditCommMessage msg);
        void HandleEvent(LoginCommMessage msg);
        void HandleEvent(RegisterCommMessage msg);
    }
}
