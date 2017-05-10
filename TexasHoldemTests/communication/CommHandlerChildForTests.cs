using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;

namespace TexasHoldemTests.communication
{
    internal class CommHandlerChildForTests : CommunicationHandler
    {
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

        public bool GetWasShutdown()
        {
            return ShouldClose && WasShutDown;
        }

        public void SetUserIdToSocket(int userId, TcpClient client)
        {
            _socketToUserId.Add(client, userId);
        }
    }
}
