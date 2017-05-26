using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
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
        private readonly ICommunicationHandler _commHandler;
        private GameCenter gameCenter;
        private SystemControl system;
        private LogControl logs;
        private ReplayManager replays;


        public MessageEventHandler(ICommunicationHandler comm, GameCenter gc, SystemControl sys, LogControl log, ReplayManager replay)
        {
            gameCenter = gc;
            system = sys;
            logs = log;
            replays = replay;
            _parser = new ParserImplementation();
            _userIdToEventHandlerMap = new ConcurrentDictionary<int, IEventHandler>();
            _commHandler = comm;
        }

        public void HandleIncomingMsgs()
        {

            while (!_shouldStop)
            {
                var allMsgs = _commHandler.GetReceivedMessages();
                if (allMsgs.Count == 0)
                {
                    Thread.Sleep(25);
                }
                else
                {
                    Console.WriteLine("event handler got msg");
                    allMsgs.ForEach(HandleRawMsgs); 
                }
            }
        }

        private void HandleRawMsgs(Tuple<string, TcpClient> msg)
        {
            string data = msg.Item1;
            var parsedMsgs = _parser.ParseString(data, true);
            parsedMsgs.ForEach(m => HandleSingleRawMsg(m, msg.Item2));
        }

        private void HandleSingleRawMsg(CommunicationMessage parsedMsg, TcpClient tcpClient)
        {
            int userId = parsedMsg.UserId;
            if (!_userIdToEventHandlerMap.ContainsKey(userId))
            {
                ServerEventHandler handler = new ServerEventHandler(tcpClient, gameCenter, system, logs, replays, _commHandler);
                _userIdToEventHandlerMap.TryAdd(userId, handler);
            }

            //call to visitor pattern
            var res = parsedMsg.Handle(_userIdToEventHandlerMap[userId]);
            _commHandler.AddMsgToSend(res, userId);
        }
    }
}