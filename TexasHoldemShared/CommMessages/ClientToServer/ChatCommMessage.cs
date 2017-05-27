using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ChatCommMessage : CommunicationMessage
    {
        public int IdSender;
        public int RoomId;
        public string ReciverUsername;
        public string MsgToSend;
        public ActionType ChatType;


        public ChatCommMessage() : base(-1, -1){ }//for parsing


        public ChatCommMessage(int _idSender, int _roomId, long sid, string _ReciverUsername, 
            string _msgToSend, ActionType _chatType) : base(_idSender, sid)
        {
            IdSender = _idSender;
            RoomId = _roomId;
            ReciverUsername = _ReciverUsername;
            MsgToSend = _msgToSend;
            ChatType = _chatType;

        }


        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ChatCommMessage))
            {
                var afterCasting = (ChatCommMessage)other;
                return IdSender == afterCasting.IdSender && RoomId == afterCasting.RoomId && UserId == afterCasting.UserId &&
                    ChatType == afterCasting.ChatType && MsgToSend.Equals(afterCasting.MsgToSend);
            }
            return false;
        }
    }
}
