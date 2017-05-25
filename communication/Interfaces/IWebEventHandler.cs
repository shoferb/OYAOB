using System.Collections.Generic;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.communication.Interfaces
{
    public interface IWebEventHandler
    {
        List<string> HandleRawMsg(string msg);

    }
}
