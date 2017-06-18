using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.Logic
{
    public interface IResponseNotifier
    {
        void Notify(ChatCommMessage originalMsg, ResponeCommMessage msg);
        void Notify(LoginCommMessage originalMsg, ResponeCommMessage msg);
        void Notify(RegisterCommMessage originalMsg, RegisterResponeCommMessage msg);
        void Notify(CreateNewRoomMessage originalMsg, ResponeCommMessage msg);
        void Notify(ReturnToGameAsPlayerCommMsg originalMsg, ResponeCommMessage msg);
        void Notify(ReturnToGameAsSpecCommMsg originalMsg, ResponeCommMessage msg);
        void Notify(SearchCommMessage originalMsg, ResponeCommMessage msg);
        void Notify(ActionCommMessage originalMsg, ResponeCommMessage msg);
        //void Notify(CommunicationMessage originalMsg, ResponeCommMessage msg); //default / else case
    }
}
