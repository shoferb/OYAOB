using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    //TODO: add encryption here
    class WebEventHandler : IWebEventHandler
    {
        private readonly ICommMsgXmlParser _parser;
        private readonly ServerEventHandler _serverHandler;
        public WebEventHandler()
        {
            _parser = new ParserImplementation();
            _serverHandler = new ServerEventHandler {ShouldUseDelim = true};
        }

        public List<string> HandleRawMsg(string msg)
        {
            var parsedLst = _parser.ParseString(_parser.JsonToXml(msg), false);
            List<string> resultList = new List<string>();
            parsedLst.ForEach(commMsg =>
            {
                var xmlStr = commMsg.Handle(_serverHandler).Substring(1); //parse and remove the letter added by parder
                resultList.Add(_parser.XmlToJson(xmlStr));
            });

            return resultList;
        }
    }
}
