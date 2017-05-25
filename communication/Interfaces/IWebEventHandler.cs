using System.Collections.Generic;

namespace TexasHoldem.communication.Interfaces
{
    public interface IWebEventHandler
    {
        List<string> HandleRawMsg(string msg);
    }
}
