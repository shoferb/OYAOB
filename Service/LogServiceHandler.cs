using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    abstract class LogServiceHandler : ServiceHandler
    {
        public abstract bool SendNotification(User receiver, Notification toSend);
        public abstract bool Log(Log log);
        //TODO: more notification stuff
    }
}
