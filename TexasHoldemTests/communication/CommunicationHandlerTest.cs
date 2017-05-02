using System;
using System.Collections.Generic;
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

            CloseHandlerAndClient();

            task.Wait();
            Assert.True(task.IsCompleted);
        }

        private void SendMsgs(List<string> msgs)
        {
            Assert.IsNotNull(_client);
            Assert.True(_client.Connected);

            var stream = _client.GetStream();
            Assert.True(stream.CanWrite);

            msgs.ForEach(m =>
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m);
                stream.Write(bytes, 0, bytes.Length);
            });
        }

        private void CloseHandlerAndClient()
        {
            _commHandler.Close();
            _client.Close();
        }

        [TestCase]
        public void ReadingOneShortMsgTest()
        {
            var task = Task.Factory.StartNew(() => _commHandler.Start());
            _client = ConnectSocketLoopback();

            SendMsgs(new List<string> { ShortMsg });

            CloseHandlerAndClient();

            task.Wait();

            Assert.AreEqual(1, _commHandler.ReceivedMsgQueue.Count);
            string inQueue;
            _commHandler.ReceivedMsgQueue.TryDequeue(out inQueue);
            Assert.AreEqual(ShortMsg, inQueue);
            Assert.True(task.IsCompleted);
        }
        
        [TestCase]
        public void ReadingManyShortMsgTest()
        {
            const int numOfMsgs = 5;
            var task = Task.Factory.StartNew(() => _commHandler.Start());
            _client = ConnectSocketLoopback();

            List<string> msgs = new List<string>();
            for (int i = 0; i < numOfMsgs; i++)
            {
                msgs.Add(ShortMsg);
            }
            SendMsgs(msgs);

            Thread.Sleep(2000);

            CloseHandlerAndClient();

            task.Wait();

            int bytesCount = 0;
            var queue = _commHandler.ReceivedMsgQueue;
            foreach (string s in queue)
            {
                bytesCount += s.Length;
            }
            Assert.GreaterOrEqual(1, queue.Count);
            Assert.AreEqual(bytesCount, numOfMsgs * ShortMsg.Length);
            Assert.True(task.IsCompleted);
        }
    }
}
