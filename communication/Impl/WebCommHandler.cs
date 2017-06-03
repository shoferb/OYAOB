using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Parser;
using TexasHoldemShared.Security;

namespace TexasHoldem.communication.Impl
{
    public class WebCommHandler : IWebCommHandler
    {
        private readonly HttpListener _listener;
        private bool _shouldStop = false;
        private static readonly string[] Prefixes = {/*"http://*:8080/",*/ "http://127.0.0.1:8080/"}; //TODO: maybe add more / change
        private readonly IWebEventHandler _eventHandler;
        private readonly ISecurity _security = new SecurityHandler();
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
                    HttpListenerContext context = _listener.GetContext(); //blocks
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
            request.InputStream.Close();
            //byte[] msgbytes = _security.Encrypt(msgStr);
            //msgStr = _security.Decrypt(msgbytes); //decrypt received msg
            var resultLst = _eventHandler.HandleRawMsg(msgStr); //handle the incoming msg
            if (resultLst.Count > 1)
            {
                Console.WriteLine("WebCommHandler: msg contained more than one task");
            }
            else
            {
                var res = resultLst[0];
                var listenerResponse = context.Response;
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
