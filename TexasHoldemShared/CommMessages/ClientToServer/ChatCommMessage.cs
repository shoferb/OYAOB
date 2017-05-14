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


        public ChatCommMessage() : base(-1){ }//for parsing


        public ChatCommMessage(int _idSender, int _roomId, string _ReciverUsername, string _msgToSend,
      ActionType _chatType, int id) : base(id)
        {
            idSender = _idSender;
            roomId = _roomId;
            ReciverUsername = _ReciverUsername;
            msgToSend = _msgToSend;
            chatType = _chatType;

        }


        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
