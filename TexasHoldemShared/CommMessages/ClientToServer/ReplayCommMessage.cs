using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ReplayCommMessage : CommunicationMessage
    {
        public int gameId;
        public bool isAllGames;


        public ReplayCommMessage() : base(-1) { } //for parsing

        public ReplayCommMessage(int _userid, bool _isAll, int _gameId ) : base(_userid)
        {
            this.gameId = _gameId;
            this.isAllGames = _isAll;
        }



        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
