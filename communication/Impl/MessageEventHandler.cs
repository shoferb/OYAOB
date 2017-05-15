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
                  //  Thread.Sleep(3000);
                    Console.WriteLine("Im here event");
                    allMsgs.ForEach(HandleRawMsgs); 
                }
            }
        }

        private void HandleRawMsgs(Tuple<string, TcpClient> msg)
        {
            Console.WriteLine("Im here HandleRawMsgs");

            string data = msg.Item1;
            var parsedMsgs = _parser.ParseString(data);
            parsedMsgs.ForEach(m => HandleSingleRawMsg(m, msg.Item2));
        }

        private void HandleSingleRawMsg(CommunicationMessage parsedMsg, TcpClient tcpClient)
        {
            Console.WriteLine("HandleSingleRawMsg");
            int userId = parsedMsg.UserId;
            if (_userIdToEventHandlerMap.ContainsKey(userId))
            {
                Console.WriteLine("calling visiotr pattern");
                //call to visitor pattern
                parsedMsg.Handle(_userIdToEventHandlerMap[userId]);
            }
            else
            {
                Console.WriteLine("just init handler");
                ServerEventHandler handler = new ServerEventHandler(tcpClient);
                Console.WriteLine("try adding to queue.....");
                _userIdToEventHandlerMap.TryAdd(userId, handler);
                Console.WriteLine("parsing");
                //call to visitor
                parsedMsg.Handle(handler);
            }
        }
    }
}