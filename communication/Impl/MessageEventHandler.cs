using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.Parser;

//takes msgs from msgQueue in CommHandler and deals with them using EventHandlers
namespace TexasHoldem.communication.Impl
{
    public class MessageEventHandler
    {
        private readonly ICommMsgXmlParser _parser;
        private readonly ConcurrentDictionary<int, IEventHandler> _userIdToEventHandlerMap;
        private bool _shouldStop = false;


        public MessageEventHandler()
        {
            _parser = new ParserImplementation();
            _userIdToEventHandlerMap = new ConcurrentDictionary<int, IEventHandler>();
        }

        public void HandleIncomingMsgs()
        {
            CommunicationHandler commHandler = CommunicationHandler.GetInstance();

            while (!_shouldStop)//TODO: busy wait maybe change this
            {
                var allMsgs = commHandler.GetReceivedMessages();
                if (allMsgs.Count == 0)
                {
                    Thread.Sleep(250);
                }
                else
                {
                    Console.WriteLine("event handler got msg");
                    allMsgs.ForEach(HandleSingleRawMsg); 
                }
            }
        }

        private void HandleSingleRawMsg(Tuple<string, TcpClient> msg)
        {
            string data = msg.Item1;
            CommunicationMessage parsedMsg = _parser.ParseString(data);
            int userId = parsedMsg.UserId;
            if (_userIdToEventHandlerMap.ContainsKey(userId))
            {
                //call to visitor pattern
                parsedMsg.Handle(_userIdToEventHandlerMap[userId]);
            }
            else
            {
                ServerEventHandler handler = new ServerEventHandler(msg.Item2);
                _userIdToEventHandlerMap.TryAdd(userId, handler);
                

                //call to visitor
                parsedMsg.Handle(handler);
            }
        }
    }
}