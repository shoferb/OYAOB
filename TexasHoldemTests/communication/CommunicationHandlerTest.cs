//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
////using Moq;
//using NUnit.Framework;
//using TexasHoldem.communication.Impl;

//namespace TexasHoldemTests.communication
//{
//    [TestFixture]
//    public class CommunicationHandlerTest
//    {
//        private const int Port = 2000;
//        private const int UserId = 1;

//        private const string ShortMsg = "short";

//        private CommHandlerChildForTests _commHandler;
//        private TcpClient _client;
//        private Thread _serverThread;

//        [SetUp]
//        public void SetUp()
//        {
//            _commHandler = new CommHandlerChildForTests();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            if (_commHandler != null && _commHandler.GetWasShutdown())
//            {
//                _commHandler.Close();
//            }
//            _commHandler = null;

//            if (_client != null )
//            {
//                if (_client.Client != null && _client.Connected)
//                {
//                    _client.Close(); 
//                }
//                _client = null;
//            }

                
//        }

//        private TcpClient ConnectSocketLoopback()
//        {
//            return new TcpClient(IPAddress.Loopback.ToString(), Port);
//        }


//        [TestCase]
//        public void AcceptClientsTest()
//        {
//            var task = Task.Factory.StartNew(() => _commHandler.Start());
//            _client = ConnectSocketLoopback();
//            Assert.True(_client.Connected);

//            CloseHandlerAndClient();

//            task.Wait();
//            Assert.True(task.IsCompleted);
//        }

//        private void SendMsgs(List<string> msgs)
//        {
//            Assert.IsNotNull(_client);
//            Assert.True(_client.Connected);

//            var stream = _client.GetStream();
//            Assert.True(stream.CanWrite);

//            msgs.ForEach(m =>
//            {
//                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m);
//                stream.Write(bytes, 0, bytes.Length);
//            });
//        }

//        private void CloseHandlerAndClient()
//        {
//            _commHandler.Close();
//            _client.Close();
//        }

//        [TestCase]
//        public void ReadingOneShortMsgTest()
//        {
//            var task = Task.Factory.StartNew(() => _commHandler.Start());
//            _client = ConnectSocketLoopback();

//            SendMsgs(new List<string> { ShortMsg });

//            CloseHandlerAndClient();

//            task.Wait();

//            Assert.AreEqual(1, _commHandler.ReceivedMsgQueue.Count);
//            string inQueue;
//            _commHandler.ReceivedMsgQueue.TryDequeue(out inQueue);
//            Assert.AreEqual(ShortMsg, inQueue);
//            Assert.True(task.IsCompleted);
//        }
        
//        [TestCase]
//        public void ReadingManyShortMsgTest()
//        {
//            const int numOfMsgs = 5;
//            var task = Task.Factory.StartNew(() => _commHandler.Start());
//            _client = ConnectSocketLoopback();

//            List<string> msgs = new List<string>();
//            for (int i = 0; i < numOfMsgs; i++)
//            {
//                msgs.Add(ShortMsg);
//            }
//            SendMsgs(msgs);

//            Thread.Sleep(2000);

//            CloseHandlerAndClient();

//            task.Wait();

//            int bytesCount = 0;
//            var queue = _commHandler.ReceivedMsgQueue;
//            foreach (string s in queue)
//            {
//                bytesCount += s.Length;
//            }
//            Assert.GreaterOrEqual(1, queue.Count);
//            Assert.AreEqual(bytesCount, numOfMsgs * ShortMsg.Length);
//            Assert.True(task.IsCompleted);
//        }

//        [TestCase]
//        public void SendingOneShortMsgTest()
//        {
//            var task = Task.Factory.StartNew(() => _commHandler.Start());
//            _client = ConnectSocketLoopback();

//            TcpClient socketAtServer = null;

//            //wait for server to connet to client
//            while (socketAtServer == null)
//            {
//                _commHandler.SocketsQueue.TryPeek(out socketAtServer); 
//            }

//            Assert.IsNotNull(socketAtServer);
//            _commHandler.SetUserIdToSocket(UserId, socketAtServer);

//            bool sent = _commHandler.AddMsgToSend(ShortMsg, UserId);
//            Assert.True(sent);

//            var msgQueue = _commHandler.UserIdToMsgQueue[UserId];
//            Assert.IsNotNull(msgQueue);

//            //wait for server to take the msg from the queue:
//            while (!msgQueue.IsEmpty)
//            {
//                Thread.Sleep(200);
//            }

//            string msg = ClientRead(2000);

//            Assert.AreEqual(ShortMsg, msg);

//            CloseHandlerAndClient();

//            task.Wait();
//            Assert.True(task.IsCompleted);
//        }
        
//        [TestCase]
//        public void SendingManyShortMsgTest()
//        {
//            const int numOfMsgs = 5;

//            var task = Task.Factory.StartNew(() => _commHandler.Start());
//            _client = ConnectSocketLoopback();

//            TcpClient socketAtServer; 
//            _commHandler.SocketsQueue.TryPeek(out socketAtServer);

//            Assert.IsNotNull(socketAtServer);
//            _commHandler.SetUserIdToSocket(UserId, socketAtServer);

//            for (int i = 0; i < numOfMsgs; i++)
//            {
//                bool sent = _commHandler.AddMsgToSend(ShortMsg, UserId);
//                Assert.True(sent); 
//            }

//            var msgQueue = _commHandler.UserIdToMsgQueue[UserId];
//            Assert.IsNotNull(msgQueue);


//            //wait for server to take the msg from the queue:
//            while (!msgQueue.IsEmpty)
//            {
//                Thread.Sleep(200);
//            }

//            int bytesRead = 0;
//            bool stillGotMsgs = true;
//            while (stillGotMsgs)
//            {
//                try
//                {
//                    string msg = ClientRead(2000);
//                    bytesRead += msg.Length;
//                }
//                catch (Exception)
//                {
//                    stillGotMsgs = false;
//                }
//            }

//            Assert.AreEqual(bytesRead, numOfMsgs * ShortMsg.Length);

//            CloseHandlerAndClient();

//            task.Wait();
//            Assert.True(task.IsCompleted);
//        }

//        private string ClientRead(int timeOutMili)
//        {
//            _client.ReceiveTimeout = timeOutMili;
//            byte[] buffer = new byte[1];
//            IList<byte> data = new List<byte>();
//            NetworkStream stream = _client.GetStream();

//            try
//            {
//                var dataReceived = 0;
//                do
//                {
//                    dataReceived = stream.Read(buffer, 0, 1);
//                    if (dataReceived > 0)
//                    {
//                        data.Add(buffer[0]);
//                    }

//                } while (dataReceived > 0);
//            }
//            catch (IOException e)
//            {
                
//                //_client timeout was reached
//            }

//            if (data.Count > 0)
//            {
//                return Encoding.UTF8.GetString(data.ToArray());
//            }

//            throw new Exception("no data was read by _client");
//        }
//    }
//}
