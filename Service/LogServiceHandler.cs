using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Service
{
    public abstract class LogServiceHandler : ServiceHandler
    {

        private SystemControl sc = new SystemControl();
        

        public Log CreateNewLog()
        {
            Log toReturn = new Log();
            Console.WriteLine("new log was created, log id is: "+ toReturn.LogId);
            return toReturn;
        }

        public ErrorLog CreateNewErrorLog(string erroeMsg)
        {
            ErrorLog toReturn = new ErrorLog(erroeMsg);
            Console.WriteLine("new error log was created, log id is: " + toReturn.LogId);
            return toReturn;
        }

        public SystemLog CreateNewSystemLog(int roomId, string errorMsg)
        {
            SystemLog toReturn = new SystemLog(roomId,errorMsg);
            Console.WriteLine("new error log was created, log id is: " + toReturn.LogId);
            return toReturn;
        }

        public string GetMsgErrorLog(ErrorLog log)
        {
            string toReturn = log.Msg;
            return toReturn;
        }

        public void PrintMsgErrorLog(ErrorLog log)
        {

            string toReturn = log.Msg;
            Console.WriteLine("The massage of errir log with id: " + log.LogId + "is: " + toReturn);
            
        }

        public string GetMsgSystemLog(SystemLog log)
        {
            string toReturn = log.Msg;
            return toReturn;
        }

        public void PrintMsgSystemLog(SystemLog log)
        {
            string toReturn = log.Msg;
            Console.WriteLine("The massage of system log with id: " + log.LogId + "is: " + toReturn);

        }

        public void PrintLogInfo(Log log)
        {
            Console.WriteLine(log.ToString());
        }

        public Notification CreateNotification(int roomId, string msg)
        {
            Notification toReturn = new Notification(roomId,msg);
            return toReturn;
        }

        public bool SendNotification(User user, int roomId, string msg)
        {
            Notification toSend = CreateNotification(roomId, msg);
            bool toReturn = user.SendNotification(toSend);
            return toReturn;
        }

        public bool SendNotificationByUserId(int userId, int roomId, string msg)
        {
            Notification toSend = CreateNotification(roomId, msg);
            User user = sc.GetUserWithId(userId);
            bool toReturn = user.SendNotification(toSend);
            return toReturn;
        }


        //todo - get priority - stuck until get log by id complete
        public Log.LogPriority GetLofPriority(int logId)
        {
            //Log getLog = GameCenter.Instance.FindLog(logId);
            throw new NotImplementedException();
        }
        
        
    }
}
