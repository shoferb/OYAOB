using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.communication.Interfaces;
using TexasHoldem.Logic.Game_Control;

namespace TexasHoldem
{
    public class MainClass
    {
        public static void Main()
        {
            //init instances:
            ICommunicationHandler commHandler = CommunicationHandler.GetInstance();
            GameCenter gameCenter = GameCenter.Instance;
            SystemControl sysControl = SystemControl.SystemControlInstance;

            Task commTask = Task.Factory.StartNew(commHandler.Start);
            Console.WriteLine("starting comm");
            commTask.Wait();
        }
    }
}
