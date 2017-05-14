using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ChatResponceCommMessage : ResponeCommMessage
    {
        int idReciver;
        string senderngUsername;
        ActionType chatType;
        string msgToSend;


    }
}
