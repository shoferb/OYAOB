using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using TexasHoldemShared.Parser;
using TexasHoldemShared.Security;

namespace Client.Handler
{
    public class ClientCommunicationHandler
    {
        
        protected readonly ConcurrentQueue<string> _receivedMsgQueue;
        protected readonly ConcurrentQueue<string> _toSendMsgQueue;
        protected TcpClient _socket;
        protected NetworkStream _stream;
        protected readonly string _server;
        private bool _shouldClose;
        private readonly ISecurity _security = new SecurityHandler();

        public ClientCommunicationHandler(string server)
        {

            _receivedMsgQueue = new ConcurrentQueue<string>();
            _toSendMsgQueue = new ConcurrentQueue<string>();
            _server = server;
            _shouldClose = false;
          

        }
       
        public bool IsSocketConnect()//for testing
        {
            return this._socket.Connected;
        }
        public bool Connect()
        {
            try
            {
                // Create a TcpClient.
                // connects to the specified port on the specified host.
                Int32 port = 2000;
                _socket = new TcpClient(_server, port);
                // Get a client stream for reading and writing.
                _stream = _socket.GetStream();
                _socket.ReceiveTimeout = 5000;
                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return false;
            }
        }
        public string TryGetMsgReceived()
        {
            string msgToRet = string.Empty;
            _receivedMsgQueue.TryDequeue(out msgToRet);
            return msgToRet;
        }
        public void addMsgToSend(string msg)
        {
            _toSendMsgQueue.Enqueue(msg);
        }

        private void SendMessages()
        {
            while (!_shouldClose)
            {
                try
                {
                    string msg = string.Empty;
                    if (!_toSendMsgQueue.IsEmpty)
                    {
                        _toSendMsgQueue.TryDequeue(out msg);
                        // Translate the passed message into ASCII and store it as a Byte array.
                        byte[] data = Encoding.UTF8.GetBytes(msg);
                        string  s = Encoding.UTF8.GetString(data);
                        byte[] encrypted = _security.Encrypt(s);
                        s = Encoding.UTF8.GetString(encrypted);
                        s = _security.Decrypt(encrypted);
                        // Send the message to the connected TcpServer. 
                        _stream.Write(encrypted, 0, encrypted.Length);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);

                }
            }
        }

        //returns new msg as string, or empty string if there's no new msg
        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1];
            while (!_shouldClose)
            {
                if (_stream.DataAvailable)
                {
                    IList<byte> data = new List<byte>();
                    var dataReceived = 0;
                    do
                    {
                        try
                        {
                            dataReceived = _stream.Read(buffer, 0, 1);
                            if (dataReceived > 0)
                            {
                                data.Add(buffer[0]);
                            }
                        }
                        catch (Exception e)
                        {
                            dataReceived = 0;
                        }

                    } while (dataReceived > 0);

                    //add msg string to queue
                    if (data.Count > 0)
                    {
                        var decrypted = _security.Decrypt(data.ToArray());
                        _receivedMsgQueue.Enqueue(decrypted);
                    } 
                }
            }
          

        }

        public bool Close()
        {
            try
            {
                _shouldClose = true;
                _stream.Close();
                _socket.Close();


                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Start()
        {
            Thread task = new Thread(ReceiveMessages);
            task.SetApartmentState(ApartmentState.STA);          

            if (Connect())
            {
                task.Start();
                SendMessages();

            }
            else
            {
                Console.WriteLine("cant connect to server");
            }
        }
    }
}
