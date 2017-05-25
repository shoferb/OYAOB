using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    class WebEventHandler : IWebEventHandler
    {
        private readonly ICommMsgXmlParser _parser = new ParserImplementation();

        public List<string> HandleRawMsg(string msg)
        {
            var parsedLst = _parser.ParseString(msg);
            List<string> resultList = new List<string>();
            ServerEventHandler serverHandler = new ServerEventHandler();
            parsedLst.ForEach(commMsg => resultList.Add(commMsg.Handle(serverHandler)));

            return resultList;
        }
    }
}
