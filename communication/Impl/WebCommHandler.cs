using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.Parser;
using System.Web;
using TexasHoldem.Service;

namespace TexasHoldem.communication.Impl
{
    public class WebCommHandler : IWebCommHandler
    {
        private readonly HttpListener _listener;
        private bool _shouldStop = false;
        private static readonly string[] Prefixes = {"http://*:8080/", "http://127.0.0.1:8080/"};
        private readonly IWebEventHandler _eventHandler;
        private readonly ConcurrentQueue<HttpListenerContext> _receivedContextsQueue; //for tests
        private readonly ConcurrentQueue<string> _resultsQueue; //for tests

        public WebCommHandler(WebEventHandler eventHandler)
        {
            _listener = new HttpListener();
            _eventHandler = eventHandler;
            foreach (var prefix in Prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }

            _receivedContextsQueue = new ConcurrentQueue<HttpListenerContext>();
            _resultsQueue = new ConcurrentQueue<string>();
        }

        public ConcurrentQueue<HttpListenerContext> GetReceivedContexts()
        {
            return _receivedContextsQueue;
        }

        public ConcurrentQueue<string> GetResults()
        {
            return _resultsQueue;
        }

        private void Accept()
        {
            while (!_shouldStop)
            {
                try
                {
                    Console.WriteLine("webCommHandler started");
                    HttpListenerContext context = _listener.GetContext(); //blocks
                    Console.WriteLine("got msg");
                    Task.Factory.StartNew(() => HandleContext(context));
                }
                catch (Exception e)
                {
                    Console.WriteLine("WebCommHandler::Accept _listener exception: " + e.Message);
                }
            }
        }

        private List<string> HandleContext(HttpListenerContext context)
        {
            var request = context.Request;
            var msgStr = new StreamReader(request.InputStream,
                context.Request.ContentEncoding).ReadToEnd();
            Console.WriteLine("received msg is: " + msgStr);
            request.InputStream.Close();
            byte[] msgbytes = Encoding.UTF8.GetBytes(msgStr);
            //msgStr = _security.Decrypt(msgbytes); //decrypt received msg
            //msgStr = _security.Decrypt(msgbytes); //decrypt received msg
            var resultLst = _eventHandler.HandleRawMsg(msgStr); //handle the incoming msg
            if (resultLst.Count > 1)
            {
                Console.WriteLine("WebCommHandler: msg contained more than one task");
            }
            else
            {
                var res = resultLst[0];
                res = res.Substring(1);
                var listenerResponse = context.Response;
                listenerResponse.AddHeader("Access-Control-Allow-Headers", "*");
                listenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
                listenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                listenerResponse.AddHeader("Access-Control-Max-Age", "86400");


                //var bytes = _security.Encrypt(res);
                var bytes = Encoding.UTF8.GetBytes(res);
                listenerResponse.ContentLength64 = bytes.Length;
                Stream output = listenerResponse.OutputStream;
                output.Write(bytes, 0, bytes.Length);
                output.Close();
            }
            return resultLst;
        }

        public void Start()
        {
            _listener.Start();
            Accept();
        }

        public void Close()
        {
            _shouldStop = true;
            _listener.Stop();
        }
    }
}
