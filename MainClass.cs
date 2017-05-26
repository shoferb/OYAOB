using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;

namespace TexasHoldem
{
    public class MainClass
    {
        public static void Main()
        {
            //init instances:
            ICommunicationHandler commHandler = new CommunicationHandler();
            LogControl logControl = new LogControl();
            SystemControl sysControl = new SystemControl(logControl);
            GameCenter gameCenter = new GameCenter(sysControl, logControl);

            Task commTask = Task.Factory.StartNew(commHandler.Start);
            Console.WriteLine("starting comm");
            MessageEventHandler eventHandler = new MessageEventHandler();
            Task eventTask = Task.Factory.StartNew(eventHandler.HandleIncomingMsgs);
            commTask.Wait();
        }
    }
}
