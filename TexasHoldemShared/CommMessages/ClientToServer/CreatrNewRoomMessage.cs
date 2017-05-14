using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class CreatrNewRoomMessage : CommunicationMessage
    {
        public GameMode _mode;
        public int _minBet;
        public int _chipPolicy;
        public int _buyInPolicy;
        public bool _canSpectate;
        public int _minPlayer;
        public int _maxPlayers;

        public CreatrNewRoomMessage() : base(-1) { } //for parsing

        public CreatrNewRoomMessage(int id, GameMode mode, int minBet, int chipPol, int buyInPol, bool canSpec, int minPlayers, int maxPlayers) : base(id)
        {
            this._mode = mode;
            this._minBet = minBet;
            this._buyInPolicy = buyInPol;
            this._chipPolicy = chipPol;
            this._minPlayer = minPlayers;
            this._maxPlayers = maxPlayers;
            this._canSpectate = canSpec;
        }

        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }

        public override bool Equals(CommunicationMessage other)
        {
            if (other.GetType() == typeof(CreatrNewRoomMessage))
            {
                var afterCasting = (CreatrNewRoomMessage)other;
                return _mode == afterCasting._mode && _minBet == afterCasting._minBet && UserId == afterCasting.UserId &&
                       _chipPolicy == afterCasting._chipPolicy && _buyInPolicy == afterCasting._buyInPolicy && 
                       _canSpectate == afterCasting._canSpectate && _minPlayer == afterCasting._minPlayer && 
                       _maxPlayers == afterCasting._maxPlayers;
            }
            return false;
        }
    }
}
