using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Reactor.Impl;

namespace TexasHoldemTests.communication
{
    [TestFixture]
    public class CommunicationHandlerTest
    {
        private const int Port = 2000;

        private const string ShortMsg = "short";

        private readonly CommHandlerChildForTests _commHandler;
        private TcpClient _client;
        private Thread _serverThread;

        public CommunicationHandlerTest()
        {
            _commHandler = new CommHandlerChildForTests(new ListenerSelector(), Port);
        }

        [SetUp]
        public void SetUp()
        {
            //int worker;
            //int io;
            //ThreadPool.GetMaxThreads(out worker, out io);
            //int activeWorker;
            //int activeIo;
            //ThreadPool.GetAvailableThreads(out activeWorker, out activeIo);
            //Console.WriteLine(worker - activeWorker);

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
        
        [TestCase]
        public async Task<bool> ReadingOneShortMsgTest()
        {
            Assert.True(_client.Connected);

            var stream =_client.GetStream();
            Assert.True(stream.CanWrite);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ShortMsg); 
            stream.Write(bytes, 0, bytes.Length);
            _commHandler.Close();
            await _commHandler.Alldone();
            //Thread.Sleep(10000);

            Assert.AreEqual(1, _commHandler.ReceivedMsgQueue.Count);
            return true;
        }
    }
}
