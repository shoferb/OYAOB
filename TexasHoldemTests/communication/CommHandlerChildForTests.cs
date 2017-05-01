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

        public TcpListener Listener
        {
            get { return _listener; }
        }

        public ConcurrentQueue<TcpClient> SocketsQueue
        {
            get { return _socketsQueue; }
        }

        public ConcurrentQueue<string> ReceivedMsgQueue
        {
            get { return _receivedMsgQueue; }
        }

        public IDictionary<int, ConcurrentQueue<string>> UserIdToMsgQueue
        {
            get { return _userIdToMsgQueue; }
        }

        public IDictionary<TcpClient, int> SocketToUserId
        {
            get { return _socketToUserId; }
        }

        public ManualResetEvent ConnectionCleanerMre
        {
            get { return _connectionCleanerMre; }
        }

        public IList<ManualResetEvent> ShutdownMreList
        {
            get { return _shutdownMreList; }
        }

        public Task<bool> Alldone()
        {
            int maxWorkers = -1;
            int maxIo = -1;
            int avWorkers = 0;
            int avIo = 0;

            while (maxWorkers - avWorkers != 1)
            {
                ThreadPool.GetMaxThreads(out maxWorkers, out maxIo);
                ThreadPool.GetMaxThreads(out avWorkers, out avIo);
            }

            return new Task<bool>(null);
        }
    }
}
