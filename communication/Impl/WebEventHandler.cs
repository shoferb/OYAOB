using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    //TODO: add encryption here
    public class WebEventHandler : IWebEventHandler
    {
        private readonly ICommMsgXmlParser _parser;
        private readonly ServerEventHandler _serverHandler;
        public WebEventHandler(ServerEventHandler serverHandler)
        {
            _serverHandler = serverHandler;
            _parser = new ParserImplementation();
        }

        public List<string> HandleRawMsg(string msg)
        {
            var parsedLst = _parser.ParseString(_parser.JsonToXml(msg), false);
            List<string> resultList = new List<string>();
            parsedLst.ForEach(commMsg =>
            {
                var xmlStr = commMsg.Handle(_serverHandler);
                resultList.Add(_parser.XmlToJson(xmlStr));
            });

            return resultList;
        }
    }
}
