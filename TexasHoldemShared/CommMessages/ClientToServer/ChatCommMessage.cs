using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ClientToServer
{
    public class ChatCommMessage : CommunicationMessage
    {
        public override void Handle(IEventHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
