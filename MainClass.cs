using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;

namespace TexasHoldem
{
    public class MainClass
    {
        public static void Main()
        {
            //init instances:
            LogControl logControl = new LogControl();
            SystemControl sysControl = new SystemControl(logControl);
            ReplayManager replayManager = new ReplayManager();
            GameCenter gameCenter = new GameCenter(sysControl, logControl, replayManager);
            var commHandler = CommunicationHandler.GetInstance();
            MessageEventHandler eventHandler = new MessageEventHandler(gameCenter, sysControl, logControl, replayManager);
            gameCenter.SetMessageHandler(eventHandler);
            var webEventHandler = new WebEventHandler(new ServerEventHandler(eventHandler, null, 
                gameCenter, sysControl, logControl, replayManager, null));
            WebCommHandler webCommHandler = new WebCommHandler(webEventHandler);
            Task commTask = Task.Factory.StartNew(commHandler.Start);
            Task webCommTask = Task.Factory.StartNew(webCommHandler.Start);
            Console.WriteLine("starting comm");
            Task eventTask = Task.Factory.StartNew(eventHandler.HandleIncomingMsgs);
            commTask.Wait();
            webCommTask.Wait();
        }
    }
}
