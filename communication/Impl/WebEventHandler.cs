using System;
using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    //TODO: add encryption here
    public class WebEventHandler : SessionIdHandler, IWebEventHandler
    {
        private readonly ICommMsgXmlParser _parser;
        private readonly IEventHandler _serverHandler;
        public WebEventHandler(IEventHandler serverHandler)
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
                if (String.IsNullOrEmpty(xmlStr))
                {
                    resultList.Add(_parser.XmlToJson(xmlStr)); 
                }
                else
                {
                    Console.WriteLine("There was a problem with server event handler. got empty result.");
                }
            });

            return resultList;
        }
    }
}
