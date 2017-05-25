using System.Collections.Generic;
using TexasHoldemShared.CommMessages;

namespace TexasHoldemShared.Parser
{
    public interface ICommMsgXmlParser
    {      

        string SerializeMsg(CommunicationMessage msg, bool addDelimiter);
        List<CommunicationMessage> ParseString(string msg, bool removeDelimiter);
        string AddDelimiter(string msg);
        string[] SeperateByDelimiter(string msg);
    }
}
