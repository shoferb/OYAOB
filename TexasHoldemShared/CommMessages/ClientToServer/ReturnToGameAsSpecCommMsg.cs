﻿using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReturnToGameAsSpecCommMsg : ReturnToGameCommMsg
    {
        public ReturnToGameAsSpecCommMsg() : base(-1, -1) { }

        public ReturnToGameAsSpecCommMsg(int id, long sid, int roomId) : base(id, sid)
        {
            RoomId = roomId;
        }

        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        //public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        //{
        //    notifier.Notify(response.OriginalMsg, response);
        //}

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(ReturnToGameAsSpecCommMsg))
            {
                var afterCasting = (ReturnToGameAsSpecCommMsg)other;
                return RoomId == afterCasting.RoomId && UserId == afterCasting.UserId;
            }
            return false;
        }
    }
}
