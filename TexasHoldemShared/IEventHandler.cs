﻿using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared
{
    public interface IEventHandler
    {
        //client to server:
        void HandleEvent(ActionCommMessage msg);
        void HandleEvent(EditCommMessage msg);
        void HandleEvent(LoginCommMessage msg);
        void HandleEvent(RegisterCommMessage msg);
        void HandleEvent(SearchCommMessage msg);

        //server to client:
        void HandleEvent(GameDataCommMessage msg);
        void HandleEvent(ResponeCommMessage msg);

    }
}
