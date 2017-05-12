using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class CreatrNewRoomMessage : CommunicationMessage
    {
        private GameMode _mode;
        private int _minBet;
        private int _chipPolicy;
        private int _buyInPolicy;
        private bool _canSpectate;
        private int _minPlayer;
        private int _maxPlayers;

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
    }
}
