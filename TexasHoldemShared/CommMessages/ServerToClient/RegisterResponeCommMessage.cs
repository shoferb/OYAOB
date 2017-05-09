using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class RegisterResponeCommMessage : ResponeCommMessage
    {
        //todo - this class is te message responce we get when register 
        public RegisterResponeCommMessage(int id, bool success, CommunicationMessage originalMsg) : base(id, success, originalMsg)
        {
        }

        //visitor pattern
        public override void Handle(IEventHandler handler)
        {
            handler.HandleEvent(this);
        }
    }
}
