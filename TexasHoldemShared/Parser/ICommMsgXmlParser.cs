using System.Collections.Generic;
using TexasHoldemShared.CommMessages;

namespace TexasHoldemShared.Parser
{
    public interface ICommMsgXmlParser
    {      

        string SerializeMsg(CommunicationMessage msg);
        List<CommunicationMessage> ParseString(string msg);
        string AddDelimiter(string msg);
        string[] SeperateByDelimiter(string msg);
    }
}
