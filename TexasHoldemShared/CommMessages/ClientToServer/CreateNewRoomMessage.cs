using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class CreateNewRoomMessage : CommunicationMessage
    {
        public GameMode _mode;
        public int _minBet;
        public int _chipPolicy;
        public int _buyInPolicy;
        public bool _canSpectate;
        public int _minPlayer;
        public int _maxPlayers;

        public CreateNewRoomMessage() : base(-1, -1) { } //for parsing

        public CreateNewRoomMessage(int id, long sid, GameMode mode, int minBet, int chipPol, 
            int buyInPol, bool canSpec, int minPlayers, int maxPlayers) : base(id, sid)
        {
            this._mode = mode;
            this._minBet = minBet;
            this._buyInPolicy = buyInPol;
            this._chipPolicy = chipPol;
            this._minPlayer = minPlayers;
            this._maxPlayers = maxPlayers;
            this._canSpectate = canSpec;
        }

        public override ResponeCommMessage Handle(IEventHandler handler)
        {
            return handler.HandleEvent(this);
        }

        public override void Notify(IResponseNotifier notifier, ResponeCommMessage response)
        {
            notifier.Notify(response.OriginalMsg, response);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other != null && other.GetType() == typeof(CreateNewRoomMessage))
            {
                var afterCasting = (CreateNewRoomMessage)other;
                return _mode == afterCasting._mode && _minBet == afterCasting._minBet && UserId == afterCasting.UserId &&
                       _chipPolicy == afterCasting._chipPolicy && _buyInPolicy == afterCasting._buyInPolicy && 
                       _canSpectate == afterCasting._canSpectate && _minPlayer == afterCasting._minPlayer && 
                       _maxPlayers == afterCasting._maxPlayers;
            }
            return false;
        }
    }
}
