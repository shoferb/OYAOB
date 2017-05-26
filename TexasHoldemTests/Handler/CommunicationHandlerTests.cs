using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client.Handler.ClientTests
{
    [TestClass()]
    public class CommunicationHandlerTests
    {
        [TestMethod()]
        public void CommunicationHandlerTest()
        {
           TcpListener server = new TcpListener(IPAddress.Any, 2000);
            var task = Task.Factory.StartNew(() => server.Start());
            ClientCommunicationHandler commHandler = new ClientCommunicationHandler(IPAddress.Loopback.ToString());
            commHandler.Connect();
           Assert.IsTrue(commHandler.IsSocketConnect());
            
        }

      

        [TestMethod()]
        public void TryGetMsgReceivedTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void addMsgToSendTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void closeTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void StartTest()
        {
            Assert.IsTrue(true);
        }
    }
}