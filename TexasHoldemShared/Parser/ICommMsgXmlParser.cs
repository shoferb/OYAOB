using System;
using System.IO;
using System.Xml.Serialization;
namespace TexasHoldemShared.Parser
{
    public interface ICommMsgXmlParser
    {      

        string SerializeMsg(CommMessages.CommunicationMessage msg);
        CommMessages.CommunicationMessage ParseString(string msg); //TODO: parse to exact msg type (GameDataMessage for example)
    }
}
