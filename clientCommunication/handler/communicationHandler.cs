using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Sockets;


namespace clientCommunication.handler
{
    class communicationHandler
    {
        protected readonly int _userId;
        protected readonly ConcurrentQueue<string> _receivedMsgQueue;
        protected readonly ConcurrentQueue<string> _toSendMsgQueue;
        protected TcpClient _socket;
        protected NetworkStream _stream;
        protected readonly string _server;
        protected ClientEventHandler handler;
        bool shouldClose;
        public communicationHandler(int id, string server)
        {
            _userId = id;
            _receivedMsgQueue = new ConcurrentQueue<string>();
            _toSendMsgQueue = new ConcurrentQueue<string>();
            _server = server;
            shouldClose = false;
            handler = new ClientEventHandler(id);
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
                return true;
            }
              catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                  return false;
            }}

        public void sendMessages()
        {
            while(!shouldClose)
            try{
                string msg = string.Empty;
              _toSendMsgQueue.TryDequeue(out msg);
                
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
              // Send the message to the connected TcpServer. 
                _stream.Write(data, 0, data.Length);
            
            } catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                
            }
        }
        

       
        //returns new msg as string, or eempty string if there's no new msg
        public void receiveMessages()
        {
             byte[] buffer = new byte[1];
             while(!shouldClose)
            {
               IList<byte> data = new List<byte>();
               var dataReceived = 0;
                    do
                    {
                        dataReceived = _stream.Read(buffer, 0, 1);
                        if (dataReceived > 0)
                        {
                            data.Add(buffer[0]);
                        }

                    } while (dataReceived > 0);

                    //add msg string to queue
                    if (data.Count > 0)
                    {
                        _receivedMsgQueue.Enqueue(Encoding.UTF8.GetString(data.ToArray()));
                    }
             }
             close();
             
        }
        //TODO
        public void handleMessages()
        {
            while (!shouldClose)
            {
                 string msg = string.Empty;
                 _receivedMsgQueue.TryDequeue(out msg);
                //parse msg
                //handle msg
                //put response in queue
            }

        }


        public bool close()
        {
            try
            {
                // Close everything.
                _stream.Close();
                _socket.Close();
            return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
           
        
 

        public void Start()
        {
            List<Task> taskList = new List<Task>
            {
                new Task(receiveMessages),
                new Task(sendMessages),
                new Task(handleMessages)
            };
            if(Connect())
            {
                taskList.ForEach(task => task.Start());
                
            }
            else{
                Console.WriteLine("cant connect to server");
            }
        }
        
    }
}
