using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Reactor.Impl;

namespace TexasHoldemTests.communication
{
    [TestFixture]
    public class CommunicationHandlerTest
    {
        private readonly CommHandlerChildForTests _commHandler;
        private const int Port = 2000;
        private TcpClient _client;
        private Thread _serverThread;

        public CommunicationHandlerTest()
        {
            _commHandler = new CommHandlerChildForTests(new ListenerSelector(), Port);
        }

        [SetUp]
        public void SetUp()
        {
            _serverThread = new Thread(_commHandler.Start);
            _serverThread.Start();
            //Thread.Sleep(2000);
            _client = ConnectSocketLoopback();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Close();
            _commHandler.Close();
            _serverThread.Join();
        }

        private TcpClient ConnectSocketLoopback()
        {
            return new TcpClient(IPAddress.Loopback.ToString(), Port);
        }


        [TestCase]
        public void AcceptClientsTest()
        {
            Assert.True(_client.Connected);
        }
    }
}
