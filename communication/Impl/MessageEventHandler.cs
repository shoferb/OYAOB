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
using TexasHoldemShared.CommMessages.ServerToClient;
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
        private readonly GameCenter _gameCenter;
        private readonly SystemControl _system;
        private readonly LogControl _logs;
        private readonly ReplayManager _replays;
        private readonly SessionIdHandler _sidHandler;

        public MessageEventHandler(GameCenter gc, SystemControl sys, LogControl log, 
            ReplayManager replay, SessionIdHandler sidHandler)
        {
            _gameCenter = gc;
            _system = sys;
            _logs = log;
            _replays = replay;
            this._sidHandler = sidHandler;
            _parser = new ParserImplementation();
            _userIdToEventHandlerMap = new ConcurrentDictionary<int, IEventHandler>();
            _commHandler = CommunicationHandler.GetInstance();
        }

        public void HandleIncomingMsgs()
        {
            while (!_shouldStop)
            {
                var allMsgs = _commHandler.GetReceivedMessages();
                if (allMsgs.Count == 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Console.WriteLine("event handler got msg");
                    allMsgs.ForEach(HandleRawMsgs); 
                }
            }
        }

        public bool SendGameDataToClient(GameDataCommMessage data)
        {
            if (_userIdToEventHandlerMap.ContainsKey(data.UserId))
            {
                var serverHandler = _userIdToEventHandlerMap[data.UserId];
                serverHandler.HandleEvent(data);
                return true;
            }
            return false;
        }

        private void HandleRawMsgs(Tuple<string, TcpClient> msg)
        {
            string data = msg.Item1;
            var parsedMsgs = _parser.ParseString(data, true);
            parsedMsgs.ForEach(m =>
            {
                if (m != null)
                {
                    HandleSingleRawMsg(m, msg.Item2);
                }
            });          
        }

        private void HandleSingleRawMsg(CommunicationMessage parsedMsg, TcpClient tcpClient)
        {
            int userId = parsedMsg.UserId;
            if (!_userIdToEventHandlerMap.ContainsKey(userId))
            {
                ServerEventHandler handler = new ServerEventHandler(_sidHandler, tcpClient, 
                    _gameCenter, _system, _logs,_replays, _commHandler) {ShouldUseDelim = true};
                _userIdToEventHandlerMap.TryAdd(userId, handler);
            }

            //call to visitor pattern
            var res = parsedMsg.Handle(_userIdToEventHandlerMap[userId]);
            if (res != null)
            {
                var resStr = _parser.SerializeMsg(res, true);
                if (userId == -1) //user id is temporary, remove from queue
                {
                  //  _userIdToEventHandlerMap.TryRemove(userId, out IEventHandler handler);
                }
                if (!String.IsNullOrEmpty(resStr))
                {
                    _commHandler.AddMsgToSend(resStr, res.UserId);
                }
                else
                {
                    Console.WriteLine("Got an empty parsed string.");
                }
            }
            else
            {
                Console.WriteLine("There was a problem with server event handler. got empty result.");
            }
        }
    }
}