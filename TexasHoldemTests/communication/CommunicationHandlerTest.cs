using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moq;
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

        private CommHandlerChildForTests _commHandler;
        private TcpClient _client;
        private Thread _serverThread;

        [SetUp]
        public void SetUp()
        {
            _commHandler = new CommHandlerChildForTests(new ListenerSelector(), Port);
        }

        [TearDown]
        public void TearDown()
        {
            if (_commHandler != null && _commHandler.GetWasShutdown())
            {
                _commHandler.Close();
            }
            _commHandler = null;

            if (_client != null )
            {
                if (_client.Client != null && _client.Connected)
                {
                    _client.Close(); 
                }
                _client = null;
            }

                
        }

        private TcpClient ConnectSocketLoopback()
        {
            return new TcpClient(IPAddress.Loopback.ToString(), Port);
        }


        [TestCase]
        public void AcceptClientsTest()
        {
            var task = Task.Factory.StartNew(() => _commHandler.Start());
            _client = ConnectSocketLoopback();
            Assert.True(_client.Connected);

            _commHandler.Close();
            _client.Close();

            task.Wait();
            Assert.True(task.IsCompleted);
        }
        
        [TestCase]
        public void ReadingOneShortMsgTest()
        {
            var task = Task.Factory.StartNew(() => _commHandler.Start());
            _client = ConnectSocketLoopback();
            
            Assert.True(_client.Connected);

            var stream =_client.GetStream();
            Assert.True(stream.CanWrite);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ShortMsg); 
            stream.Write(bytes, 0, bytes.Length);

            _commHandler.Close();
            _client.Close();

            task.Wait();

            Assert.AreEqual(1, _commHandler.ReceivedMsgQueue.Count);
            Assert.True(task.IsCompleted);
        }
    }
}
