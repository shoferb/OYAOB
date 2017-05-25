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
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ChatResponceCommMessage))
            {
                var afterCasting = (ChatResponceCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId && idReciver == afterCasting.idReciver && 
                       roomId == afterCasting.roomId && senderngUsername.Equals(afterCasting.senderngUsername) &&
                       chatType == afterCasting.chatType;
            }
            return false;
        }
    }
}
