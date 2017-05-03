using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.communication.Impl
{
    //TODO: this class
    class ServerEventHandler : IEventHandler
    {
        public void HandleEvent(ActionCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(EditCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(MoveOptionsCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            throw new System.NotImplementedException();
        }
    }
}
