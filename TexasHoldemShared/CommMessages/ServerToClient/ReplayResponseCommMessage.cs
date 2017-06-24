using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.CommMessages.ServerToClient
{
    public class ReplayResponseCommMessage : ResponeCommMessage
    {
        public string replay;
        public ReplayResponseCommMessage() : base(-1){ }//for parsing
          
        public ReplayResponseCommMessage(string wishedRep, long sid, int id, bool success, CommunicationMessage originalMsg) : base(id, sid, success, originalMsg)
        {
            this.replay = wishedRep;
        }
    }
}
