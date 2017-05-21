using System;
using System.Collections.Concurrent;
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
        private static readonly string[] _prefixes = {"http://*:8080/"}; //TODO: maybe add more / change
        private ICommMsgXmlParser _parser = new ParserImplementation();

        public WebCommHandler()
        {
            _listener = new HttpListener();
            foreach (var prefix in _prefixes)
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

        private string HandleContext(HttpListenerContext context)
        {
            var request = context.Request;
            var msgStr = new StreamReader(request.InputStream,
                context.Request.ContentEncoding).ReadToEnd();
            //TODO: handle msg with ServerEventHandler here
            return "";
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
