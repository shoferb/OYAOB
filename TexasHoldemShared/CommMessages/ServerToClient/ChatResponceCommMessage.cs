using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ChatResponceCommMessage : ResponeCommMessage
    {
       public  int idReciver;
        public string senderngUsername;
        public ActionType chatType;
        public string msgToSend;
        public int roomId;

        public ChatResponceCommMessage() : base(-1){ }//for parsing

        public ChatResponceCommMessage(int _roomId, int _idReciver, string _senderngUsername, ActionType _chatType ,string _msgToSend, int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
            idReciver = _idReciver;
            senderngUsername = _senderngUsername;
            chatType = _chatType;
            msgToSend = _msgToSend;
            roomId = _roomId;
        }
        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
