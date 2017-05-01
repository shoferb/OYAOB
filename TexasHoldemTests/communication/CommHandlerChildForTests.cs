using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Reactor.Interfaces;

namespace TexasHoldemTests.communication
{
    class CommHandlerChildForTests : CommunicationHandler
    {
        public CommHandlerChildForTests(IListenerSelector selector, int port) : base(selector, port)
        {
        }

        protected TcpListener Listener
        {
            get { return _listener; }
        }

        protected ConcurrentQueue<TcpClient> SocketsQueue
        {
            get { return _socketsQueue; }
        }

        protected ConcurrentQueue<string> ReceivedMsgQueue
        {
            get { return _receivedMsgQueue; }
        }

        protected IDictionary<int, ConcurrentQueue<string>> UserIdToMsgQueue
        {
            get { return _userIdToMsgQueue; }
        }

        protected IDictionary<TcpClient, int> SocketToUserId
        {
            get { return _socketToUserId; }
        }

        protected ManualResetEvent ConnectionCleanerMre
        {
            get { return _connectionCleanerMre; }
        }

        protected IList<ManualResetEvent> ShutdownMreList
        {
            get { return _shutdownMreList; }
        }


    }
}
