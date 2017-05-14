﻿namespace TexasHoldemShared.CommMessages
{
    public abstract class CommunicationMessage
    {
        public enum ActionType
        {
            Fold,
            Bet,
            //check = Bet with amount 0
            //Call =
            //Raise =
            Join,
            Leave,
            Spectate,
            StartGame,
            HandCard,
        }

        //TODO: add fields here
        public int UserId;

        protected CommunicationMessage(int id)
        {
            UserId = id;
        }

        public abstract void Handle(IEventHandler handler);
    }
}
