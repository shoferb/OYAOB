using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ChatCommMessage : CommunicationMessage
    {
        int idSender;
        int roomId;
        string ReciverUsername;
        string msgToSend;
        ActionType chatType;

        public override void Handle(IEventHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
