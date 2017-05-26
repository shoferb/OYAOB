using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TexasHoldem.communication.Interfaces;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    class WebCommHandler : IWebCommHandler
    {
        private readonly HttpListener _listener;
        private bool _shouldStop = false;
        private static readonly string[] Prefixes = {"http://*:8080/"}; //TODO: maybe add more / change
        private readonly IWebEventHandler _eventHandler;

        public WebCommHandler(WebEventHandler eventHandler)
        {
            _listener = new HttpListener();
            _eventHandler = eventHandler;
            foreach (var prefix in Prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }
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
            var resultLst = _eventHandler.HandleRawMsg(msgStr);
            if (resultLst.Count > 1)
            {
                Console.WriteLine("WebCommHandler: msg contained more than one task");
            }
            else
            {
                var res = resultLst[0];
                var response = context.Response;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = bytes.Length;
                Stream output = response.OutputStream;
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
