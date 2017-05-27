﻿namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ResponeCommMessage : CommunicationMessage
    {
        public bool Success;
        public CommunicationMessage OriginalMsg;

        public ResponeCommMessage() : base(-1, -1) { } //for parsing

        public ResponeCommMessage(int id) : base(id, -1)
        {
            
        }

        public ResponeCommMessage(int id, long sid, bool success, CommunicationMessage originalMsg) : base(id, sid)
        {
            Success = success;
            OriginalMsg = originalMsg;
        }

        //visitor pattern
        public override string Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ResponeCommMessage))
            {
                var afterCasting = (ResponeCommMessage)other;
                return Success == afterCasting.Success && OriginalMsg.Equals(afterCasting.OriginalMsg) &&
                       UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
