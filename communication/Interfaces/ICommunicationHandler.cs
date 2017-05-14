using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TexasHoldem.communication.Interfaces
{
    internal interface ICommunicationHandler
    {
        List<Tuple<string, TcpClient>> GetReceivedMessages();
        bool AddMsgToSend(string msg, int userId);
        void AddUserId(int id, TcpClient socket);
        void Start();
        void Close();
    }
}
