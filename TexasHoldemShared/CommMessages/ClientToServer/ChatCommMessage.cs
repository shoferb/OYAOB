using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ChatCommMessage : CommunicationMessage
    {
        public int idSender;
        public int roomId;
        public string ReciverUsername;
        public string msgToSend;
        public ActionType chatType;


        public ChatCommMessage() : base(-1){ }//for parsing


        public ChatCommMessage(int _idSender, int _roomId, string _ReciverUsername, string _msgToSend,
      ActionType _chatType) : base(_idSender)
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

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(ChatCommMessage))
            {
                var afterCasting = (ChatCommMessage)other;
                return idSender == afterCasting.idSender && roomId == afterCasting.roomId && UserId == afterCasting.UserId &&
                    chatType == afterCasting.chatType && msgToSend.Equals(afterCasting.msgToSend);
            }
            return false;
        }
    }
}
